using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LrcLib
{
    public class LrcObject
    {
        public Hashtable Headers;
        public List<LrcLine>   Lines;

        public LrcObject()
        {
            Headers = new Hashtable();
            Lines = new List<LrcLine>();
        }
    }
}
