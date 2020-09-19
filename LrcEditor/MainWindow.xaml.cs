using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HandyControl.Controls;
using MessageBox = HandyControl.Controls.MessageBox;
using System.Windows.Forms;
using NAudioPlayer;
using Player = NAudioPlayer.NAudioPlayer;
using System.Diagnostics;

namespace LrcEditor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : HandyControl.Controls.Window
    {
        Player Player = new Player();
        string AudioPath;
        string LrcPath;
        Timer Timer = new Timer();

        public MainWindow()
        {
            InitializeComponent();
            Timer.Interval = 100;
            Timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(()=> 
            {
                SetProgress(Player.CurrentTime);
            }));
        }

        private void SelectAudio_Click(object sender, RoutedEventArgs e)
        {
            // 选择文件
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Mp3|*.mp3|Wave|*.wav|MIDI|*.midi|所有文件|*.*";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                AudioPath = dlg.FileName;
                try
                {
                    Player.Init(dlg.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    // AudioName.Text = "请选择正确的音频文件";
                    return;
                }
                AudioName.Text = dlg.FileName;
                AudioProgress.Maximum = Player.TotalTime.TotalSeconds;
                AudioProgress.Value = 0;
                AudioProgress.IsEnabled = true;
                LLStep.IsEnabled = true;
                LStep.IsEnabled = true;
                Pause.IsEnabled = true;
                RStep.IsEnabled = true;
                RRStep.IsEnabled = true;
                TotalTime.Content = Time2String(Player.TotalTime);
                Timer.Start();
            }
        }

        private void InputLrc_Click(object sender, RoutedEventArgs e)
        {
            // 选择文件
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Lrc|*.lrc|Text|*.txt|所有文件|*.*";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                AudioPath = dlg.FileName;
                Title = "Lrc Editor - " + dlg.FileName;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseAll_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SetInfo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AboutSoftwave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Donate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LLStep_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LStep_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            if (!Player.IsPlaying)
            {
                Pause.Content = "Pause";
                Player.Play();
                Timer.Start();
            }
            else
            {
                Pause.Content = "Play";
                Player.Pause();
                Timer.Stop();
            }
        }

        private void RStep_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RRStep_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result;
            result = MessageBox.Show("您确定要退出吗？（自动保存）\n", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // TODO: 这里做保存工作
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void RmBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SetProgress(TimeSpan time)
        {
            Time.Content = Time2String(time);
            AudioProgress.Value = time.TotalSeconds;
        }

        private string Time2String(TimeSpan time)
        {
            return time.Minutes + ":" + time.Seconds + "." + time.Milliseconds;
        }
    }
}
