using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterLitter.Shared.Models
{
    public class ProcessedTweetResult
    {
        public List<HashtagResult> Hashtags { get; set; }
        public CancellationToken Token { get; set; }
        public ProcessedTweetResult()
        {
            Hashtags = new List<HashtagResult>();
        }

    }
}
