using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace NewsTicker
{
    public class JsonWebFeeder : BaseFeeder
    {
        public string JsonSource = "";
        public string XPathText = "";
        public string XPathTitle = "";
        public string XPathTime = "";

        public JsonWebFeeder(string identifier, string json, string xptitle, string xptext, string xptime)
        {
            Identifier = identifier;
            JsonSource = json;
            this.XPathText = xptext;
            XPathTitle = xptitle;
            XPathTime = xptime;
            Task.Run(() => FetchUpdatesAsync());
            UpdateLooper = Task.Factory.StartNew(() => UpdateLoop(), TaskCreationOptions.LongRunning);
        }

        public async override Task FetchUpdatesAsync()
        {
            try
            {
                using HttpClient web = new HttpClient();

                var result = await web.GetStringAsync(JsonSource);
                //foreach (string tag in invalidTags) result = result.Replace(tag, "");
                var json = JObject.Parse(result);

                if (json["events"] is null) return;

                foreach (var node in json["events"])
                {
                    if (node["content"] is null) break;

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(node["content"].ToString());

                    var title = doc.DocumentNode.SelectSingleNode(XPathTitle)?.InnerText ?? "";
                    var text = doc.DocumentNode.SelectSingleNode(XPathText)?.InnerText ?? "";

                    //if (counter++ >= hardlimit) break;
                    if (!(Tick.Ticks.Find(x => x.Hash == title.Hash()) is null)) continue;

                    var tick = new Tick()
                    {
                        Title = title,
                        URL = JsonSource,
                        Body = text,
                        Stamp = DateTime.Parse(doc.DocumentNode.SelectSingleNode(XPathTime)?.InnerText ?? DateTime.Now.ToString()),
                        Source = Identifier,
                        Hash = title.Hash()
                    };
                    Tick.Ticks.Add(tick);
                }
            }
            catch(Exception e)
            {
                DrawLog.LogError(e);
            }
        }
    }
}
