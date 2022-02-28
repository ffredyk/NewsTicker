using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsTicker
{
    public class Inputter
    {
        public static bool KeepAlive = true;
        public static bool Enabled = true;

        public async static Task InputLoop()
        {
            Enabled = true;

            while (KeepAlive)
            {
                var key = Console.ReadKey(true);
                Renderer.CurrentDraw.OnKeyPress(key);
                /*if(key.Key == ConsoleKey.Spacebar)
                {
                    var oldview = Renderer.CurrentDraw;
                    Renderer.CurrentDraw = new DrawLog();
                    Console.Clear();

                    Console.ReadKey(true);

                    Renderer.CurrentDraw = oldview;
                    Console.Clear();
                }*/
            }
        }
    }
}
