using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Text;
using TwitterLitter.Server.Interfaces;
using TwitterLitter.Server.Services;
using TwitterLitter.Shared;
using TwitterLitter.Shared.Models;

namespace TwitterLitter.Server.Classes
{
    public class TwitterClientHandler : ITwitterSampleStreamClient
    {
        static HttpClient? SampleStreamClient;
        private IConfiguration configuration;
        private ICancellationService cancellationService;
        private ITwitterStatisticsService statisticsService;
        private ITweetProcessingService processingService;
        public TwitterClientHandler(IConfiguration _configuration, ICancellationService _cancellationService, 
                                    ITwitterStatisticsService _statisticsService, ITweetProcessingService _processingService)
        {
            configuration = _configuration;
            cancellationService = _cancellationService;
            statisticsService = _statisticsService;
            processingService = _processingService;
            
            if (SampleStreamClient == null)
            {
#if DEBUG
                // ensure that we have configuration for unit test debugging
                configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
#endif
                SampleStreamClient = GetStreamClient();
            }
        }

        public async Task<string> ProcessStream(CancellationToken token)
        {

            string errorMessage = string.Empty;
            int retryCount = 0;

            while (retryCount < 5)
            {
                try
                {
                    // for metrics
                    DateTime startTime = DateTime.Now;

                    IAsyncEnumerable<TwitterTweetWrapper> tweetStream = ReadTweetStream(token);

                    int count = 0;

                    List<TwitterTweetWrapper> twitterTweets = new List<TwitterTweetWrapper>(500);

                    await foreach (var tweet in tweetStream)
                    {
                        // to end at a max number of tweets
                        //if (count >= 500)
                        //{
                        //    cancellationService.Cancel();
                        //}
                        //else
                        //{
                        if (tweet != null)
                        {
                            twitterTweets.Add(tweet);
                            ProcessTweetHandler(tweet, token);
                        }
                        //}

                        count++;

                        if (count % 50 == 0)
                        {
                            TwitterTweetWrapper[] wrapperArray = new TwitterTweetWrapper[50];
                            twitterTweets.CopyTo(wrapperArray);
                            Task.Run(() => statisticsService.ReplaceRecentTweets(wrapperArray));
                            twitterTweets.Clear();
                        }
                    }

                    // for metrics
                    DateTime endTime = DateTime.Now;
                    TimeSpan timeSpan = endTime - startTime;

                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                    // do something here with the error.  Maybe look for specific HTTP codes to indicate retry behavior?
                }
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return errorMessage;
            }
            else
            {
                return "Process stopped";
            }
            
        }

        public void ProcessTweetHandler(TwitterTweetWrapper tweet, CancellationToken token)
        {
            Task.Run(() => processingService.ProcessTweet(tweet, token)).ContinueWith(TweetProcessedCallback);
        }

        private async Task TweetProcessedCallback(Task<ProcessedTweetResult> task)
        {
            await statisticsService.IncrementTweetsProcessed();

            if (task.IsCompletedSuccessfully)
            {
                await Task.Run(() => statisticsService.UpdateHashtags(task.Result));
            }    
        }

        private async IAsyncEnumerable<TwitterTweetWrapper?> ReadTweetStream([EnumeratorCancellation] CancellationToken cancellationToken)
        {

            int retryCount = 0;

            while(!cancellationToken.IsCancellationRequested && retryCount < 5)
            {
                using (var response = await SampleStreamClient.GetStreamAsync(""))
                {
                    using (var reader = new StreamReader(response, Encoding.UTF8, true, 16384, leaveOpen: true))
                    {
                        while (!cancellationToken.IsCancellationRequested && 
                            !CancellationService.CancellationTokenSource.IsCancellationRequested &&
                            !reader.EndOfStream)
                        {
                            var line = await reader.ReadLineAsync().ConfigureAwait(false);
                            if (cancellationToken.IsCancellationRequested) break;
                            if (string.IsNullOrEmpty(line))
                            {
                                continue;
                            }
                            else
                            {
                                TwitterTweetWrapper? wrapper = JsonConvert.DeserializeObject<TwitterTweetWrapper>(line);
                                yield return wrapper;
                            }

                        }
                    }
                }

                if (cancellationToken.IsCancellationRequested ||
                    CancellationService.CancellationTokenSource.IsCancellationRequested ||
                    retryCount >= 5)
                {
                    SampleStreamClient.Dispose();
                    break;
                }
                else
                {
                    retryCount++;
                    SampleStreamClient = GetStreamClient();
                }
                
            }
                        
        }

        private HttpClient GetStreamClient()
        {
            if (SampleStreamClient == null)
            {
                
                string bearerToken = configuration.GetValue<string>("TwitterAPI:BearerToken");
                string streamUri = configuration.GetValue<string>("TwitterAPI:StreamUri");
                string userAgent = configuration.GetValue<string>("TwitterAPI:User-Agent");
                string streamOptions = configuration.GetValue<string>("TwitterAPI:StreamOptions");
                SampleStreamClient = new HttpClient();
                SampleStreamClient.BaseAddress = new Uri(streamUri + streamOptions);
                SampleStreamClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
                SampleStreamClient.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", bearerToken));
            }

            return SampleStreamClient;

        }

        public async Task Dispose()
        {
            cancellationService.Cancel();
            SampleStreamClient.Dispose();
        }
    }
}
