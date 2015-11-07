using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace TaiwanPP.Library.Models
{
    public class dcModel
    {
        public List<feedItem> items;
        public dcModel()
        {
            items = new List<feedItem>();
        }
    }

    public class feedItem : IEquatable<feedItem>
    {
        public string title { get; set; }
        public DateTime pubDate { get; set; }
        public string link { get; set; }
        public string content { get; set; }
        public int type { get; set; }
        public bool Equals(feedItem other)
        {
            if (title == other.title) return true;
            return false;
        }

        public override int GetHashCode()
        {
            return title.GetHashCode();
        }
    }
}
