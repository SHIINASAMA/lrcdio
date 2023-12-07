using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MessageBox = HandyControl.Controls.MessageBox;
using System.Windows.Forms;
using Player = NAudioPlayer.NAudioPlayer;
using System.Data;
using LrcLib.LrcData;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using LrcLib.LrcFileKits;

namespace LrcEditor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    [SupportedOSPlatform("windows")]
    public partial class MainWindow
    {
        private readonly Player _player = new();
        private string _lrcPath;

        private readonly Timer _timer;
        private bool _isChanging;

        private LrcHeader[] _lrcHeaders = new LrcHeader[5];
        private readonly DataTable _dt = new();

        public MainWindow()
        {
            _timer = new Timer();
            InitializeComponent();
            _timer.Interval = 100;
            _timer.Tick += Timer_Tick;

            AudioProgress.AddHandler(Slider.MouseDownEvent, new RoutedEventHandler(AudioProgress_MouseDown), true);
            AudioProgress.AddHandler(Slider.MouseUpEvent, new RoutedEventHandler(AudioProgress_MouseUp), true);

            _dt.Columns.Add(new DataColumn("时间"));
            _dt.Columns.Add(new DataColumn("文本"));
            DataView.ItemsSource = _dt.DefaultView;
            DataView.ColumnWidth = DataGridLength.SizeToCells;

            _lrcHeaders[(int)LrcHeader.Type.Ar] = new LrcHeader(LrcHeader.Type.Ar, "AR");
            _lrcHeaders[(int)LrcHeader.Type.Ti] = new LrcHeader(LrcHeader.Type.Ti, "TI");
            _lrcHeaders[(int)LrcHeader.Type.Al] = new LrcHeader(LrcHeader.Type.Al, "AL");
            _lrcHeaders[(int)LrcHeader.Type.By] = new LrcHeader(LrcHeader.Type.By, "BY");
            _lrcHeaders[(int)LrcHeader.Type.Offset] = new LrcHeader(LrcHeader.Type.Offset, "OFFSET");
        }

        #region 菜单响应

        private void About_Click(object sender, RoutedEventArgs e)
        {
            var window = new AboutWindow();
            window.ShowDialog();
        }

        private void SelectAudio_Click(object sender, RoutedEventArgs e)
        {
            // 选择文件
            var dlg = new OpenFileDialog();
            dlg.Filter = "Mp3|*.mp3|Wave|*.wav|MIDI|*.midi|All|*.*";
            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            try
            {
                _player.Load(dlg.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                // AudioName.Text = "请选择正确的音频文件";
                return;
            }

            AudioName.Text = dlg.FileName;
            AudioProgress.Maximum = _player.TotalTime.TotalSeconds;
            AudioProgress.Value = 0;
            SetPanelUsable(true);
            TotalTime.Content = Time2String(_player.TotalTime);
            Pause.Content = "Play";
            _timer.Start();
        }


        private void InputLrc_Click(object sender, RoutedEventArgs e)
        {
            // 选择文件
            var dlg = new OpenFileDialog();
            dlg.Filter = "Lrc|*.lrc|Text|*.txt|All|*.*";
            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            _lrcPath = dlg.FileName;
            Title = "Lrc Editor - " + dlg.FileName;
            var lrc = new LrcObject();
            LrcFileKits.ReadFromFile(ref lrc, _lrcPath);
            _lrcHeaders = lrc.LrcHeaders;

            foreach (LrcLine line in lrc.LrcLines)
            {
                var dr = _dt.NewRow();
                dr[0] = Time2String(line.Time);
                dr[1] = line.Text;
                _dt.Rows.Add(dr);
            }

            SetInfo.IsEnabled = true;
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
            SetPanelUsable(false);
            Title = "Lrc Editor";
            AudioName.Text = "请先打开音频文件";
            _dt.Rows.Clear();
            _lrcPath = null;
        }


        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("您确定要退出吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // TODO: 这里做保存工作
                Close();
            }
            else
            {
                return;
            }
        }

        private void SetInfo_Click(object sender, RoutedEventArgs e)
        {
            var infoWindow = new InfoWindow(_lrcHeaders);
            if ((bool)infoWindow.ShowDialog())
            {
                _lrcHeaders = infoWindow.Headers;
            }
        }

        #endregion

        #region 音频控制面板响应

        private void LLStep_Click(object sender, RoutedEventArgs e)
        {
            _player.Jump(-200);
        }

        private void LStep_Click(object sender, RoutedEventArgs e)
        {
            _player.Jump(-100);
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            PlayFunc();
        }

        private void RStep_Click(object sender, RoutedEventArgs e)
        {
            _player.Jump(100);
        }

        private void RRStep_Click(object sender, RoutedEventArgs e)
        {
            _player.Jump(200);
        }

        #endregion

        #region 其他

        private void Timer_Tick(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                SetProgress(_player.CurrentTime);
                IsEnd();
            }));
        }

        private void SetProgress(TimeSpan time)
        {
            if (_isChanging)
            {
                Time.Content = Time2String(new TimeSpan(0, 0, (int)AudioProgress.Value));
            }
            else
            {
                Time.Content = Time2String(time);
                AudioProgress.Value = time.TotalSeconds;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBox.Show("您确定要退出吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // TODO: 这里做保存工作
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void IsEnd()
        {
            if (Time.Content.ToString() == TotalTime.Content.ToString())
            {
                _player.Pause();
                Pause.Content = "Replay";
            }
        }

        private void AudioProgress_MouseDown(object sender, RoutedEventArgs e)
        {
            _isChanging = true;
        }

        private void AudioProgress_MouseUp(object sender, RoutedEventArgs e)
        {
            _isChanging = false;
            _player.CurrentTime = new TimeSpan(0, 0, (int)AudioProgress.Value);
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
                        _player.Jump(-100);
                        //SetStep(-100);
                        // LStepFunc();
                        break;
                    case Key.OemPeriod:
                        _player.Jump(100);
                        //SetStep(100);
                        // RStepFunc();
                        break;
                    case Key.Oem4:
                        _player.Jump(-200);
                        //SetStep(-200);
                        // LLStepFunc();
                        break;
                    case Key.Oem6:
                        _player.Jump(200);
                        //SetStep(200);
                        // RRStepFunc();
                        break;
                }
            }

            if (ModifierKeys.Control == e.KeyboardDevice.Modifiers)
            {
                switch (e.Key)
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

        #endregion

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
            if (Pause.Content.ToString() == "Replay") _player.CurrentTime = TimeSpan.Zero;

            if (!_player.IsPlaying)
            {
                Pause.Content = "Pause";
                _player.Play();
            }
            else
            {
                Pause.Content = "Play";
                _player.Pause();
            }
        }

        private static string Time2String(TimeSpan time)
        {
            var min = time.Minutes.ToString();
            if (min.Length == 1) min = "0" + min;

            var sec = time.Seconds.ToString();
            if (sec.Length == 1) sec = "0" + sec;

            var ms = time.Milliseconds.ToString();
            ms = ms.Length switch
            {
                1 => "00" + ms,
                2 => "0" + ms,
                _ => ms
            };

            return min + ":" + sec + "." + ms;
        }

        private void SetPanelUsable(bool b)
        {
            LlStep.IsEnabled = LStep.IsEnabled =
                Pause.IsEnabled = RStep.IsEnabled = RrStep.IsEnabled = AudioProgress.IsEnabled = b;
            if (!b && _player.IsPlaying)
            {
                _player.Pause();
                Time.Content = TotalTime.Content = "00:00.000";
            }
        }

        private void DelItem()
        {
            if (DataView.SelectedIndex == -1 && DataView.Items.Count != 0)
            {
                _dt.Rows.RemoveAt(DataView.Items.Count - 1);
            }
            else if (DataView.SelectedIndex != -1)
            {
                _dt.Rows.RemoveAt(DataView.SelectedIndex);
            }
        }

        private void AddItem()
        {
            var dr = _dt.NewRow();
            dr[0] = Time.Content;
            if (DataView.SelectedIndex == -1)
            {
                _dt.Rows.Add(dr);
                DataView.SelectedIndex = DataView.Items.Count - 1;
                DataView.ScrollIntoView(DataView.SelectedItem);
            }
            else
            {
                _dt.Rows.InsertAt(dr, DataView.SelectedIndex + 1);
                ++DataView.SelectedIndex;
                DataView.ScrollIntoView(DataView.SelectedItem);
            }
        }

        private void SetTime()
        {
            if (DataView.SelectedIndex == -1) return;
            _dt.Rows[DataView.SelectedIndex][0] = Time.Content;
        }

        private void FormatSaveFunc(string path)
        {
            // if (DataView.Items.Count == 0) return;
            List<LrcLine> lines = [];
            lines.AddRange(from DataRow dr in _dt.Rows select LrcLine.Pause("[" + dr[0] + "]" + dr[1])[0]);

            var lrc = new LrcObject
            {
                LrcHeaders = this._lrcHeaders,
                LrcLines = lines
            };
            LrcFileKits.WriteToFile(ref lrc, path);
        }

        private int CheckTimeFormat()
        {
            var index = 0;
            foreach (DataRow dr in _dt.Rows)
            {
                if (!MyRegex().IsMatch((string)dr[0]))
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
            if (_lrcPath == null)
            {
                var dlg = new SaveFileDialog();
                dlg.Filter = "LRC|*.Lrc|Plain text|*.txt";
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _lrcPath = dlg.FileName;
                    FormatSaveFunc(_lrcPath);
                    Title = "Lrc Edit -" + _lrcPath;
                }
            }
            else
            {
                var a = CheckTimeFormat();
                //if (a >= 0)
                //{
                //    DataView.SelectedIndex = a;
                //    DataView.ScrollIntoView(DataView.SelectedItem);
                //    MessageBox.Show("时间格式错误", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    return;
                //}
                //else
                //{
                FormatSaveFunc(_lrcPath);
                //e}
            }
        }

        [GeneratedRegex(@"(\d{ 1,2}\:\d{ 1,2}\.\d{ 2,3})| (\d{ 1,2}\:\d{ 1,2})")]
        private static partial Regex MyRegex();

        #endregion
    }
}