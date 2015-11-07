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
using System.IO;
using System.Xml;

namespace TaiwanPP.Library.ViewModels
{
    public class ppViewModel : viewmodelBase
    {
        HttpClient httpClient;
        HtmlDocument tHtmlDoc = new HtmlDocument();
        Uri URI = new Uri("http://web3.moeaboe.gov.tw/oil102/oil1022010/A00/Oil_Price2.asp");
        List<internationalModel> lio = new List<internationalModel>();
        DateTime cd = DateTime.Now;
        double _pprice = double.NaN;
        DateTime _pstartdate = DateTime.MinValue;
        DateTime _penddate = DateTime.MinValue;
        public bool _loaded = false;
        bool _predictpause = true;
        weekpriceModel[] wp;
        bool connectivity;
        public bool predictpause
        {
            get
            {
                _predictpause = im.ppause;
                return _predictpause;
            }
            set
            {
                im.ppause = value;
                NotifyPropertyChanged();
            }
        }
        public double pprice
        {
            get
            {
                if (double.IsNaN(_pprice)) return double.NaN;
                _pprice = im.predictgasPrice;
                return _pprice;
            }
            set
            {
                _pprice = value;
                im.predictgasPrice = value;
                NotifyPropertyChanged();
            }
        }
        double _pdprice = double.NaN;
        public double pdprice
        {
            get
            {
                if (double.IsNaN(_pdprice)) return double.NaN;
                _pdprice = im.predictdieselPrice;
                return _pdprice;
            }
            set
            {
                _pdprice = value;
                im.predictdieselPrice = value;
                NotifyPropertyChanged();
            }
        }
        public DateTime pstartdate
        {
            get
            {
                if (_pstartdate == DateTime.MinValue) return _pstartdate;
                _pstartdate = im.pstartWeek;
                return _pstartdate;
            }
            set
            {
                _pstartdate = value;
                im.pstartWeek = value;
                NotifyPropertyChanged();
            }
        }
        DateTime _prunday = DateTime.MinValue;
        public DateTime prunday
        {
            get 
            {
                if (_prunday == DateTime.MinValue) return _prunday;
                _prunday = im.prunDay;
                return _prunday;
            }
            set
            {
                _prunday = value;
                im.prunDay = value;
                NotifyPropertyChanged();
            }
        }
        public DateTime penddate
        {
            get
            {
                if (_penddate == DateTime.MinValue) return _penddate;
                _penddate = im.pendWeek;
                return _penddate;
            }
            set
            {
                _penddate = value;
                im.pendWeek = value;
                NotifyPropertyChanged();
            }
        }
        public bool loaded
        {
            get
            {
                return _loaded;
            }
            set
            {
                _loaded = value;
                NotifyPropertyChanged();
            }
        }
        double _pastbrent = double.NaN;
        public double pastbrent
        {
            get { return _pastbrent; }
            set
            {
                _pastbrent = value;
                NotifyPropertyChanged();
            }
        }
        double _pastdubai = double.NaN;
        public double pastdubai
        {
            get { return _pastdubai; }
            set
            {
                _pastdubai = value;
                NotifyPropertyChanged();
            }
        }
        double _pastcurrency = double.NaN;
        public double pastcurrency
        {
            get { return _pastcurrency; }
            set
            {
                _pastcurrency = value;
                NotifyPropertyChanged();
            }
        }
        double _currentdubai = double.NaN;
        public double currentdubai
        {
            get { return _currentdubai; }
            set
            {
                _currentdubai = value;
                NotifyPropertyChanged();
            }
        }
        double _currentbrent = double.NaN;
        public double currentbrent
        {
            get { return _currentbrent; }
            set
            {
                _currentbrent = value;
                NotifyPropertyChanged();
            }
        }
        double _currentcurrency = double.NaN;
        public double currentcurrency
        {
            get { return _currentcurrency; }
            set
            {
                _currentcurrency = value;
                NotifyPropertyChanged();
            }
        }
        bool _runPredict = true;
        public bool runPredict
        {
            get
            {
                _runPredict = im.runPredict;
                return _runPredict;
            }
            set
            {
                im.runPredict = value;
                NotifyPropertyChanged();
            }
        }
        public ppViewModel() {
            wp = new weekpriceModel[] { new weekpriceModel(), new weekpriceModel() };
        }
        public override async Task loadDB(SQLite.Net.Interop.ISQLitePlatform platform, string dbPath)
        {
 	        await base.loadDB(platform, dbPath);
            if (im.pstartWeek != DateTime.MinValue)
            {
                pstartdate = im.pstartWeek;
                penddate = im.pendWeek;
                prunday = im.prunDay;
                pprice = im.predictgasPrice;
                pdprice = im.predictdieselPrice;
            }
        }

        public async Task predictedPrice(bool connectivity, bool predictenable, IProgress<ProgressReport> messenger)
        {
            if (predictenable)
            {
                this.connectivity = connectivity;
                if (connectivity)
                {
                    messenger.Report(new ProgressReport() { progress = 0, progressMessage = "計算預測油價", display = true });
                    lio = new List<internationalModel>();
                    try
                    {
                        List<HtmlNode> tAllNodes;
                        var handler = new HttpClientHandler();
                        if (handler.SupportsAutomaticDecompression)
                        {
                            handler.AutomaticDecompression = DecompressionMethods.GZip |
                                                             DecompressionMethods.Deflate;
                        }
                        using (httpClient = new HttpClient(handler))
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                DateTime day = i == 0 ? cd : lio.First().date.AddDays(-1);
                                string body = "date_type=3&year1=2014&year2=2014&month2=3&year=" + day.Year + "&month=" + day.Month + "&date=" + day.Day + "&submit=About+submit+buttons&ttype=1";
                                StringContent theContent = new StringContent(body, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
                                messenger.Report(new ProgressReport() { progress = 15 * i, progressMessage = "下載能源局油價資料中", display = true });
                                HttpResponseMessage aResponse = await httpClient.PostAsync(URI, theContent);
                                tHtmlDoc.LoadHtml(await aResponse.Content.ReadAsStringAsync());
                                tAllNodes = tHtmlDoc.DocumentNode.Descendants("td").ToList();
                                for (int k = 12; k <= 18; k++)
                                {
                                    lio.Add(new internationalModel(tAllNodes[k].InnerText, tAllNodes[k + 27].InnerText, tAllNodes[k + 18].InnerText, tAllNodes[k + 37].InnerText));
                                }
                            }
                            messenger.Report(new ProgressReport() { progress = 50.0, progressMessage = "分析數據中...", display = true });
                            IEnumerable<internationalModel> thisweek = from obj in lio where obj.tick > cd.AddDays((int)cd.DayOfWeek * -1).Ticks select obj;
                            IEnumerable<internationalModel> pastweek = from obj in lio where obj.tick > cd.AddDays(((int)cd.DayOfWeek + 7) * -1).Ticks && obj.tick < cd.AddDays((int)cd.DayOfWeek * -1).Ticks select obj;
                            if (thisweek.Any())
                            {
                                wp = new weekpriceModel[] { new weekpriceModel(), new weekpriceModel() };
                                foreach (internationalModel io in thisweek)
                                {
                                    wp[0].dubai += io.dubai;
                                    wp[0].brent += io.brent;
                                    wp[0].currency += io.currency;
                                    wp[0].cday++;
                                }
                                foreach (internationalModel io in pastweek)
                                {
                                    wp[1].dubai += io.dubai;
                                    wp[1].brent += io.brent;
                                    wp[1].currency += io.currency;
                                    wp[1].cday++;
                                }
                                pastbrent = wp[1].avgbrent;
                                pastdubai = wp[1].avgdubai;
                                pastcurrency = wp[1].avgcurrency;
                                currentbrent = wp[0].avgbrent;
                                currentdubai = wp[0].avgdubai;
                                currentcurrency = wp[0].avgcurrency;
                                this.penddate = thisweek.Last().date;
                                this.pstartdate = thisweek.First().date;
                                this.predictpause = false;
                            }
                            else
                            {
                                this.pprice = double.NaN;
                                this.penddate = DateTime.Now;
                                this.pstartdate = DateTime.Now;
                                this.predictpause = true;
                            }
                            loaded = true;
                            prunday = DateTime.Now;
                            messenger.Report(new ProgressReport() { progress = 100.0, progressMessage = "預測分析完成！", display = true });
                        }
                    }
                    catch
                    {
                        throw new htmlException("分析歷史油價");
                    }
                    messenger.Report(new ProgressReport() { progress = 100, progressMessage = "預測分析完成！", display = false });
                }
                else
                {
                    predictpause = (int)DateTime.Now.DayOfWeek == 0 || (int)DateTime.Now.DayOfWeek == 1;
                    loaded = true;
                }
            }
        }

        public void getPrice(double cpc95price, double cpcdieselprice)
        {
            if (connectivity)
            {
                double predictgasprice = Math.Round(cpc95price + cpc95price * (wp[0].price - wp[1].price) / wp[1].price * 0.8, 1);
                double predictdieselprice = Math.Round(cpcdieselprice + cpcdieselprice * (wp[0].price - wp[1].price) / wp[1].price * 0.8, 1);
                this.pdprice = Math.Round(predictdieselprice - cpcdieselprice, 1);
                this.pprice = Math.Round(predictgasprice - cpc95price, 1);
            }
        }
    }
}
