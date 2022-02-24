using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsTicker
{
    public abstract class BaseFeeder
    {
        public List<string> FeedURLs;
        public DateTime LastUpdate;

        public string Identifier;

        public virtual void FetchUpdates()
        {
            FetchUpdatesAsync().GetAwaiter().GetResult();
        }

        public async virtual Task FetchUpdatesAsync()
        {
        }
    }
}
