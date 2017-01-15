using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using HtmlAgilityPack;
using TaiwanPP.Library.Helpers;
using TaiwanPP.Library.Models;
using TaiwanPP.Library.ViewModels;
using SQLitePCL;
using SQLite;
using System.IO;
using System.Collections.ObjectModel;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using System.Xml;
using System.Collections.Specialized;
using System.ComponentModel;
using OxyPlot.Annotations;

namespace TaiwanPP.Library.ViewModels
{
    public class cpViewModel : viewmodelBase
    {
        HttpClient httpClient;
        priceStorage cp = new priceStorage();
        List<priceStorage> priceDB;
        DateTime _CPCcurrentDate = DateTime.MinValue;
        DateTime _FPCCcurrentDate = DateTime.MinValue;
        DateTime _FPCCsavedDate = DateTime.MinValue;
        DateTime _CPCsavedDate = DateTime.MinValue;
        double _CPC95Change = double.NaN;
        double _CPCdieselChange = double.NaN;
        double _FPCC95Change = double.NaN;
        double _FPCCdieselChange = double.NaN;
        int _defaultPeriod = 0;
        bool _chartready = false;
        public List<priceStorage> save;
        public List<int> tiles;
        public ObservableCollection<priceStorage> currentCollections { set; get; }
        public ObservableCollection<string> historicalPeriod { set; get; }
        public cpViewModel() {
            currentCollections = new ObservableCollection<priceStorage>();
            save = new List<priceStorage>();
            tiles = new List<int>();
            priceDB = new List<priceStorage>();
            historicalModel = new PlotModel();
            historicalPeriod = new ObservableCollection<string>();
            historicalPeriod.Add("最近十次（資料庫存檔）");
            historicalPeriod.Add("半年");
            historicalPeriod.Add("一年");
        }
        public int defaultPeroid
        {
            get { return _defaultPeriod; }
            set
            {
                _defaultPeriod = value;
                NotifyPropertyChanged();
            }
        }
        public bool chartready
        {
            set
            {
                _chartready = value;
                NotifyPropertyChanged();
            }
            get { return _chartready; }
        }
        public DateTime CPCsavedDate
        {
            get { return _CPCsavedDate; }
            set
            {
                _CPCsavedDate = value;
                NotifyPropertyChanged();
            }
        }
        public DateTime FPCCsavedDate
        {
            get { return _FPCCsavedDate; }
            set
            {
                _FPCCsavedDate = value;
                NotifyPropertyChanged();
            }

        }
        public DateTime CPCcurrentDate
        {
            get { return _CPCcurrentDate; }
            set 
            { 
                _CPCcurrentDate = value;
                NotifyPropertyChanged();
            }
        }
        public DateTime FPCCcurrentDate
        {
            get { return _FPCCcurrentDate; }
            set
            {
                _FPCCcurrentDate = value;
                NotifyPropertyChanged();
            }
        }
        public double CPC95Change
        {
            get { return _CPC95Change; }
            set
            {
                _CPC95Change = value;
                NotifyPropertyChanged();
            }
        }
        public double CPCdieselChange
        {
            get { return _CPCdieselChange; }
            set
            {
                _CPCdieselChange = value;
                NotifyPropertyChanged();
            }
        }
        public double FPCC95Change
        {
            get { return _FPCC95Change; }
            set
            {
                _FPCC95Change = value;
                NotifyPropertyChanged();
            }
        }
        public double FPCCdieselChange
        {
            get { return _FPCCdieselChange; }
            set
            {
                _FPCCdieselChange = value;
                NotifyPropertyChanged();
            }
        }
        DateTime _priceDBdate = DateTime.MinValue;
        public DateTime priceDBdate
        {
            get
            {
                _priceDBdate = im.priceDBdate;
                return _priceDBdate;
            }
            set
            {
                im.priceDBdate = value;
                NotifyPropertyChanged();
            }
        }
        DateTime _moeaboeDBdate = DateTime.MinValue;
        public DateTime moeaboeDBdate
        {
            get
            {
                _moeaboeDBdate = im.moeaboeDBdate;
                return _moeaboeDBdate;
            }
            set
            {
                im.moeaboeDBdate = value;
                NotifyPropertyChanged();
            }
        }
        bool _soapUpdate = true;
        public bool soapUpdate
        {
            get
            {
                _soapUpdate = im.soapupdate;
                return _soapUpdate;
            }
            set
            {
                im.soapupdate = value;
                NotifyPropertyChanged();
            }
        }
        priceStorage _historicalCurrent = new priceStorage() { price = double.NaN, datetick = 0 };
        priceStorage _historicalSaved = new priceStorage() { price = double.NaN, datetick = 0 };
        public priceStorage historicalCurrent
        {
            get { return _historicalCurrent; }
            set
            {
                _historicalCurrent = value;
                NotifyPropertyChanged();
            }
        }
        public priceStorage historicalSaved
        {
            get { return _historicalSaved; }
            set
            {
                _historicalSaved = value;
                NotifyPropertyChanged();
            }
        }
        double _savedchange = double.NaN;
        public double savedchange
        {
            get { return _savedchange; }
            set
            {
                _savedchange = value;
                NotifyPropertyChanged();
            }
        }
        double _maxprice = double.NaN;
        double _minprice = double.NaN;
        double _avgprice = double.NaN;
        double _predictprice = double.NaN;
        PlotModel _historicalmodel;
        public double maxPrice
        {
            get { return _maxprice; }
            set { 
                _maxprice = value;
                NotifyPropertyChanged();
            }
        }
        public double minPrice
        {
            get { return _minprice; }
            set
            {
                _minprice = value;
                NotifyPropertyChanged();
            }
        }
        public double avgPrice
        {
            get { return _avgprice; }
            set
            {
                _avgprice = value;
                NotifyPropertyChanged();
            }
        }
        public double predictPrice
        {
            get { return _predictprice; }
            set
            {
                _predictprice = value;
                NotifyPropertyChanged();
            }
        }
        List<int> _productMonitor = new List<int>();
        public List<int> productMonitor
        {
            set
            {
                im.productMonitor = value;
                NotifyPropertyChanged();
            }
            get
            {
                _productMonitor = im.productMonitor;
                return _productMonitor;
            }
        }
        int _monitorcount = 0;
        public int monitorcount
        {
            set
            {
                _monitorcount = value;
                NotifyPropertyChanged();
            }
            get { return _monitorcount; }
        }
        int _currentcount = 0;
        public int currentcount
        {
            set
            {
                _currentcount = value;
                NotifyPropertyChanged();
            }
            get { return _currentcount; }
        }
        int _savedcount = 0;
        public int savedcount
        {
            set
            {
                _savedcount = value;
                NotifyPropertyChanged();
            }
            get { return _savedcount; }
        }
        public PlotModel historicalModel
        {
            get { return _historicalmodel; }
            set
            {
                _historicalmodel = value;
                NotifyPropertyChanged();
            }
        }
        DateTimeAxis _dtx;
        public DateTimeAxis dtx
        {
            get { return _dtx; }
            set
            {
                _dtx = value;
                NotifyPropertyChanged();
            }
        }
        LinearAxis _la;
        public LinearAxis la
        {
            get { return _la; }
            set
            {
                _la = value;
                NotifyPropertyChanged();
            }
        }
        LineSeries _prices;
        public LineSeries prices
        {
            get { return _prices; }
            set
            {
                _prices = value;
                NotifyPropertyChanged();
            }
        }
        LineSeries _avgs;
        public LineSeries avgs
        {
            get { return _avgs; }
            set
            {
                _avgs = value;
                NotifyPropertyChanged();
            }
        }
        LineSeries _saveds;
        public LineSeries saveds
        {
            get { return _saveds; }
            set
            {
                _saveds = value;
                NotifyPropertyChanged();
            }
        }
        public async Task monitorProduct(priceStorage storage, IProgress<ProgressReport> messenger, bool temp)
        {
            messenger.Report(new ProgressReport() { progress = 0, progressMessage = "將油品加入觀測清單中", display = true });
            IEnumerable<priceStorage> items = from p in priceDB where p.datetick == storage.datetick && p.kind == storage.kind select p;
            List<int> monitors = productMonitor.ToList();
            if (monitors.Contains(storage.kind))
            {
                if (!temp) monitors.Remove(storage.kind);
                foreach (priceStorage p in items)
                {
                    p.monitored = false;
                }
            }
            else
            {
                if (!temp) monitors.Add(storage.kind);
                foreach (priceStorage p in items)
                {
                    p.monitored = true;
                }
            }
            productMonitor = monitors;
            await currentPrice(messenger,false);
            messenger.Report(new ProgressReport() { progress = 100, progressMessage = "查詢完成", display = false });
        }

        public async Task savePrice(IProgress<ProgressReport> messenger)
        {
            long cpcsaveddate = (from d in priceDB where d.brand == 0 orderby d.datetick descending select d.datetick).First();
            long fpccsaveddate = (from d in priceDB where d.brand == 1 orderby d.datetick descending select d.datetick).First();
            IEnumerable<priceStorage> cpcsaved = from d in priceDB where d.brand == 0 && d.datetick == cpcsaveddate select d;
            IEnumerable<priceStorage> fpccsaved = from d in priceDB where d.brand == 1 && d.datetick == fpccsaveddate select d;
            IEnumerable<priceStorage> dbsaved = from d in priceDB where d.saved select d;
            messenger.Report(new ProgressReport() { progress = 0, progressMessage = "儲存目前油價", display = true });
            IEnumerable<priceStorage> restorelist = from p in priceDB where p.saved select p;
            foreach (priceStorage p in dbsaved)
            {
                p.saved = false;
                await dbConn.UpdateAsync(p);
            }
            foreach (priceStorage p in cpcsaved)
            {
                p.saved = true;
                await dbConn.UpdateAsync(p);
            }
            foreach (priceStorage p in fpccsaved)
            {
                p.saved = true;
                await dbConn.UpdateAsync(p);
            }
            messenger.Report(new ProgressReport() { progress = 100, progressMessage = "儲存完成！", display = false });
        }
        public async Task currentPrice(IProgress<ProgressReport> messenger, bool background)
        {
            messenger.Report(new ProgressReport() { progress = 60, progressMessage = "分析油價公告", display = true });
            List<priceStorage> cpc95 = (from oil in priceDB where oil.brand == 0 && oil.kind == typeDB.CPC95.key orderby oil.datetick descending select oil).Distinct().Take(2).ToList();
            List<priceStorage> cpcdiesel = (from oil in priceDB where oil.brand == 0 && oil.kind == typeDB.CPCdiesel.key orderby oil.datetick descending select oil).Distinct().Take(2).ToList();
            List<priceStorage> fpcc95 = (from oil in priceDB where oil.brand == 1 && oil.kind == typeDB.FPCC95.key orderby oil.datetick descending select oil).Take(2).Distinct().ToList();
            List<priceStorage> fpccdiesel = (from oil in priceDB where oil.brand == 1 && oil.kind == typeDB.FPCCdiesel.key orderby oil.datetick descending select oil).Distinct().Take(2).ToList();
            List<priceStorage> fpccsaved = (from oil in priceDB where oil.brand == 1 && oil.saved orderby oil.datetick descending select oil).Distinct().Take(1).ToList();
            List<priceStorage> cpcsaved = (from oil in priceDB where oil.brand == 0 && oil.saved orderby oil.datetick descending select oil).Distinct().Take(1).ToList();
            int[] brandarr = (from oil in priceDB where oil.saved select oil.brand).Take(1).ToArray();
            if (cpc95.Any())  //in case of there is no data
            {
                CPC95Change = Math.Round(cpc95[0].price - cpc95[1].price, 1);
                FPCC95Change = Math.Round(fpcc95[0].price - fpcc95[1].price, 1);
                CPCdieselChange = Math.Round(cpcdiesel[0].price - cpcdiesel[1].price, 1);
                FPCCdieselChange = Math.Round(fpccdiesel[0].price - fpccdiesel[1].price, 1);
                CPCcurrentDate = new DateTime(cpc95[0].datetick);
                FPCCcurrentDate = new DateTime(fpcc95[0].datetick);
                if (brandarr.Length > 0)
                {
                    CPCsavedDate = new DateTime(cpcsaved[0].datetick);
                    FPCCsavedDate = new DateTime(fpccsaved[0].datetick);
                }
                messenger.Report(new ProgressReport() { progress = 90, progressMessage = "擷取目前油價", display = true });
                List<int> cpcproductlist = new List<int>() { 0, 1, 2, 3, 4 };
                IEnumerable<priceStorage> cpc = (from oil in priceDB
                                                 orderby oil.datetick descending
                                                 where cpcproductlist.Contains(oil.kind)
                                                 select oil).Distinct().Take(cpcproductlist.Count());
                IEnumerable<priceStorage> fpcc = (from oil in priceDB
                                                  orderby oil.datetick descending
                                                  where oil.brand == 1
                                                  select oil).Distinct().Take(4);
                IEnumerable<priceStorage> saved = from oil in priceDB
                                                  orderby oil.datetick descending
                                                  where oil.saved
                                                  select oil;
                IEnumerable<priceStorage> templist = cpc.Union(fpcc);
                foreach (priceStorage cu in templist)
                {
                    cu.current = true;
                    cu.tile = false;
                }
                templist = templist.Union(saved);
                currentCollections.Clear();
                foreach (priceStorage mo in templist)   //初始化時掃描，一般狀態下資料庫都已記錄monitored狀態
                {
                    mo.vis = true;
                    if (!background)
                    {
                        if (productMonitor.Contains(mo.kind)) mo.monitored = true;
                        if (tiles.Contains(mo.kind)) mo.tile = true;    //動態磚就自動加入觀察列表
                        if (tiles.Contains(mo.kind)) mo.monitored = true;
                        if (saved.Contains(mo)) mo.saved = true;
                        if (!soapUpdate) mo.vis = mo.kind == 0 ? false : true;
                    }
                    currentCollections.Add(mo);
                }
                IEnumerable<priceStorage> duplicates = templist.GroupBy(i => i.kind).SelectMany(grp => grp.Skip(1));
                foreach (priceStorage du in duplicates)
                {
                    du.vis = false;
                }
                currentcount = currentCollections.Count();
                monitorcount = (from m in currentCollections where m.monitored select m).Count();
                savedcount = saved.Count();
            }
            messenger.Report(new ProgressReport() { progress = 100, progressMessage = "擷取目前油價", display = false });
        }
        public void setupHistorical()
        {
            historicalModel = new PlotModel();
            dtx = new DateTimeAxis() { Position = AxisPosition.Bottom, Angle = 70 };
            la = new LinearAxis() { Position = AxisPosition.Left };
            saveds = new LineSeries();
            saveds.Title = "儲存的價格";
            saveds.Color = OxyColor.FromRgb(200, 200, 200);
            prices = new LineSeries();
            prices.Title = "價格";
            prices.Color = OxyColor.FromRgb(212, 61, 61);
            prices.StrokeThickness = 2;
            prices.MarkerFill = OxyColor.FromRgb(200, 56, 56);
            prices.MarkerType = MarkerType.Circle;
            prices.MarkerSize = 5;
            prices.BrokenLineColor = OxyColor.FromRgb(212, 61, 61);
            prices.BrokenLineStyle = LineStyle.Dot;
            prices.BrokenLineThickness = 2;
            prices.Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(0)),0));
            avgs = new LineSeries();
            avgs.Color = OxyColor.FromRgb(76, 134, 203);
            avgs.StrokeThickness = 1;
            avgs.Title = "平均價";
            avgs.BrokenLineColor = OxyColor.FromRgb(76, 134, 203);
            avgs.BrokenLineStyle = LineStyle.Dot;
            avgs.BrokenLineThickness = 1;
            avgs.Points.Add(new DataPoint(0, 0));
            historicalModel.Series.Add(prices);
            historicalModel.Series.Add(avgs);
            historicalModel.Series.Add(saveds);
            historicalModel.Axes.Add(dtx);
            historicalModel.Axes.Add(la);
        }
        public void historicalPrice(double predictPrice, bool predictpause, string productid, IProgress<ProgressReport> messenger)
        {
            chartready = false;
            messenger.Report(new ProgressReport() { progress = 0, progressMessage = "擷取歷史油價資料庫", display = true });
            int id = Convert.ToInt32(productid);
            historicalCurrent = (from p in priceDB where p.kind == id orderby p.datetick descending select p).First();
            IEnumerable<priceStorage> tempsaved = from p in priceDB where p.kind == id && p.saved select p;
            historicalSaved = tempsaved.Any() ? tempsaved.First() : new priceStorage() { datetick = 0, price = double.NaN };
            savedchange = tempsaved.Any()  ? historicalCurrent.price - historicalSaved.price : double.NaN;
            IEnumerable<priceStorage> historical = (from oil in priceDB where oil.kind == id orderby oil.datetick ascending select oil).Distinct();
            messenger.Report(new ProgressReport() { progress = 30, progressMessage = "計算平均價格", display = true });
            maxPrice = historical.Max(x => x.price);
            minPrice = historical.Min(x => x.price);
            double maxDate = !predictpause ? DateTimeAxis.ToDouble(new DateTime(historical.Max(x => x.datetick)).AddDays(7)) : DateTimeAxis.ToDouble(new DateTime(historical.Max(x => x.datetick)));
            double minDate = DateTimeAxis.ToDouble(new DateTime(historical.Min(x => x.datetick)));
            double maxAxis = maxPrice;;
            double minAxis = minPrice;;
            if (!predictpause)
            {
                if (historical.Any())
                {
                    predictPrice += historical.Last().price;
                    this.predictPrice = predictPrice;
                    maxAxis = maxPrice > predictPrice ? maxPrice : predictPrice;
                    minAxis = minPrice < predictPrice ? minPrice : predictPrice;
                }
            }
            avgPrice = historical.Average(x => x.price);
            messenger.Report(new ProgressReport() { progress = 60, progressMessage = "繪製價格圖表", display = true });
            /*PlotModel tempModel = new PlotModel();
            tempModel.Axes.Add(new DateTimeAxis() { Position = AxisPosition.Bottom, Angle = 70, MajorStep = defaultPeroid == 0 ? 7 : 28, Maximum = maxDate, Minimum = minDate });
            tempModel.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Maximum = maxAxis, Minimum = minAxis });*/
            la.Maximum = maxAxis;
            la.Minimum = minAxis;
            dtx.Maximum = maxDate;
            dtx.Minimum = minDate;
            prices.Points.Clear();
            avgs.Points.Clear();
            foreach (priceStorage ps in historical)
            {
                double d = DateTimeAxis.ToDouble(new DateTime(ps.datetick));
                prices.Points.Add(new DataPoint(d, ps.price));
            }
            avgs.Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(historical.Min(x => x.datetick))), avgPrice));
            avgs.Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(historical.Max(x => x.datetick))), avgPrice));
            if (!predictpause)
            {
                prices.Points.Add(DataPoint.Undefined);
                prices.Points.Add(new DataPoint(maxDate, predictPrice));
                avgs.Points.Add(DataPoint.Undefined);
                avgs.Points.Add(new DataPoint(maxDate, avgPrice));
            }
            if (tempsaved.Any())
            {
                saveds.Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(tempsaved.First().datetick)), prices.Points.Select(x => x.Y).Max()));
                saveds.Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(tempsaved.First().datetick)), prices.Points.Where(x=> !double.IsNaN(x.Y)).Select(x => x.Y).Min()));
            }
            else
            {
                historicalModel.Series.Remove(saveds);
            }
            messenger.Report(new ProgressReport() { progress = 100, progressMessage = "歷史資料庫計算完成！", display = false });
            chartready = true;
        }
        public async Task buildDB()
        {
            try
            {
                priceDB = await dbConn.Table<priceStorage>().OrderBy(e => e.datetick).ToListAsync();
            }
            catch
            {
                throw new dbException("取得最新價格");
            }
        }
        /*public async Task clearDB()
        {
            try
            {
                await dbConn.DeleteAsync<priceStorage>();
            }
            catch
            {
                throw new dbException("清理價格資料庫");
            }
        }*/
        public async Task fetchPrice(bool connectivity, IProgress<ProgressReport> messenger)
        {
            List<priceStorage> tempDB = new List<priceStorage>();
            messenger.Report(new ProgressReport() { progress = 0, progressMessage = "擷取能源局油價資料庫", display = true });
            if (connectivity)
            {
                try
                {
                    int fetchnumber = defaultPeroid == 0 ? 10 : defaultPeroid == 1 ? 25 : 50;
                    List<HtmlNode> tAllNodes = new List<HtmlNode>();
                    var handler = new HttpClientHandler();
                    if (handler.SupportsAutomaticDecompression)
                    {
                        handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                    }
                    using (httpClient = new HttpClient(handler))
                    {
                        messenger.Report(new ProgressReport() { progress = 30, progressMessage = "開始下載油價公告", display = true });
                        httpClient.MaxResponseContentBufferSize = Int32.MaxValue;
                        httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
                        var byteData = await httpClient.GetByteArrayAsync(new Uri("https://www2.moeaboe.gov.tw/oil102/oil1022010/A01/A0108/allprices.asp?nocache=" + DateTime.Now.Ticks));
                        string data = Portable.Text.Encoding.GetEncoding(950).GetString(byteData);  //big5
                        //string data = await httpClient.GetStringAsync(new Uri("http://www.nyko.com.tw/price.htm?nocache=" + DateTime.Now.Ticks)); test code
                        HtmlDocument tHtmlDoc = new HtmlDocument();
                        tHtmlDoc.LoadHtml(data);
                        var allnode = (from node in tHtmlDoc.DocumentNode.Descendants("tr") where node.HasAttributes select node).ToList();
                        tAllNodes = tAllNodes.Concat((from node in allnode where node.Attributes["bgColor"].Value.Equals("#ffffcc") select node).Take(fetchnumber).ToList()).ToList();
                        tAllNodes = tAllNodes.Concat((from node in allnode where node.Attributes["bgColor"].Value.Equals("#E6FFE6") select node).Take(fetchnumber).ToList()).ToList();
                        List<oildata> oilarr = new List<oildata>();
                        tempDB.Clear();
                        messenger.Report(new ProgressReport() { progress = 60, progressMessage = "更新中油油價SOAP Service", display = true });
                        if (soapUpdate)
                        {
                            priceSoap gassoap = new priceSoap();
                            await gassoap.doWork();
                            if (gassoap.loaded)
                            {
                                tempDB.Add(new priceStorage() { brand = typeDB.CPCgasohol.brand, price = gassoap.price, datetick = gassoap.date.Ticks, kind = typeDB.CPCgasohol.key, saved = false });
                                tempDB.First().id = Convert.ToInt32(gassoap.date.ToString("yyMMddHH") + typeDB.CPCgasohol.key);
                            }
                            else
                            {
                                messenger.Report(new ProgressReport() { progress = 60, progressMessage = "中油SOAP Service服務異常，系統將不抓取酒精汽油油價，請聯絡開發者", display = true });
                            }
                        }
                        foreach (HtmlNode hn in tAllNodes)
                        {
                            int[] products = { 3, 2, 1, 4 };
                            for (int i = 1; i <= products.Length; i++)
                            {
                                int brand = hn.ChildNodes[0].InnerText == "台塑石化" ? 1 : 0;
                                int kind = brand == 0 ? i : i+4;
                                priceStorage ps = new priceStorage() { brand = brand, datetick = (DateTime.Parse(hn.ChildNodes[6].InnerText.Remove(hn.ChildNodes[6].InnerText.IndexOf("日")))).AddHours(Convert.ToDouble(hn.ChildNodes[7].InnerText.Remove(hn.ChildNodes[7].InnerText.IndexOf("時")))).Ticks, kind = kind, price = Convert.ToDouble(hn.ChildNodes[products[i - 1]].InnerText), saved = false };
                                ps.id = Convert.ToInt32((new DateTime(ps.datetick)).ToString("yyMMddHH") + ps.kind);
                                tempDB.Add(ps);
                            }
                        }
                    }
                    bool update = false;    //資料重複就不更新
                    IEnumerable<priceStorage> newdb = from ps in tempDB orderby ps.datetick descending select ps;
                    IEnumerable<priceStorage> olddb = from ps in priceDB orderby ps.datetick descending select ps;
                    save = newdb.Except(olddb).ToList();
                    IEnumerable<priceStorage> remove = olddb.Except(newdb);
                    if (!priceDB.Any())
                    {
                        update = true;
                    }
                    if (!update)
                    {
                        if (save.Any()) update = true;
                    }
                    if (update)
                    {
                        messenger.Report(new ProgressReport() { progress = 80, progressMessage = "更新本機資料庫", display = true });
                        List<priceStorage> saved = (from item in priceDB where item.saved select item).ToList();
                        tempDB = saved.Union(tempDB).ToList();
                        if (defaultPeroid == 0)
                        {
                            foreach (priceStorage ps in remove)
                            {
                                await dbConn.DeleteAsync(ps);
                            }
                            await dbConn.InsertAllAsync(save);  //錯在此
                        }
                        priceDB.Clear();
                        foreach (priceStorage ps in tempDB)
                        {
                            priceDB.Add(ps);
                        }
                        if (save.Count() != 1) moeaboeDBdate = DateTime.Now;
                        priceDBdate = DateTime.Now;
                    }
                    messenger.Report(new ProgressReport() { progress = 100, progressMessage = "儲存設定中", display = false });
                }
                catch(xmlException e)
                {
                    throw e;
                }
                catch (dbException e)
                {
                    throw e;
                } catch(HttpRequestException e) {
                    throw e;
                } catch(Exception) {
                    throw new systemException("擷取當期油價");
                }
            }
        }
        public tileViewModel tileExport(oilType product, DateTime pEndd, DateTime pStartd, double pPrice, double pdPrice)
        {
            priceStorage item = (from p in priceDB where p.kind == product.key orderby p.datetick descending select p).First();
            double changePrice = double.NaN;
            if (product.key == typeDB.CPCdiesel.key || product.key == typeDB.FPCCdiesel.key) changePrice = product.key == typeDB.FPCCdiesel.key ? FPCCdieselChange : CPCdieselChange;
            if (double.IsNaN(changePrice)) changePrice = product.brand == typeDB.CPCgasohol.brand ? CPC95Change : FPCC95Change;
            double predictPrice = product.key == typeDB.CPCdiesel.key ? pdPrice : pPrice;
            return new tileViewModel() { brand = product.brand, price = item.price, change = changePrice, uptime = new DateTime(item.datetick), visibility = product.key > 4 ? product.key - 4 : product.key, pEndd = pEndd, pStartd = pStartd, pPrice = predictPrice };
        }
        void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged();
        }
    }
}
