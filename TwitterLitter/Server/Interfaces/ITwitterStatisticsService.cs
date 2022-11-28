using TwitterLitter.Shared;
using TwitterLitter.Shared.Models;

namespace TwitterLitter.Server.Interfaces
{
    public interface ITwitterStatisticsService
    {
        static int TotalTweetsProcessed { get;set; }
        static List<HashtagResult> Hashtags { get; set; }
        static List<TwitterTweetWrapper> RecentTweets { get; set; }
        Task<bool> IncrementTweetsProcessed(int count = 1);
        Task<int> GetTweetsProcessedCount();
        Task<bool> UpdateHashtags(ProcessedTweetResult result);
        Task<List<HashtagResult>> GetTrendingHashtags(int count);
        Task<List<TwitterTweetWrapper>> GetTweetSamples(int count);
        Task<bool> ReplaceRecentTweets(TwitterTweetWrapper[] newTweets);

    }
}
