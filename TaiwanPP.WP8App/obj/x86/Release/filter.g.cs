﻿#pragma checksum "D:\onedrive\文件\TaiwanPetroPriceWP8\TaiwanPP.WP8App\filter.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "78D8D91EDC38CC788C3EB1844685C9D6"
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
    
    
    public partial class filter : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal Microsoft.Phone.Shell.ProgressIndicator systemtray;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBox customlocation;
        
        internal Microsoft.Phone.Maps.Controls.Map mapcontrol;
        
        internal Microsoft.Phone.Controls.Rating rating;
        
        internal Microsoft.Phone.Controls.ListPicker countrySelector;
        
        internal Microsoft.Phone.Controls.ToggleSwitch sfilterSelf;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/TaiwanPP.WP8App;component/filter.xaml", System.UriKind.Relative));
            this.systemtray = ((Microsoft.Phone.Shell.ProgressIndicator)(this.FindName("systemtray")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.customlocation = ((System.Windows.Controls.TextBox)(this.FindName("customlocation")));
            this.mapcontrol = ((Microsoft.Phone.Maps.Controls.Map)(this.FindName("mapcontrol")));
            this.rating = ((Microsoft.Phone.Controls.Rating)(this.FindName("rating")));
            this.countrySelector = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("countrySelector")));
            this.sfilterSelf = ((Microsoft.Phone.Controls.ToggleSwitch)(this.FindName("sfilterSelf")));
            this.titlebar = ((System.Windows.Controls.StackPanel)(this.FindName("titlebar")));
            this.title = ((System.Windows.Controls.TextBlock)(this.FindName("title")));
        }
    }
}

