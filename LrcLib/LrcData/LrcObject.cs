using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LrcLib.LrcData
{
    public class LrcObject
    {
        public LrcHeader[] LrcHeaders = new LrcHeader[5];
        public List<LrcLine> LrcLines = new List<LrcLine>();

        public LrcObject()
        {
            LrcHeaders[(int)LrcHeader.Type.Ar] = new LrcHeader(LrcHeader.Type.Ar, "AR");
            LrcHeaders[(int)LrcHeader.Type.Ti] = new LrcHeader(LrcHeader.Type.Ti, "TI");
            LrcHeaders[(int)LrcHeader.Type.Al] = new LrcHeader(LrcHeader.Type.Al, "AL");
            LrcHeaders[(int)LrcHeader.Type.By] = new LrcHeader(LrcHeader.Type.By, "BY");
            LrcHeaders[(int)LrcHeader.Type.Offset] = new LrcHeader(LrcHeader.Type.Offset, "0");
        }
    }
}
