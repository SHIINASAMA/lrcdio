using System.Windows;
using System.Windows.Input;
using LrcLib.LrcData;
using Type = LrcLib.LrcData.LrcHeader.Type;
using System.Text.RegularExpressions;

namespace LrcEditor
{
    /// <summary>
    /// InfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InfoWindow
    {
        public readonly LrcHeader[] Headers;

        public InfoWindow(LrcHeader[] headers)
        {
            InitializeComponent();

            this.Headers = headers;
            ArTextBox.Text = headers[(int)Type.Ar].Text;
            AlTextBox.Text = headers[(int)Type.Al].Text;
            TiTextBox.Text = headers[(int)Type.Ti].Text;
            ByTextBox.Text = headers[(int)Type.By].Text;
            OffsetTextBox.Text = headers[(int)Type.Offset].Text;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            Headers[(int)Type.Ar] = new LrcHeader(Type.Ar, ArTextBox.Text);
            Headers[(int)Type.Ti] = new LrcHeader(Type.Ti, TiTextBox.Text);
            Headers[(int)Type.Al] = new LrcHeader(Type.Al, AlTextBox.Text);
            Headers[(int)Type.By] = new LrcHeader(Type.By, ByTextBox.Text);
            Headers[(int)Type.Offset] = new LrcHeader(Type.Offset, OffsetTextBox.Text);
            DialogResult = true;
        }

        private void OffsetTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var re = MyRegex();
            e.Handled = re.IsMatch(e.Text);
        }

        [GeneratedRegex("[^0-9.-]+")]
        private static partial Regex MyRegex();
    }
}