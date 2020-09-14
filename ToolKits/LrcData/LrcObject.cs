using System;
using System.Collections.Generic;
using System.Text;

namespace ToolKits.LrcData
{
    public class LrcObject
    {
        public List<LrcHeader> Headers;
        public List<LrcLine>   Lines;

        public LrcObject()
        {
            Headers = new List<LrcHeader>();
            Lines = new List<LrcLine>();
        }
    }
}
