using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using TaiwanPP.Library.Helpers;
using TaiwanPP.Library.Models;
using System.Threading.Tasks;
using SQLite.Net.Async;
using SQLite.Net;
using System.IO;
using System.Windows.Input;
using System.Xml;
using System.Collections.Specialized;

namespace TaiwanPP.Library.ViewModels
{
    public abstract class viewmodelBase : INotifyPropertyChanged
    {
        protected SQLiteAsyncConnection dbConn;
        protected infoModel _im;
        public virtual event PropertyChangedEventHandler PropertyChanged;
        public infoModel im {
            set
            {
                _im = value;
            }
            get
            {
                return _im;
            }
        }
        public virtual async Task loadDB(SQLite.Net.Interop.ISQLitePlatform platform,string dbPath)
        {
            try
            {
                var connectionFactory = new Func<SQLiteConnectionWithLock>(() => new SQLiteConnectionWithLock(platform, new SQLiteConnectionString(dbPath, storeDateTimeAsTicks: false)));
                dbConn = new SQLiteAsyncConnection(connectionFactory);
            }
            catch
            {
                throw new dbException("開啟資料庫");
            }
        }
        protected viewmodelBase() { }
        public virtual void saveConfig(Stream sw)
        {
            im.save(sw);
            sw.Dispose();
        }
        protected virtual void NotifyPropertyChanged(string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}
