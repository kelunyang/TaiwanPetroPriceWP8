using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Data;
using TaiwanPP.Library.Models;
using Microsoft.Phone.Tasks;
using Autofac;
using TaiwanPP.Library.ViewModels;
using SQLite.Net.Platform.WindowsPhone8;
using SQLite.Net.Interop;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage;
using TaiwanPP.Library.Helpers;
using Microsoft.Phone.Scheduler;
using Windows.Devices.Geolocation;
using System.Xml;
using System.IO.IsolatedStorage;
using Nito.AsyncEx;
using System.Device.Location;
using System.Windows.Resources;
using System.Xml.Linq;
using System.ComponentModel;

namespace TaiwanPP.WP8App
{
    public partial class MainPage : PhoneApplicationPage
    {
        SQLitePlatformWP8 sqliteplaform = new SQLitePlatformWP8();
        string DB_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, "price.sqlite");
        string XML_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, "config.xml");
        string discount_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, "creditDiscount.xml");
        infoViewModel ifvm;
        ppViewModel ppvm;
        cpViewModel cpvm;
        stationViewModel stvm;
        dcViewModel dcvm;
        discountViewModel dtvm;
        IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
        PropertyProgress<ProgressReport> progress;
        ApplicationBarIconButton filterButton;
        ApplicationBarIconButton nozzleButton;
        ApplicationBarIconButton settingButton;
        ApplicationBarIconButton updateButton;
        ApplicationBarIconButton searchButton;
        ApplicationBarIconButton mailButton;
        ApplicationBarMenuItem contactButton;
        ApplicationBarMenuItem aboutButton;
        string tilemonitor;
        private object lockobj = new object();
        bool uienable = true;
        bool autoupdate = true;
        Binding connectivitybind;
        bool executeStationQ = false;
        bool forcenavi = false;
        bool loaded = false;
        bool resetconfig = false;
        int leave = 0;

        // 建構函式
        public MainPage()
        {
            /* TODO:
             * 4. 油耗?
             */
            InitializeComponent();
            // data binding
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<infoModel>();
            builder.RegisterInstance<infoModel>(new infoModel());
            builder.RegisterType<infoViewModel>().PropertiesAutowired();
            builder.RegisterType<stationViewModel>().PropertiesAutowired();
            builder.RegisterType<cpViewModel>().PropertiesAutowired();
            builder.RegisterType<ppViewModel>().PropertiesAutowired();
            builder.RegisterType<dcViewModel>().PropertiesAutowired();
            builder.RegisterType<discountViewModel>().PropertiesAutowired();
            Autofac.IContainer container = builder.Build();
            locationPage.DataContext = container.Resolve<stationViewModel>();
            predictPage.DataContext = container.Resolve<ppViewModel>();
            currentpricePage.DataContext = container.Resolve<cpViewModel>();
            DCBulletin.DataContext = container.Resolve<dcViewModel>();
            titlebar.DataContext = container.Resolve<infoViewModel>();
            discountPage.DataContext = container.Resolve<discountViewModel>();
            //build context
            ppvm = (ppViewModel)predictPage.DataContext;
            cpvm = (cpViewModel)currentpricePage.DataContext;
            stvm = (stationViewModel)locationPage.DataContext;
            dcvm = (dcViewModel)DCBulletin.DataContext;
            ifvm = (infoViewModel)titlebar.DataContext;
            dtvm = (discountViewModel)discountPage.DataContext;
            progress = new PropertyProgress<ProgressReport>();
            progress.PropertyChanged += progress_PropertyChanged;
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Mode = ApplicationBarMode.Default;
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;
            searchButton = new ApplicationBarIconButton();
            searchButton.IconUri = new Uri(@"Assets\AppBar\search.png", UriKind.Relative);
            searchButton.Text = "搜尋";
            searchButton.Click += searchButton_Click;
            updateButton = new ApplicationBarIconButton();
            updateButton.IconUri = new Uri(@"Assets\AppBar\refresh.png", UriKind.Relative);
            updateButton.Text = "更新資料";
            updateButton.Click += refresh_Click;
            contactButton = new ApplicationBarMenuItem();
            contactButton.Text = "聯絡設計者";
            contactButton.IsEnabled = true;
            contactButton.Click += Email_Click;
            settingButton = new ApplicationBarIconButton();
            settingButton.IconUri = new Uri(@"Assets\AppBar\settings.png", UriKind.Relative);
            settingButton.IsEnabled = true;
            settingButton.Text = "設定";
            aboutButton = new ApplicationBarMenuItem();
            aboutButton.Text = "關於";
            aboutButton.IsEnabled = true;
            aboutButton.Click += aboutButton_Click;
            settingButton.Click += settingButton_Click;
            ApplicationBar.MenuItems.Add(contactButton);
            ApplicationBar.MenuItems.Add(aboutButton);
            ApplicationBar.Buttons.Add(updateButton);
            ApplicationBar.Buttons.Add(settingButton);
            filterButton = new ApplicationBarIconButton();
            filterButton.IconUri = new Uri(@"Assets\AppBar\filter.png", UriKind.Relative);
            filterButton.IsEnabled = true;
            filterButton.Text = "過濾加油站";
            filterButton.Click += filterButton_Click;
            nozzleButton = new ApplicationBarIconButton();
            nozzleButton.IconUri = new Uri(@"Assets\AppBar\nozzle.png", UriKind.Relative);
            nozzleButton.IsEnabled = true;
            nozzleButton.Text = "儲存價格";
            nozzleButton.Click += nozzleButton_Click;
            mailButton = new ApplicationBarIconButton();
            mailButton.IconUri = new Uri(@"Assets\AppBar\mail.png", UriKind.Relative);
            mailButton.IsEnabled = true;
            mailButton.Text = "通報優惠";
            mailButton.Click += mailButton_Click;
            connectivity();
            connectivitybind = new Binding();
            connectivitybind.Source = ifvm.connectivity;
            connectivitybind.Converter = new TaiwanPP.WP8App.Helpers.connectivitystringConverter();
            connectivitybar.SetBinding(TextBlock.TextProperty, connectivitybind);
            /*
            unityContainer = new UnityContainer();
            unityContainer.RegisterInstance<viewmodelInterface>((viewmodelBase)settings.DataContext);
            unityContainer.RegisterType<viewmodelInterface, infoViewModel>("infoModel");
            unityContainer.RegisterType<viewmodelInterface, stationViewModel>("infoModel");
            unityContainer.RegisterType<viewmodelInterface, ppViewModel>("infoModel");
            unityContainer.RegisterType<viewmodelInterface, cpViewModel>("infoModel");
            unityContainer.RegisterType<viewmodelInterface, dcViewModel>("infoModel");
            settings.DataContext = unityContainer.Resolve<infoViewModel>();
            locationPage.DataContext = unityContainer.Resolve<stationViewModel>();
            predictPage.DataContext = unityContainer.Resolve<ppViewModel>();
            currentpricePage.DataContext = unityContainer.Resolve<cpViewModel>();
            fpccpricePage.DataContext = unityContainer.Resolve<cpViewModel>();
            aboutPage.DataContext = unityContainer.Resolve<dcViewModel>();
            ((viewmodelBase)locationPage.DataContext).ivm = ((viewmodelBase)locationPage.DataContext).ivm;
            ((viewmodelBase)predictPage.DataContext).ivm = ((viewmodelBase)locationPage.DataContext).ivm;
            ((viewmodelBase)currentpricePage.DataContext).ivm = ((viewmodelBase)locationPage.DataContext).ivm;
            ((viewmodelBase)fpccpricePage.DataContext).ivm = ((viewmodelBase)locationPage.DataContext).ivm;
            ((viewmodelBase)aboutPage.DataContext).ivm = ((viewmodelBase)locationPage.DataContext).ivm;
            */
            // 將清單方塊控制項的資料內容設為範例資料
        }

        void mailButton_Click(object sender, EventArgs e)
        {
            if (System.Windows.MessageBox.Show("歡迎提供作者關於油價折扣的資訊，可以的話請您附上網址！", "提供油價折扣資訊", System.Windows.MessageBoxButton.OKCancel) == System.Windows.MessageBoxResult.OK)
            {
                Microsoft.Phone.Tasks.EmailComposeTask emailComposeTask = new Microsoft.Phone.Tasks.EmailComposeTask();
                emailComposeTask.Subject = "Windows Phone油價查詢聯絡信（油價折扣）";
                emailComposeTask.To = "kelunyang@outlook.com";
                emailComposeTask.Body = "以下是範本，不一定要完整填寫\n加油站品牌：\n銀行名稱：\n信用卡卡種：\n折扣內容：\n折扣期間：\n";
                emailComposeTask.Show();
            }
        }

        void aboutButton_Click(object sender, EventArgs e)
        {
            if (uienable) NavigationService.Navigate(new Uri("/about.xaml", UriKind.Relative));
        }

        void nozzleButton_Click(object sender, EventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(async () =>
            {
                titlebar.Visibility = Visibility.Collapsed;
                if(uienable) await cpvm.savePrice(progress);
                MessageBox.Show("價格儲存完成後，可以點選油品名稱進入歷史查詢頁面中進行比較", "價格儲存完成", MessageBoxButton.OK);
                titlebar.Visibility = Visibility.Visible;
            });
        }

        void filterButton_Click(object sender, EventArgs e)
        {
            if (uienable)
            {
                resetconfig = true;
                NavigationService.Navigate(new Uri("/filter.xaml", UriKind.Relative));
            }
        }

        void settingButton_Click(object sender, EventArgs e)
        {
            if (uienable)
            {
                resetconfig = true;
                NavigationService.Navigate(new Uri("/settings.xaml", UriKind.Relative));
            }
        }
        private void CPCFilter(object sender, FilterEventArgs e)
        {
            if (((priceStorage)e.Item).current)
            {
                if (((priceStorage)e.Item).brand == 0)
                {
                    e.Accepted = true;
                }
                else
                {
                    e.Accepted = false;
                }
            }
            else
            {
                e.Accepted = false;
            }
        }
        private void monitorFilter(object sender, FilterEventArgs e)
        {
            if (((priceStorage)e.Item).monitored)
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = false;
            }
        }
        private void FPCCFilter(object sender, FilterEventArgs e)
        {
            if (((priceStorage)e.Item).current)
            {
                if (((priceStorage)e.Item).brand == 1)
                {
                    e.Accepted = true;
                }
                else
                {
                    e.Accepted = false;
                }
            }
            else
            {
                e.Accepted = false;
            }
        }
        private void savedCPCFilter(object sender, FilterEventArgs e)
        {
            if (((priceStorage)e.Item).brand == 0)
            {
                if (((priceStorage)e.Item).saved)
                {
                    e.Accepted = true;
                }
                else
                {
                    e.Accepted = false;
                }
            }
            else
            {
                e.Accepted = false;
            }
        }
        private void savedFPCCFilter(object sender, FilterEventArgs e)
        {
            if (((priceStorage)e.Item).brand == 1)
            {
                if (((priceStorage)e.Item).saved)
                {
                    e.Accepted = true;
                }
                else
                {
                    e.Accepted = false;
                }
            }
            else
            {
                e.Accepted = false;
            }
        }
        private void phone_Click(object sender, RoutedEventArgs e)
        {
            PhoneCallTask phoneCallTask = new PhoneCallTask();
            phoneCallTask.PhoneNumber = ((stationStorage)((MenuItem)sender).DataContext).phone;
            phoneCallTask.DisplayName = ((stationStorage)((MenuItem)sender).DataContext).name;
            phoneCallTask.Show();
        }

        private void linkhistorical(object sender, RoutedEventArgs e)
        {
            viewhistorical((priceStorage)((FrameworkElement)sender).DataContext);
        }

        private void viewhistorical(priceStorage pi) {
            if(uienable) NavigationService.Navigate(new Uri("/historicalPage.xaml?kind=" + pi.kind, UriKind.Relative));
        }

        private void App_Loaded(object sender, RoutedEventArgs e)
        {
            if (!loaded)
            {
                Deployment.Current.Dispatcher.BeginInvoke(async () =>
                {
                    if (uienable)
                    {
                        try
                        {
                            uiLock(false);
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
                            using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(discount_PATH, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, isoStore))
                            {
                                await dtvm.loadXML(isf);
                                isf.Close();
                                isf.Dispose();
                            }
                            if (ifvm.firstLoad) NavigationService.Navigate(new Uri("/fastConfig.xaml", UriKind.Relative));
                            PanoramaRoot.DefaultItem = State.Keys.Contains("defaultpage") ? PanoramaRoot.Items[(int)State["defaultpage"]] : PanoramaRoot.Items[ifvm.defaultPage]; //頁面控制
                            if (ifvm.upgrade)
                            {
                                MessageBox.Show("系統設定檔升級，以強制更新您的設定檔，請重新復原您的設定（包括動態磚）", "升級提醒", MessageBoxButton.OK);
                                ifvm.upgrade = false;
                            }
                            tilescan();
                            await cpvm.loadDB(sqliteplaform, DB_PATH);
                            await cpvm.buildDB();
                            //await cpvm.fetchPrice(connectivity, progress);
                            //await cpvm.currentPrice(progress);
                            await ppvm.loadDB(sqliteplaform, DB_PATH);
                            //await ppvm.predictedPrice(connectivity, cpvm.currentCollections[typeDB.CPC95.key].currentPrice, progress);
                            uienable = true;
                            await stvm.loadDB(sqliteplaform, DB_PATH);
                            if (autoupdate) await updateOnline(ppvm.runPredict);    //跳頁時不用重跑
                            //await dcvm.load(progress);
                            using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(XML_PATH, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, isoStore))
                            {
                                ifvm.saveConfig(isf);
                                isf.Close();
                                isf.Dispose();
                            }
                            backgroundInjector();
                            if (executeStationQ) queryStation(false, false);
                            loaded = true;
                            uiLock(true);
                        }
                        catch (Exception ex)
                        {
                            uiLock(true);
                            systemtray.IsVisible = false;
                            if (MessageBox.Show(ex.Message + "，如果您希望將此錯誤回報給開發者，請按確定，按取消關閉訊息", "發生錯誤", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                            {
                                contactMail("錯誤訊息如下：" + ex.Message + "，錯誤追蹤：" + ex.StackTrace);
                            }
                        }
                    }
                });
            }
            else
            {
                if (resetconfig)
                {
                    uiLock(false);
                    Deployment.Current.Dispatcher.BeginInvoke(async () =>
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
                        await stvm.loadDB(sqliteplaform, DB_PATH);
                        resetconfig = false;
                    });
                    uiLock(true);
                }
            }
        }

        private void tilescan()
        {
            cpvm.tiles = (from x in ShellTile.ActiveTiles where x.NavigationUri.OriginalString != "/" select Convert.ToInt32(x.NavigationUri.OriginalString.Substring(x.NavigationUri.OriginalString.IndexOf("kind=")+5, 1))).ToList();
        }

        async Task updateOnline(bool runpredict)
        {
            if (uienable)
            {
                try
                {
                    uiLock(false);
                    connectivity();
                    ifvm.firstLoad = false;
                    ifvm.connectivity = ifvm.dbcheckedDate.AddMinutes(5) < DateTime.Now ? ifvm.connectivity : false;
                    await cpvm.fetchPrice(ifvm.connectivity, progress);
                    await cpvm.currentPrice(progress, false);
                    NavigationContext.QueryString.TryGetValue("kind", out tilemonitor);
                    if (tilemonitor != null)
                    {
                        IEnumerable<priceStorage> ps = from p in cpvm.currentCollections where p.kind == Convert.ToInt32(tilemonitor) select p;
                        if (ps.Any()) await cpvm.monitorProduct(ps.First(), progress, true);
                    }
                    IEnumerable<priceStorage> defaulttile = from p in cpvm.currentCollections where p.kind == ifvm.defaultTile select p;
                    if (defaulttile.Any()) await cpvm.monitorProduct(defaulttile.First(), progress, true);
                    IEnumerable<priceStorage> product95 = from p in cpvm.currentCollections where p.kind == typeDB.CPC95.key select p;
                    IEnumerable<priceStorage> productdiesel = from p in cpvm.currentCollections where p.kind == typeDB.CPCdiesel.key select p;
                    if (product95.Any())
                    {
                        await ppvm.predictedPrice(ifvm.connectivity, runpredict, progress);
                        ppvm.getPrice(product95.First().price, productdiesel.First().price);
                    }
                    await dcvm.load(ifvm.connectivity, progress);
                    await dcvm.buildList(true, progress);
                    TimeSpan sdb = new TimeSpan(stvm.stationDBnotifyDate.Ticks);
                    TimeSpan now = new TimeSpan(DateTime.Now.Ticks);
                    if (now.Subtract(sdb).Days > 30)    //每一個月發動一次更新
                    {
                        if (System.Windows.MessageBox.Show("加油站資料庫已有約" + now.Subtract(sdb).Days + "日未更新，請確定網路穩定下按是進行更新，按否將展延一個月更新（若太久不更新，會因為Google API數量而限制無法查詢加油站經緯度）", "更新加油站資料庫", System.Windows.MessageBoxButton.OKCancel) == System.Windows.MessageBoxResult.OK)
                        {
                            await stvm.updateCPC(progress);
                            await stvm.updateFPCC(progress);
                        }
                        else
                        {
                            stvm.stationDBnotifyDate = DateTime.Now;
                        }
                    }
                    if (ifvm.dbcheckedDate.AddMinutes(5) < DateTime.Now) ifvm.dbcheckedDate = DateTime.Now;
                    TimeSpan dupdate = new TimeSpan(dtvm.dDBcheckedDate.Ticks);
                    bool updatedtXML = false;
                    if (now.Subtract(dupdate).Days > 30)    //每一個月發動一次更新
                    {
                        if (System.Windows.MessageBox.Show("折扣資料庫已有約" + now.Subtract(dupdate).Days + "日未更新，請確定網路穩定下按是進行更新，按否將展延一個月更新（作者每季會整理各信用卡折扣，但並不固定於哪一天上傳折扣訊息，請見諒）", "更新折扣資料庫", System.Windows.MessageBoxButton.OKCancel) == System.Windows.MessageBoxResult.OK)
                        {
                            using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(discount_PATH, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, isoStore))
                            {
                                updatedtXML = await dtvm.updateXML(progress, isf);
                            }
                        }
                    }
                    if (updatedtXML)
                    {
                        using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(discount_PATH, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, isoStore))
                        {
                            await dtvm.loadXML(isf);
                            isf.Close();
                            isf.Dispose();
                        }
                    }
                    using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(XML_PATH, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, isoStore))
                    {
                        ifvm.saveConfig(isf);
                        isf.Close();
                        isf.Dispose();
                    }
                    uiLock(true);
                }
                catch (Exception ex)
                {
                    uiLock(true);
                    systemtray.IsVisible = false;
                    if (MessageBox.Show(ex.Message+"，如果您希望將此錯誤回報給開發者，請按確定，按取消關閉訊息", "發生錯誤", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        contactMail("錯誤訊息如下：" + ex.Message + "，錯誤追蹤：" + ex.StackTrace);
                    }
                }
            }
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
        private void monitor_Click(object sender, RoutedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(async () =>
            {
                await cpvm.monitorProduct((priceStorage)((MenuItem)sender).DataContext, progress, false);
            });
        }
        private void settile_Click(object sender, RoutedEventArgs e)
        {
            bool create = ((MenuItem)sender).Header.ToString().Equals("設定為預設") ? true : false;
            if (create)
            {
                tileChange((priceStorage)((MenuItem)sender).DataContext, create);
            }
            else
            {
                if (((priceStorage)((MenuItem)sender).DataContext).tile)
                {
                    List<ShellTile> nodefault = (from x in ShellTile.ActiveTiles where x.NavigationUri.OriginalString != "/" select x).ToList();
                    IEnumerable<ShellTile> tile = from x in nodefault where Convert.ToInt32(x.NavigationUri.OriginalString.Substring(x.NavigationUri.OriginalString.IndexOf("kind=")+5, 1)) == ((priceStorage)((MenuItem)sender).DataContext).kind select x;
                    if(tile.Any()) tile.First().Delete();
                    ((priceStorage)((MenuItem)sender).DataContext).tile = false;
                }
                else
                {
                    tileChange((priceStorage)((MenuItem)sender).DataContext, create);
                    ((priceStorage)((MenuItem)sender).DataContext).tile = true;
                }
            }
        }
        private void tileChange(priceStorage ps, bool create)
        {
            ppViewModel pp = (ppViewModel)((FrameworkElement)predictPage).DataContext;
            try
            {
                backgroundInjector();
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    uiLock(false);
                    systemtray.IsVisible = true;
                    systemtray.IsIndeterminate = true;
                    systemtray.Text = "開始製作動態磚";
                    TaiwanPP.Tile.tile Tile = new TaiwanPP.Tile.tile();
                    Tile.DataContext = ((cpViewModel)currentpricePage.DataContext).tileExport(typeDB.productnameDB[ps.kind], pp.penddate, pp.pstartdate, pp.pprice, pp.pdprice);
                    Tile.SavePNGComplete += ((s, Tilearg) =>
                    {
                        FlipTileData tileData = new FlipTileData
                        {
                            Title = "",
                            WideBackgroundImage = new Uri("isostore:" + Tilearg.ImageFileName[2], UriKind.Absolute),
                            WideBackBackgroundImage = new Uri("isostore:" + Tilearg.ImageFileName[4], UriKind.Absolute),
                            BackgroundImage = new Uri("isostore:" + Tilearg.ImageFileName[0], UriKind.Absolute),
                            BackBackgroundImage = new Uri("isostore:" + Tilearg.ImageFileName[3], UriKind.Absolute),
                            SmallBackgroundImage = new Uri("isostore:" + Tilearg.ImageFileName[1], UriKind.Absolute)
                        };
                        string tileUri = create ? "/" : "/MainPage.xaml?kind=" + ps.kind;
                        ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Equals(tileUri));
                        if (tile != null)
                        {
                            tile.Update(tileData);
                        }
                        else
                        {
                            ShellTile.Create(new Uri(tileUri, UriKind.Relative), tileData, true);
                        }
                        if (create)
                        {
                            ifvm.defaultTile = ps.kind;
                        }
                        systemtray.IsVisible = false;
                        uiLock(true);
                    });
                    Tile.BeginSavePNG(typeDB.productnameDB[ps.kind], true);
                });
            }
            catch (systemException)
            {
                throw new systemException("製作動態磚");
            }
        }
        private void backgroundInjector()
        {
            try
            {
                foreach (PeriodicTask oldTask in ScheduledActionService.GetActions<PeriodicTask>())
                {
                    ScheduledActionService.Remove(oldTask.Name);
                }
                PeriodicTask tileTask = new PeriodicTask("oilTilePeriodicAgent");
                tileTask.Description = "定期抓取油價公告，更新動態磚，並在價格調整時發出提醒";
                //if (ScheduledActionService.Find(tileTask.Name) != null) ScheduledActionService.Remove(tileTask.Name);
                ScheduledActionService.Add(tileTask);
                //ScheduledActionService.LaunchForTest(tileTask.Name, TimeSpan.FromSeconds(10));    //test code, must be commented when released
            }
            catch
            {
                throw new systemException("背景工作");   //無法建立背景工作
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(async () =>
            {
                switch (PanoramaRoot.SelectedIndex)
                {
                    case 3:
                        queryStation(stvm.sfiltercustomLocation, stvm.sfiltercountryEnable);
                        break;
                    case 4:
                        await dtvm.queryDiscount(progress);
                        break;
                }
            });
        }
        private void queryStation(bool custom, bool country)
        {
            Deployment.Current.Dispatcher.BeginInvoke(async () =>
            {
                if (uienable)
                {
                    try
                    {
                        uiLock(false);
                        GeoPoint gp = new GeoPoint(0, 0);
                        if (!country)
                        {
                            if (!custom)
                            {
                                systemtray.IsVisible = true;
                                systemtray.IsIndeterminate = true;
                                systemtray.Text = "取得目前所在位置";
                                Geolocator geolocator = new Geolocator();
                                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                                maximumAge: TimeSpan.FromMinutes(5),
                                timeout: TimeSpan.FromSeconds(10)
                                );
                                systemtray.Text = "計算鄰近加油站";
                                gp = new GeoPoint(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);
                            }
                            else
                            {
                                gp = stvm.sfilterlocation;
                            }
                            await stvm.queryStation(gp, progress);
                        }
                        else
                        {
                            await stvm.queryStation(gp, progress);
                        }
                        systemtray.IsVisible = false;
                        uiLock(true);
                        executeStationQ = false;
                        if (forcenavi)
                        {
                            if (stvm.queryStations.Any())   //第一個是我的最愛的時候會失敗，所以要在longselection裡面能判斷favorite
                            {
                                IEnumerable<longlistCollection<stationStorage>> nearlist = (from col in stvm.queryStations where col.favorite == false orderby col.distance select col).Take(1);
                                if (nearlist.Any())
                                {
                                    if (nearlist.First().Any())
                                    {
                                        stationNavigate(nearlist.First().First());
                                    }
                                    else
                                    {
                                        MessageBox.Show("指定的搜尋範圍內找不到最近的加油站", "找不到加油站", MessageBoxButton.OK);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("指定的搜尋範圍內找不到最近的加油站", "找不到加油站", MessageBoxButton.OK);
                                }
                            }
                            else
                            {
                                MessageBox.Show("指定的搜尋範圍內找不到最近的加油站", "找不到加油站", MessageBoxButton.OK);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        uiLock(true);
                        executeStationQ = false;
                        systemtray.IsVisible = false;
                        string message = ex.Message.Contains("timeout") ? "GPS定位逾時，請再次嘗試" : ex.Message.Contains("turned on location") ? "請開啟GPS再行使用" : ex.Message;
                        if (MessageBox.Show(message + "，如果您希望將此錯誤回報給開發者，請按確定，按取消關閉訊息", "發生錯誤", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            contactMail("錯誤訊息如下：" + ex.Message + "，錯誤追蹤：" + ex.StackTrace);
                        }
                    }
                }
            });
        }
        private void querynearbystation_Click(object sender, RoutedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(async () =>
            {
                stationStorage s = (stationStorage)((FrameworkElement)sender).DataContext;
                await stvm.queryStation(s.coordinance, progress);
                mapcontrol.ZoomLevel = 13.5;
            });
        }
        /*protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }*/
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {

            if (NavigationContext.QueryString.ContainsKey("voiceCommandName")) {
                int defaultpage = 0;
                if (NavigationContext.QueryString["voiceCommandName"] == "navigation")
                {
                    defaultpage = 3;
                    autoupdate = false;
                    executeStationQ = true;
                    forcenavi = true;
                }
                else
                {
                    switch (NavigationContext.QueryString["commands"])
                    {
                        case "油價預測":
                            defaultpage = 1;
                            break;
                        case "油价预测":
                            defaultpage = 1;
                            break;
                        case "本周油價":
                            defaultpage = 2;
                            break;
                        case "本周油价":
                            defaultpage = 2;
                            break;
                        case "附近的加油站":
                            autoupdate = false;
                            defaultpage = 3;
                            executeStationQ = true;
                            break;
                    }
                }
                if (!State.Keys.Contains("defaultpage"))
                {
                    State.Add("defaultpage", defaultpage);
                }
                else
                {
                    State["defaultpage"] = defaultpage;
                }
            }
            if (!State.Keys.Contains("defaultpage"))
            {
                State.Add("defaultpage", ifvm.defaultPage);
            }
            base.OnNavigatedTo(e);
        }
        private void refresh_Click(object sender, EventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(async () =>
            {
                try
                {
                    if (uienable)
                    {
                        await updateOnline(true);
                        uiLock(true);
                    }
                }
                catch(Exception ex)
                {
                    uiLock(true);
                    systemtray.IsVisible = false;
                    if (MessageBox.Show(ex.Message + "，如果您希望將此錯誤回報給開發者，請按確定，按取消關閉訊息", "發生錯誤", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        contactMail("錯誤訊息如下：" + ex.Message + "，錯誤追蹤：" + ex.StackTrace);
                    }
                }
            });
        }
        private void Email_Click(object sender, EventArgs e)
        {
            if (System.Windows.MessageBox.Show("如果使用上遇到問題，歡迎隨時與我聯絡", "聯絡設計者？", System.Windows.MessageBoxButton.OKCancel) == System.Windows.MessageBoxResult.OK)
            {
                contactMail("");
            }
        }

        private void contactMail(string content)
        {
            if (System.Windows.MessageBox.Show("如果您願意提供您App設定，將有助於除錯，如果不願意請按否", "包含App設定？", System.Windows.MessageBoxButton.OKCancel) == System.Windows.MessageBoxResult.OK)
            {
                content += "\n config.xml內容： \n"+ifvm.im.export();
            }
            Microsoft.Phone.Tasks.EmailComposeTask emailComposeTask = new Microsoft.Phone.Tasks.EmailComposeTask();
            emailComposeTask.Subject = "Windows Phone油價查詢聯絡信";
            emailComposeTask.To = "kelunyang@outlook.com";
            emailComposeTask.Body = content;
            emailComposeTask.Show();
        }

        private void station_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            stationStorage s = (stationStorage)((FrameworkElement)sender).DataContext;
            if (stvm.stationBehavior)
            {
                stvm.centerloc = s.coordinance;
                mapcontrol.ZoomLevel = 16;
            }
            else
            {
                stationNavigate(s);
            }
        }
        private void station_Click(object sender, RoutedEventArgs e)
        {
            stationStorage s = (stationStorage)((FrameworkElement)sender).DataContext;
            stationNavigate(s);
        }
        private void savestation_Click(object sender, RoutedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(async () =>
            {
                uiLock(false);
                await stvm.saveStation((stationStorage)((FrameworkElement)sender).DataContext, progress);
                uiLock(true);
            });
        }
        void stationNavigate(stationStorage s)
        {
            forcenavi = false;
            string brand = s.brand == 0 ? "中油" : "台塑";
            MapsDirectionsTask mapsDirectionsTask = new MapsDirectionsTask();
            LabeledMapLocation spaceNeedleLML = new LabeledMapLocation(brand + s.name, new GeoCoordinate(s.latitude, s.longitude));
            mapsDirectionsTask.End = spaceNeedleLML;
            //mapsDirectionsTask.Start = new LabeledMapLocation("aa", new GeoCoordinate(25.0170, 121.4500));  //test location
            if(uienable) mapsDirectionsTask.Show();
        }

        private void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dictionary<int, ApplicationBarIconButton[]> buttonlist = new Dictionary<int, ApplicationBarIconButton[]>();
            buttonlist.Add(4, new ApplicationBarIconButton[] { searchButton, mailButton, settingButton });
            buttonlist.Add(3, new ApplicationBarIconButton[] { searchButton, filterButton });
            buttonlist.Add(1, new ApplicationBarIconButton[] { settingButton });
            buttonlist.Add(2, new ApplicationBarIconButton[] { nozzleButton });
            buttonlist.Add(0, new ApplicationBarIconButton[] { settingButton });
            foreach (KeyValuePair<int, ApplicationBarIconButton[]> kp in buttonlist)
            {
                if (kp.Key == ((Panorama)sender).SelectedIndex) continue;
                foreach (ApplicationBarIconButton btn in kp.Value)
                {
                    if (ApplicationBar.Buttons.Contains(btn)) ApplicationBar.Buttons.Remove(btn);
                }
            }
            foreach(ApplicationBarIconButton btn in buttonlist[((Panorama)sender).SelectedIndex])
            {
                if(!ApplicationBar.Buttons.Contains(btn)) ApplicationBar.Buttons.Add(btn);
            }
        }

        void uiLock(bool flag)
        {
            titlebar.Visibility = flag ? Visibility.Visible : Visibility.Collapsed;
            uienable = flag;
            updateButton.IsEnabled = flag;
            searchButton.IsEnabled = flag;
            settingButton.IsEnabled = flag;
            filterButton.IsEnabled = flag;
            nozzleButton.IsEnabled = flag;
            aboutButton.IsEnabled = flag;
            PhoneApplicationService.Current.UserIdleDetectionMode = flag ? IdleDetectionMode.Enabled : IdleDetectionMode.Disabled;  //enable/disable screen timeout
        }

        void connectivity()
        {
            ifvm.connectivity = Microsoft.Phone.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }

        private void Border_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            viewhistorical((priceStorage)((FrameworkElement)sender).DataContext);
        }

        private void station_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            stationStorage s = (stationStorage)((FrameworkElement)sender).DataContext;
            stationNavigate(s);
        }

        private void mapcontrol_Loaded(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = "e0ec2f02-94d1-46a1-9286-1207dffe0db9";
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = "wc8L6ouFdDXsLyIjkoVoew";
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(XML_PATH, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, isoStore))
            {
                ifvm.saveConfig(isf);
                isf.Close();
                isf.Dispose();
            }
            e.Cancel = true;
            leave++;
            if (leave == 2)
            {
                e.Cancel = false;
            }
            else
            {
                exitanimation.Stop();
                exitanimation.Begin();
            }
        }

    }
}