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
using Autofac;
using System.IO;
using Windows.Storage;
using System.IO.IsolatedStorage;
using TaiwanPP.Library.Models;
using System.Windows.Resources;
using System.Xml.Linq;

namespace TaiwanPP.WP8App
{
    public partial class settings : PhoneApplicationPage
    {
        string XML_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, "config.xml");
        string discount_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, "creditDiscount.xml");
        infoViewModel ifvm;
        ppViewModel ppvm;
        stationViewModel stvm;
        cpViewModel cpvm;
        IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
        ApplicationBarIconButton savebutton;

        public settings()
        {
            InitializeComponent();
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<infoModel>();
            builder.RegisterInstance<infoModel>(new infoModel());
            builder.RegisterType<infoViewModel>().PropertiesAutowired();
            builder.RegisterType<ppViewModel>().PropertiesAutowired();
            builder.RegisterType<stationViewModel>().PropertiesAutowired();
            builder.RegisterType<cpViewModel>().PropertiesAutowired();
            builder.RegisterType<discountViewModel>().PropertiesAutowired();
            IContainer container = builder.Build();
            LayoutRoot.DataContext = container.Resolve<infoViewModel>();
            predictcontrol.DataContext = container.Resolve<ppViewModel>();
            stationBeavior.DataContext = container.Resolve<stationViewModel>();
            soapcontrol.DataContext = container.Resolve<cpViewModel>();
            ifvm = (infoViewModel)LayoutRoot.DataContext;
            ppvm = (ppViewModel)predictcontrol.DataContext;
            stvm = (stationViewModel)stationBeavior.DataContext;
            cpvm = (cpViewModel)soapcontrol.DataContext;
            using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(XML_PATH, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, isoStore))
            {
                ifvm.loadConfig(isf);
                isf.Close();
                isf.Dispose();
            }
            ifvm.connectivity = Microsoft.Phone.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
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
        }

        void revertButton_Click(object sender, RoutedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(async () =>
            {
                if (MessageBox.Show("還原原始設定後您也必須重設預設動態磚，請注意", "還原警告", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    string filename = "config.xml";
                    string assetfilename = @"Assets\" + filename;
                    StreamResourceInfo streamInfo = Application.GetResourceStream(new Uri(assetfilename, UriKind.Relative));
                    XDocument assxd = XDocument.Load(streamInfo.Stream);
                    streamInfo.Stream.Close();
                    assxd.Element("appconfig").Element("info").Element("sysinfo").Attribute("upgrade").Value = "0";
                    assxd.Element("appconfig").Element("info").Element("sysinfo").Attribute("firstload").Value = "0";
                    using (IsolatedStorageFile isFile = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(filename, FileMode.Create, isFile))
                        {
                            assxd.Save(fileStream);
                            fileStream.Close();
                        }
                    }
                    filename = "creditDiscount.xml";
                    assetfilename = @"Assets\" + filename;
                    streamInfo = Application.GetResourceStream(new Uri(assetfilename, UriKind.Relative));
                    using (IsolatedStorageFile isFile = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(filename, FileMode.Create, isFile))
                        {
                            using (StreamWriter writer = new StreamWriter(fileStream))
                            {
                                using (StreamReader reader = new StreamReader(streamInfo.Stream))
                                {
                                    writer.WriteLine(reader.ReadToEnd());
                                    writer.Close();
                                }
                            }
                        }
                    }
                    filename = "price.sqlite";
                    assetfilename = @"Assets\" + filename;
                    streamInfo = Application.GetResourceStream(new Uri(assetfilename, UriKind.Relative));
                    using (IsolatedStorageFile isFile = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(filename, FileMode.Create, isFile))
                        {
                            using (BinaryWriter writer = new BinaryWriter(fileStream))
                            {
                                Stream resourceStream = streamInfo.Stream;
                                long length = resourceStream.Length;
                                byte[] buffer = new byte[32];
                                int readCount = 0;
                                using (BinaryReader reader = new BinaryReader(streamInfo.Stream))
                                {
                                    // read file in chunks in order to reduce memory consumption and increase performance
                                    while (readCount < length)
                                    {
                                        int actual = reader.Read(buffer, 0, buffer.Length);
                                        readCount += actual;
                                        writer.Write(buffer, 0, actual);
                                    }
                                }
                            }
                        }
                    }
                    using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(XML_PATH, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, isoStore))
                    {
                        ifvm.loadConfig(isf);
                        isf.Close();
                        isf.Dispose();
                    }
                    MessageBox.Show("程式重置完成，建議重啟App（不過仍可正常使用）", "還原警告", MessageBoxButton.OK);
                }
            });
        }

    }
}