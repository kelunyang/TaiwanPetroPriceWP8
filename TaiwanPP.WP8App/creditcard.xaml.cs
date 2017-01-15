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
    public partial class creditcard : PhoneApplicationPage
    {
        string DB_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, "price.sqlite");
        string XML_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, "config.xml");
        string discount_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, "creditDiscount.xml");
        IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
        infoViewModel ifvm;
        discountViewModel ccvm;
        SQLitePlatformWP8 sqliteplaform = new SQLitePlatformWP8();
        ApplicationBarIconButton savebutton;
        ApplicationBarIconButton refreshbutton;
        PropertyProgress<ProgressReport> progress;

        public creditcard()
        {
            InitializeComponent();
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<infoModel>();
            builder.RegisterInstance<infoModel>(new infoModel());
            builder.RegisterType<discountViewModel>().PropertiesAutowired();
            builder.RegisterType<infoViewModel>().PropertiesAutowired();
            IContainer container = builder.Build();
            discountList.DataContext = container.Resolve<discountViewModel>();
            titlebar.DataContext = container.Resolve<infoViewModel>();
            ifvm = (infoViewModel)titlebar.DataContext;
            ccvm = (discountViewModel)discountList.DataContext;
            ifvm.connectivity = Microsoft.Phone.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(XML_PATH, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, isoStore))
            {
                ifvm.loadConfig(isf);
                isf.Close();
                isf.Dispose();
            }
            progress = new PropertyProgress<ProgressReport>();
            progress.PropertyChanged += progress_PropertyChanged;
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Mode = ApplicationBarMode.Default;
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;
            savebutton = new ApplicationBarIconButton();
            savebutton.IconUri = new Uri(@"Assets\AppBar\save.png", UriKind.Relative);
            savebutton.Text = "儲存設定";
            savebutton.Click += savebutton_Click;
            refreshbutton = new ApplicationBarIconButton();
            refreshbutton.IconUri = new Uri(@"Assets\AppBar\refresh.png", UriKind.Relative);
            refreshbutton.Text = "更新折扣";
            refreshbutton.Click += refreshbutton_Click;
            ApplicationBar.Buttons.Add(savebutton);
            ApplicationBar.Buttons.Add(refreshbutton);
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(async () =>
            {
                await ccvm.loadDB(sqliteplaform, DB_PATH);
                using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(discount_PATH, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, isoStore))
                {
                    await ccvm.loadXML(isf);
                    isf.Close();
                    isf.Dispose();
                }
            });
        }

        void savebutton_Click(object sender, EventArgs e)
        {

        }

        void refreshbutton_Click(object sender, EventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(async () =>
            {
                try
                {
                    title.Visibility = System.Windows.Visibility.Collapsed;
                    PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
                    using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(discount_PATH, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, isoStore))
                    {
                        await ccvm.updateXML(progress, isf);
                        isf.Close();
                        isf.Dispose();
                    }
                    systemtray.IsVisible = false;
                    title.Visibility = System.Windows.Visibility.Visible;
                    PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Enabled;
                }
                catch (Exception ex)
                {
                    systemtray.IsVisible = false;
                    MessageBox.Show(ex.Message, "發生錯誤", MessageBoxButton.OK);
                }
            });
        }

        private void save()
        {
            using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(XML_PATH, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, isoStore))
            {
                ifvm.saveConfig(isf);
                isf.Close();
                isf.Dispose();
            }
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            save();
            base.OnNavigatedFrom(e);
        }

        void progress_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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