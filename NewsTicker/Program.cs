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

            //Loops
            var displayLoop = Task.Factory.StartNew(() => Renderer.DrawLoop(new DrawRecords()), TaskCreationOptions.LongRunning);
        }
    }
}
