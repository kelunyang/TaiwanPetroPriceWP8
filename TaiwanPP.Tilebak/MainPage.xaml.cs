using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TaiwanPP.Tile.Resources;

namespace TaiwanPP.Tile
{
    public partial class MainPage : PhoneApplicationPage
    {
        // 建構函式
        public MainPage()
        {
            InitializeComponent();

            // 將 ApplicationBar 當地語系化的程式碼範例
            //BuildLocalizedApplicationBar();
        }

        // 建置當地語系化 ApplicationBar 的程式碼範例
        //private void BuildLocalizedApplicationBar()
        //{
        //    // 將頁面的 ApplicationBar 設定為 ApplicationBar 的新執行個體。
        //    ApplicationBar = new ApplicationBar();

        //    // 建立新的按鈕並將文字值設定為 AppResources 的當地語系化字串。
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // 用 AppResources 的當地語系化字串建立新的功能表項目。
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}