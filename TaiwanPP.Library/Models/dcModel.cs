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
        public string uri = "https://topmwp.codeplex.com/project/feeds/rss?ProjectRSSFeed=codeplex%3a%2f%2fwiki%2ftopmwp";
        public XDocument feed;
        public List<feedItem> items;
        public dcModel()
        {
        }
    }

    public class feedItem : INotifyPropertyChanged, IEquatable<feedItem>
    {
        public string _title;
        public DateTime _pubDate;
        public string _content;
        public string title
        {
            get { return _title; }
            set 
            {
                _title = value;
                NotifyPropertyChanged();
            }
        }
        public DateTime pubDate
        {
            get { return _pubDate; }
            set
            {
                _pubDate = value;
                NotifyPropertyChanged();
            }
        }
        public string content
        {
            get { return _content; }
            set
            {
                _content = value;
                NotifyPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }

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
