using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace LrcEditor
{
    /// <summary>
    /// AboutWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AboutWindow
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        #region 关于界面响应

        private void About_Selected(object sender, RoutedEventArgs e)
        {
            DependView.Visibility = Visibility.Hidden;
            AboutView.Visibility = Visibility.Visible;
        }

        private void Jump(object sender, RequestNavigateEventArgs e)
        {
            Process.Start("explorer.exe", e.Uri.AbsoluteUri);
            e.Handled = true;
        }

        #endregion

        #region 依赖界面响应

        private void Depend_Selected(object sender, RoutedEventArgs e)
        {
            AboutView.Visibility = Visibility.Hidden;
            DependView.Visibility = Visibility.Visible;
        }

        #endregion
    }
}