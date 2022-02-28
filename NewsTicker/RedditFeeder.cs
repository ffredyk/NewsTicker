using Reddit;
using Reddit.Controllers;
using Reddit.Controllers.EventArgs;
using Reddit.Inputs.LiveThreads;
using Reddit.Things;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using RestSharp;
using HtmlAgilityPack;

namespace NewsTicker
{
    public class RedditFeeder : BaseFeeder
    {
        public string Query = "";

        public static RedditClient client;
        public static RestClient r_client;
        public LiveThread thread;

        public static readonly string AppID = "KS56Hyc91x7MSUqplo-cIA";
        public static readonly string AppSecret = "uIRfnO6El2XiBRQ7VMqcSfI1547gvg";

        public RedditFeeder(string identifier, string query, Tweetinvi.Models.LanguageFilter lang = Tweetinvi.Models.LanguageFilter.English)
        {
            try
            {
                Identifier = identifier;
                Query = query;

                Task.Run(() => FetchUpdatesAsync());
                UpdateLooper = Task.Factory.StartNew(() => UpdateLoop(), TaskCreationOptions.LongRunning);
            }
            catch (Exception e)
            {
                DrawLog.LogError(e);
            }
        }

        private void PromptFinish(object sender, DrawPrompt.PromptEventArgs args)
        {
            thread = client.LiveThread(Query);
            thread.UpdatesUpdated += UpdateHandler;
            thread.MonitorUpdates();
        }

        private void HandleUpdate(LiveUpdate update)
        {
            try
            {
                var tick = new Tick()
                {
                    Title = update.Name,
                    URL = string.Format("https://reddit.com/live/{0}/", thread.Id),
                    Body = update.Body,
                    Stamp = update.CreatedUTC,
                    Source = Identifier,
                    Hash = update.Id.Hash()
                };
                Tick.Ticks.Add(tick);
            }
            catch (Exception e)
            {
                DrawLog.LogError(e);
            }
        }

        private void UpdateHandler(object sender, LiveThreadUpdatesUpdateEventArgs args)
        {
            try
            {
                foreach (var update in args.Added)
                {
                    if (!(Tick.Ticks.Find(x => x.Hash == update.Id.Hash()) is null)) continue;
                    HandleUpdate(update);
                }
            }
            catch (Exception e)
            {
                DrawLog.LogError(e);
            }
        }

        public async override Task FetchUpdatesAsync()
        {
            try
            {
                using HttpClient web = new HttpClient();

                var result = await web.GetStringAsync(Query);
                //foreach (string tag in invalidTags) result = result.Replace(tag, "");
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(result);

                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {

                    //if (counter++ >= hardlimit) break;
                    if (node.Name == "entry")
                    {
                        Dictionary<string, string> parsednode = new Dictionary<string, string>();
                        foreach (XmlNode subnode in node)
                        {
                            parsednode.Add(subnode.Name, subnode.InnerText);
                        }

                        if (!(Tick.Ticks.Find(x => x.Hash == parsednode["id"].Hash()) is null)) continue;

                        var title = "";
                        var content = new HtmlDocument();
                        content.LoadHtml(parsednode["content"]);

                        var href = content.DocumentNode.SelectSingleNode("//a[starts-with(@href,'https://twitter.com/')]");
                        if (href is null)
                        {
                            href = content.DocumentNode.SelectSingleNode("//div");
                            parsednode["content"] = href.InnerText;
                        }
                        else
                        {
                            var id = href.Attributes["href"].Value.Split("/")[5].Substring(0,19);
                            var tweet = await TwitterFeeder.GetTweetInfo(long.Parse(id));
                            parsednode["content"] = tweet.FullText;
                            title = tweet.CreatedBy.ScreenName;
                        }


                        var tick = new Tick()
                        {
                            Title = title,
                            URL = parsednode["link"],
                            Body = parsednode["content"],
                            Stamp = DateTime.Parse(parsednode["updated"]),
                            Source = Identifier,
                            Hash = parsednode["id"].Hash()
                        };
                        Tick.Ticks.Add(tick);
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
