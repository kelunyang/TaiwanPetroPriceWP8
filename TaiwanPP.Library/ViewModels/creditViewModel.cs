using HtmlAgilityPack;
using Newtonsoft.Json;
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
using System.Xml.Linq;
using TaiwanPP.Library.Helpers;
using TaiwanPP.Library.Models;

namespace TaiwanPP.Library.ViewModels
{
    public class discountViewModel : viewmodelBase
    {
        private List<discountStorage> discountDB;
        public void loadConfig(Stream str)
        {
            im.load(str);
            str.Dispose();
        }

        public ObservableCollection<discountStorage> queryDiscounts { get; set; }
        public ObservableCollection<discountStorage> monitorDiscounts { get; set; }
        public ObservableCollection<string> sourceList { get; set; }
        public ObservableCollection<string> brandList { get; set; }
        public ObservableCollection<string> servetypeList { get; set; }
        public ObservableCollection<string> bankList { get; set; }
        public ObservableCollection<int> monitorLengths { get; set; }
        private string XMLcontent = "";
        public DateTime dDBcheckedDate
        {
            get { return im.dDBupdateDate; }
            set
            {
                im.dDBupdateDate = value;
                NotifyPropertyChanged();
            }
        }
        private bool _initial = false;
        public bool initial
        {
            get { return _initial; }
            set
            {
                _initial = value;
                NotifyPropertyChanged();
            }
        }
        private int _indexBrand = -1;
        public int indexBrand
        {
            get { return _indexBrand; }
            set
            {
                _indexBrand = value;
                NotifyPropertyChanged();
            }
        }
        private int _indexServetype = -1;
        public int indexServetype
        {
            get { return _indexServetype; }
            set
            {
                _indexServetype = value;
                NotifyPropertyChanged();
            }
        }
        private int _indexBank = -1;
        public int indexBank
        {
            get { return _indexBank; }
            set
            {
                _indexBank = value;
                NotifyPropertyChanged();
            }
        }
        public string dbRev
        {
            get { return im.discountRev; }
            set
            {
                im.discountRev = value;
                NotifyPropertyChanged();
            }
        }
        private int _discountDBcount = 0;
        public int discountDBcount
        {
            get { return _discountDBcount; }
            set
            {
                _discountDBcount = value;
                NotifyPropertyChanged();
            }
        }
        private int _foundcount = 0;
        public int foundcount
        {
            get { return _foundcount; }
            set
            {
                _foundcount = value;
                NotifyPropertyChanged();
            }
        }
        public discountViewModel() 
        {
            monitorDiscounts = new ObservableCollection<discountStorage>();
            queryDiscounts = new ObservableCollection<discountStorage>();
            discountDB = new List<discountStorage>();
            monitorLengths = new ObservableCollection<int>();
            monitorLengths.Add(2);
            monitorLengths.Add(3);
            monitorLengths.Add(4);
            monitorLengths.Add(5);
            sourceList = new ObservableCollection<string>();
            sourceList.Add("載入中...");
            brandList = new ObservableCollection<string>();
            brandList.Add("載入中...");
            servetypeList = new ObservableCollection<string>();
            servetypeList.Add("載入中...");
            bankList = new ObservableCollection<string>();
            bankList.Add("載入中...");
        }

        public async Task loadXML(Stream xml)
        {
            try
            {
                using (StreamReader sr = new StreamReader(xml))
                {
                    XMLcontent = await sr.ReadToEndAsync();
                    XDocument xd = XDocument.Parse(XMLcontent.Replace("\t", ""));
                    discountDB = (from node in xd.Root.Descendants("discount")
                                  select new discountStorage()
                                  {
                                      bank = node.Element("bank").Value,
                                      card = node.Element("card").Value,
                                      brand = node.Element("brand").Value,
                                      content = node.Element("content").Value,
                                      servetype = node.Element("servetype").Value,
                                      startdate = Convert.ToInt64(node.Element("startdate").Value),
                                      enddate = Convert.ToInt64(node.Element("enddate").Value)
                                  }).ToList();
                    discountDBcount = discountDB.Count();
                    IEnumerable<string> sources = (from source in discountDB orderby source.source select source.source).Distinct();
                    sourceList.Clear();
                    foreach (string s in sources)
                    {
                        sourceList.Add(s);
                    }
                    IEnumerable<string> brands = (from brand in discountDB orderby brand.brand select brand.brand).Distinct();
                    brandList.Clear();
                    brandList.Add("不指定");
                    foreach (string b in brands)
                    {
                        if (b == "不指定") continue;
                        brandList.Add(b);
                    }
                    IEnumerable<string> servetypes = (from servetype in discountDB orderby servetype.servetype select servetype.servetype).Distinct();
                    servetypeList.Clear();
                    servetypeList.Add("不指定");
                    foreach (string s in servetypes)
                    {
                        if (s == "不指定") continue;
                        servetypeList.Add(s);
                    }
                    IEnumerable<string> banks = (from bank in discountDB orderby bank.bank select bank.bank).Distinct();
                    bankList.Clear();
                    bankList.Add("不指定");
                    foreach (string s in banks)
                    {
                        if (s == "不指定") continue;
                        bankList.Add(s);
                    }
                }
            }
            catch (XmlException)
            {
                dbRev = "";
                throw new Exception("折扣資料庫剖析失敗，請連上網路並更新資料庫！");
            }
        }

        public async Task queryDiscount(IProgress<ProgressReport> messenger)
        {
            initial = true;
            messenger.Report(new ProgressReport() { progress = 0, progressMessage = "查詢折扣...", display = true });
            IEnumerable<discountStorage> td = discountDB;
            if (indexBank > 1) td = from discount in td where discount.bank == bankList[indexBank] select discount;
            if (indexBrand > 1) td = from discount in td where discount.brand == brandList[indexBrand] select discount;
            if (indexServetype > 1) td = from discount in td where discount.servetype == servetypeList[indexServetype] select discount;
            td.OrderBy(p => p.bank);
            messenger.Report(new ProgressReport() { progress = 80, progressMessage = "查詢完成！輸出中...", display = true });
            queryDiscounts.Clear();
            foreach (discountStorage ds in td)
            {
                queryDiscounts.Add(ds);
            }
            foundcount = queryDiscounts.Count();
            messenger.Report(new ProgressReport() { progress = 100, progressMessage = "查詢完成！", display = false });
        }

        public async Task<bool> updateXML(IProgress<ProgressReport> messenger, Stream xml)
        {
            try
            {
                messenger.Report(new ProgressReport() { progress = 0, progressMessage = "開始擷取最新折扣資料", display = true });
                dDBcheckedDate = DateTime.Now;
                string urlstr = "https://bitbucket.org/api/1.0/repositories/kelunyang/taiwan-petrol-price/wiki/%E5%90%84%E5%AE%B6%E4%BF%A1%E7%94%A8%E5%8D%A1%E6%B2%B9%E5%83%B9%E5%84%AA%E6%83%A0XML";
                HttpClientHandler handler = new HttpClientHandler();
                if (handler.SupportsAutomaticDecompression)
                {
                    handler.AutomaticDecompression = DecompressionMethods.GZip |
                                                        DecompressionMethods.Deflate;
                }
                var httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.MaxResponseContentBufferSize = 256000;
                httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
                bitbucketPage obj = JsonConvert.DeserializeObject<bitbucketPage>(await httpClient.GetStringAsync(new Uri(urlstr)));
                if(obj.rev != dbRev) {
                    messenger.Report(new ProgressReport() { progress = 80, progressMessage = "開始更新本機折扣資料...", display = true });
                    using(StreamWriter sw = new StreamWriter(xml)) {
                        await sw.WriteAsync(obj.data);
                    }
                    dbRev = obj.rev;
                    messenger.Report(new ProgressReport() { progress = 100, progressMessage = "折扣資料擷取完成", display = false });
                    return true;
                } else {
                    messenger.Report(new ProgressReport() { progress = 100, progressMessage = "線上折扣資料與本機版本相同，不須更新", display = true });
                    using (StreamWriter sw = new StreamWriter(xml))
                    {
                        await sw.WriteAsync(XMLcontent);
                    }
                    messenger.Report(new ProgressReport() { progress = 100, progressMessage = "折扣資料擷取完成", display = false });
                    return false;
                }
            }
            catch
            {
                throw new htmlException("取得折扣內容");
            }
        }
    }
}
