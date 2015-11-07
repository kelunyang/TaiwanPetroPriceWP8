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

namespace TaiwanPP.Library.ViewModels
{
    class cpViewModel
    {
        HttpClient httpClient;
        int range = 21; //keep 9 in comment
        public cpViewModel() { }
        /*public async Task predictedPrice(IProgress<ProgressReport> status)
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
                httpClient.MaxResponseContentBufferSize = 256000;
                httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
                Uri url = new Uri("http://web3.moeaboe.gov.tw/oil102/oil1022010/A01/A0108/allprices.asp?nocache=" + DateTime.Now.Ticks);
                var str = await httpClient.GetStringAsync(url);
                /*
                 * var byteData = await client.GetByteArrayAsync(url);
    data = Encoding.UTF8.GetString(byteData);
            }
        }*/

    }
}
