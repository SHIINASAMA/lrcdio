using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MessageBox = HandyControl.Controls.MessageBox;
using System.Windows.Forms;
using Player = NAudioPlayer.NAudioPlayer;
using Window = HandyControl.Controls.Window;
using System.Data;
using LrcLib.LrcData;
using LrcLib.LrcAdapter;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

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
        LrcHeader[] LrcHeaders = new LrcHeader[5];
        DataTable dt = new DataTable();

        public MainWindow()
        {
            InitializeComponent();

            Timer.Interval = 100;
            Timer.Tick += Timer_Tick;

            AudioProgress.AddHandler(Slider.MouseDownEvent, new RoutedEventHandler(AudioProgress_MouseDown), true);
            AudioProgress.AddHandler(Slider.MouseUpEvent, new RoutedEventHandler(AudioProgress_MouseUp), true);

            dt.Columns.Add(new DataColumn("时间"));
            dt.Columns.Add(new DataColumn("文本"));
            DataView.ItemsSource = dt.DefaultView;
            DataView.ColumnWidth = DataGridLength.SizeToCells;

            LrcHeaders[(int)LrcHeader.Type.AR] = new LrcHeader(LrcHeader.Type.AR, "AR");
            LrcHeaders[(int)LrcHeader.Type.TI] = new LrcHeader(LrcHeader.Type.TI, "TI");
            LrcHeaders[(int)LrcHeader.Type.AL] = new LrcHeader(LrcHeader.Type.AL, "AL");
            LrcHeaders[(int)LrcHeader.Type.BY] = new LrcHeader(LrcHeader.Type.BY, "BY");
            LrcHeaders[(int)LrcHeader.Type.OFFSET] = new LrcHeader(LrcHeader.Type.OFFSET, "OFFSET");
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
                    Player.Load(dlg.FileName);
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
                LrcPath = dlg.FileName;
                Title = "Lrc Editor - " + dlg.FileName;
                LrcObject Lrc = new LrcObject();
                LrcAdapter.ReadFromFile(ref Lrc, LrcPath);
                LrcHeaders = Lrc.LrcHeaders;

                DataRow dr = null;
                foreach (LrcLine line in Lrc.LrcLines)
                {
                    dr = dt.NewRow();
                    dr[0] = Time2String(line.Time);
                    dr[1] = line.Text;
                    dt.Rows.Add(dr);
                }

                SetInfo.IsEnabled = true;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFunc();
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseAll_Click(object sender, RoutedEventArgs e)
        {
            SetPanalUsable(false);
            Title = "Lrc Editor";
            AudioName.Text = "请先打开音频文件";
            dt.Rows.Clear();
            LrcPath = null;
            AudioPath = null;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SetInfo_Click(object sender, RoutedEventArgs e)
        {
            InfoWindow infoWindow = new InfoWindow(LrcHeaders);
            if ((bool)infoWindow.ShowDialog())
            {
                LrcHeaders = infoWindow.Headers;
            }
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
            Player.Jump(-200);
        }

        private void LStep_Click(object sender, RoutedEventArgs e)
        {
            Player.Jump(-100);
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            PlayFunc();
        }

        private void RStep_Click(object sender, RoutedEventArgs e)
        {
            Player.Jump(100);
        }

        private void RRStep_Click(object sender, RoutedEventArgs e)
        {
            Player.Jump(200);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result;
            result = MessageBox.Show("您确定要退出吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // TODO: 这里做保存工作
            }
            else
            {
                e.Cancel = true;
            }
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

        // 快捷键响应
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
                        Player.Jump(-100);
                        //SetStep(-100);
                        // LStepFunc();
                        break;
                    case Key.OemPeriod:
                        Player.Jump(100);
                        //SetStep(100);
                        // RStepFunc();
                        break;
                    case Key.Oem4:
                        Player.Jump(-200);
                        //SetStep(-200);
                        // LLStepFunc();
                        break;
                    case Key.Oem6:
                        Player.Jump(200);
                        //SetStep(200);
                        // RRStepFunc();
                        break;
                }
            }
            if(ModifierKeys.Control == e.KeyboardDevice.Modifiers)
            {
                switch(e.Key)
                {
                    case Key.T:
                        SetTime();
                        break;
                    case Key.OemPlus:
                        AddItem();
                        break;
                    case Key.OemMinus:
                        DelItem();
                        break;
                    case Key.S:
                        SaveFunc();
                        break;
                }
            }
        }

        #region DataView响应
        private void RmBtn_Click(object sender, RoutedEventArgs e)
        {
            DelItem();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            AddItem();
        }

        #endregion

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

        private void DelItem()
        {
            if (DataView.SelectedIndex == -1 && DataView.Items.Count != 0)
            {
                dt.Rows.RemoveAt(DataView.Items.Count - 1);
            }
            else if (DataView.SelectedIndex != -1)
            {
                dt.Rows.RemoveAt(DataView.SelectedIndex);
            }
        }

        private void AddItem()
        {
            DataRow dr;
            dr = dt.NewRow();
            dr[0] = Time.Content;
            if (DataView.SelectedIndex == -1)
            {
                dt.Rows.Add(dr);
                DataView.SelectedIndex = DataView.Items.Count - 1;
                DataView.ScrollIntoView(DataView.SelectedItem);
            }
            else
            {
                dt.Rows.InsertAt(dr, DataView.SelectedIndex + 1);
                ++DataView.SelectedIndex;
                DataView.ScrollIntoView(DataView.SelectedItem);
            }
        }

        private void SetTime()
        {
            if (DataView.SelectedIndex == -1) return;
            dt.Rows[DataView.SelectedIndex][0] = Time.Content;
        }

        private void FormatSaveFunc(string path)
        {
            // if (DataView.Items.Count == 0) return;
            List<LrcLine> Lines = new List<LrcLine>();
            foreach(DataRow dr in dt.Rows)
            {
                Lines.Add(LrcLine.Pause("[" + dr[0] + "]" + dr[1])[0]);
            }

            LrcObject lrc = new LrcObject();
            lrc.LrcHeaders = this.LrcHeaders;
            lrc.LrcLines = Lines;
            LrcAdapter.WriteToFile(ref lrc, path);
        }

        private int CheckTimeFormat()
        {
            int index = 0;
            foreach(DataRow dr in dt.Rows)
            {
                if (!Regex.IsMatch((string)dr[0], @"(\d{ 1,2}\:\d{ 1,2}\.\d{ 2,3})| (\d{ 1,2}\:\d{ 1,2})"))
                {
                    return index;
                }
                ++index;
            }
            return -1;
        }

        private void SaveFunc()
        {
            if (DataView.Items.Count == 0)
            {
                MessageBox.Show("内容不能为空", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            //int i = CheckTimeFormat();
            //if (i >= 0)
            //{
            //    DataView.SelectedIndex = i;
            //    DataView.ScrollIntoView(DataView.SelectedItem);
            //    MessageBox.Show("时间格式错误", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return;
            //}
            if (LrcPath == null)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "LRC文件|*.Lrc|文本文件|*.txt";
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LrcPath = dlg.FileName;
                    FormatSaveFunc(LrcPath);
                    Title = "Lrc Edit -" + LrcPath;
                }
            }
            else
            {
                int a = CheckTimeFormat();
                //if (a >= 0)
                //{
                //    DataView.SelectedIndex = a;
                //    DataView.ScrollIntoView(DataView.SelectedItem);
                //    MessageBox.Show("时间格式错误", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    return;
                //}
                //else
                //{
                    FormatSaveFunc(LrcPath);
                //e}
            }
        }
        #endregion
    }
}
