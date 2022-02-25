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

        public static void LoadFile()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar + "ticker.cfg";
            if (!File.Exists(path)) return;

            var cfg = File.ReadAllText(path);
            dynamic vals = JObject.Parse(cfg);

            TwitterSettings.APIKey =           vals.twitter.api_key ?? "";
            TwitterSettings.APISecret =        vals.twitter.api_secret ?? "";
            TwitterSettings.AccessToken =      vals.twitter.access_token ?? "";
            TwitterSettings.AccessSecret =     vals.twitter.access_secret ?? "";
            TwitterSettings.BEARER =           vals.twitter.bearer ?? "";
            TwitterSettings.MaxPosts =         vals.twitter.max_posts ?? 10;

            Loaded = true;
        }
    }
}
