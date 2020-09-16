using System;
using System.Collections.Generic;
using System.Text;

namespace LrcLib
{
    public class LrcLine : ILrc , IComparable
    {
        public TimeSpan Time;
        public string   Text;

        public LrcLine(TimeSpan Time,string Text)
        {
            this.Time = Time;
            this.Text = Text;
        }

        public int CompareTo(object obj)
        {
            LrcLine otherLine = obj as LrcLine;
            if (otherLine.Time.TotalMilliseconds > Time.TotalMilliseconds)
                return -1;
            else
                return 1;
        }

        public string FormatLrc()
        {
            return "[" + Time.Minutes + ":" + Time.Seconds + "." + Time.Milliseconds + "]" + Text;
        }
    }
}
