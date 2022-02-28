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

        #region Screen input handlers

        public int CurrentSelection = 0;

        public virtual bool OnKeyPress(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Enter)
                OnSelected();
            else if (key.Key == ConsoleKey.Spacebar)
                OnBack();

            else if (key.Key == ConsoleKey.UpArrow)
                CurrentSelection--;
            else if (key.Key == ConsoleKey.DownArrow)
                CurrentSelection++;
            else return false;

            CurrentSelection = (int)MathF.Max(CurrentSelection, 0);
            Renderer.Draw();

            return true;
        }

        public virtual void OnSelected()
        { }

        public virtual void OnBack()
        { }

        #endregion

        #region Console draw methods

        protected void Write(string text = "", ConsoleColor color = ConsoleColor.White, params object[] args)
        {
            Console.ResetColor();
            Console.ForegroundColor = color;
            Console.BackgroundColor = ConsoleColor.Black;

            string output = string.Format(text, args);

            int space = Console.WindowWidth - (Console.CursorLeft + 1);
            if (space < 1) return;
            if (output.Length > space) output = output.Substring(0, (int)MathF.Max(space - 4, 0)) + "...";

            Console.Write(output);
        }

        protected void WriteWrap(string text = "", ConsoleColor color = ConsoleColor.White, params object[] args)
        {
            Console.ResetColor();
            Console.ForegroundColor = color;
            Console.BackgroundColor = ConsoleColor.Black;

            string output = string.Format(text, args);

            int space = Console.WindowWidth - (Console.CursorLeft + 1);
            if (space < 1) return;
            //if (output.Length > space) output = output.Substring(0, space - 4) + "...";

            Console.Write(output);
        }

        protected void WriteInvert(string text = "", ConsoleColor color = ConsoleColor.White, params object[] args)
        {
            Console.ResetColor();
            Console.BackgroundColor = color;
            Console.ForegroundColor = ConsoleColor.Black;

            string output = string.Format(text, args);

            int space = Console.WindowWidth - (Console.CursorLeft + 1);
            if (output.Length > space) output = output.Substring(0, (int)MathF.Max(space - 4, 0)) + "...";

            Console.Write(output);
        }

        protected void Tab(int index = 0)
        {
            if (index > 0) index *= 10;
            else index = Console.CursorLeft + 10;

            //if (index > Console.WindowLeft) return;

            Console.SetCursorPosition((int)MathF.Min(index,Console.WindowWidth), Console.CursorTop);
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

        #endregion
    }
}
