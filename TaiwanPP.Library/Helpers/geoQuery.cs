using Newtonsoft.Json;
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
        public geoQuery() { }
        public async Task<GeoResponse> geoCoding(string address)
        {
            using (client = new HttpClient())
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
                    httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36");
                    Uri url = new Uri(Uri.EscapeUriString("http://maps.googleapis.com/maps/api/geocode/json?address=" + address + "&sensor=false&language=zh-TW"));
                    return JsonConvert.DeserializeObject<GeoResponse>(await httpClient.GetStringAsync(url));
                }
                catch
                {
                    throw new jsonException("Google Map地理位置查詢");
                }
            }
        }
        public async Task<stationStorage> geoCodeStation(stationStorage s)
        {
            try
            {
                GeoResponse res = await geoCoding(s.address);
                if (res.Status == "OK")
                {
                    s.latitude = res.Results[0].Geometry.Location.Lat;
                    s.longitude = res.Results[0].Geometry.Location.Lng;
                    IEnumerable<string> distname = from dist in res.Results[0].address_components where dist.long_name.Substring(dist.long_name.Length - 1, 1) == "鄉" || dist.long_name.Substring(dist.long_name.Length - 1, 1) == "鎮" || dist.long_name.Substring(dist.long_name.Length - 1, 1) == "市" || dist.long_name.Substring(dist.long_name.Length - 1, 1) == "區" select dist.long_name;
                    s.district = distname.Any() ? distname.First() : "";
                }
            }
            catch
            {
                throw new gpsException();
            }
            return s;
        }
    }
    class GeoResponse
    {
        public string Status { get; set; }
        public CResult[] Results { get; set; }
        public class CResult
        {
            public CAddressComponent[] address_components { get; set; }
            public string formatted_address { get; set; }
            public class CAddressComponent
            {
                public string long_name { get; set; }
                public string short_name { get; set; }
                public List<string> types { get; set; }
            }
            public CGeometry Geometry { get; set; }
            public class CGeometry
            {
                public CLocation Location { get; set; }
                public class CLocation
                {
                    public double Lat { get; set; }
                    public double Lng { get; set; }
                }
            }
        }
    }
}