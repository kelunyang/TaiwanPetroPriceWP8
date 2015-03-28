using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using System.IO;
using Windows.Storage;
using System;
using Microsoft.Phone.Shell;
using System.Collections.Generic;
using TaiwanPP.Tile;
using TaiwanPP.schedulerAgent.Helpers;
using System.Linq;
using TaiwanPP.Library.ViewModels;
using SQLite.Net.Platform.WindowsPhone8;
using System.Threading.Tasks;
using Autofac;
using TaiwanPP.Library.Models;
using TaiwanPP.Library.Helpers;
using System.Xml;
using System.IO.IsolatedStorage;
using Nito.AsyncEx;

namespace TaiwanPP.schedulerAgent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        string DB_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, "price.sqlite");
        string XML_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, "config.xml");
        string discount_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, "creditDiscount.xml");
        List<int> active = new List<int>();
        cpViewModel cpvm = new cpViewModel();
        infoViewModel ifvm = new infoViewModel();
        ppViewModel ppvm = new ppViewModel();
        discountViewModel dtvm = new discountViewModel();
        SQLitePlatformWP8 sqliteplaform = new SQLitePlatformWP8();
        IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
        bool connectivity = false;
        bool insequence = false;
        bool lastevent = true;
        /// <remarks>
        /// ScheduledAgent 建構函式，會初始化 UnhandledException 處理常式
        /// </remarks>
        static ScheduledAgent()
        {
            // 訂閱 Managed 例外狀況處理常式
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        private async Task startTileTask(ScheduledTask task)
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<infoModel>();
            builder.RegisterInstance<infoModel>(new infoModel());
            builder.RegisterType<infoViewModel>().PropertiesAutowired();
            builder.RegisterType<stationViewModel>().PropertiesAutowired();
            builder.RegisterType<cpViewModel>().PropertiesAutowired();
            builder.RegisterType<ppViewModel>().PropertiesAutowired();
            IContainer container = builder.Build();
            ppvm = container.Resolve<ppViewModel>();
            cpvm = container.Resolve<cpViewModel>();
            ifvm = container.Resolve<infoViewModel>();
            dtvm = container.Resolve<discountViewModel>();
            connectivity = Microsoft.Phone.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            PeriodicTask tileTask = (PeriodicTask)ScheduledActionService.Find("oilTilePeriodicAgent");
            lastevent = tileTask.LastExitReason == AgentExitReason.Completed ? true : false;
            using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(XML_PATH, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, isoStore))
            {
                ifvm.loadConfig(isf);
                isf.Close();
                isf.Dispose();
            }
            using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(XML_PATH, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, isoStore))
            {
                dtvm.loadConfig(isf);
                isf.Close();
                isf.Dispose();
            }
            if (connectivity)
            {
                var progress = new PropertyProgress<ProgressReport>();
                long originalDate = cpvm.moeaboeDBdate.Ticks;
                await cpvm.loadDB(sqliteplaform, DB_PATH);
                await cpvm.buildDB();
                await ppvm.loadDB(sqliteplaform, DB_PATH);
                if ((int)DateTime.Today.DayOfWeek == 0)
                {
                    if (ifvm.notifycheckedHour.Hour != DateTime.Now.Hour)
                    {
                        await cpvm.fetchPrice(connectivity, progress);
                        await cpvm.currentPrice(progress, true);
                        ifvm.notifycheckedHour = DateTime.Now;
                        if (originalDate - cpvm.moeaboeDBdate.Ticks != 0)
                        {
                            insequence = true;
                            IEnumerable<priceStorage> product95 = from p in cpvm.currentCollections where p.kind == typeDB.CPC95.key select p;
                            IEnumerable<priceStorage> productdiesel = from p in cpvm.currentCollections where p.kind == typeDB.CPCdiesel.key select p;
                            if (product95.Any())
                            {
                                await ppvm.predictedPrice(ifvm.connectivity, true, progress);
                                ppvm.getPrice(product95.First().price, productdiesel.First().price);
                            }
                            IEnumerable<int> brands = cpvm.save.Select(p => p.brand).Distinct();
                            foreach (int b in brands)
                            {
                                switch (b)
                                {
                                    case 0:
                                        if (!ifvm.CPCnotified) toast(b, cpvm.CPC95Change, cpvm.CPCdieselChange);
                                        ifvm.CPCnotified = true;
                                        break;
                                    case 1:
                                        if (!ifvm.FPCCnotified) toast(b, cpvm.FPCC95Change, cpvm.FPCCdieselChange);
                                        ifvm.FPCCnotified = true;
                                        break;
                                }
                            }
                            using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(XML_PATH, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, isoStore))
                            {
                                ifvm.saveConfig(isf);
                                isf.Close();
                                isf.Dispose();
                            }
                            GC.Collect();
                            tilescan();
                            runtile();
                        }
                        using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(XML_PATH, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, isoStore))
                        {
                            ifvm.saveConfig(isf);
                            isf.Close();
                            isf.Dispose();
                        }
                    }
                }
                else if ((int)DateTime.Now.DayOfWeek != 1)
                {
                    ifvm.notifycheckedHour = DateTime.Now;
                    ifvm.CPCnotified = false;
                    ifvm.FPCCnotified = false;
                    if (DateTime.Now.Day != ifvm.dailynotify.Day)
                    {
                        if (DateTime.Now.Hour == ifvm.dailynotifytime.Hour)
                        {
                            if (!ifvm.dailynotified)
                            {
                                insequence = true;
                                ifvm.dailynotified = true;
                                ifvm.dailycheckHour = DateTime.Now;
                                connectivity = ifvm.dbcheckedDate.AddMinutes(5) < DateTime.Now ? connectivity : false;
                                await cpvm.fetchPrice(connectivity, progress);
                                await cpvm.currentPrice(progress, true);
                                IEnumerable<priceStorage> product95 = from p in cpvm.currentCollections where p.kind == typeDB.CPC95.key select p;
                                IEnumerable<priceStorage> productdiesel = from p in cpvm.currentCollections where p.kind == typeDB.CPCdiesel.key select p;
                                if (product95.Any())
                                {
                                    await ppvm.predictedPrice(ifvm.connectivity, true, progress);
                                    ppvm.getPrice(product95.First().price, productdiesel.First().price);
                                }
                                if (ifvm.dbcheckedDate.AddMinutes(5) < DateTime.Now) ifvm.dbcheckedDate = DateTime.Now;
                                if (ifvm.dailynotifyEnable)
                                {
                                    ShellToast toast = new ShellToast();
                                    toast.Title = "油價預測";
                                    string pprice = ppvm.pprice > 0 ? "+" + ppvm.pprice.ToString() : ppvm.pprice.ToString();
                                    toast.Content = ppvm.predictpause ? "能源局尚未更新國際油價" : "下周預測將調整" + pprice + "元";
                                    toast.Show();
                                }
                                ifvm.dailynotify = DateTime.Now;
                                using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(XML_PATH, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, isoStore))
                                {
                                    ifvm.saveConfig(isf);
                                    isf.Close();
                                    isf.Dispose();
                                }
                                GC.Collect();
                                tilescan();
                                runtile();
                            }
                        }
                        else
                        {
                            ifvm.dailynotified = false;
                            using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(XML_PATH, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, isoStore))
                            {
                                ifvm.saveConfig(isf);
                                isf.Close();
                                isf.Dispose();
                            }
                        }
                    }
                }
                TimeSpan now = new TimeSpan(DateTime.Now.Ticks);
                TimeSpan dupdate = new TimeSpan(dtvm.dDBcheckedDate.Ticks);
                if (now.Subtract(dupdate).Days > 15)
                {
                    using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(discount_PATH, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, isoStore))
                    {
                        await dtvm.loadXML(isf);
                        isf.Close();
                        isf.Dispose();
                    }
                    using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(discount_PATH, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, isoStore))
                    {
                        if (await dtvm.updateXML(progress, isf))
                        {
                            ShellToast toast = new ShellToast();
                            toast.Title = "油價預測";
                            toast.Content = "已更新加油折扣資料庫";
                            toast.Show();
                        }
                    }
                    using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(XML_PATH, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, isoStore))
                    {
                        ifvm.saveConfig(isf);
                        isf.Close();
                        isf.Dispose();
                    }
                }
                if (!insequence)
                {
                    if (!lastevent)
                    {
                        if (Math.Abs(DateTime.Now.Subtract(ifvm.tileupdateTime).Hours) > 6)
                        {
                            await cpvm.currentPrice(progress, true);
                            tilescan();
                            runtile();
                        }
                    }
                }
            }
            // If debugging is enabled, launch the agent again in one minute.
#if DEBUG_AGENT
            ScheduledActionService.LaunchForTest(task.Name, TimeSpan.FromSeconds(60));
#endif

            // Call NotifyComplete to let the system know the agent is done working.
        }


        /// 發生未處理的例外狀況時要執行的程式碼
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // 發生未處理的例外狀況; 切換到偵錯工具
                Debugger.Break();
            }
        }

        /// <summary>
        /// 執行排程工作的代理程式
        /// </summary>
        /// <param name="task">
        /// 叫用的工作
        /// </param>
        /// <remarks>
        /// 這個方法的呼叫時機為叫用週期性或耗用大量資料的工作時
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            //TODO:  加入程式碼，以在背景執行您的工作
            Deployment.Current.Dispatcher.BeginInvoke(async () =>
            {
                await startTileTask(task);
            });
            
        }
        private void toast(int b, double c95, double cdiesel)
        {
            string bname = b == 0 ? "中油" : "台塑";
            ShellToast toast = new ShellToast();
            toast.Title = "油價公告";
            string c95s = c95 > 0 ? "+" + c95.ToString() : c95.ToString();
            string cdiesels = cdiesel > 0 ? "+" + cdiesel.ToString() : cdiesel.ToString();
            toast.Content = bname + "公告，汽油" + c95s + "元，柴油" + cdiesels + "元";
            toast.Show();
        }
        private void tilescan() //fire the first tile and prepare prediction for all tiles
        {
            tileupdateEvents += ScheduledAgent_tileUpdated;
            active = (from x in ShellTile.ActiveTiles where x.NavigationUri.OriginalString != "/" select Convert.ToInt32(x.NavigationUri.OriginalString.Substring(x.NavigationUri.OriginalString.IndexOf("kind=") + 5, 1))).ToList();
            if (ifvm.defaultTile != -1) active.Add(ifvm.defaultTile);
        }
        private void runtile()
        {
            if (active.Any())
            {
                tileChange(active.First(), true, active.First() == ifvm.defaultTile);
            }
        }

        void ScheduledAgent_tileUpdated(object sender, tileUpdated e)
        {
            if (e.Success)
            {
                active.RemoveAt(0);
                if (active.Any())
                {
                    tileChange(active.First(), false, active.First() == ifvm.defaultTile);
                }
                else
                {
                    ifvm.tileupdateTime = DateTime.Now;
                    using (IsolatedStorageFileStream isf = new IsolatedStorageFileStream(XML_PATH, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, isoStore))
                    {
                        ifvm.saveConfig(isf);
                        isf.Close();
                        isf.Dispose();
                    }
                    NotifyComplete();
                }
            }
        }
        public event EventHandler<tileUpdated> tileupdateEvents;
        private void tileChange(int obj, bool prediction, bool defTile)
        {
            oilType product = typeDB.productnameDB[obj];
            try
            {
                tile Tile = new tile();
                Tile.DataContext = cpvm.tileExport(product, ppvm.penddate, ppvm.pstartdate, ppvm.pprice, ppvm.pdprice);
                Tile.SavePNGComplete += ((s, Tilearg) =>
                {
                    Tile = null;
                    GC.Collect();
                    FlipTileData tileData = new FlipTileData
                    {
                        Title = "",
                        WideBackgroundImage = new Uri("isostore:" + Tilearg.ImageFileName[2], UriKind.Absolute),
                        WideBackBackgroundImage = new Uri("isostore:" + Tilearg.ImageFileName[4], UriKind.Absolute),
                        BackgroundImage = new Uri("isostore:" + Tilearg.ImageFileName[0], UriKind.Absolute),
                        BackBackgroundImage = new Uri("isostore:" + Tilearg.ImageFileName[3], UriKind.Absolute),
                        SmallBackgroundImage = new Uri("isostore:" + Tilearg.ImageFileName[1], UriKind.Absolute)
                    };
                    string tileUri = defTile ? "/" : "/MainPage.xaml?kind=" + product.key;
                    ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Equals(tileUri));
                    tile.Update(tileData);
                    if (tileupdateEvents != null)
                    {
                        tileupdateEvents(this, new tileUpdated(true));
                    }
                });
                Tile.BeginSavePNG(product, prediction);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception in tile update: " + ex.ToString());

                if (tileupdateEvents != null)
                {
                    var args1 = new tileUpdated(false);
                    args1.Exception = ex;
                    tileupdateEvents(this, args1);
                }
            }
        }
    }
}