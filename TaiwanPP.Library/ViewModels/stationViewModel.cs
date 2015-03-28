using HtmlAgilityPack;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using TaiwanPP.Library.Helpers;
using TaiwanPP.Library.Models;
//using Windows.Devices.Geolocation;

namespace TaiwanPP.Library.ViewModels
{
    public class stationViewModel : viewmodelBase
    {
        List<string> filtername = new List<string>() { "僅顯示台塑", "僅顯示中油", "自助服務", "直營站", "儲存的站台", "僅限營業中的站台", "提供92無鉛汽油", "提供95無鉛汽油", "提供98無鉛汽油", "提供柴油", "提供酒精汽油" };
        private List<stationStorage> stationDB;
        private phoneQuery phoneDB;
        IEnumerable<stationStorage> queryList;
        //private Geolocator _geolocator = null; 8.1 code
        private CancellationTokenSource _cts = null;
        public void loadConfig(Stream str)
        {
            im.load(str);
            str.Dispose();
        }
        public ObservableCollection<longlistCollection<stationStorage>> queryStations { get; set; }
        public ObservableCollection<stationStorage> mapStations { get; set; }
        public ObservableCollection<string> countryitems { get; set; }
        DateTime _stationDBdate = DateTime.MinValue;
        public int _filtercount = 0;
        public int filtercount
        {
            get { return _filtercount; }
            set
            {
                _filtercount = value;
                NotifyPropertyChanged();
            }
        }
        public GeoPoint _centerloc = new GeoPoint(0, 0);
        public GeoPoint centerloc
        {
            get { return _centerloc; }
            set
            {
                _centerloc = value;
                NotifyPropertyChanged();
            }
        }
        public DateTime stationDBdate
        {
            get 
            {
                _stationDBdate = im.stationDBdate;
                return _stationDBdate; 
            }
            set
            {
                im.stationDBdate = value;
                NotifyPropertyChanged();
            }
        }
        bool _sfilterFPCC = false;
        public bool sfilterFPCC
        {
            get
            {
                _sfilterFPCC = im.sfilterFPCC;
                return _sfilterFPCC;
            }
            set
            {
                im.sfilterFPCC = value;
                NotifyPropertyChanged();
            }
        }
        bool _sfilterInservice = false;
        public bool sfilterInservice
        {
            get
            {
                _sfilterInservice = im.sfilterInservice;
                return _sfilterInservice;
            }
            set
            {
                im.sfilterInservice = value;
                NotifyPropertyChanged();
            }
        }
        List<string> _sfilterSubbrand = new List<string>();
        public List<string> sfilterSubbrand
        {
            get
            {
                _sfilterSubbrand = im.sfilterSubBrands;
                return _sfilterSubbrand;
            }
            set
            {
                im.sfilterSubBrands = value;
                NotifyPropertyChanged();
            }
        }
        bool _sfilterCPC = false;
        public bool sfilterCPC
        {
            get
            {
                _sfilterCPC = im.sfilterCPC;
                return _sfilterCPC;
            }
            set
            {
                im.sfilterCPC = value;
                NotifyPropertyChanged();
            }
        }
        bool _sfilterDirect = false;
        public bool sfilterDirect
        {
            get
            {
                _sfilterDirect = im.sfilterDirect;
                return _sfilterDirect;
            }
            set
            {
                im.sfilterDirect = value;
                NotifyPropertyChanged();
            }
        }
        bool _sfilterSelf = false;
        public bool sfilterSelf
        {
            get
            {
                _sfilterSelf = im.sfilterSelf;
                return _sfilterSelf;
            }
            set
            {
                im.sfilterSelf = value;
                NotifyPropertyChanged();
            }
        }
        bool _sfilterFavoirte = true;
        public bool sfilterFavoirte
        {
            get
            {
                _sfilterFavoirte = im.sfilterFavoirte;
                return _sfilterFavoirte;
            }
            set
            {
                im.sfilterFavoirte = value;
                NotifyPropertyChanged();
            }
        }
        double _sfilterKilonmeter = 3;
        public double sfilterKilonmeter
        {
            get
            {
                _sfilterKilonmeter = im.sfilterKilonmeter;
                return _sfilterKilonmeter;
            }
            set
            {
                im.sfilterKilonmeter = value;              
                NotifyPropertyChanged();
            }
        }
        bool _sfiltercountryEnable = false;
        public bool sfiltercountryEnable
        {
            get
            {
                _sfiltercountryEnable = im.sfiltercountryEnable;
                return _sfiltercountryEnable;
            }
            set
            {
                im.sfiltercountryEnable = value;
                NotifyPropertyChanged();
            }
        }
        bool _sfilterp92 = false;
        public bool sfilterp92
        {
            get
            {
                _sfilterp92 = im.sfilterp92;
                return _sfilterp92;
            }
            set
            {
                im.sfilterp92 = value;
                NotifyPropertyChanged();
            }
        }
        bool _sfilterp95 = false;
        public bool sfilterp95
        {
            get
            {
                _sfilterp95 = im.sfilterp95;
                return _sfilterp95;
            }
            set
            {
                im.sfilterp95 = value;
                NotifyPropertyChanged();
            }
        }
        bool _sfilterp98 = false;
        public bool sfilterp98
        {
            get
            {
                _sfilterp98 = im.sfilterp98;
                return _sfilterp98;
            }
            set
            {
                im.sfilterp98 = value;
                NotifyPropertyChanged();
            }
        }
        bool _sfilterpdiesel = false;
        public bool sfilterpdiesel
        {
            get
            {
                _sfilterpdiesel = im.sfilterpdiesel;
                return _sfilterpdiesel;
            }
            set
            {
                im.sfilterpdiesel = value;
                NotifyPropertyChanged();
            }
        }
        bool _sfilterpgasohol = false;
        public bool sfilterpgasohol
        {
            get
            {
                _sfilterpgasohol = im.sfilterpgasohol;
                return _sfilterpgasohol;
            }
            set
            {
                im.sfilterpgasohol = value;
                NotifyPropertyChanged();
            }
        }
        int _sfiltercountry = 0;
        public int sfiltercountry
        {
            get
            {
                _sfiltercountry = im.sfiltercountry;
                return _sfiltercountry;
            }
            set
            {
                im.sfiltercountry = value;
                NotifyPropertyChanged();
            }
        }
        int _stationDBcount = 0;
        public int stationDBcount
        {
            get { return _stationDBcount; }
            set
            {
                _stationDBcount = value;
                NotifyPropertyChanged();
            }
        }
        int _foundcount = 0;
        public int foundcount
        {
            get { return _foundcount; }
            set
            {
                _foundcount = value;
                NotifyPropertyChanged();
            }
        }
        DateTime _stationDBnotifyDate = DateTime.MinValue;
        public DateTime stationDBnotifyDate
        {
            get
            {
                _stationDBnotifyDate = im.stationDBnotifydate;
                return _stationDBnotifyDate;
            }
            set
            {
                im.stationDBnotifydate = value;
                NotifyPropertyChanged();
            }
        }
        GeoPoint _sfilterlocation = new GeoPoint(0,0);
        public GeoPoint sfilterlocation
        {
            get
            {
                _sfilterlocation = im.sfilterrangeLocation;
                return _sfilterlocation;
            }
            set
            {
                im.sfilterrangeLocation = value;
                NotifyPropertyChanged();
            }
        }
        string _sfilterlocationname = "";
        public string sfilterlocationname
        {
            get
            {
                _sfilterlocationname = im.sfilterrangeLocationname;
                return _sfilterlocationname;
            }
            set
            {
                im.sfilterrangeLocationname = value;
                NotifyPropertyChanged();
            }
        }
        bool _sfilterlocated = false;
        public bool sfilterlocated
        {
            get { return _sfilterlocated; }
            set
            {
                _sfilterlocated = value;
                NotifyPropertyChanged();
            }
        }
        bool _sfiltercustomLocation = false;
        public bool sfiltercustomLocation
        {
            get
            {
                _sfiltercustomLocation = im.sfiltercustomLocation;
                return _sfiltercustomLocation;
            }
            set
            {
                im.sfiltercustomLocation = value;
                NotifyPropertyChanged();
            }
        }
        bool _stationBehavior = true;
        public bool stationBehavior
        {
            get
            {
                _stationBehavior = im.stationBehavior;
                return _stationBehavior;
            }
            set
            {
                im.stationBehavior = value;
                NotifyPropertyChanged();
            }
        }
        public ObservableCollection<string> filters { get; set; }
        public stationViewModel() 
        {
            _cts = new CancellationTokenSource();
            queryStations = new ObservableCollection<longlistCollection<stationStorage>>();
            mapStations = new ObservableCollection<stationStorage>();
            filters = new ObservableCollection<string>();
            countryitems = new ObservableCollection<string>();
            countryitems.Add("載入中...");
        }

        public override async Task loadDB(SQLite.Net.Interop.ISQLitePlatform platform,string dbPath)
        {
            await base.loadDB(platform, dbPath);
            stationDB = await dbConn.Table<stationStorage>().ToListAsync();
            stationDBcount = stationDB.Count();
            IEnumerable<string> citylist = (from city in stationDB orderby city.city select city.city).Distinct();
            countryitems.Clear();
            foreach (string c in citylist)
            {
                countryitems.Add(c);
            }
            filters.Clear();
            if (sfilterCPC) filters.Add(filtername[0]);
            if (sfilterFPCC) filters.Add(filtername[1]);
            if (sfilterDirect) filters.Add(filtername[3]);
            if (sfilterSelf) filters.Add(filtername[2]);
            if (sfilterFavoirte) filters.Add(filtername[4]);
            if (sfilterInservice) filters.Add(filtername[5]);
            if (sfilterp92) filters.Add(filtername[6]);
            if (sfilterp95) filters.Add(filtername[7]);
            if (sfilterp98) filters.Add(filtername[8]);
            if (sfilterpdiesel) filters.Add(filtername[9]);
            if (sfilterpgasohol) filters.Add(filtername[10]);
            filters.Add(sfiltercountryEnable ? countryitems[sfiltercountry] : sfiltercustomLocation ? "指定位置方圓" + sfilterKilonmeter + "公里內" : "目前位置方圓" + sfilterKilonmeter + "公里內");
            filtercount = filters.Count();
        }

        public async Task queryCustomLocation(IProgress<ProgressReport> messenger, string address)
        {
            geoQuery gq = new geoQuery();
            messenger.Report(new ProgressReport() { display = true, progress = 0, progressMessage = "查詢Google Map API中..." });
            GeoResponse gr = await gq.geoCoding(address);
            messenger.Report(new ProgressReport() { display = true, progress = 80, progressMessage = "查詢完成，輸出結果..." });
            try
            {
                if (gr.Status == "OK")
                {
                    messenger.Report(new ProgressReport() { display = true, progress = 100, progressMessage = "輸出完成！" });
                    centerloc = new GeoPoint(gr.Results[0].Geometry.Location.Lat, gr.Results[0].Geometry.Location.Lng);
                    sfilterlocation = new GeoPoint(gr.Results[0].Geometry.Location.Lat, gr.Results[0].Geometry.Location.Lng);
                    sfilterlocationname = gr.Results[0].formatted_address;
                    sfilterlocated = true;
                }
                else
                {
                    throw new gpsException();
                }
            }
            catch (jsonException je)
            {
                messenger.Report(new ProgressReport() { display = false, progress = 0, progressMessage = "查詢失敗" });
                throw je;
            }
            catch
            {
                messenger.Report(new ProgressReport() { display = false, progress = 0, progressMessage = "查詢失敗" });
                sfilterlocationname = "找不到指定位置";
                sfilterlocated = false;
            }
            messenger.Report(new ProgressReport() { display = false, progress = 100, progressMessage = "輸出完成！" });
        }
        public async Task queryCityStation(IProgress<ProgressReport> messenger)
        {
            foundcount = 0;
            CancellationToken token = _cts.Token;
            List<string> distlist = (from dist in stationDB where dist.city == countryitems[sfiltercountry] select dist.district).Distinct().ToList();
            //messenger.Report(new ProgressReport() { progress = 10, progressMessage = "查詢本機目前位置", display=true });
            //Geoposition currentloc = await _geolocator.GetGeopositionAsync().AsTask(token);   //8.1 code
            //currentloc = new GeoCoordinate(25.087651, 121.52201);  //temp location
            messenger.Report(new ProgressReport() { progress = 30, progressMessage = "載入加油站資料庫", display = true });
            /* Dynamic LINQ PCL 目前不存在，先算出宇集在根據條件算出差集，最後聯集儲存的站台，輸出距離 */
            queryStations.Clear();
            queryList = from sta in stationDB where sta.city == countryitems[sfiltercountry] select sta;
            if (sfilterSelf) queryList = queryList.Except(from sta in queryList where sta.selftype == 0 select sta);
            if (sfilterDirect) queryList = queryList.Except(from sta in queryList where !sta.type select sta);
            if (sfilterFPCC) queryList = queryList.Except(from sta in queryList where sta.brand == 1 select sta);
            if (sfilterCPC) queryList = queryList.Except(from sta in queryList where sta.brand == 0 select sta);
            if (sfilterp92) queryList = queryList.Where(sta => sta.p92);
            if (sfilterp95) queryList = queryList.Where(sta => sta.p95);
            if (sfilterp98) queryList = queryList.Where(sta => sta.p98);
            if (sfilterpdiesel) queryList = queryList.Where(sta => sta.pdiesel);
            if (sfilterpgasohol) queryList = queryList.Where(sta => sta.pgasohol);
            if (sfilterFavoirte) queryList = queryList.Union(from sta in stationDB where sta.favorite select sta);
            messenger.Report(new ProgressReport() { progress = 50, progressMessage = "查詢鄰近加油站", display = true });
            queryList = from sta in queryList
                        select new stationStorage()
                        {
                            address = sta.address,
                            brand = sta.brand,
                            favorite = sta.favorite,
                            distance = double.NaN,
                            cpcid = sta.cpcid,
                            latitude = sta.latitude,
                            longitude = sta.longitude,
                            name = sta.name,
                            phone = sta.phone,
                            selftype = sta.selftype,
                            type = sta.type,
                            current = false,
                            regular = true,
                            district = sta.district,
                            starttime = sta.starttime,
                            duration = sta.duration,
                            open = sta.duration == 0 ? false : DateTime.Now.Subtract(DateTime.Today.AddTicks(sta.starttime)).Ticks < sta.duration,
                            endtime = sta.duration == 0 ? DateTime.MinValue : sta.duration == 1 ? DateTime.MinValue.AddTicks(1) : new DateTime(sta.starttime).AddTicks(sta.duration)
                        };
            if (sfilterInservice) queryList = queryList.Except(from sta in queryList where !sta.open select sta);
            messenger.Report(new ProgressReport() { progress = 70, progressMessage = "過濾完成，輸出清單", display = true, });
            if(queryList.Any()) centerloc = queryList.First().coordinance;
            mapStations.Clear();
            if (sfilterFavoirte)
            {
                longlistCollection<stationStorage> item = new longlistCollection<stationStorage>();
                IEnumerable<stationStorage> slot = from s in queryList
                                                   where s.favorite
                                                   select s;
                int count = slot.Count();
                item.header = "儲存的加油站(" + count + ")";
                item.favorite = false;
                foundcount += count;
                foreach (stationStorage s in slot)
                {
                    s.regular = false;
                    item.Add(s);
                    mapStations.Add(s);
                }
                queryStations.Add(item);
            }
            for (int i = 0; i < distlist.Count(); i++)
            {
                IEnumerable<stationStorage> slot = sfilterFavoirte ?
                                                   from s in queryList
                                                   where s.district == distlist[i] && !s.favorite
                                                   select s :
                                                   from s in queryList
                                                   where s.district == distlist[i]
                                                   select s;
                int count = slot.Count();
                if (count == 0) continue;    //沒有內容就跳出
                foundcount += count;
                longlistCollection<stationStorage> item = new longlistCollection<stationStorage>();
                item.header = distlist[i] + "(" + count + ")";
                item.favorite = true;
                foreach (stationStorage s in slot)
                {
                    item.Add(s);
                    mapStations.Add(s);
                }
                queryStations.Add(item);
            }
            messenger.Report(new ProgressReport() { progress = 100, progressMessage = "查詢完成", display = false });
        }

        public async Task queryStation(GeoPoint loc, IProgress<ProgressReport> messenger)
        {
            if (sfiltercountryEnable)
            {
                await queryCityStation(messenger);
            }
            else
            {
                await queryDistStation(loc, messenger);
            }
        }

        public async Task queryDistStation(GeoPoint loc, IProgress<ProgressReport> messenger)
        {
            foundcount = 0;
            CancellationToken token = _cts.Token;
            centerloc = loc;
            //messenger.Report(new ProgressReport() { progress = 10, progressMessage = "查詢本機目前位置", display=true });
            //Geoposition currentloc = await _geolocator.GetGeopositionAsync().AsTask(token);   //8.1 code
            //currentloc = new GeoCoordinate(25.087651, 121.52201);  //temp location
            messenger.Report(new ProgressReport() { progress = 30, progressMessage = "載入加油站資料庫", display=true });
            /* Dynamic LINQ PCL 目前不存在，先算出宇集在根據條件算出差集，最後聯集儲存的站台，輸出距離 */
            queryStations.Clear();
            queryList = from sta in stationDB where this.GetDistanceTo(centerloc, new GeoPoint(sta.latitude,sta.longitude)) < (sfilterKilonmeter * 1000) select sta;
            if (sfilterSelf) queryList = queryList.Except(from sta in queryList where sta.selftype == 0 select sta);
            if (sfilterDirect) queryList = queryList.Except(from sta in queryList where !sta.type select sta);
            if (sfilterFPCC) queryList = queryList.Except(from sta in queryList where sta.brand == 1 select sta);
            if (sfilterCPC) queryList = queryList.Except(from sta in queryList where sta.brand == 0 select sta);
            if (sfilterp92) queryList = queryList.Where(sta => sta.p92);
            if (sfilterp95) queryList = queryList.Where(sta => sta.p95);
            if (sfilterp98) queryList = queryList.Where(sta => sta.p98);
            if (sfilterpdiesel) queryList = queryList.Where(sta => sta.pdiesel);
            if (sfilterpgasohol) queryList = queryList.Where(sta => sta.pgasohol);
            if (sfilterFavoirte) queryList = queryList.Union(from sta in stationDB where sta.favorite select sta);
            messenger.Report(new ProgressReport() { progress = 50, progressMessage = "查詢鄰近加油站", display=true });
            queryList = from sta in queryList
                        select new stationStorage()
                        {
                            address = sta.address,
                            brand = sta.brand,
                            favorite = sta.favorite,
                            distance = this.GetDistanceTo(centerloc, new GeoPoint(sta.latitude, sta.longitude)) / 1000,
                            cpcid = sta.cpcid,
                            latitude = sta.latitude,
                            longitude = sta.longitude,
                            name = sta.name,
                            phone = sta.phone,
                            selftype = sta.selftype,
                            type = sta.type,
                            current = false,
                            regular = true,
                            starttime = sta.starttime,
                            duration = sta.duration,
                            open = sta.duration == 0 ? false : DateTime.Now.Subtract(DateTime.Today.AddTicks(sta.starttime)).Ticks < 0 ? false : DateTime.Now.Subtract(DateTime.Today.AddTicks(sta.starttime)).Ticks < sta.duration,
                            endtime = sta.duration == 0 ? DateTime.MinValue : sta.duration == 1 ? DateTime.MinValue.AddTicks(1) : new DateTime(sta.starttime).AddTicks(sta.duration)
                        };
            if (sfilterInservice) queryList = queryList.Except(from sta in queryList where !sta.open select sta);
            messenger.Report(new ProgressReport() { progress = 70, progressMessage = "過濾完成，輸出清單", display=true, });
            mapStations.Clear();
            mapStations.Add(new stationStorage() { current = true, latitude=loc.Latitude, longitude = loc.Longitude, phone="0", regular= false });
            if (sfilterFavoirte)
            {
                longlistCollection<stationStorage> item = new longlistCollection<stationStorage>();
                IEnumerable<stationStorage> slot = from s in queryList
                                                    where s.favorite
                                                    orderby s.distance ascending
                                                    select s;
                int count = slot.Count();
                item.header = "儲存的加油站("+count+")";
                item.favorite = true;
                foundcount += count;
                foreach (stationStorage s in slot)
                {
                    s.regular = false;
                    item.Add(s);
                    mapStations.Add(s);
                }
                queryStations.Add(item);
            }
            for (int i = 1; i <= sfilterKilonmeter; i++)
            {
                IEnumerable<stationStorage> slot = sfilterFavoirte ?
                                                   from s in queryList
                                                   where s.distance > (i - 1) && s.distance < i && !s.favorite
                                                   orderby s.distance ascending
                                                   select s :
                                                   from s in queryList
                                                   where s.distance > (i - 1) && s.distance < i
                                                   orderby s.distance ascending
                                                   select s;
                int count = slot.Count();
                if (count == 0) continue;    //沒有內容就跳出
                foundcount += count;
                longlistCollection<stationStorage> item = new longlistCollection<stationStorage>();
                item.header = "方圓" + i + "公里內("+count+")";
                item.distance = i;
                item.favorite = false;
                foreach (stationStorage s in slot)
                {
                    item.Add(s);
                    mapStations.Add(s);
                }
                queryStations.Add(item);
            }
            messenger.Report(new ProgressReport() { progress = 100, progressMessage = "查詢完成", display=false });
        }

        public async Task saveStation(stationStorage s,IProgress<ProgressReport> messenger)
        {
            messenger.Report(new ProgressReport() { progress = 100, progressMessage = "儲存加油站完成", display = true });
            IEnumerable<stationStorage> dbs = (from sta in stationDB where sta.Equals(s) select sta);
            foreach (stationStorage d in dbs)
            {
                d.favorite = !d.favorite;
            }
            s.favorite = !s.favorite;
            await dbConn.UpdateAsync(s);
            messenger.Report(new ProgressReport() { progress = 100, progressMessage = "查詢完成", display = false });
        }

        public async Task updateCPC(IProgress<ProgressReport> messenger)
        {
            try
            {
                phoneDB = new phoneQuery();
                messenger.Report(new ProgressReport() { progress = 0, progressMessage = "分析能源局加油站列表", display = true });
                await phoneDB.loadStation();
                messenger.Report(new ProgressReport() { progress = 15, progressMessage = "開始下載中油加油站SOAP Service", display=true });
                stationSoap soap = new stationSoap();
                soap.pq = phoneDB;
                await soap.doWork();
                IEnumerable<stationStorage> onlineCPCStation = soap.cpcStation;
                IEnumerable<stationStorage> dbCPCStation = from station in stationDB where station.brand == 0 select station;
                messenger.Report(new ProgressReport() { progress = 30, progressMessage = "分析中油加油站資料庫...線上"+onlineCPCStation.Count()+"個", display=true });
                IEnumerable<stationStorage> removes = dbCPCStation.Except(onlineCPCStation);
                List<stationStorage> adds = onlineCPCStation.Except(dbCPCStation).ToList();
                messenger.Report(new ProgressReport() { progress = 40, progressMessage = "刪除過期中油加油站資料..."+removes.Count()+"個", display=true });
                foreach (stationStorage rd in removes)  //remove old data from local db
                {
                    await dbConn.DeleteAsync(rd);
                }
                messenger.Report(new ProgressReport() { progress = 50, progressMessage = "查詢營業時間與中油加油站座標中..."+adds.Count+"個", display=true });
                for (int i = 0; i < adds.Count; i++)
                {
                    geoQuery gq = new geoQuery();
                    adds[i] = await gq.geoCodeStation(adds[i]);
                    stationinfoSoap si = new stationinfoSoap(adds[i]);
                    await si.doWork();
                    adds[i] = si.qstation;
                }
                List<stationStorage> replaced = new List<stationStorage>();
                List<stationStorage> added = new List<stationStorage>();
                foreach (stationStorage ss in adds)
                {
                    IEnumerable<stationStorage> match = from sta in stationDB where sta.minimumEquals(ss) select sta;
                    if (match.Any())
                    {
                        IEnumerable<stationStorage> replace = from sta in match
                                                              where sta.address != ss.address
                                                              select new stationStorage()
                                                              {
                                                                  address = ss.address,
                                                                  brand = ss.brand,
                                                                  city = ss.city,
                                                                  district = ss.district,
                                                                  duration = ss.duration,
                                                                  favorite = ss.favorite,
                                                                  latitude = ss.latitude,
                                                                  longitude = ss.longitude,
                                                                  name = ss.name,
                                                                  phone = ss.phone,
                                                                  selftype = ss.selftype,
                                                                  sid = sta.sid,
                                                                  starttime = ss.starttime,
                                                                  type = ss.type
                                                              };
                        if (replace.Any()) replaced.Add(replace.First());
                    }
                    else
                    {
                        added.Add(ss);
                    }
                }
                messenger.Report(new ProgressReport() { progress = 100, progressMessage = "更新中油加油站..." + replaced.Count() + "筆", display = true });
                await dbConn.InsertOrReplaceAllAsync(replaced);
                messenger.Report(new ProgressReport() { progress = 100, progressMessage = "新增中油加油站..."+added.Count()+"筆", display = true });
                await dbConn.InsertAllAsync(added); 
                messenger.Report(new ProgressReport() { progress = 100, progressMessage = "中油加油站儲存完成！", display = false });
                stationDBdate = DateTime.Now;
                stationDBnotifyDate = DateTime.Now;
                stationDBcount = stationDB.Count();
            }
            catch (soapException se)
            {
                throw se;
            }
            catch (SQLiteException)
            {
                throw new dbException("儲存中油加油站");
            }
            catch (gpsException ge)
            {
                throw ge;
            }
        }

        public async Task updateFPCC(IProgress<ProgressReport> messenger)
        {
            messenger.Report(new ProgressReport() { progress = 0, progressMessage = "更新台塑加油站資料庫", display = true });
            try
            {
                List<stationStorage> onlineFPCCStation = new List<stationStorage>();
                int[] region = { 5, 6, 7, 8 };
                List<int[]> county = new List<int[]>() { new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }, new int[] { 1, 2, 3 }, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2 } };
                messenger.Report(new ProgressReport() { progress = 0, progressMessage = "更新台塑線上資料庫", display = true });
                for (int r = 0; r < region.Length; r++)
                {
                    messenger.Report(new ProgressReport() { progress = 0, progressMessage = "更新台塑線上資料庫...第" + (r+1) + "/" + region.Length + "個區域", display = true });
                    for (int c = 0; c < county[r].Length; c++)
                    {
                        onlineFPCCStation = onlineFPCCStation.Concat(await queryFPCCpage(region[r], county[r][c])).ToList();
                    }
                }
                messenger.Report(new ProgressReport() { progress = 30, progressMessage = "比對台塑加油站資料庫...線上"+onlineFPCCStation.Count+"個加油站", display = true });
                IEnumerable<stationStorage> dbFPCCStation = from station in stationDB where station.brand == 1 select station;
                IEnumerable<stationStorage> removes = dbFPCCStation.Except(onlineFPCCStation);
                List<stationStorage> adds = onlineFPCCStation.Except(dbFPCCStation).ToList();
                messenger.Report(new ProgressReport() { progress = 50, progressMessage = "刪除過期台塑加油站資料庫中..."+removes.Count()+"個", display = true });
                foreach (stationStorage rd in removes)  //remove old data from local db
                {
                    await dbConn.DeleteAsync(rd);
                }
                messenger.Report(new ProgressReport() { progress = 70, progressMessage = "查詢台塑加油站座標中..."+adds.Count+"個", display = true });
                for (int i = 0; i < adds.Count; i++)
                {
                    geoQuery gq = new geoQuery();
                    adds[i] = await gq.geoCodeStation(adds[i]);
                }
                List<stationStorage> replaced = new List<stationStorage>();
                List<stationStorage> added = new List<stationStorage>();
                foreach (stationStorage ss in adds)
                {
                    IEnumerable<stationStorage> match = from sta in stationDB where sta.minimumEquals(ss) select sta;
                    if (match.Any())
                    {
                        IEnumerable<stationStorage> replace = from sta in match
                                                              where sta.address != ss.address
                                                              select new stationStorage()
                                                              {
                                                                  address = ss.address,
                                                                  brand = ss.brand,
                                                                  city = ss.city,
                                                                  district = ss.district,
                                                                  duration = ss.duration,
                                                                  favorite = ss.favorite,
                                                                  latitude = ss.latitude,
                                                                  longitude = ss.longitude,
                                                                  name = ss.name,
                                                                  phone = ss.phone,
                                                                  selftype = ss.selftype,
                                                                  sid = sta.sid,
                                                                  starttime = ss.starttime,
                                                                  type = ss.type
                                                              };
                        if (replace.Any()) replaced.Add(replace.First());
                    }
                    else
                    {
                        added.Add(ss);
                    }
                } 
                messenger.Report(new ProgressReport() { progress = 80, progressMessage = "更新台塑加油站..." + replaced.Count() + "筆", display = true });
                await dbConn.InsertOrReplaceAllAsync(replaced);
                messenger.Report(new ProgressReport() { progress = 80, progressMessage = "新增台塑加油站..." + added.Count() + "筆", display = true });
                await dbConn.InsertAllAsync(added);
                messenger.Report(new ProgressReport() { progress = 100, progressMessage = "台塑加油站儲存完成！", display = false });
                stationDBdate = DateTime.Now;
                stationDBnotifyDate = DateTime.Now;
                stationDBcount = stationDB.Count();
                stationDB.Clear();
                stationDB = await dbConn.Table<stationStorage>().ToListAsync();
            }
            catch (gpsException ge)
            {
                throw ge;
            }
            catch (SQLiteException)
            {
                throw new dbException("儲存台塑加油站");
            }
            catch (xmlException xe)
            {
                throw xe;
            }
            catch (Exception)
            {
                throw new htmlException("擷取台塑加油站");
            }
        }

        private async Task<List<stationStorage>> queryFPCCpage(int region, int county)
        {
            try
            {
                List<stationStorage> fpccList = new List<stationStorage>();
                using (HttpClient hc = new HttpClient())
                {
                    var handler = new HttpClientHandler();
                    if (handler.SupportsAutomaticDecompression)
                    {
                        handler.AutomaticDecompression = DecompressionMethods.GZip |
                                                         DecompressionMethods.Deflate;
                    }
                    var httpClient = new HttpClient(handler);
                    httpClient.MaxResponseContentBufferSize = 256000;
                    httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
                    HtmlDocument tHtmlDoc = new HtmlDocument();
                    tHtmlDoc.LoadHtml(await httpClient.GetStringAsync(new Uri(Uri.EscapeUriString("http://www.fpcc.com.tw/tc/station_full.php?region=" + region + "&county=" + county))));
                    var attrSpan = from node in tHtmlDoc.DocumentNode.Descendants("a") where node.HasAttributes select node;
                    var nullSpan = from node in attrSpan where node.Attributes["href"] != null select node;
                    var tAllNodes = (from node in nullSpan where node.Attributes["href"].Value.Contains("station_full.php") select node).ToList();
                    int pages = tAllNodes.Count();
                    if (pages == 0)
                    {
                        fpccList = fpccList.Concat(await queryFPCCstation(region, county, 0)).ToList();
                    }
                    else
                    {
                        pages++;    //當前頁面的連結會消失，必須補一
                        for (int i = 1; i < pages; i++)
                        {
                            fpccList = fpccList.Concat(await queryFPCCstation(region, county, i)).ToList();
                        }
                    }
                    return fpccList;
                }
            }
            catch
            {
                throw new htmlException("擷取台塑加油站");
            }
        }

        private async Task<List<stationStorage>> queryFPCCstation(int region, int county, int page)
        {
            try
            {
                List<stationStorage> fpccList = new List<stationStorage>();
                using (HttpClient hc = new HttpClient())
                {
                    var handler = new HttpClientHandler();
                    if (handler.SupportsAutomaticDecompression)
                    {
                        handler.AutomaticDecompression = DecompressionMethods.GZip |
                                                         DecompressionMethods.Deflate;
                    }
                    var httpClient = new HttpClient(handler);
                    httpClient.MaxResponseContentBufferSize = 256000;
                    httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
                    HtmlDocument tHtmlDoc = new HtmlDocument();
                    string url = page != 0 ? "http://www.fpcc.com.tw/tc/station_full.php?region=" + region + "&county=" + county + "&page=" + page : "http://www.fpcc.com.tw/tc/station_full.php?region=" + region + "&county=" + county;
                    tHtmlDoc.LoadHtml(await httpClient.GetStringAsync(new Uri(Uri.EscapeUriString(url))));
                    var trAttr = from node in tHtmlDoc.DocumentNode.Descendants("tr") where node.HasAttributes select node;
                    var aligntr = from node in trAttr where node.Attributes["align"] != null select node;
                    var tr = (from station in aligntr
                              where station.Attributes["align"].Value == "left" && station.Descendants("td").Count() == 14
                              select new stationStorage()
                                  {
                                      address = station.ChildNodes[3].InnerText + station.ChildNodes[7].InnerText.Trim('\n').Trim(' ').Trim('\t').Trim('\n'),
                                      brand = 1,
                                      phone = station.ChildNodes[9].InnerText,
                                      name = station.ChildNodes[1].InnerText,
                                      selftype = station.ChildNodes[15].HasChildNodes ? 1 : station.ChildNodes[27].HasChildNodes ? 3 : 2,
                                      distance = 0,
                                      favorite = false,
                                      latitude = 0.0,
                                      longitude = 0.0,
                                      type = false,
                                      city = station.ChildNodes[3].InnerText,
                                      starttime = GetOpenTime(station.ChildNodes[11].InnerText),
                                      duration = GetDurationTime(station.ChildNodes[11].InnerText),
                                      p92 = station.ChildNodes[13].HasChildNodes,
                                      p95 = station.ChildNodes[17].HasChildNodes,
                                      p98 = station.ChildNodes[21].HasChildNodes,
                                      pdiesel = station.ChildNodes[25].HasChildNodes,
                                      pgasohol = false
                                  });
                    return tr.ToList();
                }
            }
            catch
            {
                throw new htmlException("擷取台塑加油站所在縣市");
            }
        }

        private long GetOpenTime(string time)
        {
            if (time == "24小時")
            {
                return 0;
            } else if (time == "暫停營業" || time == "整修中")
            {
                return 0;
            }
            else
            {
                char sep = time.Contains("-") ? '-' : '~';
                string[] starttime = time.Split(sep)[0].Split(':');
                string shour = time.Split(sep)[0].Length == 4 ? time.Split(sep)[0].Substring(0, 2) : starttime[0];
                string sminute = time.Split(sep)[0].Length == 4 ? time.Split(sep)[0].Substring(2, 2) : starttime[1];
                DateTime stime = new DateTime(1, 1, 1, Convert.ToInt32(shour), Convert.ToInt32(sminute), 0);
                return stime.Ticks;
            }
        }

        private long GetDurationTime(string time)
        {
            if (time == "24小時")
            {
                return 864000000000;
            } else if (time == "暫停營業" || time == "整修中")
            {
                return 0;
            }
            else
            {
                char sep = time.Contains("-") ? '-' : '~';
                string[] starttime = time.Split(sep)[0].Split(':');
                string shour = time.Split(sep)[0].Length == 4 ? time.Split(sep)[0].Substring(0, 2) : starttime[0];
                string sminute = time.Split(sep)[0].Length == 4 ? time.Split(sep)[0].Substring(2, 2) : starttime[1];
                DateTime stime = new DateTime(1, 1, 1, Convert.ToInt32(shour), Convert.ToInt32(sminute), 0);
                DateTime duration = DateTime.MinValue;
                if (starttime.Length > 1)
                {
                    string[] endtime = time.Split(sep)[1].Split(':');
                    string dhour = time.Split(sep)[1].Length == 4 ? time.Split(sep)[1].Substring(0, 2) : endtime[0];
                    dhour = dhour == "24" ? "0" : dhour;
                    string dminute = time.Split(sep)[1].Length == 4 ? time.Split(sep)[1].Substring(2, 2) : endtime[1];
                    duration = new DateTime(1, 1, 1, Convert.ToInt32(dhour), Convert.ToInt32(dminute), 0);
                }
                return duration.Subtract(stime).Ticks;
            }
        }

        private double GetDistanceTo(GeoPoint one, GeoPoint other)
        {
            if (double.IsNaN(one.Latitude) || double.IsNaN(one.Longitude) || double.IsNaN(other.Latitude) || double.IsNaN(other.Longitude))
            {
                throw new ArgumentException("Argument_LatitudeOrLongitudeIsNotANumber");
            }
            else
            {
                double latitude = one.Latitude * 0.0174532925199433;
                double longitude = one.Longitude * 0.0174532925199433;
                double num = other.Latitude * 0.0174532925199433;
                double longitude1 = other.Longitude * 0.0174532925199433;
                double num1 = longitude1 - longitude;
                double num2 = num - latitude;
                double num3 = Math.Pow(Math.Sin(num2 / 2), 2) + Math.Cos(latitude) * Math.Cos(num) * Math.Pow(Math.Sin(num1 / 2), 2);
                double num4 = 2 * Math.Atan2(Math.Sqrt(num3), Math.Sqrt(1 - num3));
                double num5 = 6376500 * num4;
                return num5;
            }
        }
    }
}
