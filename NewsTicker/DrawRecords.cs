using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsTicker
{
    public class DrawRecords : BaseDraw
    {
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

        #endregion

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

            Tab(5);
            Write("Last activity:");
            Tab(8);
            Write(Records[Records.Count - 1]?.when.ToLongTimeString() ?? DateTime.Now.ToLongTimeString());
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

            if (Records.Count > 0)
            {
                int start = Console.CursorTop + 1;
                for (int i = Math.Max(Records.Count - (Console.WindowHeight - start), 0); i < Records.Count; i++)
                {
                    Line();
                    Write("{0} {1}", ConsoleColor.Gray, Records[i].when.ToShortDateString(), Records[i].when.ToLongTimeString());
                    Write(" {0}:", Records[i].color, Records[i].title);
                    Write(" {0}", ConsoleColor.White, Records[i].text);
                    ClearFix();
                }
            }
        }
    }
}
