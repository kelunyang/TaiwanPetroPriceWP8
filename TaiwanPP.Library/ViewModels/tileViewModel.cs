using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaiwanPP.Library.ViewModels
{
    public class tileViewModel : viewmodelBase
    {
        int _brand = 0;
        double _price = double.NaN;
        int _visibility = 0;
        DateTime _uptime = DateTime.Now;
        double _change = double.NaN;
        double _pPrice = double.NaN;
        DateTime _pStartd = DateTime.Now;
        DateTime _pEndd = DateTime.Now;
        public DateTime pEndd
        {
            set
            {
                _pEndd = value;
                NotifyPropertyChanged();
            }
            get { return _pEndd; }
        }
        public DateTime pStartd
        {
            set
            {
                _pStartd = value;
                NotifyPropertyChanged();
            }
            get { return _pStartd; }
        }
        public double pPrice
        {
            set
            {
                _pPrice = value;
                NotifyPropertyChanged();
            }
            get { return _pPrice; }
        }
        public double change
        {
            set
            {
                _change = value;
                NotifyPropertyChanged();
            }
            get { return _change; }
        }
        public DateTime uptime
        {
            set
            {
                _uptime = value;
                NotifyPropertyChanged();
            }
            get
            {
                return _uptime;
            }
        }
        public int visibility
        {
            set
            {
                _visibility = value;
                NotifyPropertyChanged();
            }
            get
            {
                return _visibility;
            }
        }
        public double price
        {
            set
            {
                _price = value;
                NotifyPropertyChanged();
            }
            get { return _price; }
        }
        public int brand
        {
            set
            {
                _brand = value;
                NotifyPropertyChanged();
            }
            get
            {
                return _brand;
            }
        }
    }
}
