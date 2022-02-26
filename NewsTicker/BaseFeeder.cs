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

        public bool KeepAlive = true;
        public Task UpdateLooper;

        public string Identifier;

        public virtual void FetchUpdates()
        {
            FetchUpdatesAsync().GetAwaiter().GetResult();
        }

        public async virtual Task FetchUpdatesAsync()
        {
        }

        protected async Task UpdateLoop()
        {
            KeepAlive = true;

            while (KeepAlive)
            {
                await FetchUpdatesAsync();
                await Task.Delay(1000 * 60);
            }

            KeepAlive = false;
        }
    }
}
