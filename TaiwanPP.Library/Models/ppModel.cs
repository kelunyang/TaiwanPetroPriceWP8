using System;
using System.Collections.Generic;
using System.Text;

namespace TaiwanPP.Library.Models
{
    public class weekpriceModel
    {
        public double dubai;
        public double brent;
        public double currency;
        public int cday;
        public double price
        {
            get
            {
                return Math.Round(Math.Round(dubai / cday, 2) * 0.7 + Math.Round(brent / cday, 2) * 0.3, 2) * Math.Round(currency / cday, 3);
            }
        }
        public double avgcurrency
        {
            get
            {
                return Math.Round(currency / cday, 3);
            }
        }
        public double avgdubai
        {
            get
            {
                return Math.Round(dubai / cday, 2);
            }
        }
        public double avgbrent
        {
            get
            {
                return Math.Round(brent / cday, 2);
            }
        }
        public weekpriceModel()
        {
            cday = 0;
            dubai = 0;
            currency = 0;
            currency = 0;
        }
    }
    public class internationalModel
    {
        public DateTime date;
        public long tick;
        public double dubai;
        public double brent;
        public double currency;
        public internationalModel(string dt, string db, string bt, string cr)
        {
            this.date = DateTime.Parse(dt.Substring(0, 4) + "." + dt.Substring(4));
            this.tick = date.Ticks;
            this.dubai = Convert.ToDouble(db);
            this.brent = Convert.ToDouble(bt);
            this.currency = Convert.ToDouble(cr);
        }
        public override string ToString()
        {
            return "Date:" + date + "/Dubai:" + dubai + "/Brent:" + brent + "/Currency:" + currency;
        }
    }
}
