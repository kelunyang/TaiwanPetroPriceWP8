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

namespace TaiwanPP.Library.ViewModel
{
    public class ppViewModel : viewmodelBase
    {
        HttpClient httpClient;
        HtmlDocument tHtmlDoc = new HtmlDocument();
        Uri URI = new Uri("http://web3.moeaboe.gov.tw/oil102/oil1022010/A00/Oil_Price2.asp");
        List<internationalModel> lio = new List<internationalModel>();
        DateTime cd = DateTime.Today;
        public double _pprice;
        public DateTime _pstartdate;
        public DateTime _penddate;
        public bool _loaded = false;
        public double _cprice;

        public double cprice
        {
            get
            {
                return _cprice;
            }
            set
            {
                _cprice = value;
                NotifyPropertyChanged();
            }
        }
        public double pprice
        {
            get
            {
                return _pprice;
            }
            set
            {
                _pprice = value;
                NotifyPropertyChanged();
            }
        }
        public DateTime pstartdate
        {
            get
            {
                return pstartdate;
            }
            set
            {
                _pstartdate = value;
                NotifyPropertyChanged();
            }
        }
        public DateTime penddate
        {
            get
            {
                return _penddate;
            }
            set
            {
                _penddate = value;
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
        public ppViewModel() { }

        public async Task predictedPrice(IProgress<ProgressReport> status)
        {
            List<HtmlNode> tAllNodes = new List<HtmlNode>();
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
                    status.Report(new ProgressReport() { progress = 15 * i, progressMessage = "下載資料中" });
                    HttpResponseMessage aResponse = await httpClient.PostAsync(URI, theContent);
                    tAllNodes.Concat(tHtmlDoc.DocumentNode.Descendants().Where(n => n.Name == "td").ToList());
                    for (int k = 12; k <= 18; k++)
                    {
                        lio.Add(new internationalModel(tAllNodes[k].InnerText, tAllNodes[k + 27].InnerText, tAllNodes[k + 18].InnerText, tAllNodes[k + 37].InnerText));
                    }
                }
                status.Report(new ProgressReport() { progress = 50.0, progressMessage = "分析數據中..." });
                IEnumerable<internationalModel> thisweek = from obj in lio where obj.tick > cd.AddDays((int)cd.DayOfWeek * -1).Ticks select obj;
                IEnumerable<internationalModel> pastweek = from obj in lio where obj.tick > cd.AddDays(((int)cd.DayOfWeek + 7) * -1).Ticks && obj.tick < cd.AddDays((int)cd.DayOfWeek * -1).Ticks select obj;
                if (thisweek.Count() > 0)
                {
                    weekpriceModel[] wp = new weekpriceModel[] { new weekpriceModel(), new weekpriceModel() };
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
                    double predictprice = Math.Round(cprice + cprice * (wp[0].price - wp[1].price) / wp[1].price * 0.8, 1);
                    this.pprice = Math.Round(predictprice - cprice, 1);
                    this.penddate = thisweek.Last().date;
                    this.pstartdate = thisweek.First().date;
                }
                else
                {
                    this.pprice = 0.0;
                    this.penddate = thisweek.Last().date;
                    this.pstartdate = thisweek.First().date;
                }
                loaded = true;
                status.Report(new ProgressReport() { progress = 100.0, progressMessage = "預測分析完成！" });
            }
        }
    }
}
