using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TaiwanPP.Library.Models;

namespace TaiwanPP.Library.Helpers
{
    public class stationinfoSoap : soapPrototype
    {
        public stationStorage qstation;
        public stationinfoSoap(stationStorage s)
        {
            qstation = s;
            queryuri = new Uri(@"http://vipmember.tmtd.cpc.com.tw/CPCSTN/STNWebService.asmx?op=getStationInfo");
            envelope = @"<?xml version='1.0' encoding='utf-8'?>
<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>
  <soap:Body>
    <getStationInfo xmlns='http://tmtd.cpc.com.tw/'>
      <stnno>"+s.cpcid+@"</stnno>
    </getStationInfo>
  </soap:Body>
</soap:Envelope>";
            SOAPAction = @"http://tmtd.cpc.com.tw/getStationInfo";
        }
        public override async Task doWork()
        {
            try
            {
                List<stationStorage> cpcCollection = new List<stationStorage>();
                XDocument xd = await fetchSOAP();
                datacontainer = from node in xd.Root.Descendants() where node.Name.LocalName == "Time" select node;
                if (!datacontainer.Any())
                {
                    qstation.starttime = 0;
                    qstation.duration = 1;
                }
                else
                {
                    XElement station = datacontainer.First();
                    try
                    {
                        if (station.Value != "")
                        {
                            string[] times = station.Value.Split('~');
                            string[] endtime;
                            string[] starttime;
                            if (times[0] == "     ")
                            {
                                endtime = new string[2] { "0", "0" };
                                starttime = new string[2] { "0", "0" };
                            }
                            else
                            {
                                endtime = times[1].Split(':');
                                starttime = times[0].Split(':');
                            }
                            if (endtime[0] == "24") endtime[0] = "0";
                            if (endtime[1] == "60")
                            {
                                endtime[0] = "1";
                                endtime[1] = "0";
                            }
                            if (starttime[1] == "60")
                            {
                                starttime[0] = "1";
                                starttime[1] = "0";
                            }
                            DateTime start = new DateTime(1, 1, 1, Convert.ToInt32(starttime[0]), Convert.ToInt32(starttime[1]), 0);
                            DateTime end = new DateTime(1, 1, 1, Convert.ToInt32(endtime[0]), Convert.ToInt32(endtime[1]), 0);
                            qstation.starttime = start.Ticks;
                            qstation.duration = start.Equals(end) ? 864000000000 : end.Subtract(start).Ticks;
                        }
                        else
                        {
                            qstation.starttime = new DateTime(1, 1, 1, 10, 00, 0).Ticks;
                            qstation.duration = (new DateTime(1, 1, 1, 14, 30, 0)).Subtract(new DateTime(1, 1, 1, 10, 00, 0)).Ticks;
                        }
                    }
                    catch (TimeoutException)
                    {
                        qstation.starttime = 0;
                        qstation.duration = 0;
                    }
                    catch (soapException)
                    {
                        qstation.starttime = 0;
                        qstation.duration = 0;
                    }
                }
            }
            catch (soapException e)
            {
                throw e;
            }
            /* data sample
             * <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <soap:Body>
    <getStationInfoResponse xmlns="http://tmtd.cpc.com.tw/">
      <getStationInfoResult>
        <ID>CC6212C54</ID>
        <Name>建利</Name>
        <ManaDept>D412B</ManaDept>
        <BusNo>97408969  </BusNo>
        <City>台中市大甲區</City>
        <ZipCode>437</ZipCode>
        <Address>奉化里經國路921號</Address>
        <Tel>04-26801261</Tel>
        <Fax>04-26801262</Fax>
        <State>營業中</State>
        <Time>06:00~00:30</Time>
        <Longitude>120.620667</Longitude>
        <Latitude>24.356187</Latitude>
        <Prodlist>
          <ListItem>
            <Text>９２無鉛汽油</Text>
            <Value>113F 1209200</Value>
          </ListItem>
          <ListItem>
            <Text>９５無鉛汽油</Text>
            <Value>113F 1209500</Value>
          </ListItem>
          <ListItem>
            <Text>超級柴油</Text>
            <Value>113F 5100100</Value>
          </ListItem>
        </Prodlist>
        <Servlist>
          <ListItem>
            <Text>洗車</Text>
            <Value>E1001</Value>
          </ListItem>
          <ListItem>
            <Text>信用卡</Text>
            <Value>E4001</Value>
          </ListItem>
          <ListItem>
            <Text>車隊卡</Text>
            <Value>E4002</Value>
          </ListItem>
          <ListItem>
            <Text>捷利卡</Text>
            <Value>E4003</Value>
          </ListItem>
          <ListItem>
            <Text>中油會員卡</Text>
            <Value>E4004</Value>
          </ListItem>
        </Servlist>
        <Eqlist />
        <Result>true</Result>
      </getStationInfoResult>
    </getStationInfoResponse>
  </soap:Body>
</soap:Envelope>
             */
        }
    }
}
