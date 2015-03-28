using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Autofac;
using TaiwanPP.Library.Models;
using TaiwanPP.Library.ViewModels;
using SQLite.Net.Platform.WindowsPhone8;
using Windows.Storage;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Tasks;

namespace TaiwanPP.WP8App
{
    public partial class about : PhoneApplicationPage
    {
        SQLitePlatformWP8 sqliteplaform = new SQLitePlatformWP8();
        string DB_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, "price.sqlite");
        string XML_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, "config.xml");
        infoViewModel ifvm;
        cpViewModel cpvm;
        stationViewModel stvm;
        discountViewModel dtvm;
        IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
        bool loaded = false;

        public about()
        {
            InitializeComponent();
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<infoModel>();
            builder.RegisterInstance<infoModel>(new infoModel());
            builder.RegisterType<infoViewModel>().PropertiesAutowired();
            builder.RegisterType<stationViewModel>().PropertiesAutowired();
            builder.RegisterType<cpViewModel>().PropertiesAutowired();
            builder.RegisterType<ppViewModel>().PropertiesAutowired();
            builder.RegisterType<dcViewModel>().PropertiesAutowired();
            builder.RegisterType<discountViewModel>().PropertiesAutowired();
            IContainer container = builder.Build();
            station.DataContext = container.Resolve<stationViewModel>();
            pricedb.DataContext = container.Resolve<cpViewModel>();
            info.DataContext = container.Resolve<infoViewModel>();
            discount.DataContext = container.Resolve<discountViewModel>();
            //build context
            ifvm = (infoViewModel)info.DataContext;
            cpvm = (cpViewModel)pricedb.DataContext;
            stvm = (stationViewModel)station.DataContext;
            dtvm = (discountViewModel)discount.DataContext;
            ifvm.connectivity = Microsoft.Phone.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            if (!loaded)
            {
                using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(XML_PATH, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, isoStore))
                {
                    ifvm.loadConfig(isf);
                    isf.Close();
                    isf.Dispose();
                }
                using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(XML_PATH, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, isoStore))
                {
                    stvm.loadConfig(isf);
                    isf.Close();
                    isf.Dispose();
                }
                using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(XML_PATH, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, isoStore))
                {
                    dtvm.loadConfig(isf);
                    isf.Close();
                    isf.Dispose();
                }
                loaded = true;
            }
        }

        private void Email_Click(object sender, EventArgs e)
        {
            if (System.Windows.MessageBox.Show("如果使用上遇到問題，歡迎隨時與我聯絡", "聯絡設計者？", System.Windows.MessageBoxButton.OKCancel) == System.Windows.MessageBoxResult.OK)
            {
                Microsoft.Phone.Tasks.EmailComposeTask emailComposeTask = new Microsoft.Phone.Tasks.EmailComposeTask();
                emailComposeTask.Subject = "Windows Phone油價查詢聯絡信";
                emailComposeTask.To = "kelunyang@outlook.com";
                emailComposeTask.Show();
            }
        }

        private void survey_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri(@"https://onedrive.live.com/redir?page=survey&resid=C836A59DFA673571!49792&authkey=!APrZLL0aHQkJzvQ&ithint=file%2c.xlsx", UriKind.Absolute);
            webBrowserTask.Show();
        }
        private void Website_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri(@"https://bitbucket.org/kelunyang/taiwan-petrol-price", UriKind.Absolute);
            webBrowserTask.Show();
        }
    }
}