using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaiwanPP.Library.Helpers
{
    public class GeoPoint
    {
        public double Latitude;
        public double Longitude;
        public GeoPoint(double lat, double lon)
        {
            Latitude = lat;
            Longitude = lon;
        }
    }
}
