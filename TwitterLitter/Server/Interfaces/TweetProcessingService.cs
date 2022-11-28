using System.Text.RegularExpressions;
using TwitterLitter.Server.Services;
using TwitterLitter.Shared;
using TwitterLitter.Shared.Models;

namespace TwitterLitter.Server.Interfaces
{
    public class TweetProcessingService : ITweetProcessingService
    {
                
        private IConfiguration configuration;
        static Regex HashtagRegex;

        public TweetProcessingService(IConfiguration _configuration)
        {
            this.configuration = _configuration;
            if (HashtagRegex == null)
            {
#if DEBUG
                // ensure that we have configuration for unit test debugging
                configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
#endif
                HashtagRegex = new Regex(configuration.GetValue<string>("TwitterAPI:HashtagRegex"));
            }
        }

        public async Task<ProcessedTweetResult> ProcessTweet(TwitterTweetWrapper wrapper, CancellationToken token)
        {
            ProcessedTweetResult result = new ProcessedTweetResult();

            Dictionary<string, HashtagResult> hashtags = new Dictionary<string, HashtagResult>(10);

            if (!token.IsCancellationRequested)
            {
                foreach (var match in HashtagRegex.Matches(wrapper.Tweet.Text))
                {
                    if (hashtags.TryGetValue(match.ToString(), out HashtagResult? value))
                    {
                        value.Count++;
                    }
                    else if (hashtags.Count < 10)
                    {
                        hashtags.Add(match.ToString(), new HashtagResult(match.ToString()));
                    }

                }

            }

            result.Hashtags = hashtags.Values.ToList();
            result.Token = token;

            return result;
        }
        
        
    }
}
