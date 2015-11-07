using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaiwanPP.Library.Helpers;
using System.ComponentModel;
using System.Collections;

namespace TaiwanPP.Library.Models
{
    public class discountStorage
    {
        public string brand { get; set; }
        public string bank { get; set; }
        public string servetype { get; set; }
        public string card { get; set; }
        public string content { get; set; }
        public string source { get; set; }
        public long startdate { get; set; }
        public long enddate { get; set; }
    }
}
