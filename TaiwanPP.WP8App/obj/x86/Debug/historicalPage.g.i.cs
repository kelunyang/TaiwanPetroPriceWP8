﻿#pragma checksum "D:\OneDrive\程式\TaiwanPetrolWP8\TaiwanPP.WP8App\historicalPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2578A7292A0A40A3929E4EB5D951F29C"
//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.42000
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using OxyPlot.WP8;
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
    
    
    public partial class historicalPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal Microsoft.Phone.Controls.PhoneApplicationPage historicalchartPage;
        
        internal Microsoft.Phone.Shell.ProgressIndicator systemtray;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel ContentPanel;
        
        internal Microsoft.Phone.Controls.ListPicker peroidSelector;
        
        internal OxyPlot.WP8.PlotView chart;
        
        internal System.Windows.Controls.StackPanel stackPanel;
        
        internal System.Windows.Controls.TextBlock predict;
        
        internal System.Windows.Controls.StackPanel titlebar;
        
        internal System.Windows.Controls.TextBlock title;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/TaiwanPP.WP8App;component/historicalPage.xaml", System.UriKind.Relative));
            this.historicalchartPage = ((Microsoft.Phone.Controls.PhoneApplicationPage)(this.FindName("historicalchartPage")));
            this.systemtray = ((Microsoft.Phone.Shell.ProgressIndicator)(this.FindName("systemtray")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.StackPanel)(this.FindName("ContentPanel")));
            this.peroidSelector = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("peroidSelector")));
            this.chart = ((OxyPlot.WP8.PlotView)(this.FindName("chart")));
            this.stackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("stackPanel")));
            this.predict = ((System.Windows.Controls.TextBlock)(this.FindName("predict")));
            this.titlebar = ((System.Windows.Controls.StackPanel)(this.FindName("titlebar")));
            this.title = ((System.Windows.Controls.TextBlock)(this.FindName("title")));
        }
    }
}

