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
        public string Hash = "";

        public static event EventHandler OnTickUpdate;
        public static volatile List<Tick> Ticks = new List<Tick>();

        public override string ToString()
        {
            return string.Format("[{0}] {1}",Source,Title);
        }
    }
}
