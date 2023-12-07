using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LrcLib.LrcData
{
    public class LrcLine
    {
        public TimeSpan Time;
        public readonly string Text;

        private static readonly string[] FormatString =
            { @"(\[\d{1,2}\:\d{1,2}\.\d{2,3}\])", @"(\[\d{1,2}\:\d{1,2}\]))" };

        public LrcLine()
        {
        }

        private LrcLine(TimeSpan time, string text)
        {
            this.Time = time;
            this.Text = text;
        }

        private static string TimeToString(TimeSpan time)
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

        public override string ToString()
        {
            return "[" + TimeToString(Time) + "]" + Text;
        }

        public static bool IsLine(string line)
        {
            if (Regex.IsMatch(line, FormatString[0]) || Regex.IsMatch(line, FormatString[1]))
                return true;
            else
                return false;
        }

        public static List<LrcLine> Pause(string line)
        {
            string[] temp = line.Split(']');
            string text;
            List<LrcLine> result = new List<LrcLine>();
            // 有歌词
            if (!Regex.IsMatch(temp[^1], @"(\[\d{ 1,2}\:\d{ 1,2}\.\d{ 2,3}\])| (\[\d{ 1,2}\:\d{ 1,2}\])"))
            {
                text = temp[^1];
                for (int i = 0; i < temp.Length - 1; i++)
                {
                    TimeSpan timeSpan = GetLrcTime(temp[i] + "]");
                    result.Add(new LrcLine(timeSpan, text));
                }
            }
            else
            {
                text = "";
                foreach (var t in temp)
                {
                    TimeSpan timeSpan = GetLrcTime(t + "]");
                    result.Add(new LrcLine(timeSpan, text));
                }
            }

            return result;
        }

        private static TimeSpan GetLrcTime(string line)
        {
            int min;
            int sec;
            // 有毫秒格式
            if (Regex.IsMatch(line, FormatString[0]))
            {
                int ms;
                line = line.Substring(1, line.Length - 2);
                string[] temp = line.Split(':', '.');
                min = int.Parse(temp[0]);
                sec = int.Parse(temp[1]);
                if (temp[2].Length == 2) ms = 10 * int.Parse(temp[2]);
                else ms = int.Parse(temp[2]);
                return new TimeSpan(0, 0, min, sec, ms);
            }
            else if (Regex.IsMatch(line, FormatString[1]))
            {
                line = line.Substring(1, line.Length - 2);
                string[] temp = line.Split(':');
                // temp[0] is min
                // temp[1] is sec
                min = Int32.Parse(temp[0]);
                sec = Int32.Parse(temp[1]);
                return new TimeSpan(0, 0, min, sec);
            }
            else
            {
                return TimeSpan.Zero;
            }
        }
    }
}