using TwitterLitter.Client.Interfaces;

namespace TwitterLitter.Client.Classes
{
    public class TweetFormatHelper : ITweetFormatHelper
    {
        public async Task<string> FormatTweetHTML(string tweetText)
        {
            // add formatting for hyperlinks, etc here, depending on use cases and rules
            return "";
        }

        public string FormatTweetUserDisplay(string name, string username)
        {
            return String.Format("{0} (<a href='tweetsamples' @onclick:preventDefault>@{1}</a>)", name, username);
        }
    }
}
