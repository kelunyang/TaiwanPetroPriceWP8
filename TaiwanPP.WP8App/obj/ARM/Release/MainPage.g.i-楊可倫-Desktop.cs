﻿#pragma checksum "D:\Documents\Skydrive\文件\TaiwanPetroPriceWP8\TaiwanPP.WP8App\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F3E748CEFF2ABDCA543B435C580FF49D"
//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.34014
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace TaiwanPP.WP8App {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal Microsoft.Phone.Controls.PhoneApplicationPage root;
        
        internal System.Windows.Data.CollectionViewSource CPCcollection;
        
        internal System.Windows.Data.CollectionViewSource FPCCcollection;
        
        internal System.Windows.Data.CollectionViewSource SAVEDCPCcollection;
        
        internal System.Windows.Data.CollectionViewSource SAVEDFPCCcollection;
        
        internal Microsoft.Phone.Shell.ProgressIndicator systemtray;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.VisualStateGroup VisualStateGroup;
        
        internal System.Windows.VisualState Load;
        
        internal Microsoft.Phone.Controls.Panorama PanoramaRoot;
        
        internal Microsoft.Phone.Controls.PanoramaItem predictPage;
        
        internal System.Windows.Controls.StackPanel forcastbulletin;
        
        internal Microsoft.Phone.Controls.PanoramaItem currentpricePage;
        
        internal System.Windows.Controls.ItemsControl CPCitems;
        
        internal System.Windows.Controls.ItemsControl FPCCitems;
        
        internal Microsoft.Phone.Controls.PanoramaItem savedpricePage;
        
        internal System.Windows.Controls.ItemsControl SAVEDCPCitems;
        
        internal System.Windows.Controls.ItemsControl SAVEDFPCCitems;
        
        internal Microsoft.Phone.Controls.PanoramaItem locationPage;
        
        internal System.Windows.Controls.ItemsControl filters;
        
        internal System.Windows.Controls.Button nearbysBtn;
        
        internal Microsoft.Phone.Controls.LongListSelector stationLLS;
        
        internal Microsoft.Phone.Controls.PanoramaItem aboutPage;
        
        internal System.Windows.Controls.ItemsControl DCBulletin;
        
        internal System.Windows.Controls.Canvas by_sa;
        
        internal System.Windows.Controls.TextBlock title;
        
        internal System.Windows.Controls.Grid splash;
        
        internal System.Windows.Controls.Canvas 圖層_1;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/TaiwanPP.WP8App;component/MainPage.xaml", System.UriKind.Relative));
            this.root = ((Microsoft.Phone.Controls.PhoneApplicationPage)(this.FindName("root")));
            this.CPCcollection = ((System.Windows.Data.CollectionViewSource)(this.FindName("CPCcollection")));
            this.FPCCcollection = ((System.Windows.Data.CollectionViewSource)(this.FindName("FPCCcollection")));
            this.SAVEDCPCcollection = ((System.Windows.Data.CollectionViewSource)(this.FindName("SAVEDCPCcollection")));
            this.SAVEDFPCCcollection = ((System.Windows.Data.CollectionViewSource)(this.FindName("SAVEDFPCCcollection")));
            this.systemtray = ((Microsoft.Phone.Shell.ProgressIndicator)(this.FindName("systemtray")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.VisualStateGroup = ((System.Windows.VisualStateGroup)(this.FindName("VisualStateGroup")));
            this.Load = ((System.Windows.VisualState)(this.FindName("Load")));
            this.PanoramaRoot = ((Microsoft.Phone.Controls.Panorama)(this.FindName("PanoramaRoot")));
            this.predictPage = ((Microsoft.Phone.Controls.PanoramaItem)(this.FindName("predictPage")));
            this.forcastbulletin = ((System.Windows.Controls.StackPanel)(this.FindName("forcastbulletin")));
            this.currentpricePage = ((Microsoft.Phone.Controls.PanoramaItem)(this.FindName("currentpricePage")));
            this.CPCitems = ((System.Windows.Controls.ItemsControl)(this.FindName("CPCitems")));
            this.FPCCitems = ((System.Windows.Controls.ItemsControl)(this.FindName("FPCCitems")));
            this.savedpricePage = ((Microsoft.Phone.Controls.PanoramaItem)(this.FindName("savedpricePage")));
            this.SAVEDCPCitems = ((System.Windows.Controls.ItemsControl)(this.FindName("SAVEDCPCitems")));
            this.SAVEDFPCCitems = ((System.Windows.Controls.ItemsControl)(this.FindName("SAVEDFPCCitems")));
            this.locationPage = ((Microsoft.Phone.Controls.PanoramaItem)(this.FindName("locationPage")));
            this.filters = ((System.Windows.Controls.ItemsControl)(this.FindName("filters")));
            this.nearbysBtn = ((System.Windows.Controls.Button)(this.FindName("nearbysBtn")));
            this.stationLLS = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("stationLLS")));
            this.aboutPage = ((Microsoft.Phone.Controls.PanoramaItem)(this.FindName("aboutPage")));
            this.DCBulletin = ((System.Windows.Controls.ItemsControl)(this.FindName("DCBulletin")));
            this.by_sa = ((System.Windows.Controls.Canvas)(this.FindName("by_sa")));
            this.title = ((System.Windows.Controls.TextBlock)(this.FindName("title")));
            this.splash = ((System.Windows.Controls.Grid)(this.FindName("splash")));
            this.圖層_1 = ((System.Windows.Controls.Canvas)(this.FindName("圖層_1")));
        }
    }
}

