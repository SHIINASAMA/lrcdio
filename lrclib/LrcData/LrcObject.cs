using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace LrcLib
{
    public class LrcObject
    {
        public Hashtable     Headers;
        public List<LrcLine> Lines;

        public LrcObject()
        {
            Headers = new Hashtable();
            Lines = new List<LrcLine>();
        }

        private string GetInfo(LrcHeader.TagType tag)
        {
            string temp = (string)Headers[tag];
            if (temp == null) return "NULL";
            else return temp;
        }
    }
}
