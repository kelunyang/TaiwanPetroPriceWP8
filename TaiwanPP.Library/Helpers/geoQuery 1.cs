using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using TaiwanPP.Library.Models;

namespace TaiwanPP.Library.Helpers
{
    class geoQuery
    {
        HttpClient client;
        public geoQuery();
        public async Task<stationStorage> geoCoding(stationStorage s)
        {
            using (HttpClient hc = new HttpClient())
            {
                try
                {
                    var handler = new HttpClientHandler();
                    if (handler.SupportsAutomaticDecompression)
                    {
                        handler.AutomaticDecompression = DecompressionMethods.GZip |
                                                         DecompressionMethods.Deflate;
                    }
                    var httpClient = new HttpClient(handler);
                    httpClient.MaxResponseContentBufferSize = 256000;
                    httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
                    Uri url = new Uri(Uri.EscapeUriString("http://maps.googleapis.com/maps/api/geocode/json?address=" + s.address + "&sensor=false"));
                    var str = await httpClient.GetStringAsync(url);
                    byte[] byteArray = Encoding.Unicode.GetBytes(str);
                    MemoryStream stream = new MemoryStream(byteArray);
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GeoResponse));
                    var res = (GeoResponse)serializer.ReadObject(stream);
                    if (res.Status == "OK")
                    {
                        s.latitude = res.Results[0].Geometry.Location.Lat;
                        s.longitude = res.Results[0].Geometry.Location.Lng;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return s;
        }
    }
    class GeoResponse
    {
        [DataMember(Name = "status")]
        public string Status { get; set; }
        [DataMember(Name = "results")]
        public CResult[] Results { get; set; }

        [DataContract]
        public class CResult
        {
            [DataMember(Name = "geometry")]
            public CGeometry Geometry { get; set; }

            [DataContract]
            public class CGeometry
            {
                [DataMember(Name = "location")]
                public CLocation Location { get; set; }

                [DataContract]
                public class CLocation
                {
                    [DataMember(Name = "lat")]
                    public double Lat { get; set; }
                    [DataMember(Name = "lng")]
                    public double Lng { get; set; }
                }
            }
        }
    }
}
}
