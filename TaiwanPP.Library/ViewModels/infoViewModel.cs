using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TaiwanPP.Library.Helpers;
using TaiwanPP.Library.Models;

namespace TaiwanPP.Library.ViewModels
{
    public class infoViewModel : viewmodelBase
    {
        public infoViewModel() {
            pagename = new ObservableCollection<string>();
            pagename.Add("摘要資訊");
            pagename.Add("油價預測");
            pagename.Add("目前價格");
            pagename.Add("加油站查詢");
            pagename.Add("折扣查詢");
        }
        public void loadConfig(Stream str)
        {
            im.load(str);
        }
        bool _firstLoad = false;
        public bool firstLoad
        {
            get 
            {
                _firstLoad = im.firstLoad;
                return _firstLoad;
            }
            set
            {
                im.firstLoad = value;
                NotifyPropertyChanged();
            }
        }
        bool _savetime = false;
        public bool savetime
        {
            get
            {
                _savetime = im.savetime;
                return _savetime; 
            }
            set
            {
                im.savetime = value;
                NotifyPropertyChanged();
            }
        }
        bool _updatefreq = false;
        public bool updatefreq
        {
            get 
            {
                _updatefreq = im.updatefreq;
                return _updatefreq;
            }
            set
            {
                im.updatefreq = value;
                NotifyPropertyChanged();
            }
        }
        int _cpcdefaultProduct = 0;
        public int cpcdefaultProduct
        {
            get 
            {
                _cpcdefaultProduct = im.cpcdefaultProduct;
                return _cpcdefaultProduct;
            }
            set
            {
                im.cpcdefaultProduct = value;
                NotifyPropertyChanged();
            }
        }
        int _fpccdefaultProduct = 0;
        public int fpccdefaultProduct
        {
            get 
            {
                _fpccdefaultProduct = im.fpccdefaultProduct;
                return _fpccdefaultProduct; 
            }
            set
            {
                im.fpccdefaultProduct = value;
                NotifyPropertyChanged();
            }
        }
        int _defaultTile = 1;
        public int defaultTile
        {
            get 
            {
                _defaultTile = im.defaultTile;
                return _defaultTile; 
            }
            set
            {
                im.defaultTile = value;
                NotifyPropertyChanged();
            }
        }
        bool _CPCnotidied = false;
        public bool CPCnotified
        {
            get 
            {
                _CPCnotidied = im.CPCnotified;
                return _CPCnotidied;
            }
            set
            {
                im.CPCnotified = value;
                NotifyPropertyChanged();
            }
        }
        bool _FPCCnotidied = false;
        public bool FPCCnotified
        {
            get
            {
                _FPCCnotidied = im.FPCCnotified;
                return _CPCnotidied;
            }
            set
            {
                im.FPCCnotified = value;
                NotifyPropertyChanged();
            }
        }
        DateTime _dailynotify = DateTime.MinValue;
        public DateTime dailynotify
        {
            get
            {
                _dailynotify = im.dailynotify;
                return _dailynotify;
            }
            set
            {
                im.dailynotify = value;
                NotifyPropertyChanged();
            }
        }
        DateTime _dailynotifytime = DateTime.MinValue;
        public DateTime dailynotifytime
        {
            get
            {
                _dailynotifytime = im.dailynotifytime;
                return _dailynotifytime;
            }
            set
            {
                im.dailynotifytime = value;
                NotifyPropertyChanged();
            }
        }
        bool _dailynotified = false;
        public bool dailynotified
        {
            get
            {
                _dailynotified = im.dailynotified;
                return _dailynotified;
            }
            set
            {
                im.dailynotified = value;
                NotifyPropertyChanged();
            }
        }
        DateTime _notifycheckedHour = DateTime.MinValue;
        public DateTime notifycheckedHour
        {
            get
            {
                _notifycheckedHour = im.notifycheckedHour;
                return _notifycheckedHour;
            }
            set
            {
                im.notifycheckedHour = value;
                NotifyPropertyChanged();
            }
        }
        DateTime _dailycheckHour = DateTime.MinValue;
        public DateTime dailycheckHour
        {
            get
            {
                _notifycheckedHour = im.dailynotifiedHour;
                return _notifycheckedHour;
            }
            set
            {
                im.dailynotifiedHour = value;
                NotifyPropertyChanged();
            }
        }
        DateTime _version = DateTime.MinValue;
        public DateTime version
        {
            get
            {
                _version = im.version;
                return _version;
            }
            set
            {
                im.version = value;
                NotifyPropertyChanged();
            }
        }
        public ObservableCollection<string> pagename { get; set; }
        int _defaultPage = 0;
        public int defaultPage
        {
            get
            {
                _defaultPage = im.defaultPage;
                return _defaultPage;
            }
            set
            {
                im.defaultPage = value;
                NotifyPropertyChanged();
            }
        }
        bool _upgrade = false;
        public bool upgrade
        {
            get
            {
                _upgrade = im.upgrade;
                return _upgrade;
            }
            set
            {
                im.upgrade = value;
                NotifyPropertyChanged();
            }
        }
        DateTime _dbcheckedDate = DateTime.MinValue;
        public DateTime dbcheckedDate
        {
            set
            {
                im.DBcheckedDate = value;
                NotifyPropertyChanged();
            }
            get
            {
                _dbcheckedDate = im.DBcheckedDate;
                return _dbcheckedDate;
            }
        }
        bool _autoUpdate = true;
        public bool autoUpdate
        {
            get
            {
                _autoUpdate = im.autoupdate;
                return _autoUpdate;
            }
            set
            {
                im.autoupdate = value;
                NotifyPropertyChanged();
            }
        }
        bool _dailynotifyEnable = true;
        public bool dailynotifyEnable
        {
            get
            {
                _dailynotifyEnable = im.dailynotifyEnable;
                return _dailynotifyEnable;
            }
            set
            {
                im.dailynotifyEnable = value;
                NotifyPropertyChanged();
            }
        }
        DateTime _tileupdateTime = DateTime.MinValue;
        public DateTime tileupdateTime
        {
            get
            {
                _tileupdateTime = im.tileupdateTime;
                return _tileupdateTime;
            }
            set
            {
                im.tileupdateTime = value;
                NotifyPropertyChanged();
            }
        }
        bool _connectivity = true;
        public bool connectivity
        {
            get { return _connectivity; }
            set
            {
                _connectivity = value;
                NotifyPropertyChanged();
            }
        }
    }
}
