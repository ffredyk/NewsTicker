using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NewsTicker
{
    class Program
    {
        public static List<Tick> TickList;

        static async Task Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.WriteLine("Hello World!");

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            GlobalData.LoadFile();
            Console.WriteLine(GlobalData.Loaded);

            Renderer.CurrentDraw = new DrawTicks();

            List<BaseFeeder> feeders = new List<BaseFeeder>();
            foreach (var src in GlobalData.Sources)
            {
                if (src.Type == GlobalData.Source.SourceType.RSS) feeders.Add(new RssFeeder(src.Identificator, src.Query));
                if (src.Type == GlobalData.Source.SourceType.Twitter) feeders.Add(new TwitterFeeder(src.Identificator, src.Query));
                if (src.Type == GlobalData.Source.SourceType.JsonWeb) 
                    feeders.Add(new JsonWebFeeder(src.Identificator, src.Query, (string)src.Parameters.title, (string)src.Parameters.text, (string)src.Parameters.time));
                if (src.Type == GlobalData.Source.SourceType.Reddit) feeders.Add(new RedditFeeder(src.Identificator, src.Query));
                //*/
            }

            /*RssFeeder ct = new RssFeeder("CT24", "https://ct24.ceskatelevize.cz/rss/hlavni-zpravy");
            RssFeeder idnes = new RssFeeder("iDNES", "https://servis.idnes.cz/rss.aspx?c=zpravodaj");
            TwitterFeeder twitter = new TwitterFeeder("Twitter", "(#ukraine) min_faves:100 lang:en -filter:replies");
            TwitterFeeder tw_ukraine = new TwitterFeeder("Twitter", "#ukraine");
            TwitterFeeder tw_swu = new TwitterFeeder("Twitter", "#standwithukraine");
            TwitterFeeder tw_ww3 = new TwitterFeeder("Twitter", "@WW3updated");
            JsonWebFeeder idnesWeb = new JsonWebFeeder(
                "iDNES LIVE",
                "https://www.idnes.cz/zpravy/zahranicni/online-valka-na-ukrajine-rusko-zahajilo-invazi-idnes-cz.B1007797?g=js",
                "//div[@class='event']/p/b",
                "//div[@class='event']/p",
                "//div[@class='time']/div[0]");
            RedditFeeder wn = new RedditFeeder("Reddit", "18hnzysb1elcs");*/


            //Loops
            var displayLoop = Task.Factory.StartNew(() => Renderer.DrawLoop(), TaskCreationOptions.LongRunning);
            var inputLoop = Task.Factory.StartNew(() => Inputter.InputLoop(), TaskCreationOptions.LongRunning);

            bool keepalive = true;
            while (keepalive)
            {
                await Task.Delay(1000);
            }
            Console.ReadLine();
        }
    }
}
