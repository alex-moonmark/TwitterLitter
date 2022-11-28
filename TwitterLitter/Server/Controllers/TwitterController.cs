using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Text;
using TwitterLitter.Server.Classes;
using TwitterLitter.Server.Interfaces;
using TwitterLitter.Shared;
using TwitterLitter.Shared.Models;

namespace TwitterLitter.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwitterController : ControllerBase
    {
        private ICancellationService cancellationService;
        private ITwitterStatisticsService twitterStatisticsService;

        public TwitterController(ICancellationService _cancellationService, ITwitterStatisticsService _twitterStatisticsService)
        {
            cancellationService = _cancellationService;
            twitterStatisticsService = _twitterStatisticsService;
        }
        
        [HttpGet("CancelTwitterFeed")]
        public async Task<bool> CancelTwitterFeed()
        {
            cancellationService.Cancel();
            return true;
        }

        [HttpGet("GetTweetsProcessedCount")]
        public async Task<int> GetTweetsProcessedCount()
        {
            return await twitterStatisticsService.GetTweetsProcessedCount();
        }

        [HttpGet("GetTrendingHashtags/{count}")]
        public async Task<List<HashtagResult>> GetTrendingHashtags(int count)
        {
            return await twitterStatisticsService.GetTrendingHashtags(count);
        }

        [HttpGet("GetTweetSamples/{count}")]
        public async Task<List<TwitterTweetWrapper>> GetTweetSamples(int count)
        {
            return await twitterStatisticsService.GetTweetSamples(count);
        }

    }
}
