using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TaiwanPP.TileMaker.Helpers;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using TaiwanPP.Library.Helpers;
using System.Threading.Tasks;

namespace TaiwanPP.TileMaker
{
    public partial class tile : UserControl
    {
        public tile()
        {
            InitializeComponent();
        }
        public event EventHandler<savePNGEvent> SavePNGComplete;
        private WriteableBitmap wbp;
        public async Task BeginSavePNG(oilType product, bool predict)
        {
            try
            {
                string[] filename = { "/Shared/ShellContent/smallTile" + product.key + ".png", "/Shared/ShellContent/tinyTile" + product.key + ".png", "/Shared/ShellContent/wideTile" + product.key + ".png", "/Shared/ShellContent/smallpredictTile.png", "/Shared/ShellContent/widepredictTile.png" };
                // Set the loaded image
                this.Measure(new Size(691, 336));
                this.Arrange(new Rect(0, 0, 691, 336));
                this.UpdateLayout();
                smalltileGrid.Measure(new Size(336, 336));
                smalltileGrid.Arrange(new Rect(0, 0, 336, 336));
                smalltileGrid.UpdateLayout();
                wbp = new WriteableBitmap(smalltileGrid, null);
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (myIsolatedStorage.FileExists(filename[0]))
                    {
                        myIsolatedStorage.DeleteFile(filename[0]);
                    }
                    IsolatedStorageFileStream fileStream = myIsolatedStorage.CreateFile(filename[0]);
                    await Cimbalino.Phone.Toolkit.Extensions.WriteableBitmapExtensions.SavePngAsync(wbp, fileStream);
                    fileStream.Close();
                }
                tinytileGrid.Measure(new Size(159, 159));
                tinytileGrid.Arrange(new Rect(0, 0, 159, 159));
                tinytileGrid.UpdateLayout();
                wbp = new WriteableBitmap(tinytileGrid, null);
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (myIsolatedStorage.FileExists(filename[1]))
                    {
                        myIsolatedStorage.DeleteFile(filename[1]);
                    }
                    IsolatedStorageFileStream fileStream = myIsolatedStorage.CreateFile(filename[1]);
                    await Cimbalino.Phone.Toolkit.Extensions.WriteableBitmapExtensions.SavePngAsync(wbp, fileStream);
                    fileStream.Close();
                }
                widetileGrid.Measure(new Size(691, 336));
                widetileGrid.Arrange(new Rect(0, 0, 691, 336));
                widetileGrid.UpdateLayout();
                wbp = new WriteableBitmap(widetileGrid, null);
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (myIsolatedStorage.FileExists(filename[2]))
                    {
                        myIsolatedStorage.DeleteFile(filename[2]);
                    }
                    IsolatedStorageFileStream fileStream = myIsolatedStorage.CreateFile(filename[2]);
                    await Cimbalino.Phone.Toolkit.Extensions.WriteableBitmapExtensions.SavePngAsync(wbp, fileStream);
                    fileStream.Close();
                }
                if (predict)
                {
                    smallbackGrid.Measure(new Size(336, 336));
                    smallbackGrid.Arrange(new Rect(0, 0, 336, 336));
                    smallbackGrid.UpdateLayout();
                    wbp = new WriteableBitmap(smallbackGrid, null);
                    using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        if (myIsolatedStorage.FileExists(filename[3]))
                        {
                            myIsolatedStorage.DeleteFile(filename[3]);
                        }
                        IsolatedStorageFileStream fileStream = myIsolatedStorage.CreateFile(filename[3]);
                        await Cimbalino.Phone.Toolkit.Extensions.WriteableBitmapExtensions.SavePngAsync(wbp, fileStream);
                        fileStream.Close();
                    }
                    widebackGrid.Measure(new Size(691, 336));
                    widebackGrid.Arrange(new Rect(0, 0, 691, 336));
                    widebackGrid.UpdateLayout();
                    wbp = new WriteableBitmap(widebackGrid, null);
                    using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        if (myIsolatedStorage.FileExists(filename[4]))
                        {
                            myIsolatedStorage.DeleteFile(filename[4]);
                        }
                        IsolatedStorageFileStream fileStream = myIsolatedStorage.CreateFile(filename[4]);
                        await Cimbalino.Phone.Toolkit.Extensions.WriteableBitmapExtensions.SavePngAsync(wbp, fileStream);
                        fileStream.Close();
                    }
                }

                // Fire completion event
                if (SavePNGComplete != null)
                {
                    SavePNGComplete(this, new savePNGEvent(true, filename));
                }
            }
            catch (Exception ex)
            {
                // Log it
                System.Diagnostics.Debug.WriteLine("Exception in SavePNG: " + ex.ToString());

                if (SavePNGComplete != null)
                {
                    var args1 = new savePNGEvent(false, new string[1]);
                    args1.Exception = ex;
                    SavePNGComplete(this, args1);
                }
            }
        }
    }
}
