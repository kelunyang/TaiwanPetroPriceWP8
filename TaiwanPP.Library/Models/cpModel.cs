using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLitePCL;
using SQLite;
using TaiwanPP.Library.Helpers;
using System.ComponentModel;
using System.Collections;

namespace TaiwanPP.Library.Models
{
    public class priceStorage : IEquatable<priceStorage>
    {
        [PrimaryKey]
        public int id { get; set; }
        public long datetick { get; set; }
        public double price { get; set; }
        public int kind { get; set; }
        public int brand { get; set; }
        public bool saved { get; set; }
        [Ignore]
        public bool monitored { get; set; }
        [Ignore]
        public bool current { get; set; }
        [Ignore]
        public bool tile { get; set; }
        [Ignore]
        public bool vis { get; set; }
        public override int GetHashCode()
        {
            return id;
        }
        public bool Equals(priceStorage obj)
        {
            if (id != obj.id) return false;
            if (price != obj.price) return false;
            return true;
        }
        public override bool Equals(object obj)
        {
            priceStorage p = (priceStorage)obj;
            if (id != p.id) return false;
            if (price != p.price) return false;
            return true;
        }
    }
    public class oildata
    {
        public DateTime date;
        public double price95;
        public double price92;
        public double price98;
        public double pricediesel;
        public int kind;
        public oildata(string date, string p98, string p95, string p92, string pd, string kind)
        {
            this.kind = kind == "台塑石化" ? 1 : 0;
            this.price92 = Convert.ToDouble(p92);
            this.price95 = Convert.ToDouble(p95);
            this.price98 = Convert.ToDouble(p98);
            this.pricediesel = Convert.ToDouble(pd);
            this.date = DateTime.Parse(date.Remove(date.IndexOf("日")));
        }
    }
}