﻿#pragma checksum "D:\onedrive\文件\TaiwanPetroPriceWP8\TaiwanPP.WP8App\fastConfig.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0B49253DBCB2F897D091EF7CC094EFDB"
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
    
    
    public partial class fastConfig : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal Microsoft.Phone.Shell.ProgressIndicator systemtray;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.StackPanel bulletin;
        
        internal Microsoft.Phone.Controls.ToggleSwitch sfilterFPCC;
        
        internal System.Windows.Controls.TextBlock soapcontrol;
        
        internal System.Windows.Controls.StackPanel stationfilters;
        
        internal System.Windows.Controls.StackPanel titlebar;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/TaiwanPP.WP8App;component/fastConfig.xaml", System.UriKind.Relative));
            this.systemtray = ((Microsoft.Phone.Shell.ProgressIndicator)(this.FindName("systemtray")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.bulletin = ((System.Windows.Controls.StackPanel)(this.FindName("bulletin")));
            this.sfilterFPCC = ((Microsoft.Phone.Controls.ToggleSwitch)(this.FindName("sfilterFPCC")));
            this.soapcontrol = ((System.Windows.Controls.TextBlock)(this.FindName("soapcontrol")));
            this.stationfilters = ((System.Windows.Controls.StackPanel)(this.FindName("stationfilters")));
            this.titlebar = ((System.Windows.Controls.StackPanel)(this.FindName("titlebar")));
        }
    }
}

