using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TwitterLitter.Shared
{
    public class TwitterTweet
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("author_id")]
        public string Author { get; set; }
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

    }

    public class TwitterTweetWrapper
    {
        [JsonProperty("data")]
        public TwitterTweet Tweet { get; set; }
        [JsonProperty("includes")]
        public TwitterIncludesWrapper Includes { get; set; }
    }

    public class TwitterIncludesWrapper
    {
        [JsonProperty("users")]
        public List<TwitterUser> Users { get; set; }
    }

    public class TwitterUser
    {
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("username")]
        public string UserName { get; set; }
    }
}
