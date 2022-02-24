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
            Renderer.Line();
            Renderer.Tab(1);
            Renderer.Write("Start Time:");
            Renderer.Tab(3);
            Renderer.Write(Renderer.TimeStart.ToLongTimeString());

            Renderer.Tab(5);
            Renderer.Write("Current Time:");
            Renderer.Tab(8);
            Renderer.Write(DateTime.Now.ToLongTimeString());
            ///////
            Renderer.Line();
            Renderer.Tab(1);
            Renderer.Write("Uptime:");
            Renderer.Tab(3);
            Renderer.Write(DateTime.Now.Subtract(Renderer.TimeStart).ToString(@"hh\:mm\:ss"));

            Renderer.Tab(5);
            Renderer.Write("Last activity:");
            Renderer.Tab(8);
            Renderer.Write(Records[Records.Count - 1]?.when.ToLongTimeString() ?? DateTime.Now.ToLongTimeString());
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

            Renderer.Line();
            Renderer.Write(new string('-', Console.BufferWidth - 1)); //separator

            if (Records.Count > 0)
            {
                int start = Console.CursorTop + 1;
                for (int i = Math.Max(Records.Count - (Console.WindowHeight - start), 0); i < Records.Count; i++)
                {
                    Renderer.Line();
                    Renderer.Write("{0} {1}", ConsoleColor.Gray, Records[i].when.ToShortDateString(), Records[i].when.ToLongTimeString());
                    Renderer.Write(" {0}:", Records[i].color, Records[i].title);
                    Renderer.Write(" {0}", ConsoleColor.White, Records[i].text);
                    Renderer.ClearFix();
                }
            }
        }
    }
}
