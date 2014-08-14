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

namespace TaiwanPP.Library.ViewModels
{
    public class dcViewModel : viewmodelBase
    {
        dcModel dc;
        HttpClient httpClient;
        bool connectivity = true;
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
                messenger.Report(new ProgressReport() { progress = 0, progressMessage = "開發者公告擷取中", display = true });
                try
                {
                    feedlist.Clear();
                    httpClient = new HttpClient();
                    httpClient.MaxResponseContentBufferSize = 256000;
                    httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
                    string content = await httpClient.GetStringAsync(new Uri(dc.uri + "&nocache=" + DateTime.Now.Ticks));
                    dc.feed = XDocument.Parse(content);
                    messenger.Report(new ProgressReport() { progress = 100, progressMessage = "開發者公告擷取完成", display = true });
                }
                catch
                {
                    throw new htmlException("取得開發者公告");
                }
                messenger.Report(new ProgressReport() { progress = 100, progressMessage = "開發者公告儲存完成", display = false });
            }
        }
        public void buildList(bool fulllog)
        {
            if (connectivity)
            {
                IEnumerable<feedItem> fulllist = from el in dc.feed.Descendants("item")
                                                 where el.Element("title").Value.Contains("開發") || el.Element("title").Value.Contains("調查")
                                                 select new feedItem()
                                                 {
                                                     title = el.Element("title").Value.Replace("Updated Wiki:", ""),
                                                     content = el.Element("description").Value,
                                                     pubDate = DateTime.Parse(el.Element("pubDate").Value)
                                                 };
                IEnumerable<feedItem> list = fulllog ? (from item in fulllist select item).Distinct().Take(3)
                                                        :
                                                       (from item in fulllist where item.title.Contains("開發") select item).Distinct().Take(1);
                feedlist.Clear();
                foreach (feedItem fi in list)
                {
                    if (!fulllog)
                    {
                        string content = PCLWebUtility.WebUtility.HtmlDecode(fi.content);
                        XDocument xml = XDocument.Parse("<root>" + content + "</root>");
                        IEnumerable<XElement> lilist = from li in xml.Descendants("li") select li;
                        if (!lilist.Any())
                        {
                            fi.content = xml.Root.Element("div").Value;
                        }
                        else
                        {
                            string output = "";
                            foreach (XElement li in lilist)
                            {
                                output += li.Value + "\n";
                            }
                            fi.content = output;
                        }
                    }
                    feedlist.Add(fi);
                }
            }
            else
            {
                feedlist.Clear();
                feedlist.Add(new feedItem() { title = "網路連線關閉", content = "網路連線關閉，無資料", pubDate = DateTime.MinValue });
            }
        }
    }
}
