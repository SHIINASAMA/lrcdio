using System;
using System.Runtime.Versioning;
using NAudio.Wave;

namespace NAudioPlayer
{
    [SupportedOSPlatform("windows")]
    public class NAudioPlayer
    {
        private IWavePlayer _wavePlayer;
        private AudioFileReader _audioFileReader;

        /// <summary>
        /// 当前的播放状态
        /// </summary>
        public bool IsPlaying { get; private set; }

        /// <summary>
        /// 播放文件路径及名称
        /// </summary>
        private string SongName { get; set; } = "";

        private float _volume = 0.5f;
        /// <summary>
        /// 播放器声音大小，默认为50%
        /// </summary>
        public float Volume
        {
            get => _volume;
            set
            {
                if (value is < 0 or > 1f) return;
                _volume = value;
                if (_audioFileReader != null)
                {
                    _audioFileReader.Volume = value;
                }
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public NAudioPlayer()
        {
        }

        /// <summary>
        /// 初始化播放器,初始化参数为播放文件路径
        /// </summary>
        public void Load(string songName)
        {
            if (IsPlaying)
            {
                IsPlaying = false;
            }

            if (string.IsNullOrEmpty(songName))
                return;

            this.SongName = songName;

            if (_wavePlayer != null)
            {
                _wavePlayer.Dispose();
                GC.Collect();
            }

            _wavePlayer = new WaveOut();
            _audioFileReader = new AudioFileReader(SongName);
            _audioFileReader.Volume = Volume;
            _wavePlayer.Init(_audioFileReader);
            _wavePlayer.PlaybackStopped += WavePlayer_PlaybackStopped;
            _wavePlayer.Pause();
        }

        private void WavePlayer_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            //if (audioFileReader != null)
            //{
            //    audioFileReader.Dispose();
            //    audioFileReader = null;
            //}
            //if (wavePlayer != null)
            //{
            //    wavePlayer.Dispose();
            //    wavePlayer = null;
            //}
            CurrentTime = TimeSpan.Zero;
            IsPlaying = false;
        }

        /// <summary>
        /// 获取当前播放位置
        /// </summary>
        public TimeSpan CurrentTime
        {
            get
            {
                if (_audioFileReader == null)
                {
                    return TimeSpan.Zero;
                }
                else
                {
                    return _audioFileReader.CurrentTime;
                }

            }
            set => _audioFileReader.CurrentTime = value;
        }

        /// <summary>
        /// 获取总长度
        /// </summary>
        public TimeSpan TotalTime
        {
            get
            {
                if (_audioFileReader == null)
                {
                    return TimeSpan.Zero;
                }
                else
                {
                    return _audioFileReader.TotalTime;
                }
            }
        }

        /// <summary>
        /// 向前或向后跳转一定的毫秒数
        /// </summary>
        /// <param name="ms"></param>
        public void Jump(long ms)
        {
            TimeSpan temp = TimeSpan.FromMilliseconds(ms);
            if(ms >= 0)
            {
                if(temp + CurrentTime >= TotalTime)
                {
                    CurrentTime = TotalTime;
                }
                else
                {
                    CurrentTime += temp;
                }
            }
            else
            {
                if(CurrentTime + temp <= TimeSpan.Zero)
                {
                    CurrentTime = TimeSpan.Zero;
                }
                else
                {
                    CurrentTime += temp;
                }
            }
        }

        /// <summary>
        /// 开始播放或者继续播放
        /// </summary>
        public void Play()
        {
            if (IsPlaying)
            {
                return;
            }

            if (_wavePlayer == null)
            {
                return;
            }

            _wavePlayer.Play();
            IsPlaying = true;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            if (_wavePlayer != null)
            {
                _wavePlayer.Pause();
                IsPlaying = false;
            }
        }
    }
}
