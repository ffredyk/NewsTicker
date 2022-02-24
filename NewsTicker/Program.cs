using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsTicker
{
    class Program
    {
        public static List<Tick> TickList;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            RssFeeder idnes = new RssFeeder("iDNES", "https://servis.idnes.cz/rss.aspx?c=zpravodaj");
            //RssFeeder ct = new RssFeeder("CT24","https://ct24.ceskatelevize.cz/rss/hlavni-zpravy");

            //Loops
            var displayLoop = Task.Factory.StartNew(() => Renderer.DrawLoop(new DrawTicks()), TaskCreationOptions.LongRunning);

            bool keepalive = true;
            while (keepalive)
            {
                await Task.Delay(1000);
            }
            Console.ReadLine();
        }
    }
}
