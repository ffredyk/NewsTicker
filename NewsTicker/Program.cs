using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NewsTicker
{
    class Program
    {
        public static List<Error> TickList;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            GlobalData.LoadFile();
            Console.WriteLine(GlobalData.Loaded);

            RssFeeder ct = new RssFeeder("CT24", "https://ct24.ceskatelevize.cz/rss/hlavni-zpravy");
            RssFeeder idnes = new RssFeeder("iDNES", "https://servis.idnes.cz/rss.aspx?c=zpravodaj");
            //TwitterFeeder twitter = new TwitterFeeder("Twitter", "(#ukraine) min_faves:100 lang:en -filter:replies");
            TwitterFeeder twitter = new TwitterFeeder("Twitter", "#ukraine");
            JsonWebFeeder idnesWeb = new JsonWebFeeder(
                "iDNES LIVE",
                "https://www.idnes.cz/zpravy/zahranicni/online-valka-na-ukrajine-rusko-zahajilo-invazi-idnes-cz.B1007797?g=js",
                "//div[@class='event']/p/b",
                "//div[@class='event']/p",
                "//div[@class='time']/div[0]");

            //Loops
            var displayLoop = Task.Factory.StartNew(() => Renderer.DrawLoop(new DrawTicks()), TaskCreationOptions.LongRunning);
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
