using TwitterLitter.Shared;
using TwitterLitter.Shared.Models;

namespace TwitterLitter.Client
{
    public interface ITwitterService
    {
        Task<bool> CancelTwitterFeed();
        Task<int> GetTweetsProcessedCount();
        Task<List<HashtagResult>> GetTrendingHashtags(int count);
        Task<List<TwitterTweetWrapper>> GetTweetSamples(int count);
    }
}
