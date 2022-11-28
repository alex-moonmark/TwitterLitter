using TwitterLitter.Shared;
using TwitterLitter.Shared.Models;

namespace TwitterLitter.Server.Services
{
    public interface ITweetProcessingService
    {
        Task<ProcessedTweetResult> ProcessTweet(TwitterTweetWrapper tweet, CancellationToken token);
    }
}
