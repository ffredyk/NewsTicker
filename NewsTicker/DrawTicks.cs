using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsTicker
{
    public class DrawTicks : BaseDraw
    {
        /*
        #region Record manipulation

        public class Record
        {
            public ConsoleColor color = ConsoleColor.Gray;
            public string title = "";
            public string text = "";
            public DateTime when = DateTime.Now;
        }

        public static List<Record> Records = new List<Record>();

        public static void AddRecord(string title, params object[] args) => DrawRecords.Records.Add(new DrawRecords.Record() { title = string.Format(title, args) });
        public static void AddRecord(
            string title,
            ConsoleColor color = ConsoleColor.White,
            string text = "",
            params object[] args) => DrawRecords.Records.Add(new DrawRecords.Record() { title = title, color = color, text = string.Format(text, args) });

        #endregion*/

        public override void Draw()
        {
            List<Error> orderedTicks = new List<Error>(Error.Ticks);
            orderedTicks.Sort((x, y) => DateTime.Compare(x.Stamp, y.Stamp)*-1);

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

            Tab(5);
            Write("Last activity:");
            Tab(8);
            Write(orderedTicks[orderedTicks.Count - 1]?.Stamp.ToLongTimeString() ?? DateTime.Now.ToLongTimeString());
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

            if (orderedTicks.Count > 0)
            {
                int start = Console.CursorTop + 1;
                for (int i = 0; i < (Console.WindowHeight - start); i++)
                {
                    Line();
                    Write("{0} {1}", ConsoleColor.Gray, orderedTicks[i].Stamp.ToShortDateString(), orderedTicks[i].Stamp.ToLongTimeString());
                    Write(" [{0}]", ConsoleColor.Gray, orderedTicks[i].Source.Substring(0,(int)MathF.Min(orderedTicks[i].Source.Length,20)));
                    Write(" {0}:", ConsoleColor.Red, orderedTicks[i].Title.Replace("\n", ""));
                    if((Console.WindowWidth - Console.CursorLeft) >= 10) Write(" {0}", ConsoleColor.White, orderedTicks[i].Body.Replace("\n", ""));
                    ClearFix();
                }
            }
        }
    }
}
