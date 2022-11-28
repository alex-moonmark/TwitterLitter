using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TwitterLitter.Server.Controllers;
using TwitterLitter.Server.Interfaces;
using TwitterLitter.Server.Services;
using TwitterLitter.Shared;
using TwitterLitter.Shared.Models;
using Xunit;

namespace TwitterLitter.Tests
{
    
    public class UnitTestTwitterSample
    {
        private readonly ICancellationService cancellationService;
        private readonly ITwitterSampleStreamClient sampleClient;
        private readonly ITwitterStatisticsService statisticsService;
        private readonly ITweetProcessingService processingService;
        public UnitTestTwitterSample(ICancellationService _cancellationService,
                                        ITwitterSampleStreamClient _sampleClient,
                                        ITwitterStatisticsService _statisticsService,
                                        ITweetProcessingService _processingService)
        {
            cancellationService = _cancellationService;
            sampleClient = _sampleClient;
            statisticsService = _statisticsService;
            processingService = _processingService;

            
        }
        
        [Fact]
        public async Task TestTwitterSampleFeedController()
        {
            // setup
            TwitterController controller = new TwitterController(cancellationService, statisticsService);

            TwitterTweetWrapper wrapper1 = new TwitterTweetWrapper();
            TwitterTweetWrapper wrapper2 = new TwitterTweetWrapper();
            TwitterTweetWrapper wrapper3 = new TwitterTweetWrapper();

            TwitterTweet tweet1 = new TwitterTweet()
            { 
                Author = "123456",
                CreatedAt = DateTime.Now.ToLongDateString(),
                ID = "444555",
                Text = "Testing single hashtag #hastagsingle"
            };

            wrapper1.Tweet = tweet1;
            CancellationTokenSource tokenSource = new CancellationTokenSource();

            // two hashtags
            var tweet2 = new TwitterTweet()
            {
                Author = "123456",
                CreatedAt = DateTime.Now.ToLongDateString(),
                ID = "444555",
                Text = "Testing two hashtag #hastagsingleone #hashtagsingletwo"
            };

            wrapper2.Tweet = tweet2;

            // multiple same hashtag
            var tweet3 = new TwitterTweet()
            {
                Author = "123456",
                CreatedAt = DateTime.Now.ToLongDateString(),
                ID = "444555",
                Text = "Testing two hashtag #hashtagdouble #hashtagdouble"
            };

            wrapper3.Tweet = tweet3;

            // execute
            sampleClient.ProcessTweetHandler(wrapper1, tokenSource.Token);
            
            sampleClient.ProcessTweetHandler(wrapper2, tokenSource.Token);
            
            sampleClient.ProcessTweetHandler(wrapper3, tokenSource.Token);

            Thread.Sleep(5000);

            List<HashtagResult> finalresult = null;

            finalresult = await controller.GetTrendingHashtags(10);

            Assert.True(finalresult.Count == 4);
            Assert.True(finalresult[0].Hashtag == "#hashtagdouble");
            
        }

        [Fact]
        public async Task TestTwitterSampleFeedRequest()
        {
            // setup
            TwitterController controller = new TwitterController(cancellationService, statisticsService);
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            TwitterTweetWrapper wrapper1 = new TwitterTweetWrapper();
            TwitterTweetWrapper wrapper2 = new TwitterTweetWrapper();
            TwitterTweetWrapper wrapper3 = new TwitterTweetWrapper();
            List<TwitterTweetWrapper> wrapperList = new List<TwitterTweetWrapper>();

            TwitterTweet tweet1 = new TwitterTweet()
            {
                Author = "123456",
                CreatedAt = DateTime.Now.ToLongTimeString(),
                ID = "444555",
                Text = "Testing single hashtag #hastagsingle"
            };
            wrapper1.Tweet = tweet1;
            wrapperList.Add(wrapper1);

            // two hashtags
            var tweet2 = new TwitterTweet()
            {
                Author = "123456",
                CreatedAt = DateTime.Now.ToLongTimeString(),
                ID = "444555",
                Text = "Testing two hashtag #hastagsingleone #hashtagsingletwo"
            };
            wrapper2.Tweet = tweet2;
            wrapperList.Add(wrapper2);

            // multiple same hashtag
            var tweet3 = new TwitterTweet()
            {
                Author = "123456",
                CreatedAt = DateTime.Now.ToLongTimeString(),
                ID = "444555",
                Text = "Testing two hashtag #hashtagdouble #hashtagdouble"
            };

            wrapper3.Tweet = tweet3;
            wrapperList.Add(wrapper3);

            // execute
            TwitterTweetWrapper[] wrapperArray = new TwitterTweetWrapper[50];
            wrapperList.CopyTo(wrapperArray);

            await statisticsService.ReplaceRecentTweets(wrapperArray);

            var result = await controller.GetTweetSamples(4);

            // assert
            Assert.True(result.Count == 3);
        }
    }
}