using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using TaiwanPP.Library.Helpers;
using PCLWebUtility;

namespace TaiwanPP.Library.Models
{
    public class infoModel : INotifyPropertyChanged
    {
        private object xmllock = new object();
        string defaultxml = @"<?xml version='1.0' encoding='utf-8' ?>
<!DOCTYPE appconfig [
  <!ELEMENT appconfig (info ,predictPrice ,settings, tiles)>
  <!ELEMENT info (updateDate ,notified ,sysinfo, scheduledTaskError)>
  <!ELEMENT predictPrice (duration ,gasprice ,dieselprice)>
  <!ELEMENT settings (timing , productMonitor ,stationFilters, dailynotify, defaultPage, autoUpdate, soapUpdate, stationBehavior, runPredict)>
  <!ELEMENT scheduledTaskError (#PCDATA)><!-- 背景程式錯誤紀錄，內容是trace -->
  <!ELEMENT tiles (tile*)>
  <!ELEMENT tile (#PCDATA)> <!-- 動態磚 -->
  <!ELEMENT updateDate EMPTY>
  <!ELEMENT priceChange EMPTY>
  <!ELEMENT duration EMPTY>
  <!ELEMENT timing EMPTY>
  <!ELEMENT sysinfo EMPTY><!-- 初次載入和升級資訊 -->
  <!ELEMENT defaultPage EMPTY><!-- 預設頁面 -->
  <!ELEMENT dailynotify (#PCDATA)><!-- 每日預測提醒 -->
  <!ELEMENT autoUpdate EMPTY><!-- 自動更新 -->
  <!ELEMENT soapUpdate EMPTY><!-- 中油SOAP連接 -->
  <!ELEMENT runPredict EMPTY><!-- 執行預測 -->
  <!ELEMENT stationBehavior EMPTY><!-- 點擊加油站行為 -->
  <!ELEMENT stationFilters (FPCC ,CPC ,Self ,Direct ,Favorite ,Kilometer ,Country ,Inservice, Product, Subbrand)>
  <!ELEMENT Subbrand (term*)><!-- 子品牌名稱 -->
  <!ELEMENT term (#PCDATA)><!-- 搜尋關鍵字 -->
  <!ELEMENT productMonitor (product*)><!-- 追蹤油價清單 -->
  <!ELEMENT product EMPTY>
  <!ELEMENT Product EMPTY>
  <!ELEMENT gasprice (#PCDATA)> <!-- 預測汽油價格 -->
  <!ELEMENT dieselprice (#PCDATA)> <!-- 預測柴油價格 -->
  <!ELEMENT FPCC EMPTY><!-- 僅顯示台塑加油站 -->
  <!ELEMENT Self EMPTY><!-- 僅顯示自助加油 -->
  <!ELEMENT CPC EMPTY><!-- 僅顯示中油加油站 -->
  <!ELEMENT Direct EMPTY><!-- 僅顯示直營站 -->
  <!ELEMENT Favorite EMPTY><!-- 僅顯示存檔的加油站 -->
  <!ELEMENT Kilometer (#PCDATA)><!-- 搜尋範圍 -->
  <!ELEMENT notified EMPTY><!-- 背景程式提醒 -->
  <!ELEMENT Country (#PCDATA)><!-- 縣市 -->
  <!ELEMENT Inservice EMPTY><!-- 僅限營業時間 -->
  <!ATTLIST FPCC filter (0 | 1) #REQUIRED>
  <!ATTLIST CPC filter (0|1) #REQUIRED>
  <!ATTLIST Self filter (0 | 1) #REQUIRED>
  <!ATTLIST Direct filter (0 | 1) #REQUIRED>
  <!ATTLIST Favorite filter (0 | 1) #REQUIRED>
  <!ATTLIST Inservice filter (0 | 1) #REQUIRED>
  <!ATTLIST autoUpdate enable (0 | 1) #REQUIRED>
  <!ATTLIST soapUpdate enable (0 | 1) #REQUIRED>
  <!ATTLIST runPredict enable (0|1) #REQUIRED><!-- 載入時執行預測 -->
  <!ATTLIST Country filter (0 | 1) #REQUIRED><!-- 區域過濾 -->
  <!ATTLIST appconfig version CDATA #REQUIRED><!-- 版本 -->
  <!ATTLIST defaultPage page (0|1|2|3) #REQUIRED><!-- 預設頁面，0表示自動，其餘-1 -->
  <!ATTLIST updateDate stationDB  CDATA #REQUIRED> <!-- 加油站資料庫更新時間 -->
  <!ATTLIST updateDate sDBnotifyDate CDATA #REQUIRED><!-- 加油站資料庫更新通知 -->
  <!ATTLIST updateDate DBcheckedDate CDATA #REQUIRED><!-- 資料庫檢查時間 -->
  <!ATTLIST updateDate priceDB CDATA #REQUIRED> <!-- 價格資料庫更新時間 -->
  <!ATTLIST updateDate moeaboeDBdate CDATA #REQUIRED><!-- 能源局油價資料庫 -->
  <!ATTLIST updateDate discountRev CDATA #REQUIRED><!-- 折扣頁面版本 -->
  <!ATTLIST updateDate dDBcheckedDate CDATA #REQUIRED><!-- 折扣資料庫檢查時間 -->
  <!ATTLIST notified CPC (0|1) #REQUIRED> <!-- 背景程式已通知中油更新日期 -->
  <!ATTLIST notified FPCC (0|1) #REQUIRED> <!-- 背景程式已通知台塑更新日期 -->
  <!ATTLIST notified checkHour CDATA #REQUIRED> <!-- 背景程式已通知台塑更新日期 -->
  <!ATTLIST Kilometer lat CDATA #REQUIRED><!-- 搜尋地點 -->
  <!ATTLIST Kilometer long CDATA #REQUIRED><!-- 搜尋地點 -->
  <!ATTLIST Kilometer name CDATA #REQUIRED><!-- 地名 -->
  <!ATTLIST Kilometer filter (0|1) #REQUIRED><!-- 啟動指定位置過濾 -->
  <!ATTLIST Product p92 (0|1) #REQUIRED>
  <!ATTLIST Product p95 (0|1) #REQUIRED>
  <!ATTLIST Product p98 (0|1) #REQUIRED>
  <!ATTLIST Product pdiesel (0|1) #REQUIRED>
  <!ATTLIST Product pgasohol (0|1) #REQUIRED>
  <!ATTLIST dailynotify notified (0|1) #REQUIRED><!-- 本日已提醒 -->
  <!ATTLIST dailynotify checkHour CDATA #REQUIRED><!-- 本日已提醒 -->
  <!ATTLIST dailynotify enable (0|1) #REQUIRED><!-- 啟動本日提醒 -->
  <!ATTLIST duration start CDATA #REQUIRED> <!-- 預測周期開始日 -->
  <!ATTLIST duration end CDATA #REQUIRED> <!-- 預測周期結束日 -->
  <!ATTLIST duration runday CDATA #REQUIRED> <!-- 預測日 -->
  <!ATTLIST timing dailynotifytime CDATA #REQUIRED><!-- 每日提醒時刻 -->
  <!ATTLIST timing updateFreq (0 | 1) #REQUIRED> <!-- 更新時機，每次/周末 -->
  <!ATTLIST timing saveTime (0 | 1) #REQUIRED> <!-- 儲存價格時機，自動/手動 -->
  <!ATTLIST stationBehavior behavior (0|1) #REQUIRED> <!-- 點擊加油站行為，0為單點導航，1為單點看加油站位置 -->
  <!ATTLIST product id (0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8) #REQUIRED> <!-- 追蹤商品清單 -->
  <!ATTLIST tiles default (0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | -1) #REQUIRED> <!-- 預設動態磚 -->
  <!ATTLIST tiles updateDate CDATA #REQUIRED><!-- 動態磚更新日期 -->
  <!ATTLIST sysinfo upgrade (0|1) #REQUIRED><!-- 是否為升級版 -->
  <!ATTLIST sysinfo firstload (0|1) #REQUIRED><!-- 初次使用 -->
  <!ATTLIST predictPrice pause (0|1) #REQUIRED><!-- 啟動預測 -->
  <!ATTLIST scheduledTaskError lasterror CDATA #REQUIRED><!-- 最後一次錯誤 -->
  <!ATTLIST scheduledTaskError timestamp CDATA #REQUIRED><!-- 錯誤發生時間 -->
]>
<appconfig version='636201098086080768'>
  <info>
    <updateDate priceDB='0' stationDB='636201098085370768' sDBnotifyDate='636201098085370768' DBcheckedDate='0' moeaboeDBdate='0' discountRev='3f94badb0df2ffe8d47ffc7362e9adb6840e6e3c' dDBcheckedDate='0'/>
    <notified CPC='0' FPCC='0' checkHour='0'/>
    <sysinfo firstload='0' upgrade='0'/>
    <scheduledTaskError lasterror='0' timestamp='0'>0</scheduledTaskError>
  </info>
  <predictPrice pause='0'>
    <duration end='0' start='0' runday='0'/>
    <gasprice>0</gasprice>
    <dieselprice>0</dieselprice>
  </predictPrice>
  <settings>
    <timing saveTime='0' updateFreq='0' dailynotifytime='0'/>
    <productMonitor></productMonitor>
    <stationFilters>
      <FPCC filter='0'/>
      <CPC filter='0'/>
      <Self filter='0'/>
      <Direct filter='0'/>
      <Favorite filter='1'/>
      <Kilometer lat='0' long='0' name='' filter='0'>3</Kilometer>
      <Country filter='0'>0</Country>
      <Inservice filter='1'/>
      <Product p92='0' p95='1' p98='0' pdiesel='0' pgasohol='0'/>
      <Subbrand></Subbrand>
    </stationFilters>
    <dailynotify notified='0' checkHour='0' enable='1'>0</dailynotify>
    <defaultPage page='0'/>
    <autoUpdate enable='1'/>
    <soapUpdate enable='1'/>
    <stationBehavior behavior='1'/>
    <runPredict enable='1'/>
  </settings>
  <tiles default='-1' updateDate='0'></tiles>
</appconfig>";
        XDocument config = new XDocument();
        public infoModel() 
        {
            config = XDocument.Parse(defaultxml);
        }
        public void load(Stream str)
        {
            try
            {
                lock (xmllock)
                {
                    config = XDocument.Load(str);
                }
            }
            catch
            {
                throw new xmlException();
            }
        }
        public void save(Stream str)
        {
            try
            {
                lock (xmllock)
                {
                    config.Save(str);
                }
            }
            catch
            {
                throw new xmlException();
            }
        }
        public string export()
        {
            return config.ToString();
        }
        public bool firstLoad {
            get
            {
                return config.Element("appconfig").Element("info").Element("sysinfo").Attribute("firstload").Value == "1" ? true : false;
            }
            set
            {
                config.Element("appconfig").Element("info").Element("sysinfo").Attribute("firstload").Value = value ? "1" : "0";
            }
        }
        public bool upgrade
        {
            get
            {
                return config.Element("appconfig").Element("info").Element("sysinfo").Attribute("upgrade").Value == "1" ? true : false;
            }
            set
            {
                config.Element("appconfig").Element("info").Element("sysinfo").Attribute("upgrade").Value = value ? "1" : "0";
            }
        }
        public string scheduledTaskErrorcode
        {
            get
            {
                return WebUtility.UrlDecode(config.Element("appconfig").Element("info").Element("scheduledTaskError").Attribute("lasterror").Value);
            }
            set
            {
                config.Element("appconfig").Element("info").Element("scheduledTaskError").Attribute("lasterror").Value = WebUtility.UrlEncode(value);
            }
        }
        public DateTime scheduledTaskErrortime
        {
            get
            {
                return new DateTime(Convert.ToInt64(config.Element("appconfig").Element("info").Element("scheduledTaskError").Attribute("timestamp").Value));
            }
            set
            {
                config.Element("appconfig").Element("info").Element("scheduledTaskError").Attribute("timestamp").Value = value.Ticks.ToString();
            }
        }
        public string scheduledTaskErrortrace   //encoded by urlencode
        {
            get
            {
                return WebUtility.UrlDecode(config.Element("appconfig").Element("info").Element("scheduledTaskError").Value);
            }
            set
            {
                config.Element("appconfig").Element("info").Element("scheduledTaskError").Value = WebUtility.UrlEncode(value);
            }
        }
        public bool ppause
        {
            get
            {
                return config.Element("appconfig").Element("predictPrice").Attribute("pause").Value == "1" ? true : false;
            }
            set
            {
                config.Element("appconfig").Element("predictPrice").Attribute("pause").Value = value ? "1" : "0";
            }
        }
        public DateTime pstartWeek
        {
            get
            {
                return new DateTime(Convert.ToInt64(config.Element("appconfig").Element("predictPrice").Element("duration").Attribute("start").Value));
            }
            set
            {
                config.Element("appconfig").Element("predictPrice").Element("duration").Attribute("start").Value = value.Ticks.ToString();
            }
        }
        public DateTime pendWeek
        {
            get
            {
                return new DateTime(Convert.ToInt64(config.Element("appconfig").Element("predictPrice").Element("duration").Attribute("end").Value));
            }
            set
            {
                config.Element("appconfig").Element("predictPrice").Element("duration").Attribute("end").Value = value.Ticks.ToString();
            }
        }
        public DateTime prunDay
        {
            get
            {
                return new DateTime(Convert.ToInt64(config.Element("appconfig").Element("predictPrice").Element("duration").Attribute("runday").Value));
            }
            set
            {
                config.Element("appconfig").Element("predictPrice").Element("duration").Attribute("runday").Value = value.Ticks.ToString();
            }
        }
        public double predictgasPrice
        {
            get
            {
                double r = config.Element("appconfig").Element("predictPrice").Element("gasprice").Value == "" ? double.NaN : Convert.ToDouble(config.Element("appconfig").Element("predictPrice").Element("gasprice").Value);
                return r;
            }
            set
            {
                string v = double.IsNaN(value) ? "" : value.ToString();
                config.Element("appconfig").Element("predictPrice").Element("gasprice").Value = v;
            }
        }
        public double predictdieselPrice
        {
            get
            {
                double r = config.Element("appconfig").Element("predictPrice").Element("dieselprice").Value == "" ? double.NaN : Convert.ToDouble(config.Element("appconfig").Element("predictPrice").Element("dieselprice").Value);
                return r;
            }
            set
            {
                string v = double.IsNaN(value) ? "" : value.ToString();
                config.Element("appconfig").Element("predictPrice").Element("dieselprice").Value = v;
            }
        }
        public DateTime priceDBdate {
            get
            {
                return new DateTime(Convert.ToInt64(config.Element("appconfig").Element("info").Element("updateDate").Attribute("priceDB").Value));
            }
            set
            {
                config.Element("appconfig").Element("info").Element("updateDate").Attribute("priceDB").Value = value.Ticks.ToString();
            }
        }
        public DateTime stationDBdate
        {
            get
            {
                return new DateTime(Convert.ToInt64(config.Element("appconfig").Element("info").Element("updateDate").Attribute("stationDB").Value));
            }
            set
            {
                config.Element("appconfig").Element("info").Element("updateDate").Attribute("stationDB").Value = value.Ticks.ToString();
            }
        }
        public DateTime stationDBnotifydate
        {
            get
            {
                return new DateTime(Convert.ToInt64(config.Element("appconfig").Element("info").Element("updateDate").Attribute("sDBnotifyDate").Value));
            }
            set
            {
                config.Element("appconfig").Element("info").Element("updateDate").Attribute("sDBnotifyDate").Value = value.Ticks.ToString();
            }
        }
        public bool savetime {
            get
            {
                return config.Element("appconfig").Element("settings").Element("timing").Attribute("saveTime").Value == "1" ? true : false;
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("timing").Attribute("saveTime").Value = value ? "1" : "0";
            }
        }
        public bool autoupdate
        {
            get
            {
                return config.Element("appconfig").Element("settings").Element("autoUpdate").Attribute("enable").Value == "1" ? true : false;
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("autoUpdate").Attribute("enable").Value = value ? "1" : "0";
            }
        }
        public bool soapupdate
        {
            get
            {
                return config.Element("appconfig").Element("settings").Element("soapUpdate").Attribute("enable").Value == "1" ? true : false;
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("soapUpdate").Attribute("enable").Value = value ? "1" : "0";
            }
        }
        public bool runPredict
        {
            get
            {
                return config.Element("appconfig").Element("settings").Element("runPredict").Attribute("enable").Value == "1" ? true : false;
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("runPredict").Attribute("enable").Value = value ? "1" : "0";
            }
        }
        public bool stationBehavior
        {
            get
            {
                return config.Element("appconfig").Element("settings").Element("stationBehavior").Attribute("behavior").Value == "1" ? true : false;
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("stationBehavior").Attribute("behavior").Value = value ? "1" : "0";
            }
        }
        public bool updatefreq {
            get
            {
                return config.Element("appconfig").Element("settings").Element("timing").Attribute("updateFreq").Value == "1" ? true : false;
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("timing").Attribute("updateFreq").Value = value ? "1" : "0";
            }
        }
        public DateTime dailynotifytime
        {
            get
            {
                return new DateTime(Convert.ToInt64(config.Element("appconfig").Element("settings").Element("timing").Attribute("dailynotifytime").Value));
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("timing").Attribute("dailynotifytime").Value = value.Ticks.ToString();
            }
        }
        public int cpcdefaultProduct
        {
            get
            {
                return Convert.ToInt32(config.Element("appconfig").Element("settings").Element("defaultProduct").Attribute("CPC").Value);
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("defaultProduct").Attribute("CPC").Value = value.ToString();
            }
        }
        public int fpccdefaultProduct
        {
            get
            {
                return Convert.ToInt32(config.Element("appconfig").Element("settings").Element("defaultProduct").Attribute("FPCC").Value);
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("defaultProduct").Attribute("FPCC").Value = value.ToString();
            }
        }
        public bool sfilterFPCC
        {
            get
            {
                return config.Element("appconfig").Element("settings").Element("stationFilters").Element("FPCC").Attribute("filter").Value == "1" ? true : false;
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("stationFilters").Element("FPCC").Attribute("filter").Value = value ? "1" : "0";
            }
        }
        public bool sfilterInservice
        {
            get
            {
                return config.Element("appconfig").Element("settings").Element("stationFilters").Element("Inservice").Attribute("filter").Value == "1" ? true : false;
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("stationFilters").Element("Inservice").Attribute("filter").Value = value ? "1" : "0";
            }
        }
        public bool sfiltercountryEnable
        {
            get
            {
                return config.Element("appconfig").Element("settings").Element("stationFilters").Element("Country").Attribute("filter").Value == "1" ? true : false;
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("stationFilters").Element("Country").Attribute("filter").Value = value ? "1" : "0";
            }
        }
        public int sfiltercountry
        {
            get
            {
                return Convert.ToInt32(config.Element("appconfig").Element("settings").Element("stationFilters").Element("Country").Value);
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("stationFilters").Element("Country").Value = value.ToString();
            }
        }
        public bool sfilterCPC
        {
            get
            {
                return config.Element("appconfig").Element("settings").Element("stationFilters").Element("CPC").Attribute("filter").Value == "1" ? true : false;
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("stationFilters").Element("CPC").Attribute("filter").Value = value ? "1" : "0";
            }
        }
        public bool sfilterDirect
        {
            get
            {
                return config.Element("appconfig").Element("settings").Element("stationFilters").Element("Direct").Attribute("filter").Value == "1" ? true : false;
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("stationFilters").Element("Direct").Attribute("filter").Value = value ? "1" : "0";
            }
        }
        public bool sfilterSelf
        {
            get
            {
                return config.Element("appconfig").Element("settings").Element("stationFilters").Element("Self").Attribute("filter").Value == "1" ? true : false;
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("stationFilters").Element("Self").Attribute("filter").Value = value ? "1" : "0";
            }
        }
        public bool sfilterFavoirte
        {
            get
            {
                return config.Element("appconfig").Element("settings").Element("stationFilters").Element("Favorite").Attribute("filter").Value == "1" ? true : false;
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("stationFilters").Element("Favorite").Attribute("filter").Value = value ? "1" : "0";
            }
        }
        public double sfilterKilonmeter
        {
            get
            {
                return Convert.ToDouble(config.Element("appconfig").Element("settings").Element("stationFilters").Element("Kilometer").Value);
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("stationFilters").Element("Kilometer").Value = value.ToString();
            }
        }
        public bool sfilterp92
        {
            get
            {
                return config.Element("appconfig").Element("settings").Element("stationFilters").Element("Product").Attribute("p92").Value == "1" ? true : false;
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("stationFilters").Element("Product").Attribute("p92").Value = value ? "1" : "0";
            }
        }
        public bool sfilterp95
        {
            get
            {
                return config.Element("appconfig").Element("settings").Element("stationFilters").Element("Product").Attribute("p95").Value == "1" ? true : false;
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("stationFilters").Element("Product").Attribute("p95").Value = value ? "1" : "0";
            }
        }
        public bool sfilterp98
        {
            get
            {
                return config.Element("appconfig").Element("settings").Element("stationFilters").Element("Product").Attribute("p98").Value == "1" ? true : false;
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("stationFilters").Element("Product").Attribute("p98").Value = value ? "1" : "0";
            }
        }
        public bool sfilterpdiesel
        {
            get
            {
                return config.Element("appconfig").Element("settings").Element("stationFilters").Element("Product").Attribute("pdiesel").Value == "1" ? true : false;
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("stationFilters").Element("Product").Attribute("pdiesel").Value = value ? "1" : "0";
            }
        }
        public bool sfilterpgasohol
        {
            get
            {
                return config.Element("appconfig").Element("settings").Element("stationFilters").Element("Product").Attribute("pgasohol").Value == "1" ? true : false;
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("stationFilters").Element("Product").Attribute("pgasohol").Value = value ? "1" : "0";
            }
        }
        public int defaultTile {
            get
            {
                return Convert.ToInt32(config.Element("appconfig").Element("tiles").Attribute("default").Value);
            }
            set
            {
                config.Element("appconfig").Element("tiles").Attribute("default").Value = value.ToString();
            }
        }
        public DateTime tileupdateTime
        {
            get
            {
                return new DateTime(Convert.ToInt64(config.Element("appconfig").Element("tiles").Attribute("updateDate").Value));
            }
            set
            {
                config.Element("appconfig").Element("tiles").Attribute("updateDate").Value = value.Ticks.ToString();
            }
        }
        public DateTime dailynotify
        {
            get
            {
                return new DateTime(Convert.ToInt64(config.Element("appconfig").Element("settings").Element("dailynotify").Value));
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("dailynotify").Value = value.Ticks.ToString();
            }
        }
        public bool CPCnotified
        {
            get
            {
                return config.Element("appconfig").Element("info").Element("notified").Attribute("CPC").Value == "1" ? true : false;
            }
            set
            {
                config.Element("appconfig").Element("info").Element("notified").Attribute("CPC").Value = value ? "1" : "0";
            }
        }
        public bool FPCCnotified
        {
            get
            {
                return config.Element("appconfig").Element("info").Element("notified").Attribute("FPCC").Value == "1" ? true : false;
            }
            set
            {
                config.Element("appconfig").Element("info").Element("notified").Attribute("FPCC").Value = value ? "1" : "0";
            }
        }
        public bool dailynotified
        {
            get
            {
                return config.Element("appconfig").Element("settings").Element("dailynotify").Attribute("notified").Value == "1" ? true : false;
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("dailynotify").Attribute("notified").Value = value ? "1" : "0";
            }
        }
        public bool dailynotifyEnable
        {
            get
            {
                return config.Element("appconfig").Element("settings").Element("dailynotify").Attribute("enable").Value == "1" ? true : false;
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("dailynotify").Attribute("enable").Value = value ? "1" : "0";
            }
        }
        public DateTime notifycheckedHour
        {
            get
            {
                return new DateTime(Convert.ToInt64(config.Element("appconfig").Element("info").Element("notified").Attribute("checkHour").Value));
            }
            set
            {
                config.Element("appconfig").Element("info").Element("notified").Attribute("checkHour").Value = value.Ticks.ToString();
            }
        }
        public DateTime dailynotifiedHour
        {
            get
            {
                return new DateTime(Convert.ToInt64(config.Element("appconfig").Element("settings").Element("dailynotify").Attribute("checkHour").Value));
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("dailynotify").Attribute("checkHour").Value = value.Ticks.ToString();
            }
        }
        public int defaultPage
        {
            get
            {
                return Convert.ToInt32(config.Element("appconfig").Element("settings").Element("defaultPage").Attribute("page").Value);
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("defaultPage").Attribute("page").Value = value.ToString();
            }
        }
        public DateTime version
        {
            get
            {
                return new DateTime(Convert.ToInt64(config.Element("appconfig").Attribute("version").Value));
            }
            set
            {
                config.Element("appconfig").Attribute("version").Value = value.Ticks.ToString();
            }
        }
        public DateTime DBcheckedDate
        {
            get
            {
                return new DateTime(Convert.ToInt64(config.Element("appconfig").Element("info").Element("updateDate").Attribute("DBcheckedDate").Value));
            }
            set
            {
                config.Element("appconfig").Element("info").Element("updateDate").Attribute("DBcheckedDate").Value = value.Ticks.ToString();
            }
        }
        public GeoPoint sfilterrangeLocation
        {
            get
            {
                return new GeoPoint(Convert.ToDouble(config.Element("appconfig").Element("settings").Element("stationFilters").Element("Kilometer").Attribute("lat").Value), Convert.ToDouble(config.Element("appconfig").Element("settings").Element("stationFilters").Element("Kilometer").Attribute("long").Value));
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("stationFilters").Element("Kilometer").Attribute("lat").Value = value.Latitude.ToString();
                config.Element("appconfig").Element("settings").Element("stationFilters").Element("Kilometer").Attribute("long").Value = value.Longitude.ToString();
            }
        }
        public string sfilterrangeLocationname
        {
            get
            {
                return config.Element("appconfig").Element("settings").Element("stationFilters").Element("Kilometer").Attribute("name").Value;
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("stationFilters").Element("Kilometer").Attribute("name").Value = value;
            }
        }
        public bool sfiltercustomLocation
        {
            get
            {
                return config.Element("appconfig").Element("settings").Element("stationFilters").Element("Kilometer").Attribute("filter").Value == "0" ? false : true;
            }
            set
            {
                config.Element("appconfig").Element("settings").Element("stationFilters").Element("Kilometer").Attribute("filter").Value = value ? "1" : "0";
            }
        }
        public List<int> productMonitor
        {
            get
            {
                return (from node in config.Element("appconfig").Element("settings").Element("productMonitor").Descendants("product") select Convert.ToInt32(node.Attribute("id").Value)).ToList();
            }
            set
            {
                IEnumerable<XElement> nlist = from v in value select new XElement("product", new XAttribute("id", v));
                config.Element("appconfig").Element("settings").Element("productMonitor").RemoveAll();
                foreach (XElement n in nlist)
                {
                    config.Element("appconfig").Element("settings").Element("productMonitor").Add(n);
                }
            }
        }
        public List<string> sfilterSubBrands
        {
            get
            {
                return (from node in config.Element("appconfig").Element("settings").Element("stationFilters").Element("Subbrand").Descendants("term") select node.Attribute("id").Value).ToList();
            }
            set
            {
                IEnumerable<XElement> nlist = from v in value select new XElement("term", new XCData(v));
                config.Element("appconfig").Element("settings").Element("stationFilters").Element("Subbrand").RemoveAll();
                foreach (XElement n in nlist)
                {
                    config.Element("appconfig").Element("settings").Element("stationFilters").Element("Subbrand").Add(n);
                }
            }
        }
        public DateTime moeaboeDBdate
        {
            get { return new DateTime(Convert.ToInt64(config.Element("appconfig").Element("info").Element("updateDate").Attribute("moeaboeDBdate").Value)); }
            set
            {
                config.Element("appconfig").Element("info").Element("updateDate").Attribute("moeaboeDBdate").Value = value.Ticks.ToString();
            }
        }
        public string discountRev
        {
            get { return config.Element("appconfig").Element("info").Element("updateDate").Attribute("discountRev").Value; }
            set
            {
                config.Element("appconfig").Element("info").Element("updateDate").Attribute("discountRev").Value = value;
            }
        }
        public DateTime dDBupdateDate
        {
            get { return new DateTime(Convert.ToInt64(config.Element("appconfig").Element("info").Element("updateDate").Attribute("dDBcheckedDate").Value)); }
            set
            {
                config.Element("appconfig").Element("info").Element("updateDate").Attribute("dDBcheckedDate").Value = value.Ticks.ToString();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}