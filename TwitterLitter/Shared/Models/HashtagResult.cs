using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterLitter.Shared.Models
{
    public class HashtagResult
    {
        public string Hashtag { get; set; }
        public int Count { get; set; } = 0;

        public HashtagResult(string Hashtag)
        {
            this.Hashtag = Hashtag;
            this.Count = 1;
        }
    }
}
