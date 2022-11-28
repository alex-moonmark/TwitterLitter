using System.Net.Http.Json;
using TwitterLitter.Shared;
using TwitterLitter.Shared.Models;

namespace TwitterLitter.Client.Services
{
    public class TwitterService : ITwitterService
    {
        private readonly HttpClient httpClient;

        public TwitterService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<bool> CancelTwitterFeed()
        {
            try
            {
                bool result = await httpClient.GetFromJsonAsync<bool>($"/api/Twitter/CancelTwitterFeed");

                return result;
            }
            catch (Exception ex)
            {
                // handle condition based on business rules
            }

            return false;

        }

        public async Task<List<HashtagResult>> GetTrendingHashtags(int count)
        {
            try
            {
                List<HashtagResult> result = await httpClient.GetFromJsonAsync<List<HashtagResult>>($"/api/Twitter/GetTrendingHashtags/" + count.ToString());

                if (result != null)
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                // handle condition based on business rules
            }

            return new List<HashtagResult>();
            
        }

        public async Task<List<TwitterTweetWrapper>> GetTweetSamples(int count)
        {
            try
            {
                List<TwitterTweetWrapper> result = await httpClient.GetFromJsonAsync<List<TwitterTweetWrapper>>($"/api/Twitter/GetTweetSamples/" + count.ToString());

                if (result != null)
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                // handle condition based on business rules
            }

            return new List<TwitterTweetWrapper>();
            
        }

        public async Task<int> GetTweetsProcessedCount()
        {
            try
            {
                int result = await httpClient.GetFromJsonAsync<int>($"/api/Twitter/GetTweetsProcessedCount");

                if (result != null)
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                // handle condition based on business rules
            }

            return 0;
            
        }
    }
}
