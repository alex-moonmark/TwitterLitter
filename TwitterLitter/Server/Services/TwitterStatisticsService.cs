using System.Linq;
using TwitterLitter.Server.Interfaces;
using TwitterLitter.Shared;
using TwitterLitter.Shared.Models;

namespace TwitterLitter.Server.Services
{
    public class TwitterStatisticsService : ITwitterStatisticsService
    {
        public static int TotalTweetsProcessed { get; set; }
        public static List<HashtagResult> Hashtags { get; set; } = new List<HashtagResult>(1000);
        public static List<TwitterTweetWrapper> RecentTweets { get; set; } = new List<TwitterTweetWrapper>(50);

        private readonly object TweetsProcessedLock = new object();
        private readonly object HashtagsUpdateLock = new object();
        private readonly object HashtagsAddLock = new object();
        private readonly object RecentTweetsLock = new object();
        public TwitterStatisticsService()
        {
            if (Hashtags == null)
            {
                Hashtags = new List<HashtagResult>(1000);
            }
        }

        public async Task<bool> IncrementTweetsProcessed(int count = 1)
        {
            lock(TweetsProcessedLock)
            {
                TotalTweetsProcessed += count;
            }

            return true;
        }

        public async Task<int> GetTweetsProcessedCount()
        {
            return TotalTweetsProcessed;
        }

        public async Task<bool> UpdateHashtags(ProcessedTweetResult result)
        {
            if (!result.Token.IsCancellationRequested)
            {
                foreach (var tag in result.Hashtags)
                {
                    var foundTagResults = (from x in Hashtags
                                           where x.Hashtag == tag.Hashtag
                                           select x);

                    if (foundTagResults.Any())
                    {
                        lock (HashtagsUpdateLock)
                        {
                            foundTagResults.First().Count += tag.Count;
                        }
                    }
                    else if (Hashtags.Count <= 1000)
                    {
                        lock (HashtagsAddLock)
                        {
                            Hashtags.Add(tag);
                        }
                    }
                }
            }           
                        
            return true;
        }

        public async Task<List<HashtagResult>> GetTrendingHashtags(int count = 50)
        {
            return Hashtags.Where(x => x != null).OrderByDescending(x => x.Count).Take(count).ToList();
        }

        public async Task<List<TwitterTweetWrapper>> GetTweetSamples(int count)
        {
            return RecentTweets.Where(x => x != null).OrderByDescending(x => x.Tweet.CreatedAt).Take(count).ToList();   
        }

        public async Task<bool> ReplaceRecentTweets(TwitterTweetWrapper[] newTweets)
        {
            lock(RecentTweetsLock)
            {
                RecentTweets.Clear();
                RecentTweets.AddRange(newTweets.ToList());
            }

            return true;
        }
    }
}
