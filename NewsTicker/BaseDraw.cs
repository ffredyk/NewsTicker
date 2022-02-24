using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsTicker
{
    public abstract class BaseDraw
    {
        public virtual void Draw()
        {
        }

        protected void Write(string text = "", ConsoleColor color = ConsoleColor.White, params object[] args)
        {
            Console.ResetColor();
            Console.ForegroundColor = color;
            Console.BackgroundColor = ConsoleColor.Black;

            string output = string.Format(text, args);

            int space = Console.WindowWidth - (Console.CursorLeft + 1);
            if (output.Length > space) output = output.Substring(0, space - 4) + "...";

            Console.Write(output);
        }

        protected void WriteInvert(string text = "", ConsoleColor color = ConsoleColor.White, params object[] args)
        {
            Console.ResetColor();
            Console.BackgroundColor = color;
            Console.ForegroundColor = ConsoleColor.Black;

            string output = string.Format(text, args);

            int space = Console.WindowWidth - (Console.CursorLeft + 1);
            if (output.Length > space) output = output.Substring(0, space - 4) + "...";

            Console.Write(output);
        }

        protected void Tab(int index = 0)
        {
            if (index > 0) index *= 10;
            else index = Console.CursorLeft + 10;

            Console.SetCursorPosition(index, Console.CursorTop);
        }

        protected void Line()
        {
            ClearFix();
            Console.SetCursorPosition(0, Console.CursorTop + 1);
        }

        protected void ClearFix()
        {
            Console.Write(new string(' ', Console.BufferWidth - (Console.CursorLeft + 1)));
        }
    }
}
