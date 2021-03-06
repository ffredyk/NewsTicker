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
    public class RssFeeder : BaseFeeder
    {
        public string RssSource = "";

        private string[] invalidTags = new string[]
        {
            "<![CDATA[ ]]>",
            "<![CDATA[",
            "]]>"
        };

        public RssFeeder(string identifier, string rss)
        {
            Identifier = identifier;
            RssSource = rss;
            Task.Run(() => FetchUpdatesAsync());
            UpdateLooper = Task.Factory.StartNew(() => UpdateLoop(), TaskCreationOptions.LongRunning);
        }

        public async override Task FetchUpdatesAsync()
        {
            try
            { 
                using HttpClient web = new HttpClient();

                var result = await web.GetStringAsync(RssSource);
                //foreach (string tag in invalidTags) result = result.Replace(tag, "");
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(result);

                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes[0])
                {

                    //if (counter++ >= hardlimit) break;
                    if (node.Name == "item")
                    {
                        Dictionary<string, string> parsednode = new Dictionary<string, string>();
                        foreach (XmlNode subnode in node)
                        {
                            parsednode.Add(subnode.Name, subnode.InnerText);
                        }

                        if (!(Tick.Ticks.Find(x => x.Hash == parsednode["title"].Hash()) is null)) continue;

                        var tick = new Tick()
                        {
                            Title = parsednode["title"],
                            URL = parsednode["link"],
                            Body = parsednode["description"],
                            Stamp = DateTime.Parse(parsednode["pubDate"]),
                            Source = Identifier,
                            Hash = parsednode["title"].Hash()
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
