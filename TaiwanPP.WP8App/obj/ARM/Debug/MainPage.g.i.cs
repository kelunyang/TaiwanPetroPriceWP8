﻿#pragma checksum "D:\onedrive\文件\TaiwanPetroPriceWP8\TaiwanPP.WP8App\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "353E42FEE24C64512FB23C01E89819AA"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
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
        
        internal System.Windows.Data.CollectionViewSource monitoredCollection;
        
        internal Microsoft.Phone.Shell.ProgressIndicator systemtray;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.VisualStateGroup VisualStateGroup;
        
        internal System.Windows.VisualState Load;
        
        internal Microsoft.Phone.Controls.Panorama PanoramaRoot;
        
        internal Microsoft.Phone.Controls.PanoramaItem digestPage;
        
        internal System.Windows.Controls.ItemsControl monitoritems;
        
        internal System.Windows.Controls.ItemsControl DCBulletin;
        
        internal Microsoft.Phone.Controls.PanoramaItem predictPage;
        
        internal System.Windows.Controls.StackPanel forcastbulletin;
        
        internal Microsoft.Phone.Controls.PanoramaItem currentpricePage;
        
        internal System.Windows.Controls.ItemsControl CPCitems;
        
        internal System.Windows.Controls.ItemsControl FPCCitems;
        
        internal Microsoft.Phone.Controls.PanoramaItem locationPage;
        
        internal System.Windows.Controls.ItemsControl filters;
        
        internal Microsoft.Phone.Maps.Controls.Map mapcontrol;
        
        internal Microsoft.Phone.Controls.LongListSelector stationLLS;
        
        internal Microsoft.Phone.Controls.PanoramaItem discountPage;
        
        internal Microsoft.Phone.Controls.ListPicker brandSelector;
        
        internal Microsoft.Phone.Controls.ListPicker servetypeSelector;
        
        internal Microsoft.Phone.Controls.ListPicker bankSelector;
        
        internal System.Windows.Controls.Grid grid;
        
        internal System.Windows.Controls.StackPanel titlebar;
        
        internal System.Windows.Controls.TextBlock connectivitybar;
        
        internal System.Windows.Controls.StackPanel exitbar;
        
        internal System.Windows.Media.Animation.Storyboard exitanimation;
        
        internal System.Windows.Controls.Grid splash;
        
        internal System.Windows.Controls.Canvas 圖層_1;
        
        internal System.Windows.Shapes.Path path;
        
        internal System.Windows.Shapes.Path path1;
        
        internal System.Windows.Shapes.Path path2;
        
        internal System.Windows.Shapes.Path path3;
        
        internal System.Windows.Shapes.Path path4;
        
        internal System.Windows.Shapes.Path path5;
        
        internal System.Windows.Shapes.Path path6;
        
        internal System.Windows.Shapes.Path path7;
        
        internal System.Windows.Shapes.Path path8;
        
        internal System.Windows.Controls.TextBlock OldWP8Alarm;
        
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
            this.monitoredCollection = ((System.Windows.Data.CollectionViewSource)(this.FindName("monitoredCollection")));
            this.systemtray = ((Microsoft.Phone.Shell.ProgressIndicator)(this.FindName("systemtray")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.VisualStateGroup = ((System.Windows.VisualStateGroup)(this.FindName("VisualStateGroup")));
            this.Load = ((System.Windows.VisualState)(this.FindName("Load")));
            this.PanoramaRoot = ((Microsoft.Phone.Controls.Panorama)(this.FindName("PanoramaRoot")));
            this.digestPage = ((Microsoft.Phone.Controls.PanoramaItem)(this.FindName("digestPage")));
            this.monitoritems = ((System.Windows.Controls.ItemsControl)(this.FindName("monitoritems")));
            this.DCBulletin = ((System.Windows.Controls.ItemsControl)(this.FindName("DCBulletin")));
            this.predictPage = ((Microsoft.Phone.Controls.PanoramaItem)(this.FindName("predictPage")));
            this.forcastbulletin = ((System.Windows.Controls.StackPanel)(this.FindName("forcastbulletin")));
            this.currentpricePage = ((Microsoft.Phone.Controls.PanoramaItem)(this.FindName("currentpricePage")));
            this.CPCitems = ((System.Windows.Controls.ItemsControl)(this.FindName("CPCitems")));
            this.FPCCitems = ((System.Windows.Controls.ItemsControl)(this.FindName("FPCCitems")));
            this.locationPage = ((Microsoft.Phone.Controls.PanoramaItem)(this.FindName("locationPage")));
            this.filters = ((System.Windows.Controls.ItemsControl)(this.FindName("filters")));
            this.mapcontrol = ((Microsoft.Phone.Maps.Controls.Map)(this.FindName("mapcontrol")));
            this.stationLLS = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("stationLLS")));
            this.discountPage = ((Microsoft.Phone.Controls.PanoramaItem)(this.FindName("discountPage")));
            this.brandSelector = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("brandSelector")));
            this.servetypeSelector = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("servetypeSelector")));
            this.bankSelector = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("bankSelector")));
            this.grid = ((System.Windows.Controls.Grid)(this.FindName("grid")));
            this.titlebar = ((System.Windows.Controls.StackPanel)(this.FindName("titlebar")));
            this.connectivitybar = ((System.Windows.Controls.TextBlock)(this.FindName("connectivitybar")));
            this.exitbar = ((System.Windows.Controls.StackPanel)(this.FindName("exitbar")));
            this.exitanimation = ((System.Windows.Media.Animation.Storyboard)(this.FindName("exitanimation")));
            this.splash = ((System.Windows.Controls.Grid)(this.FindName("splash")));
            this.圖層_1 = ((System.Windows.Controls.Canvas)(this.FindName("圖層_1")));
            this.path = ((System.Windows.Shapes.Path)(this.FindName("path")));
            this.path1 = ((System.Windows.Shapes.Path)(this.FindName("path1")));
            this.path2 = ((System.Windows.Shapes.Path)(this.FindName("path2")));
            this.path3 = ((System.Windows.Shapes.Path)(this.FindName("path3")));
            this.path4 = ((System.Windows.Shapes.Path)(this.FindName("path4")));
            this.path5 = ((System.Windows.Shapes.Path)(this.FindName("path5")));
            this.path6 = ((System.Windows.Shapes.Path)(this.FindName("path6")));
            this.path7 = ((System.Windows.Shapes.Path)(this.FindName("path7")));
            this.path8 = ((System.Windows.Shapes.Path)(this.FindName("path8")));
            this.OldWP8Alarm = ((System.Windows.Controls.TextBlock)(this.FindName("OldWP8Alarm")));
        }
    }
}

