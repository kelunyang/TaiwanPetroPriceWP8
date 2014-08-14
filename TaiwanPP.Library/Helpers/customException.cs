using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaiwanPP.Library.Helpers
{
    public class gpsException : Exception
    {
        public gpsException() : base("GPS查詢失敗") {}
    }
    public class soapException : Exception
    {
        public soapException() : base("中油SOAP服務分析失敗") {}
    }
    public class xmlException : Exception
    {
        public xmlException() : base("儲存設定的XML檔案操作失敗，請按下是還原原始設定") { }
    }
    public class dbException : Exception
    {
        public dbException(string message) : base("正在執行" + message + "資料庫操作發生錯誤，請重新執行本程式") { }
    }
    public class htmlException : Exception
    {
        public htmlException(string message) : base("正在執行" + message + "線上資料操作發生錯誤，請重新執行本程式") { }
    }
    public class systemException :Exception
    {
        public systemException(string message) : base ("正在執行"+message+"系統作業時發生錯誤，請嘗試重新執行，若失敗請聯絡開發者") {}
    }
    public class jsonException : Exception
    {
        public jsonException(string message) : base("正在分析" + message + "JSON內容時發生錯誤，請嘗試重新執行，若失敗請聯絡開發者") { }
    }
}
