using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NewsTicker
{
    public static class GlobalData
    {
        public class Source
        {
            public enum SourceType
            {
                Undefined = 0,
                RSS = 1,
                Twitter = 2,
                Web = 3,
                JsonWeb = 4,
                Reddit = 5,
            }
            public SourceType Type;

            public string Identificator;
            public string Query;
            public dynamic Parameters;
        }

        public static class TwitterSettings
        {
            public static string APIKey = "";
            public static string APISecret = "";
            public static string AccessToken = "";
            public static string AccessSecret = "";
            public static string BEARER = "";
            public static int MaxPosts = 10;
        }

        public static bool Loaded = false;
        public static List<Source> Sources = new List<Source>();

        public static void LoadFile()
        {
            try
            { 
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar + "ticker.cfg";
                if (!File.Exists(path)) return;

                var cfg = File.ReadAllText(path);
                dynamic vals = JObject.Parse(cfg);

                if (!(vals.sources is null))
                {
                    foreach (var src in vals.sources)
                    {
                        var type = Source.SourceType.Undefined;
                        if (src.type == "rss") type = Source.SourceType.RSS;
                        if (src.type == "twitter") type = Source.SourceType.Twitter;
                        if (src.type == "web") type = Source.SourceType.Web;
                        if (src.type == "jsonweb") type = Source.SourceType.JsonWeb;
                        if (src.type == "reddit") type = Source.SourceType.Reddit;

                        Sources.Add(new Source()
                        {
                            Type = type,
                            Identificator = src.id,
                            Query = src.query,
                            Parameters = src.args
                        });
                    }
                }

                if (!(vals.twitter is null))
                {
                    TwitterSettings.APIKey = vals.twitter.api_key ?? "";
                    TwitterSettings.APISecret = vals.twitter.api_secret ?? "";
                    TwitterSettings.AccessToken = vals.twitter.access_token ?? "";
                    TwitterSettings.AccessSecret = vals.twitter.access_secret ?? "";
                    TwitterSettings.BEARER = vals.twitter.bearer ?? "";
                    TwitterSettings.MaxPosts = vals.twitter.max_posts ?? 10;
                }

                Loaded = true;
            }
            catch (Exception e)
            {
                DrawLog.LogError(e);
            }
        }
    }
}
