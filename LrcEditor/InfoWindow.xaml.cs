using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Window = HandyControl.Controls.Window;
using LrcLib.LrcData;
using Type = LrcLib.LrcData.LrcHeader.Type;
using System.Windows.Forms;
using MessageBox = HandyControl.Controls.MessageBox;
using System.Text.RegularExpressions;

namespace LrcEditor
{
    /// <summary>
    /// InfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InfoWindow : Window
    {
        public LrcHeader[] Headers;
        
        public InfoWindow(LrcHeader[] Headers)
        {
            InitializeComponent();

            this.Headers = Headers;
            ArTextBox.Text = Headers[(int)Type.Ar].Text;
            AlTextBox.Text = Headers[(int)Type.Al].Text;
            TiTextBox.Text = Headers[(int)Type.Ti].Text;
            ByTextBox.Text = Headers[(int)Type.By].Text;
            OffsetTextBox.Text = Headers[(int)Type.Offset].Text;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            Headers[(int)LrcHeader.Type.Ar] = new LrcHeader(LrcHeader.Type.Ar, ArTextBox.Text);
            Headers[(int)LrcHeader.Type.Ti] = new LrcHeader(LrcHeader.Type.Ti, TiTextBox.Text);
            Headers[(int)LrcHeader.Type.Al] = new LrcHeader(LrcHeader.Type.Al, AlTextBox.Text);
            Headers[(int)LrcHeader.Type.By] = new LrcHeader(LrcHeader.Type.By, ByTextBox.Text);
            Headers[(int)LrcHeader.Type.Offset] = new LrcHeader(LrcHeader.Type.Offset, OffsetTextBox.Text);
            DialogResult = true;
        }

        private void OffsetTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9.-]+");
            e.Handled = re.IsMatch(e.Text);
        }
    }
}
