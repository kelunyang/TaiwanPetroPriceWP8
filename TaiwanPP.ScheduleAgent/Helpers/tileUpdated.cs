using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaiwanPP.ScheduleAgent.Helpers
{
    public class tileUpdated : EventArgs
    {
        public bool Success { get; set; }
        public int Count { get; set; }
        public Exception Exception { get; set; }
        public tileUpdated(bool success)
        {
            Success = success;
        }
    }
}
