﻿#pragma checksum "C:\Users\kelun\OneDrive\程式\TaiwanPetrolWP8\TaiwanPP.WP8App\settings.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9CB1D2EB1F6261F3B1B77A7DA1278A84"
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
    
    
    public partial class settings : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBlock predictcontrol;
        
        internal System.Windows.Controls.TextBlock soapcontrol;
        
        internal Microsoft.Phone.Controls.ListPicker pageSelector;
        
        internal System.Windows.Controls.TextBlock stationBeavior;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/TaiwanPP.WP8App;component/settings.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.predictcontrol = ((System.Windows.Controls.TextBlock)(this.FindName("predictcontrol")));
            this.soapcontrol = ((System.Windows.Controls.TextBlock)(this.FindName("soapcontrol")));
            this.pageSelector = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("pageSelector")));
            this.stationBeavior = ((System.Windows.Controls.TextBlock)(this.FindName("stationBeavior")));
            this.title = ((System.Windows.Controls.TextBlock)(this.FindName("title")));
        }
    }
}

