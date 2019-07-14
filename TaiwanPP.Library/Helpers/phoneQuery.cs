using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using HtmlAgilityPack;
using TaiwanPP.Library.Models;
using System.Net.Http;
using System.Net;
using System.Text.RegularExpressions;
//using AngleSharp;

namespace TaiwanPP.Library.Helpers
{
    public class phoneQuery
    {
        /*IEnumerable<AngleSharp.Dom.IElement> CPCstation;
        IEnumerable<AngleSharp.Dom.IElement> FPCCstation;
        IConfiguration config = new Configuration().WithDefaultLoader();*/
        List<phoneData> stations = new List<phoneData>();
        public phoneQuery()
        {

        }
        public async Task loadStation()
        {
            try
            {
                string stationurl = "https://www2.moeaboe.gov.tw/oil102/oil2017/A04/A0407/report.asp";
                var handler = new HttpClientHandler();
                if (handler.SupportsAutomaticDecompression)
                {
                    handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                }
                var httpClient = new HttpClient(handler);
                httpClient.MaxResponseContentBufferSize = 2147483647;
                httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36");
                var parameters = new Dictionary<string, string> { { "MarkID", "10001" } };
                var encodedContent = new FormUrlEncodedContent(parameters);

                var response = await httpClient.PostAsync(stationurl, encodedContent).ConfigureAwait(false);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var byteData = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                    // Do something with response. Example get content:
                    // var responseContent = await response.Content.ReadAsStringAsync ().ConfigureAwait (false);
                    string data = Portable.Text.Encoding.GetEncoding(950).GetString(byteData);
                    RegexOptions opt = RegexOptions.None;
                    Regex stat = new Regex(@"<(tr).+?>(.+\n){3}.+(a3)(.+\n){4}.+<\/(tr)>", opt);
                    MatchCollection stationlines = stat.Matches(data);
                    int count = 0;
                    foreach (Match m in stationlines)
                    {
                        count++;
                        if (count == 1) continue;
                        stations.Add(generatePhoneData(m, 0));
                    }
                    /*AngleSharp.Parser.Html.HtmlParser parser = new AngleSharp.Parser.Html.HtmlParser();
                    AngleSharp.Dom.IDocument tHtmlDoc = parser.Parse(data);
                    /*var ttable = from node in tHtmlDoc.DocumentNode.Descendants("table") where node.Attributes["summary"] != null select node;
                    var tcell = (from node in ttable where node.Attributes["summary"].Value == "加油站品牌明細表，第一直行是序號，第二直行是編號，第三直行是查報名稱，第四直行是鄉鎮市區，第五直行是地址，第六直行是電話。" select node.Descendants("tr")).ToList();*/
                    /*CPCstation = tHtmlDoc.QuerySelectorAll("table[summary='加油站品牌明細表，第一直行是序號，第二直行是編號，第三直行是查報名稱，第四直行是鄉鎮市區，第五直行是地址，第六直行是電話。'] tr[style='font-size:9pt;']").Where(node => node.ChildNodes.Count() == 13);
                    /*var temp = (from node in tcell[0]
                                where node.Attributes["style"] != null
                                select node);
                    CPCstation = (from node in temp
                                  where node.Attributes["style"].Value == "font-size:9pt;" && node.ChildNodes.Count == 13
                                  select node);*/
                }
                httpClient = new HttpClient(handler);
                httpClient.MaxResponseContentBufferSize = 2147483647;
                httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36");
                parameters = new Dictionary<string, string> { { "MarkID", "10002" } };
                encodedContent = new FormUrlEncodedContent(parameters);

                response = await httpClient.PostAsync(stationurl, encodedContent).ConfigureAwait(false);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var byteData = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                    var data = Portable.Text.Encoding.GetEncoding(950).GetString(byteData);
                    RegexOptions opt = RegexOptions.None;
                    var stat = new Regex(@"<(tr).+?>(.+\n){3}.+(a3)(.+\n){4}.+<\/(tr)>", opt);
                    var stationlines = stat.Matches(data);
                    var count = 0;
                    foreach (Match m in stationlines)
                    {
                        count++;
                        if (count == 1) continue;
                        stations.Add(generatePhoneData(m, 1));
                    }
                    /*tHtmlDoc = await BrowsingContext.New(config).OpenAsync(m => m.Content(data));
                    FPCCstation = tHtmlDoc.QuerySelectorAll("table[summary='加油站品牌明細表，第一直行是序號，第二直行是編號，第三直行是查報名稱，第四直行是鄉鎮市區，第五直行是地址，第六直行是電話。'] tr[style='font-size:9pt;']").Where(node => node.ChildNodes.Count() == 13);
                    /*ttable = from node in tHtmlDoc.DocumentNode.Descendants("table") where node.Attributes["summary"] != null select node;
                    tcell = (from node in ttable where node.Attributes["summary"].Value == "加油站品牌明細表，第一直行是序號，第二直行是編號，第三直行是查報名稱，第四直行是鄉鎮市區，第五直行是地址，第六直行是電話。" select node.Descendants("tr")).ToList();
                    temp = (from node in tcell[0]
                            where node.Attributes["style"] != null
                            select node);
                    FPCCstation = (from node in temp
                                   where node.Attributes["style"].Value == "font-size:9pt;" && node.ChildNodes.Count == 13
                                   select node);*/
                }
            }
            catch
            {
                throw new htmlException("擷取能源局網站加油站資料失敗");
            }
        }
        phoneData generatePhoneData(Match m, int type)
        {
            RegexOptions opt = RegexOptions.None;
            string s = m.Value;
            Regex namereg = new Regex(@"a3' nowrap>.+<", opt);
            Match name = namereg.Match(s);
            Regex phonereg = new Regex(@"a6' nowrap>.+<", opt);
            Match phone = phonereg.Match(s);
            return new phoneData(name.Value, phone.Value, type);
        }
        public string queryCPCPhone(string phone, string name)
        {
            //System.Diagnostics.Debug.WriteLine("Online:"+name+"/"+phone+"/"+phone.Length);
            string test = (from station in stations where station.name.IndexOf(name) > -1 & station.brand == 0 select station.phone).ToList().FirstOrDefault();
            string dbphone = test == null ? "0" : test;
            //System.Diagnostics.Debug.WriteLine("DB:"+name + "/" + dbphone + "/" + dbphone.Length);
            if (phone.Length > 1)
            {
                return phone;
                /*string cpcphone = Regex.Replace(phone, "-", "");
                if (cpcphone == dbphone)
                {
                    return cpcphone;
                } else
                {
                    return cpcphone;
                }*/
                /*string[] phonearr = phone.Split('-');
                if (phonearr.Length == 2)
                {
                    if (phonearr[1].Length == 10) return phonearr[1];
                }
                if (phone.Substring(0, 1) != "-") return phone;*/
            }
            return dbphone;
            //string test = (from node in CPCstation where node.ChildNodes[5].TextContent == name + "加油站" select node.ChildNodes[11].TextContent).ToList().FirstOrDefault();
            //return test == null ? "0" : Regex.Replace(test, @"<[^>]+>|&nbsp;", "").Trim();
        }
        public string queryFPCCPhone(string phone, string name)
        {
            System.Diagnostics.Debug.WriteLine(name + "/" + phone);
            string test = (from station in stations where station.name.IndexOf(name) > -1 & station.brand == 1 select station.phone).ToList().FirstOrDefault();
            string dbphone = test == null ? "0" : test;
            if (phone.Length > 1)
            {
                return phone;
                /*string fpccphone = Regex.Replace(phone, "-", "");
                if (fpccphone == dbphone)
                {
                    return dbphone;
                }
                else
                {
                    return fpccphone;
                }*/
            }
            return dbphone;
            /*if (phone != "") return phone;
            if (phone.Substring(0, 1) != "-") return phone;
            string[] phonearr = phone.Split('-');
            if (phonearr.Length == 2)
            {
                if (phonearr[1].Length == 10) return phonearr[1];
            }
            
            return test == null ? "" : test;
            string test = (from  node in FPCCstation where node.ChildNodes[5].TextContent == name + "加油站" select node.ChildNodes[11].TextContent).ToList().FirstOrDefault();
            return test == null ? "0" : Regex.Replace(test, @"<[^>]+>|&nbsp;", "").Trim();*/
        }
    }
}
class phoneData
{
    public string name;
    public string phone;
    public int brand;
    public phoneData(string name, string phone, int brand)
    {
        this.name = Regex.Replace(name, @".+ nowrap>|-|\s|<", "").Trim();
        this.phone = Regex.Replace(phone, @".+ nowrap>|-|\s|&nbsp;<", "").Trim();
        this.brand = brand;
    }
}