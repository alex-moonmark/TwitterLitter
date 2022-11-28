using Microsoft.AspNetCore.Components;
using TwitterLitter.Client.Services;
using TwitterLitter.Shared.Models;

namespace TwitterLitter.Client.Pages
{
    public class CounterBase : ComponentBase 
    {
        [Inject] 
        public ITwitterService twitterService { get; set; }

        public int currentCount = 0;
        public List<HashtagResult> trendingHashtags { get; set; }
        public bool enableAutoRefresh { get; set; } = true;
        protected async override Task OnInitializedAsync()
        {
            await ReloadAllData(true);
        }

        public async Task IncrementCount()
        {
            // make controller call to increment counter here
            currentCount = await twitterService.GetTweetsProcessedCount();
        }

        public async Task CancelTwitterFeed()
        {
            await twitterService.CancelTwitterFeed();
        }

        public async Task LoadTrendingHashtags()
        {
            trendingHashtags = await twitterService.GetTrendingHashtags(50);
        }
        public async Task AutoRefreshChanged(ChangeEventArgs e)
        {
            enableAutoRefresh = (bool)e.Value;
        }
        public async Task ReloadAllData(bool autoEnabled)
        {
            if (autoEnabled)
            {
                await LoadTrendingHashtags();
                await IncrementCount();
            }            
        }
    }
}
