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
using System.IO.IsolatedStorage;

namespace TaiwanPP.WP8App
{
    public partial class historicalPage : PhoneApplicationPage
    {
        string kind;
        string DB_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, "price.sqlite");
        string XML_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, "config.xml");
        IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
        infoViewModel ifvm;
        ppViewModel ppvm;

        public historicalPage()
        {
            InitializeComponent();
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<infoModel>();
            builder.RegisterInstance<infoModel>(new infoModel());
            builder.RegisterType<cpViewModel>().PropertiesAutowired();
            builder.RegisterType<infoViewModel>().PropertiesAutowired();
            builder.RegisterType<ppViewModel>().PropertiesAutowired();
            IContainer container = builder.Build();
            historicalchartPage.DataContext = container.Resolve<cpViewModel>();
            titlebar.DataContext = container.Resolve<infoViewModel>();
            predict.DataContext = container.Resolve<ppViewModel>();
            ifvm = (infoViewModel)titlebar.DataContext;
            ppvm = (ppViewModel)predict.DataContext;
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
                    title.Visibility = System.Windows.Visibility.Collapsed;
                    systemtray.IsVisible = true;
                    systemtray.Text = "載入油價變化資料...";
                    systemtray.IsIndeterminate = true;
                    using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(XML_PATH, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, isoStore))
                    {
                        ifvm.loadConfig(isf);
                        isf.Close();
                        isf.Dispose();
                    }
                    NavigationContext.QueryString.TryGetValue("kind", out kind);
                    cpViewModel cvm = (cpViewModel)historicalchartPage.DataContext;
                    await cvm.loadDB(new SQLitePlatformWP8(), DB_PATH);
                    await cvm.buildDB();
                    await ppvm.loadDB(new SQLitePlatformWP8(), DB_PATH);
                    if (double.IsNaN(ppvm.pprice))
                    {
                        await cvm.currentPrice(progress, true);
                        await ppvm.predictedPrice(ifvm.connectivity, true, progress);
                        IEnumerable<double> p95 = from item in cvm.currentCollections where item.kind == typeDB.CPC95.key select item.price;
                        IEnumerable<double> pdiesel = from item in cvm.currentCollections where item.kind == typeDB.CPCdiesel.key select item.price;
                        if (p95.Any())
                        {
                            if (pdiesel.Any())
                            {
                                ppvm.getPrice(p95.First(), pdiesel.First());
                            }
                        }
                    }
                    double pp = kind == "4" || kind == "8" ? ppvm.pdprice : ppvm.pprice;
                    cvm.historicalPrice(pp, ppvm.predictpause, kind, progress);
                    title.Visibility = System.Windows.Visibility.Visible;
                }
                catch (Exception ex)
                {
                    title.Visibility = System.Windows.Visibility.Visible;
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