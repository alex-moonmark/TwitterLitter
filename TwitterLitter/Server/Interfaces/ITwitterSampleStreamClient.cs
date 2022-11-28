using TwitterLitter.Shared;

namespace TwitterLitter.Server.Interfaces
{
    public interface ITwitterSampleStreamClient
    {
        static HttpClient? SampleStreamClient { get; set; }
        Task<string> ProcessStream(CancellationToken token);
        void ProcessTweetHandler(TwitterTweetWrapper tweet, CancellationToken token);
        Task Dispose();
    }
}
