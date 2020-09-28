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
            LrcHeaders[(int)LrcHeader.Type.AR] = new LrcHeader(LrcHeader.Type.AR, "AR");
            LrcHeaders[(int)LrcHeader.Type.TI] = new LrcHeader(LrcHeader.Type.TI, "TI");
            LrcHeaders[(int)LrcHeader.Type.AL] = new LrcHeader(LrcHeader.Type.AL, "AL");
            LrcHeaders[(int)LrcHeader.Type.BY] = new LrcHeader(LrcHeader.Type.BY, "BY");
            LrcHeaders[(int)LrcHeader.Type.OFFSET] = new LrcHeader(LrcHeader.Type.OFFSET, "0");
        }
    }
}
