using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaiwanPP.Library.Helpers
{
    public class typeDB
    {
        static public oilType CPCgasohol = new oilType() { key = 0, name = "酒精汽油", brand = 0 };
        static public oilType CPC92 = new oilType() { key = 1, name = "92無鉛汽油", brand = 0 };
        static public oilType CPC95 = new oilType() { key = 2, name = "95無鉛汽油", brand = 0 };
        static public oilType CPC98 = new oilType() { key = 3, name = "98無鉛汽油", brand = 0 };
        static public oilType CPCdiesel = new oilType() { key = 4, name = "高級/超級柴油", brand = 0 };
        static public oilType FPCC92 = new oilType() { key = 5, name = "92無鉛汽油", brand = 1 };
        static public oilType FPCC95 = new oilType() { key = 6, name = "95無鉛汽油", brand = 1 };
        static public oilType FPCC98 = new oilType() { key = 7, name = "98無鉛汽油", brand = 1 };
        static public oilType FPCCdiesel = new oilType() { key = 8, name = "普通柴油", brand = 1 };
        static public oilType[] productnameDB = { CPCgasohol, CPC92, CPC95, CPC98, CPCdiesel, FPCC92, FPCC95, FPCC98, FPCCdiesel };
    }
    public class oilType : INotifyPropertyChanged
    {
        string _name = "";
        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }
        int _key = 0;
        public int key
        {
            get { return _key; }
            set
            {
                _key = value;
                NotifyPropertyChanged();
            }
        }
        int _brand = 0;
        public int brand
        {
            get { return _brand; }
            set
            {
                _brand = value;
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

    }
}
