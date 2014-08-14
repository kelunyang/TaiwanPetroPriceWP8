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
using System.IO.IsolatedStorage;
using System.IO;
using Windows.Storage;
using Nito.AsyncEx;
using TaiwanPP.Library.Helpers;
using Windows.Phone.Speech.VoiceCommands;

namespace TaiwanPP.WP8App
{
    public partial class fastConfig : PhoneApplicationPage
    {
        string XML_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, "config.xml");
        infoViewModel ifvm;
        stationViewModel stvm;
        cpViewModel cpvm;
        dcViewModel dcvm;
        IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
        ApplicationBarIconButton savebutton;
        PropertyProgress<ProgressReport> progress;
        public fastConfig()
        {
            InitializeComponent();
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<infoModel>();
            builder.RegisterInstance<infoModel>(new infoModel());
            builder.RegisterType<infoViewModel>().PropertiesAutowired();
            builder.RegisterType<stationViewModel>().PropertiesAutowired();
            builder.RegisterType<cpViewModel>().PropertiesAutowired();
            builder.RegisterType<dcViewModel>().PropertiesAutowired();
            IContainer container = builder.Build();
            LayoutRoot.DataContext = container.Resolve<infoViewModel>();
            stationfilters.DataContext = container.Resolve<stationViewModel>();
            soapcontrol.DataContext = container.Resolve<cpViewModel>();
            bulletin.DataContext = container.Resolve<dcViewModel>();
            ifvm = (infoViewModel)LayoutRoot.DataContext;
            stvm = (stationViewModel)stationfilters.DataContext;
            cpvm = (cpViewModel)soapcontrol.DataContext;
            dcvm = (dcViewModel)bulletin.DataContext;
            progress = new PropertyProgress<ProgressReport>();
            progress.PropertyChanged += progress_PropertyChanged;
            using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(XML_PATH, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, isoStore))
            {
                ifvm.loadConfig(isf);
                isf.Close();
                isf.Dispose();
            }
            ifvm.firstLoad = false;
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Mode = ApplicationBarMode.Default;
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;
            savebutton = new ApplicationBarIconButton();
            savebutton.IconUri = new Uri(@"Assets\AppBar\next.png", UriKind.Relative);
            savebutton.Text = "儲存設定";
            savebutton.Click += savebutton_Click;
            ApplicationBar.Buttons.Add(savebutton);
            RegisterVoiceCommands();    //註冊語音指令
        }

        private async void RegisterVoiceCommands()
        {
            await VoiceCommandService.InstallCommandSetsFromFileAsync(new Uri("ms-appx:///vCommand.xml", UriKind.RelativeOrAbsolute));
        }

        void savebutton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
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

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(async () =>
            {
                try
                {
                    titlebar.Visibility = System.Windows.Visibility.Collapsed;
                    ifvm.connectivity = Microsoft.Phone.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
                    await dcvm.load(ifvm.connectivity, progress);
                    dcvm.buildList(false);
                    titlebar.Visibility = System.Windows.Visibility.Visible;
                }
                catch (Exception ex)
                {
                    titlebar.Visibility = System.Windows.Visibility.Visible;
                    systemtray.IsVisible = false;
                    if (MessageBox.Show(ex.Message + "，如果您希望將此錯誤回報給開發者，請按確定，按取消關閉訊息", "發生錯誤", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Microsoft.Phone.Tasks.EmailComposeTask emailComposeTask = new Microsoft.Phone.Tasks.EmailComposeTask();
                        emailComposeTask.Subject = "Windows Phone油價查詢聯絡信";
                        emailComposeTask.To = "kelunyang@outlook.com";
                        emailComposeTask.Body = "錯誤訊息如下：" + ex.Message + "，錯誤追蹤：" + ex.StackTrace;
                        emailComposeTask.Show();
                    }
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
    }
}