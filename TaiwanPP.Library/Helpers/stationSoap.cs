using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TaiwanPP.Library.Models;

namespace TaiwanPP.Library.Helpers
{
    public class stationSoap : soapPrototype
    {
        public phoneQuery pq;
        public stationSoap()
        {
            queryuri = new Uri(@"http://vipmember.tmtd.cpc.com.tw/CPCSTN/STNWebService.asmx?op=getCityStation");
            envelope = @"<?xml version='1.0' encoding='utf-8'?>
<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>
  <soap:Body>
    <getCityStation xmlns='http://tmtd.cpc.com.tw/'>
    </getCityStation>
  </soap:Body>
</soap:Envelope>";
            SOAPAction = "http://tmtd.cpc.com.tw/getCityStation";
        }
        public IEnumerable<stationStorage> cpcStation
        {
            get
            {
                return (from station in datacontainer
                        select new stationStorage()
                        {
                            address = station.Element("縣市").Value + station.Element("鄉鎮區").Value + station.Element("地址").Value,
                            brand = 0,
                            favorite = false,
                            cpcid = station.Element("站代號").Value,
                            distance = 0,
                            name = station.Element("站名").Value,
                            phone = pq.queryCPCPhone(station.Element("電話").Value, station.Element("站名").Value),
                            selftype = station.Element("刷卡自助").Value == "false" ? 0 : station.Element("自助柴油站").Value == "false" ? 1 : 2,
                            type = station.Element("類別").Value == "自營站" ? true : false,
                            latitude = station.Descendants("經度").Any() ? Convert.ToDouble(station.Element("經度").Value) : 0,
                            longitude = station.Descendants("緯度").Any() ? Convert.ToDouble(station.Element("緯度").Value) : 0,
                            city = station.Element("縣市").Value,
                            district = station.Element("鄉鎮區").Value,
                            p92 = station.Element("無鉛92").Value == "true",
                            p95 = station.Element("無鉛95").Value == "true",
                            p98 = station.Element("無鉛98").Value == "true",
                            pdiesel = station.Element("超柴").Value == "true",
                            pgasohol = station.Element("酒精汽油").Value == "true"
                        });
            }
            /*foreach (XElement node in datacontainer)
            {
                List<XElement> childNodes = node.Elements().ToList();
                int selftype = 0;
                if (childNodes[15].Value == "true") selftype = 1;
                if (childNodes[16].Value == "true") selftype = 2;
                cpcCollection.Add(new stationStorage()
                {
                    address = childNodes[3].Value + childNodes[4].Value + childNodes[5].Value,
                    brand = 0,
                    favorite = false,
                    id = childNodes[0].Value,
                    distance = 0,
                    name = childNodes[2].Value,
                    phone = childNodes[6].Value,
                    selftype = selftype,
                    type = childNodes[1].Value == "自營站" ? true : false,
                    latitude = childNodes.Count > 17 ? Convert.ToDouble(childNodes[17].Value) : 0,
                    longitude = childNodes.Count > 17 ? Convert.ToDouble(childNodes[18].Value) : 0
                });
            }
            return cpcCollection;*/
            /* data sample
             * <tbTable diffgr:id="tbTable1996" msdata:rowOrder="1995">
	<站代號>D436F</站代號>
	<類別>自營站</類別>
	<站名>湖西站</站名>
	<縣市>澎湖縣</縣市>
	<鄉鎮區>湖西鄉</鄉鎮區>
	<地址>湖西村77-15號</地址>
	<電話>06-9921064</電話>
	<服務中心>D431H</服務中心>
	<營業中>true</營業中>
	<無鉛92>true</無鉛92>
	<無鉛95>true</無鉛95>
	<無鉛98>false</無鉛98>
	<酒精汽油>false</酒精汽油>
	<超柴>true</超柴>
	<會員卡>true</會員卡>
	<刷卡自助>false</刷卡自助>
	<自助柴油站>false</自助柴油站>
	<經度>119.655649776417</經度>
	<緯度>23.5861529735</緯度>
    </tbTable>
             */
        } 
        public override async Task doWork()
        {
            try
            {
                List<stationStorage> cpcCollection = new List<stationStorage>();
                XDocument xd = await fetchSOAP();
                datacontainer = from node in xd.Root.Descendants("tbTable") select node;
            }
            catch (soapException e)
            {
                throw e;
            }
        }
    }
}
