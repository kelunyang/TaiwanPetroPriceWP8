using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaiwanPP.Library.Helpers
{
    public class longlistCollection<T> : ObservableCollection<T>, INotifyPropertyChanged
    {
        string _header = "";
        public string header
        {
            get { return _header; }
            set
            {
                _header = value;
                NotifyPropertyChanged();
            }
        }
        int _distance = 0;
        public int distance
        {
            get { return _distance; }
            set
            {
                _distance = value;
                NotifyPropertyChanged();
            }
        }
        bool _favorite = false;
        public bool favorite
        {
            get { return _favorite; }
            set
            {
                _favorite = value;
                NotifyPropertyChanged();
            }
        }
        GeoPoint _currentloc = new GeoPoint(0,0);
        public GeoPoint currentloc
        {
            set
            {
                _currentloc = value;
                NotifyPropertyChanged();
            }
            get { return _currentloc; }
        }
        long _startTime = 0;
        public long startTime
        {
            set
            {
                _startTime = value;
                NotifyPropertyChanged();
            }
            get { return _startTime; }
        }
        long _endTime = 0;
        public long endTime
        {
            set
            {
                _endTime = value;
                NotifyPropertyChanged();
            }
            get { return _endTime; }
        }
        protected override event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}
