using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;

namespace LrcLib.LrcData
{
    public class LrcHeader
    {
        public enum Type
        {
            AR,AL,TI,OFFSET,BY,UNKNOW
        }

        public Type HeaderType = Type.UNKNOW;

        public string Text;

        public static string FormatString = @"\[\w{2,6}\:.*\]";

        public LrcHeader() { }

        public LrcHeader(Type Type,string Text)
        {
            HeaderType = Type;
            this.Text = Text;
        }

        public static bool IsHeader(string line)
        {
            return Regex.IsMatch(line,FormatString);
        }

        public static LrcHeader Pause(string line) 
        {
            LrcHeader header = new LrcHeader();
            line = line.Substring(1, line.Length - 2);
            string[] temp = line.Split(':');
            switch (temp[0].ToUpper())
            {
                case "AR":
                    header.HeaderType = Type.AR;
                    break;
                case "TI":
                    header.HeaderType = Type.TI;
                    break;
                case "AL":
                    header.HeaderType = Type.AL;
                    break;
                case "BY":
                    header.HeaderType = Type.BY;
                    break;
                case "OFFSET":
                    header.HeaderType = Type.OFFSET;
                    break;
                default:
                    header.HeaderType = Type.UNKNOW;
                    break;
            }
            header.Text = temp[1];
            return header;
        }

        public override string ToString()
        {
            switch (HeaderType)
            {
                case Type.AR:
                    return "[Ar:" + Text + "]";
                case Type.TI:
                    return "[Ti:" + Text + "]";
                case Type.AL:
                    return "[Al:" + Text + "]";
                case Type.BY:
                    return "[By:" + Text + "]";
                case Type.OFFSET:
                    return "[Offset:" + Text + "]";
                default:
                    return null;
            }
        }
    }
}
