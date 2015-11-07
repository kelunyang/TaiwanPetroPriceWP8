using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLite.Net.Attributes;
using TaiwanPP.Library.Helpers;

namespace TaiwanPP.Library.Models
{
    public class stationStorage : IEquatable<stationStorage>
    {
        [PrimaryKey, AutoIncrement]
        public int sid { get; set; }
        public string phone { get; set; }
        public string name { get; set; }
        public bool type { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string address { get; set; }
        public long starttime { get; set; }
        public long duration { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int selftype { get; set; }
        public int brand { get; set; }
        public bool favorite { get; set; }
        public bool p92 { get; set; }
        public bool p95 { get; set; }
        public bool p98 { get; set; }
        public bool pdiesel { get; set; }
        public bool pgasohol { get; set; }
        [Ignore]
        public string cpcid { get; set; }  //temp CPC station id
        [Ignore]
        public double distance { get; set; }
        [Ignore]
        public bool current { get; set; }
        [Ignore]
        public bool regular { get; set; }
        [Ignore]
        public bool open { get; set; }
        [Ignore]
        public DateTime endtime { get; set; }
        [Ignore]
        public GeoPoint coordinance
        {
            set { }
            get
            {
                return new GeoPoint(latitude, longitude);
            }
        }
        public override bool Equals(object obj)
        {
            stationStorage s = (stationStorage)obj;
            if (this.name != s.name) return false;
            if(this.phone != s.phone) return false;
            if (this.address != s.address) return false;
            if (this.p92 != s.p92) return false;
            if (this.p95 != s.p95) return false;
            if (this.p98 != s.p98) return false;
            if (this.pdiesel != s.pdiesel) return false;
            if (this.pgasohol != s.pgasohol) return false;
            return true;
        }
        public override int GetHashCode()
        {

            return Convert.ToInt32(phone.Replace("-","").Replace("(","").Replace(")",""));
        }
        public bool Equals(stationStorage obj)
        {
            if (this.name != obj.name) return false;
            if (this.phone != obj.phone) return false;
            if (this.address != obj.address) return false;
            if (this.p92 != obj.p92) return false;
            if (this.p95 != obj.p95) return false;
            if (this.p98 != obj.p98) return false;
            if (this.pdiesel != obj.pdiesel) return false;
            if (this.pgasohol != obj.pgasohol) return false;
            return true;
        }
        public bool minimumEquals(stationStorage obj)
        {
            if (this.name != obj.name) return false;
            if (this.phone != obj.phone) return false;
            return true;
        }
    }
}
