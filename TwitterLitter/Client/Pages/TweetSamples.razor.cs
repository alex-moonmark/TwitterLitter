using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitterLitter.Client.Interfaces;
using TwitterLitter.Shared;

namespace TwitterLitter.Client.Pages
{
    public partial class TweetSamples : ComponentBase
    {
        [Inject]
        public ITwitterService twitterService { get; set; }
        [Inject]
        public ITweetFormatHelper formatHelper { get; set; }
        public List<TwitterTweetWrapper> tweetWrappers { get; set; }
        public bool enableAutoRefresh { get; set; } = true;

        protected async override Task OnInitializedAsync()
        {
            await ReloadAllData(true);
        }

        public async Task GetTweetSamples()
        {
            tweetWrappers = await twitterService.GetTweetSamples(50);
        }
        public async Task AutoRefreshChanged(ChangeEventArgs e)
        {
            enableAutoRefresh = (bool)e.Value;
        }

        public async Task ReloadAllData(bool autoEnabled)
        {
            if (autoEnabled)
            {
                await GetTweetSamples();
            }            
        }

    }
}
