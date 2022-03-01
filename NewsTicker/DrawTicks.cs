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

        public static int LastViewedErrorCount = 0;

        public override void Draw()
        {
            try
            { 
                List<Tick> orderedTicks = new List<Tick>(Tick.Ticks.GroupBy(t => t.Hash).Select(grp => grp.FirstOrDefault()));
                orderedTicks.Sort((x, y) => DateTime.Compare(x.Stamp, y.Stamp)*-1);

                Line();
                Tab(1);
                Write("Start Time:");
                Tab(3);
                Write(Renderer.TimeStart.ToLongTimeString());

                Tab(5);
                Write("Current Time:");
                Tab(7);
                Write(DateTime.Now.ToLongTimeString());

                Tab(8);
                Write("Selection:");
                Tab(9);
                Write(CurrentSelection.ToString());
                ///////
                Line();
                Tab(1);
                Write("Uptime:");
                Tab(3);
                Write(DateTime.Now.Subtract(Renderer.TimeStart).ToString(@"hh\:mm\:ss"));

                Tab(5);
                Write("Last activity:");
                Tab(7);
                if(orderedTicks.Count > 0) Write(orderedTicks[orderedTicks.Count - 1]?.Stamp.ToLongTimeString() ?? DateTime.Now.ToLongTimeString());

                if (DrawLog.Errors.Count > LastViewedErrorCount)
                {
                    Tab(8);
                    Write("= Errors logged! Press SPACE to view =",ConsoleColor.Red);
                }
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

                if (CurrentSelection > 0)
                {
                    Line();
                    WriteInvert(new string('^', Console.BufferWidth/2 - 2), ConsoleColor.DarkCyan);
                    WriteInvert(" UP ", ConsoleColor.DarkCyan);
                    WriteInvert(new string('^', Console.BufferWidth/2 - 2), ConsoleColor.DarkCyan);
                }

                if (orderedTicks.Count > 0)
                {
                    int start = Console.CursorTop + 1;
                    int end = ((Console.WindowHeight - start) + CurrentSelection);
                    end = (int)MathF.Min(end, orderedTicks.Count);
                    for (int i = CurrentSelection; i < end; i++)
                    {
                        if (i + 1 == end && orderedTicks.Count > i)
                        {
                            Line();
                            WriteInvert(new string('v', Console.BufferWidth / 2 - 3), ConsoleColor.DarkCyan);
                            WriteInvert(" DOWN ", ConsoleColor.DarkCyan);
                            WriteInvert(new string('v', Console.BufferWidth / 2 - 3), ConsoleColor.DarkCyan);
                        }
                        else
                        {
                            Line();
                            Write("{0} {1}", ConsoleColor.Gray, orderedTicks[i].Stamp.ToShortDateString(), orderedTicks[i].Stamp.ToLongTimeString());
                            Write(" [{0}]", ConsoleColor.Gray, orderedTicks[i].Source.Substring(0, (int)MathF.Min(orderedTicks[i].Source.Length, 20)));
                            Write(" {0}:", ConsoleColor.Red, orderedTicks[i].Title.Replace("\n", ""));
                            if ((Console.WindowWidth - Console.CursorLeft) >= 10) Write(" {0}", ConsoleColor.White, orderedTicks[i].Body.Replace("\n", ""));
                            ClearFix();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                DrawLog.LogError(e);
            }
        }

        public override bool OnKeyPress(ConsoleKeyInfo key)
        {
            base.OnKeyPress(key);
            Console.Clear();
            return true;
        }

        public override void OnBack()
        {
            Console.Clear();
            Renderer.CurrentDraw = new DrawLog();
        }
    }
}
