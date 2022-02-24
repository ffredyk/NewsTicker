using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NewsTicker
{
    public static class Renderer
    {
        public static bool KeepAlive = true;
        public static bool Enabled = true;

        private static uint clearer = 0;
        public static DateTime TimeStart = DateTime.Now;

        public static BaseDraw CurrentDraw;

        public async static Task DrawLoop(BaseDraw InitialDraw)
        {
            Enabled = true;
            Console.Clear();

            CurrentDraw = InitialDraw;

            while (KeepAlive)
            {
                Draw();
                await Task.Delay(100);
            }

            Console.Clear();
            Console.CursorVisible = true;
            Enabled = false;
        }

        public static void Draw()
        {
            try
            {
                //Clear();
                Console.SetCursorPosition(0, 0);
                if (clearer++ % 100 == 0) Console.Clear();

                Write("NewsTicker (CZ edition) v{0}", ConsoleColor.White, Assembly.GetExecutingAssembly().GetName().Version.ToString());
                var args = Environment.GetCommandLineArgs();
                for (int i = 1; i < args.Length; i++)
                {
                    var arg = args[i];
                    //if (arg == "paramarg") arg += " " + args[++i];
                    Tab();
                    Write(arg, ConsoleColor.DarkGray);
                }
                Line();
                Write(new string('=', Console.BufferWidth - 1)); //separator

                CurrentDraw.Draw();
            }
            catch (Exception e)
            {
                /*Logger.Log("[RENDERER]", ConsoleColor.Red, "Crashed! {0}: {1}", e.Source, e.Message);
                Logger.Log("[STACKTRACE]", ConsoleColor.DarkRed, e.StackTrace);*/
            }
        }

        public static void Clear()
        {
            for (int i = 0; i < 20; i--)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.BufferWidth));
            }
            Console.CursorVisible = false;
        }

        public static void Write(string text = "", ConsoleColor color = ConsoleColor.White, params object[] args)
        {
            Console.ResetColor();
            Console.ForegroundColor = color;
            Console.BackgroundColor = ConsoleColor.Black;

            string output = string.Format(text, args);

            int space = Console.BufferWidth - (Console.CursorLeft + 1);
            if (output.Length > space) output = output.Substring(0, space - 4) + "...";

            Console.Write(output);
        }

        public static void WriteInvert(string text = "", ConsoleColor color = ConsoleColor.White, params object[] args)
        {
            Console.ResetColor();
            Console.BackgroundColor = color;
            Console.ForegroundColor = ConsoleColor.Black;

            string output = string.Format(text, args);

            int space = Console.BufferWidth - (Console.CursorLeft + 1);
            if (output.Length > space) output = output.Substring(0, space - 4) + "...";

            Console.Write(output);
        }

        public static void Tab(int index = 0)
        {
            if (index > 0) index *= 10;
            else index = Console.CursorLeft + 10;

            Console.SetCursorPosition(index, Console.CursorTop);
        }

        public static void Line()
        {
            ClearFix();
            Console.SetCursorPosition(0, Console.CursorTop + 1);
        }

        public static void ClearFix()
        {
            Console.Write(new string(' ', Console.BufferWidth - (Console.CursorLeft + 1)));
        }
    }
}
