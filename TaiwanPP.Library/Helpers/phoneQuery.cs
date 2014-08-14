using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using TaiwanPP.Library.Models;
using System.Net.Http;
using System.Net;
using System.Text.RegularExpressions;

namespace TaiwanPP.Library.Helpers
{
    public class phoneQuery
    {
        IEnumerable<HtmlNode> CPCstation;
        IEnumerable<HtmlNode> FPCCstation;
        public phoneQuery()
        {

        }
        public async Task loadStation()
        {
            try
            {
                string cpcuri = "http://web3.moeaboe.gov.tw/oil102/oil1022010/A04/A0407/report.asp?MarkID=10001";
                string fpccuri = "http://web3.moeaboe.gov.tw/oil102/oil1022010/A04/A0407/report.asp?MarkID=10002";
                var handler = new HttpClientHandler();
                if (handler.SupportsAutomaticDecompression)
                {
                    handler.AutomaticDecompression = DecompressionMethods.GZip |
                                                     DecompressionMethods.Deflate;
                }
                var httpClient = new HttpClient(handler);
                httpClient.MaxResponseContentBufferSize = 2147483647;
                httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
                HtmlDocument tHtmlDoc = new HtmlDocument();
                var byteData = await httpClient.GetByteArrayAsync(new Uri(Uri.EscapeUriString(cpcuri)));
                string data = Portable.Text.Encoding.GetEncoding(950).GetString(byteData);
                tHtmlDoc.LoadHtml(data);
                var ttable = from node in tHtmlDoc.DocumentNode.Descendants("table") where node.Attributes["summary"] != null select node;
                var tcell = (from node in ttable where node.Attributes["summary"].Value == "加油站品牌明細表，第一直行是序號，第二直行是編號，第三直行是查報名稱，第四直行是鄉鎮市區，第五直行是地址，第六直行是電話。" select node.Descendants("tr")).ToList();
                var temp = (from node in tcell[0]
                            where node.Attributes["style"] != null
                            select node);
                CPCstation = (from node in temp
                              where node.Attributes["style"].Value == "font-size:9pt;" && node.ChildNodes.Count == 13
                              select node);
                httpClient = new HttpClient(handler);
                httpClient.MaxResponseContentBufferSize = 2147483647;
                httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
                tHtmlDoc = new HtmlDocument();
                byteData = await httpClient.GetByteArrayAsync(new Uri(Uri.EscapeUriString(fpccuri)));
                data = Portable.Text.Encoding.GetEncoding(950).GetString(byteData);
                tHtmlDoc.LoadHtml(data);
                ttable = from node in tHtmlDoc.DocumentNode.Descendants("table") where node.Attributes["summary"] != null select node;
                tcell = (from node in ttable where node.Attributes["summary"].Value == "加油站品牌明細表，第一直行是序號，第二直行是編號，第三直行是查報名稱，第四直行是鄉鎮市區，第五直行是地址，第六直行是電話。" select node.Descendants("tr")).ToList();
                temp = (from node in tcell[0]
                        where node.Attributes["style"] != null
                        select node);
                FPCCstation = (from node in temp
                               where node.Attributes["style"].Value == "font-size:9pt;" && node.ChildNodes.Count == 13
                               select node);
            }
            catch
            {
                throw new htmlException("擷取能源局網站加油站資料失敗");
            }
        }
        public string queryCPCPhone(string phone, string name)
        {
            if (phone.Length > 1)
            {
                string[] phonearr = phone.Split('-');
                if (phonearr.Length == 2)
                {
                    if (phonearr[1].Length == 10) return phonearr[1];
                }
                if (phone.Substring(0, 1) != "-") return phone;
            }
            string test = (from node in CPCstation where node.ChildNodes[5].InnerText == name + "加油站" select node.ChildNodes[11].InnerText).ToList().First();
            return Regex.Replace(test, @"<[^>]+>|&nbsp;", "").Trim();
        }
        public string queryFPCCPhone(string phone, string name)
        {
            if (phone != "") return phone;
            if (phone.Substring(0, 1) != "-") return phone;
            string[] phonearr = phone.Split('-');
            if (phonearr.Length == 2)
            {
                if (phonearr[1].Length == 10) return phonearr[1];
            }
            string test = (from node in FPCCstation where node.ChildNodes[5].InnerText == name + "加油站" select node.ChildNodes[11].InnerText).ToList().First();
            return Regex.Replace(test, @"<[^>]+>|&nbsp;", "").Trim();
        }
    }
}
