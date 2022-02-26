using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace NewsTicker
{
    public class WebFeeder : BaseFeeder
    {
        public string WebSource = "";
        public string XPathArticle = "";
        public string XPathText = "";
        public string XPathTitle = "";
        public string XPathTime = "";

        private string[] invalidTags = new string[]
        {
            "<![CDATA[ ]]>",
            "<![CDATA[",
            "]]>"
        };

        public WebFeeder(string identifier, string web, string xpatharticle, string xptitle, string xptext, string xptime)
        {
            Identifier = identifier;
            WebSource = web;
            XPathArticle = xpatharticle;
            XPathText = xptext;
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
                var result = "";

                //encoding fail tempfix
                try { result = await web.GetStringAsync(WebSource); }
                catch
                {
                    var res = await web.GetAsync(WebSource);
                    using var sr = new StreamReader(await res.Content.ReadAsStreamAsync(), Encoding.GetEncoding("windows-1250"));
                    result = sr.ReadToEnd();
                }

                //foreach (string tag in invalidTags) result = result.Replace(tag, "");
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(result);

                var nodes = doc.DocumentNode.SelectNodes(XPathArticle);
                foreach (var node in nodes)
                {
                    var title = node.SelectSingleNode(XPathTitle)?.InnerText ?? "";
                    var text = node.SelectSingleNode(XPathText)?.InnerText ?? "";

                    if (Error.Ticks.Find(x => x.Body == text) is not null) continue;

                    var tick = new Error()
                    {
                        Title = title,
                        URL = WebSource,
                        Body = text,
                        Stamp = DateTime.Parse(node.SelectSingleNode(XPathTime)?.InnerText ?? DateTime.Now.ToString()),
                        Source = Identifier,
                    };
                    Error.Ticks.Add(tick);
                }
            }
            catch (Exception e)
            {
                DrawLog.LogError(e);
            }
        }
    }
}
