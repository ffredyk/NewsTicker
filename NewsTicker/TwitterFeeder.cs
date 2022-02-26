using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Tweetinvi;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.V2;

namespace NewsTicker
{
    public class TwitterFeeder : BaseFeeder
    {
        public string TwitterQuery = "";
        public Tweetinvi.Models.LanguageFilter Language;

        public TwitterClient client;

        public TwitterFeeder(string identifier, string query, Tweetinvi.Models.LanguageFilter lang = Tweetinvi.Models.LanguageFilter.English)
        {
            Identifier = identifier;
            TwitterQuery = query;
            Language = lang;

            if (!GlobalData.Loaded) return;

            client = new TwitterClient(GlobalData.TwitterSettings.APIKey, GlobalData.TwitterSettings.APISecret, GlobalData.TwitterSettings.AccessToken, GlobalData.TwitterSettings.AccessSecret);

            /*var stream = client.Streams.CreateFilteredStream(new CreateFilteredTweetStreamParameters()
            {
                CustomQueryParameters = new List<Tuple<string, string>>()
                {
                    ""
                }
            });*/
            

            Task.Run(() => FetchUpdatesAsync());
            UpdateLooper = Task.Factory.StartNew(() => UpdateLoop(), TaskCreationOptions.LongRunning);
        }

        public async override Task FetchUpdatesAsync()
        {
            try
            {
                /*var pars = new SearchTweetsV2Parameters(TwitterQuery)
                {
                    TweetFields = { "public_metrics,like_count,public_metrics.like_count" },
                    UserFields = { "public_metrics,like_count,public_metrics.like_count" },
                    Expansions = { "public_metrics,like_count,public_metrics.like_count" },
                    CustomQueryParameters =
                    {
                        new Tuple<string, string>("tweet.fields", "public_metrics,created_at,author_id,like_count,public_metrics.like_count"),
                        new Tuple<string, string>("expansions", "author_id,like_count,public_metrics.like_count")
                    },
                                      
                };
                pars.
                var tweets = await client.SearchV2.SearchTweetsAsync(pars);*/
                var tweets = await client.Search.SearchTweetsAsync(new SearchTweetsParameters(TwitterQuery)
                {
                    TweetMode = TweetMode.Extended,
                    Lang = Language,
                    SearchType = Tweetinvi.Models.SearchResultType.Popular,
                    Locale = "cs",
                    IncludeEntities = true,
                    /*CustomQueryParameters =
                    {
                        new Tuple<string, string>("tweet.fields", "public_metrics,created_at,author_id,like_count,public_metrics.like_count"),
                        new Tuple<string, string>("expansions", "public_metrics.like_count")
                    },*/
                });

                int limiter = 0;
                if (tweets.Length > 0)
                {
                    foreach (var t in tweets)
                    {
                        if (t.RetweetCount < 500) continue;
                        if (Error.Ticks.Find(x => x.Title == t.CreatedBy.ScreenName) is not null) continue;
                        if (++limiter > GlobalData.TwitterSettings.MaxPosts) break;

                        var tick = new Error()
                        {
                            Title = t.CreatedBy.ScreenName,
                            URL = string.Format("https://twitter.com/{0}/status/{1}", t.CreatedBy.ScreenName, t.Id),
                            Body = t.Text,
                            Stamp = t.CreatedAt.DateTime.AddHours(1),
                            Source = Identifier,
                        };
                        Error.Ticks.Add(tick);
                    }
                }
            }
            catch (Exception e)
            {
                DrawLog.LogError(e);
            }
        }
    }
}
