using System;
using System.Collections.Generic;
using System.Text;

namespace ToolKits.LrcData
{
    public class LrcLine : ILrc
    {
        public TimeSpan Time;
        public string   Text;

        public LrcLine(TimeSpan Time,string Text)
        {
            this.Time = Time;
            this.Text = Text;
        }

        public string FormatLrc()
        {
            return "[" Time.Minutes + ":" + Time.Seconds + "." + Time.Milliseconds + "]" + Text;
        }
    }
}
