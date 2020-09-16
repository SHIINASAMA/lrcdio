﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LrcLib
{
    public class LrcHeader : ILrc
    {
        public enum TagType
        {
            Ar,
            Ti,
            Al,
            By,
            Offset,
            UnKnow
        };

        public TagType Type;
        public string  Text;

        public LrcHeader(TagType Type,string Text)
        {
            this.Type = Type;
            this.Text = Text;
        }

        public string FormatLrc()
        {
            switch (Type)
            {
                case TagType.Ar:
                    return "[Ar:" + Text + "]";
                case TagType.Ti:
                    return "[Ti:" + Text + "]";
                case TagType.Al:
                    return "[Al:" + Text + "]";
                case TagType.By:
                    return "[By:" + Text + "]";
                case TagType.Offset:
                    return "[Offset:" + Text + "]";
                default:
                    return null;
            }
        }
    }
}
