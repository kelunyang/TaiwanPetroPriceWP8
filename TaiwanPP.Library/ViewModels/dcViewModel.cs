using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaiwanPP.Library.Models;
using TaiwanPP.Library.Helpers;
using System.Xml;
using System.Xml.Linq;
using System.Net.Http;
using System.Collections.ObjectModel;
using System.Net;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace TaiwanPP.Library.ViewModels
{
    public class dcViewModel : viewmodelBase
    {
        dcModel dc;
        HttpClient httpClient;
        bool connectivity = true;
        string urlstr = "https://bitbucket.org/api/1.0/repositories/kelunyang/taiwan-petrol-price/wiki/Home";
        public ObservableCollection<feedItem> feedlist { set; get; }
        public dcViewModel()
        {
            feedlist = new ObservableCollection<feedItem>();
            feedlist.Add(new feedItem() { content = "載入中...", pubDate = DateTime.MinValue, title = "載入中..." });
            dc = new dcModel();
            httpClient = new HttpClient();
        }
        public async Task load(bool connectivity, IProgress<ProgressReport> messenger)
        {
            this.connectivity = connectivity;
            if (connectivity)
            {
                messenger.Report(new ProgressReport() { progress = 0, progressMessage = "開發者公告清單擷取中", display = true });
                try
                {
                    feedlist.Clear();
                    HttpClientHandler handler = new HttpClientHandler();
                    if (handler.SupportsAutomaticDecompression)
                    {
                        handler.AutomaticDecompression = DecompressionMethods.GZip |
                                                         DecompressionMethods.Deflate;
                    }
                    httpClient = new HttpClient(handler);
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.MaxResponseContentBufferSize = 256000;
                    httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
                    Uri url = new Uri(Uri.EscapeUriString(urlstr));
                    bitbucketPage obj = JsonConvert.DeserializeObject<bitbucketPage>(await httpClient.GetStringAsync(url));
                    IEnumerable<string> data = from s in obj.data.Split('\n') where s.Contains("1.") || s.Contains("2.") select s;
                    foreach (string s in data)
                    {
                        var da = Regex.Match(s, @"\*(\d*\/\d*\/\d*)\*");
                        string date = da.Value.Replace("*", "");
                        var li = Regex.Match(s, @"\wiki\/\S*\)");
                        string link = li.Value.Replace(")", "");
                        var ti = Regex.Match(s, @"\[\S*\]");
                        string title = ti.Value.Replace("[", "").Replace("]", "");
                        dc.items.Add(new feedItem() { pubDate = DateTime.Parse(date), title = title, link = link, type = s.Contains("1.") ? 1 : 2 });
                    }
                    messenger.Report(new ProgressReport() { progress = 100, progressMessage = "開發者公告清單擷取完成", display = true });
                }
                catch
                {
                    throw new htmlException("取得開發者公告清單");
                }
                messenger.Report(new ProgressReport() { progress = 100, progressMessage = "開發者公告清單儲存完成", display = false });
            }
        }
        public async Task buildList(bool fulllog, IProgress<ProgressReport> messenger)
        {
            if (connectivity)
            {
                try
                {
                    messenger.Report(new ProgressReport() { progress = 100, progressMessage = "開始擷取開發者公告內容", display = true });
                    IEnumerable<feedItem> list = fulllog ? (from item in dc.items where item.type == 1 orderby item.pubDate descending select item).Distinct().Take(2).Concat((from item in dc.items where item.type == 2 select item).Distinct().Take(1))
                        :
                                                           (from item in dc.items where item.type == 1 orderby item.pubDate descending select item).Distinct().Take(1);
                    feedlist.Clear();
                    messenger.Report(new ProgressReport() { progress = 100, progressMessage = "開發者公告內容擷取中", display = true });
                    foreach (feedItem fi in list)
                    {
                        string urlstr = "https://bitbucket.org/api/1.0/repositories/kelunyang/taiwan-petrol-price/" + fi.link;
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
                        fi.content = obj.data;
                        feedlist.Add(fi);
                    }
                }
                catch
                {
                    throw new htmlException("取得開發者公告內容");
                }
                messenger.Report(new ProgressReport() { progress = 100, progressMessage = "開發者公告擷取完成", display = false });
            }
            else
            {
                feedlist.Clear();
                feedlist.Add(new feedItem() { title = "網路連線關閉", content = "網路連線關閉，無資料", pubDate = DateTime.MinValue });
            }
        }
    }
}
