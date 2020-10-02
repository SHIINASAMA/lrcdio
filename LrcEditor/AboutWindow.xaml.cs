using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Window = HandyControl.Controls.Window;

namespace LrcEditor
{
    /// <summary>
    /// AboutWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        #region 关于界面响应
        private void About_Selected(object sender, RoutedEventArgs e)
        {
            DependView.Visibility = Visibility.Hidden;
            DonateView.Visibility = Visibility.Hidden;
            AboutView.Visibility = Visibility.Visible;
        }

        private void GitLink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(GitLink.NavigateUri.ToString());
        }

        private void Maker_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Maker.NavigateUri.ToString());
        }

        private void License_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(License.NavigateUri.ToString());
        }
        #endregion
        #region 依赖界面响应
        private void Depend_Selected(object sender, RoutedEventArgs e)
        {
            DonateView.Visibility = Visibility.Hidden;
            AboutView.Visibility = Visibility.Hidden;
            DependView.Visibility = Visibility.Visible;
        }

        private void NAudio_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(NAudio.NavigateUri.ToString());
        }

        private void NAudioPlayer_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(NAudioPlayer.NavigateUri.ToString());
        }

        private void HandyControl_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(HandyControl.NavigateUri.ToString());
        }
        private void LrcLib_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(LrcLib.NavigateUri.ToString());
        }
        #endregion
        #region 捐赠界面响应
        private void Donate_Selected(object sender, RoutedEventArgs e)
        {
            AboutView.Visibility = Visibility.Hidden;
            DependView.Visibility = Visibility.Hidden;
            DonateView.Visibility = Visibility.Visible;
        }
        #endregion
    }
}
