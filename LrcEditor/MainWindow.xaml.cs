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
using Window = HandyControl.Controls.Window;
using System.Diagnostics;

namespace LrcEditor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Player Player = new Player();
        string AudioPath;
        string LrcPath;
        Timer Timer = new Timer();
        bool IsChanging = false;

        public MainWindow()
        {
            InitializeComponent();

            Timer.Interval = 100;
            Timer.Tick += Timer_Tick;

            AudioProgress.AddHandler(Slider.MouseDownEvent, new RoutedEventHandler(AudioProgress_MouseDown), true);
            AudioProgress.AddHandler(Slider.MouseUpEvent, new RoutedEventHandler(AudioProgress_MouseUp), true);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(()=> 
            {
                SetProgress(Player.CurrentTime);
                IsEnd();
            }));
        }

        #region 菜单响应
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
                SetPanalUsable(true);
                TotalTime.Content = Time2String(Player.TotalTime);
                Pause.Content = "Play";
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
            SetPanalUsable(false);
            Title = "Lrc Editor";
            AudioName.Text = "请先打开音频文件";
            DataView.Items.Clear();
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
        #endregion
        #region 音频控制面板响应
        private void LLStep_Click(object sender, RoutedEventArgs e)
        {
            // LLStepFunc();
            SetStep(-200);
        }

        private void LStep_Click(object sender, RoutedEventArgs e)
        {
            // LStepFunc();
            SetStep(-100);
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            PlayFunc();
        }

        private void RStep_Click(object sender, RoutedEventArgs e)
        {
            // RStepFunc();
            SetStep(100);
        }

        private void RRStep_Click(object sender, RoutedEventArgs e)
        {
            // RRStepFunc();
            SetStep(200);
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
        #endregion

        private void SetProgress(TimeSpan time)
        {
            if (IsChanging)
            {
                Time.Content = Time2String(new TimeSpan(0, 0,(int)AudioProgress.Value));
            }
            else
            {
                Time.Content = Time2String(time);
                AudioProgress.Value = time.TotalSeconds;
            }
        }

        private void IsEnd()
        {
            if (Time.Content.ToString() == TotalTime.Content.ToString()) 
            {
                Player.Pause();
                Pause.Content = "Replay";
            }
        }

        private void AudioProgress_MouseDown(object sender, RoutedEventArgs e)
        {
            IsChanging = true;
        }

        private void AudioProgress_MouseUp(object sender, RoutedEventArgs e)
        {
            IsChanging = false;
            Player.CurrentTime = new TimeSpan(0, 0, (int)AudioProgress.Value);
        }

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // MessageBox.Show(e.Key.ToString());
            if (Pause.IsEnabled && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                switch (e.Key)
                {
                    case Key.OemQuestion:
                        PlayFunc();
                        break;
                    case Key.OemComma:
                        LStepFunc();
                        break;
                    case Key.OemPeriod:
                        RStepFunc();
                        break;
                    case Key.Oem4:
                        LLStepFunc();
                        break;
                    case Key.Oem6:
                        RRStepFunc();
                        break;
                }
            }
        }

        #region 通用方法
        private void PlayFunc()
        {
            if (Pause.Content.ToString() == "Replay") Player.CurrentTime = TimeSpan.Zero;

            if (!Player.IsPlaying)
            {
                Pause.Content = "Pause";
                Player.Play();
            }
            else
            {
                Pause.Content = "Play";
                Player.Pause();
            }
        }

        private void LLStepFunc()
        {
            TimeSpan temp = new TimeSpan(0, 0, 0, 0, 200);
            if (Player.CurrentTime - temp < TimeSpan.Zero)
            {
                Player.CurrentTime = TimeSpan.Zero;
            }
            else
            {
                Player.CurrentTime -= temp;
            }
        }

        private void LStepFunc()
        {
            TimeSpan temp = new TimeSpan(0, 0, 0, 0, 100);
            if (Player.CurrentTime - temp < TimeSpan.Zero)
            {
                Player.CurrentTime = TimeSpan.Zero;
            }
            else
            {
                Player.CurrentTime -= temp;
            }
        }

        private void RRStepFunc()
        {
            TimeSpan temp = new TimeSpan(0, 0, 0, 0, 200);
            if (Player.CurrentTime + temp >= Player.TotalTime)
            {
                Player.CurrentTime = Player.TotalTime;
            }
            else
            {
                Player.CurrentTime += temp;
            }
        }

        private void RStepFunc()
        {
            TimeSpan temp = new TimeSpan(0, 0, 0, 0, 100);
            if (Player.CurrentTime + temp >= Player.TotalTime)
            {
                Player.CurrentTime = Player.TotalTime;
            }
            else
            {
                Player.CurrentTime += temp;
            }
        }

        private void SetStep(int ms)
        {
            bool IsAdd = true;
            if(ms < 0)
            {
                IsAdd = false;
                ms *= -1;
            }
            TimeSpan temp = new TimeSpan(0, 0, 0, 0, ms);
            if (IsAdd)
            {
                if(Player.CurrentTime + temp >= Player.TotalTime)
                {
                    Player.CurrentTime = Player.TotalTime;
                }
                else 
                {
                    Player.CurrentTime += temp;
                }
            }
            else
            {
                if(Player.CurrentTime - temp <= TimeSpan.Zero)
                {
                    Player.CurrentTime = TimeSpan.Zero;
                }
                else
                {
                    Player.CurrentTime -= temp;
                }
            }
        }

        private string Time2String(TimeSpan time)
        {
            string min = time.Minutes.ToString();
            if (min.Length == 1) min = "0" + min;

            string sec = time.Seconds.ToString();
            if (sec.Length == 1) sec = "0" + sec;

            string ms = time.Milliseconds.ToString();
            if (ms.Length == 1) ms = "00" + ms;
            else if (ms.Length == 2) ms = "0" + ms;

            return min + ":" + sec + "." + ms;
        }

        private void SetPanalUsable(bool b)
        {
            LLStep.IsEnabled = LStep.IsEnabled = Pause.IsEnabled = RStep.IsEnabled = RRStep.IsEnabled = AudioProgress.IsEnabled = b;
            if (!b && Player.IsPlaying)
            {
                Player.Pause();
                Time.Content = TotalTime.Content = "00:00.000";
            }
        }
        #endregion
    }
}
