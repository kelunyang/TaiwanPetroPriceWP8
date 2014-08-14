using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TaiwanPP.Library.ViewModels;
using System.Threading.Tasks;
using Autofac;
using TaiwanPP.Library.Models;
using SQLite.Net.Platform.WindowsPhone8;
using System.IO;
using Windows.Storage;
using Nito.AsyncEx;
using TaiwanPP.Library.Helpers;

namespace TaiwanPP.WP8App
{
    public partial class historicalPage : PhoneApplicationPage
    {
        string predictPrice;
        string predictDate;
        string kind;
        string DB_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, "price.sqlite");
        infoViewModel ifvm;
        public historicalPage()
        {
            InitializeComponent();
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<infoModel>();
            builder.RegisterInstance<infoModel>(new infoModel());
            builder.RegisterType<cpViewModel>().PropertiesAutowired();
            builder.RegisterType<infoViewModel>().PropertiesAutowired();
            IContainer container = builder.Build();
            historicalchartPage.DataContext = container.Resolve<cpViewModel>();
            titlebar.DataContext = container.Resolve<infoViewModel>();
            ifvm = (infoViewModel)titlebar.DataContext;
            ifvm.connectivity = Microsoft.Phone.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(async () =>
            {
                var progress = new PropertyProgress<ProgressReport>();
                progress.PropertyChanged += progress_PropertyChanged;
                try
                {
                    NavigationContext.QueryString.TryGetValue("predictPrice", out predictPrice);
                    NavigationContext.QueryString.TryGetValue("predictDate", out predictDate);
                    NavigationContext.QueryString.TryGetValue("kind", out kind);
                    cpViewModel cvm = (cpViewModel)historicalchartPage.DataContext;
                    await cvm.loadDB(new SQLitePlatformWP8(), DB_PATH);
                    await cvm.buildDB();
                    double pp = predictPrice == "" ? double.NaN : Convert.ToDouble(predictPrice);
                    cvm.historicalPrice(pp, Convert.ToInt64(DateTime.Parse(predictDate).Ticks), kind, progress);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "載入資料發生錯誤", MessageBoxButton.OK);
                }
            });
        }

        private void progress_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyProgress<ProgressReport> obj = (PropertyProgress<ProgressReport>)sender;
            ProgressReport pr = obj.Progress;
            systemtray.IsVisible = pr.display;
            systemtray.IsIndeterminate = pr.display;
            systemtray.Text = pr.progressMessage;
            systemtray.Value = pr.progress;
        }
    }
}