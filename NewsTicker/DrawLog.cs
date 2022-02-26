using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsTicker
{
    public class DrawLog : BaseDraw
    {
        #region Error holder

        public class Error
        {
            public ConsoleColor color = ConsoleColor.Red;
            public Exception exception;
            public DateTime when = DateTime.Now;
        }

        public static List<Error> Errors = new List<Error>();

        public static void LogError(
            Exception e,
            ConsoleColor color = ConsoleColor.Red) => Errors.Add(new Error() { exception = e, color = color });

        #endregion

        public override void Draw()
        {
            List<Error> orderedList = new List<Error>(Errors);
            orderedList.Sort((x, y) => DateTime.Compare(x.when, y.when)*-1);

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

            if (orderedList.Count > 0)
            {
                int start = Console.CursorTop + 1;
                for (int i = 0; i < (Console.WindowHeight - start); i++)
                {
                    Line();
                    Write("{0} {1}", ConsoleColor.Gray, orderedList[i].when.ToShortDateString(), orderedList[i].when.ToLongTimeString());
                    WriteInvert(" [{0}]", ConsoleColor.Red, orderedList[i].exception.Source.Substring(0,(int)MathF.Min(orderedList[i].exception.Source.Length,20)));
                    Write(" {0}:", ConsoleColor.Red, orderedList[i].exception.Message.Replace("\n", ""));
                    Line();
                    WriteWrap(" {0}", ConsoleColor.White, orderedList[i].exception.StackTrace);
                    //if((Console.WindowWidth - Console.CursorLeft) >= 10) Write(" {0}", ConsoleColor.White, orderedList[i].exception.StackTrace);
                    ClearFix();
                }
            }
        }
    }
}
