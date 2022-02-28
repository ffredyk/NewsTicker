using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsTicker
{
    public class DrawPrompt : BaseDraw
    {
        #region EventArgs definition

        public class PromptEventArgs : EventArgs
        {
            public List<string> Results;

            public PromptEventArgs(List<string> results)
            { Results = results; }
        }

        #endregion

        public static volatile string Question = "";
        public static volatile List<string> Prompts = new List<string>();
        public static volatile List<string> Responds = new List<string>();

        public int CurrentPrompt = 0;

        public event EventHandler<PromptEventArgs> OnFinished;

        public override void Draw()
        {
            Line();
            Tab(1);
            Write("Start Time:");
            Tab(3);
            Write(Renderer.TimeStart.ToLongTimeString());

            Tab(5);
            Write("Current Time:");
            Tab(8);
            Write(DateTime.Now.ToLongTimeString());
            ///////
            Line();
            Tab(1);
            Write("Uptime:");
            Tab(3);
            Write(DateTime.Now.Subtract(Renderer.TimeStart).ToString(@"hh\:mm\:ss"));

            /*Tab(5);
            Write("Last error:");
            Tab(8);
            Write(orderedList[orderedList.Count - 1]?.when.ToLongTimeString() ?? DateTime.Now.ToLongTimeString());*/
            ///////
            /*Line();
            Tab(1);
            Write("Active feeds:");
            Tab(3);
            Write("{0}/{1}", ConsoleColor.White, Program.viewers.Count, Cache.ViewerCache.InstanceCount);*/

            /*Line();
            Tab(1);
            Write("Latest URL:");
            Tab(3);
            Write(Program.viewers.Count > 0 ? Program.viewers[Program.viewers.Count - 1].CurrentWeb : "None");*/

            Line();
            Write(new string('-', Console.BufferWidth - 1)); //separator
            Line();
            WriteInvert(Question, ConsoleColor.Gray);
            Line();

            for (int i = 0; i <= CurrentPrompt; i++)
            {
                Line();
                if (CurrentSelection == i) Write(">");
                else Write("");

                Write(Prompts[i]);
                Line();
                if (CurrentSelection == i) WriteInvert(Responds[i]);
                else Write(Responds[i]);
            }
        }

        public override bool OnKeyPress(ConsoleKeyInfo key)
        {
            base.OnKeyPress(key);

            if(key.Key == ConsoleKey.Backspace)
            {
                var str = Responds[CurrentSelection];
                if (str.Length <= 1) Responds[CurrentSelection] = "";
                Responds[CurrentSelection] = str.Substring(0, str.Length - 1);
            }
            else if(key.Key == ConsoleKey.Enter)
            {
                CurrentPrompt++;
                if(CurrentPrompt == Prompts.Count)
                {
                    OnFinished.Invoke(this, new PromptEventArgs(Responds));
                }
            }
            else if(key.KeyChar >= 'a' && key.KeyChar <= 'Z')
            {
                Responds[CurrentSelection] += key.KeyChar;
            }

            return true;
        }
    }
}
