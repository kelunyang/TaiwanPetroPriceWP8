using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaiwanPP.Library.Helpers
{
    public class gpsException : Exception
    {
        public gpsException() : base() {}
        public override string Message
        {
            get
            {
                return "GPS查詢失敗，錯誤代碼：" + base.Message;
            }
        }
    }
    public class soapException : Exception
    {
        public soapException() : base() {}
        public override string Message
        {
            get
            {
                return "中油SOAP服務分析失敗，錯誤代碼：" + base.Message;
            }
        }
    }
    public class xmlException : Exception
    {
        public xmlException() { }
        public override string Message
        {
            get
            {
                return "儲存設定的XML檔案操作失敗，請按下是還原原始設定，錯誤代碼："+base.Message;
            }
        }
    }
    public class dbException : Exception
    {
        public dbException(string message) : base(message) { }
        public override string Message
        {
            get
            {
                return "正在執行" + this.Message + "資料庫操作發生錯誤，請重新執行本程式，錯誤代碼：" + base.Message;
            }
        }
    }
    public class htmlException : Exception
    {
        public htmlException(string message) : base(message) { }
        public override string Message
        {
            get
            {
                return "正在執行" + this.Message + "線上資料操作發生錯誤，請重新執行本程式，錯誤代碼：" + base.Message;
            }
        }
    }
    public class systemException :Exception
    {
        public systemException(string message) : base(message) {}
        public override string Message
        {
            get
            {
                return "正在執行" + this.Message + "系統作業時發生錯誤，請嘗試重新執行，若失敗請聯絡開發者，錯誤代碼：" + base.Message;
            }
        }
    }
    public class jsonException : Exception
    {
        public jsonException(string message) : base(message) { }
        public override string Message
        {
            get
            {
                return "正在分析" + this.Message + "JSON內容時發生錯誤，請嘗試重新執行，若失敗請聯絡開發者，錯誤代碼：" + base.Message;
            }
        }
    }
}
