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
        public string Text;

        public static string[] FormatString = { @"(\[\d{1,2}\:\d{1,2}\.\d{2,3}\])", @"(\[\d{1,2}\:\d{1,2}\]))" };

        public LrcLine() { }

        public LrcLine(TimeSpan Time,string Text)
        {
            this.Time = Time;
            this.Text = Text;
        }

        public static string TimeToString(TimeSpan Time)
        {
            string min = Time.Minutes.ToString();
            if (min.Length == 1) min = "0" + min;

            string sec = Time.Seconds.ToString();
            if (sec.Length == 1) sec = "0" + sec;

            string ms = Time.Milliseconds.ToString();
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
            if (!Regex.IsMatch(temp[temp.Length - 1], @"(\[\d{ 1,2}\:\d{ 1,2}\.\d{ 2,3}\])| (\[\d{ 1,2}\:\d{ 1,2}\])"))
            {
                text = temp[temp.Length - 1];
                for (int i = 0; i < temp.Length - 1; i++)
                {
                    TimeSpan timeSpan = GetLrcTime(temp[i] + "]");
                    result.Add(new LrcLine(timeSpan, text));
                }
            }
            else
            {
                text = "";
                for (int i = 0; i < temp.Length; i++)
                {
                    TimeSpan timeSpan = GetLrcTime(temp[i] + "]");
                    result.Add(new LrcLine(timeSpan, text));
                }
            }
            return result;
        }

        public static TimeSpan GetLrcTime(string line)
        {
            int min;
            int sec;
            int ms;
            // 有毫秒格式
            if (Regex.IsMatch(line, FormatString[0]))
            {
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
