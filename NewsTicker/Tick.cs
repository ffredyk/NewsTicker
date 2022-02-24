using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsTicker
{
    public class Tick
    {
        public string Title = "";
        public string URL = "";
        public string Body = "";
        public DateTime Stamp = DateTime.Now;
        public string Source = "";

        public static event EventHandler OnTickUpdate;
        public static List<Tick> Ticks = new List<Tick>();
    }
}
