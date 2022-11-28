namespace TwitterLitter.Client.Interfaces
{
    public interface ITweetFormatHelper
    {
        string FormatTweetUserDisplay(string name, string username);
        Task<string> FormatTweetHTML(string tweetText);
    }
}
