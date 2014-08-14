using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TaiwanPP.Tile.Helpers
{
    public class savePNGEvent : EventArgs
    {
        public bool Success { get; set; }
        public Exception Exception { get; set; }
        public string[] ImageFileName { get; set; }

        public savePNGEvent(bool success, string[] fileName)
        {
            Success = success;
            ImageFileName = fileName;
        }
    }
}
