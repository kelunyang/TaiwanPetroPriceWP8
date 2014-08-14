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
using System.IO.IsolatedStorage;
using Windows.Storage;
using System.IO;
using TaiwanPP.Library.Models;
using Autofac;
using SQLite.Net.Platform.WindowsPhone8;
using System.Windows.Data;
using Nito.AsyncEx;
using TaiwanPP.Library.Helpers;

namespace TaiwanPP.WP8App
{
    public partial class filter : PhoneApplicationPage
    {
        string XML_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, "config.xml");
        string DB_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, "price.sqlite");
        stationViewModel stvm;
        infoViewModel ifvm;
        IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
        SQLitePlatformWP8 sqliteplaform = new SQLitePlatformWP8();
        int selectionlock = 0;
        PropertyProgress<ProgressReport> progress;
        ApplicationBarIconButton savebutton;

        public filter()
        {
            InitializeComponent();
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<infoModel>();
            builder.RegisterInstance<infoModel>(new infoModel());
            builder.RegisterType<stationViewModel>().PropertiesAutowired();
            builder.RegisterType<infoViewModel>().PropertiesAutowired();
            IContainer container = builder.Build();
            LayoutRoot.DataContext = container.Resolve<stationViewModel>();
            titlebar.DataContext = container.Resolve<infoViewModel>();
            stvm = (stationViewModel)LayoutRoot.DataContext;
            ifvm = (infoViewModel)titlebar.DataContext;
            progress = new PropertyProgress<ProgressReport>();
            progress.PropertyChanged += progress_PropertyChanged;
            ifvm.connectivity = Microsoft.Phone.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(XML_PATH, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, isoStore))
            {
                stvm.loadConfig(isf);
                isf.Close();
                isf.Dispose();
            }
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Mode = ApplicationBarMode.Default;
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;
            savebutton = new ApplicationBarIconButton();
            savebutton.IconUri = new Uri(@"Assets\AppBar\save.png", UriKind.Relative);
            savebutton.Text = "儲存設定";
            savebutton.Click += savebutton_Click;
            ApplicationBar.Buttons.Add(savebutton);
        }

        void savebutton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void save()
        {
            using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(XML_PATH, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, isoStore))
            {
                stvm.saveConfig(isf);
                isf.Close();
                isf.Dispose();
            }
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            save();
            base.OnNavigatedFrom(e);
        }

        private void App_Loaded(object sender, RoutedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(async () =>
            {
                await stvm.loadDB(sqliteplaform, DB_PATH);
                countrySelector.SelectedIndex = stvm.sfiltercountry;
            });
        }

        private void countrySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ListPicker)sender).SelectedIndex == -1) { 
                selectionlock += 1;
            }
            else if (selectionlock == 1)
            {
                selectionlock += 1;
            }
            stvm.sfiltercountry = selectionlock == 0 ? ((ListPicker)sender).SelectedIndex : stvm.sfiltercountry;
            if (selectionlock == 2) selectionlock = 0;
        }

        private void queryCustom_Click(object sender, RoutedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(async () =>
            {
                    try
                    {
                        title.Visibility = System.Windows.Visibility.Collapsed;
                        PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
                        await stvm.queryCustomLocation(progress, customlocation.Text);
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
        void progress_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyProgress<ProgressReport> obj = (PropertyProgress<ProgressReport>)sender;
            ProgressReport pr = obj.Progress;
            systemtray.IsVisible = pr.display;
            systemtray.IsIndeterminate = pr.display;
            systemtray.Text = pr.progressMessage;
            systemtray.Value = pr.progress;
        }

        private void mapcontrol_Loaded(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = "e0ec2f02-94d1-46a1-9286-1207dffe0db9";
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = "wc8L6ouFdDXsLyIjkoVoew";
        }
    }
}