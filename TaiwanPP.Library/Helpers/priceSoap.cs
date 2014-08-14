using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TaiwanPP.Library.Helpers
{
    class priceSoap : soapPrototype
    {
        public priceSoap()
        {
            queryuri = new Uri("https://vipmember.tmtd.cpc.com.tw/OpenData/ListPriceWebService.asmx?op=getCPCMainProdListPrice");
            envelope = @"<?xml version='1.0' encoding='utf-8'?>
<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>
  <soap:Body>
    <getCPCMainProdListPrice xmlns='http://tmtd.cpc.com.tw/' />
  </soap:Body>
</soap:Envelope>";
            SOAPAction = "http://tmtd.cpc.com.tw/getCPCMainProdListPrice";
        }
        public double price
        {
            get 
            {
                return Convert.ToDouble((from node in datacontainer select node.Element("參考牌價").Value).Take(1).ToList()[0]);
            }
            private set {}
        }
        public DateTime date {
            get 
            {
                return DateTime.Parse((from node in datacontainer select node.Element("牌價生效時間").Value).Take(1).ToList()[0]);
            }
        }
        public override async Task doWork()
        {
            try
            {
                XDocument xd = await fetchSOAP();
                datacontainer = from node in xd.Root.Descendants("tbTable") where node.Element("交貨地點").Value == "中油自營站" && node.Element("產品名稱").Value == "酒精汽油" select node;
                loaded = datacontainer.Any() ? true : false;
                /*Data Sample
                 * 
                 * <tbTable diffgr:id="tbTable4" msdata:rowOrder="3">
	                <型別名稱>汽柴油零售</型別名稱>
	                <產品編號>113F 1229500</產品編號>
	                <產品名稱>酒精汽油</產品名稱>
	                <包裝>散裝</包裝>
	                <銷售對象>一般自用客戶 </銷售對象>
	                <交貨地點>中油加油站</交貨地點>
	                <計價單位>元/ 公升</計價單位>
	                <參考牌價>35.5</參考牌價>
	                <營業稅>5%</營業稅>
	                <貨物稅>內含</貨物稅>
	                <牌價生效時間>2014-07-07T00:00:00+08:00</牌價生效時間>
                   </tbTable>
                 */
            }
            catch 
            {
                throw new soapException();
            }
        }
    }
}
