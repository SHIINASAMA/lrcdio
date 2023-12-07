using System.Text.RegularExpressions;

namespace LrcLib.LrcData
{
    public class LrcHeader
    {
        public enum Type
        {
            Ar,
            Al,
            Ti,
            Offset,
            By,
            Unknown
        }

        public Type HeaderType;

        public string Text;

        public static readonly string FormatString = @"\[\w{2,6}\:.*\]";

        public static bool IsHeader(string line)
        {
            return Regex.IsMatch(line, FormatString);
        }

        private LrcHeader()
        {
        }

        public LrcHeader(Type type, string text)
        {
            this.HeaderType = type;
            this.Text = text;
        }

        public static LrcHeader Pause(string line)
        {
            LrcHeader header = new LrcHeader();
            line = line.Substring(1, line.Length - 2);
            string[] temp = line.Split(':');
            switch (temp[0].ToUpper())
            {
                case "AR":
                    header.HeaderType = Type.Ar;
                    break;
                case "TI":
                    header.HeaderType = Type.Ti;
                    break;
                case "AL":
                    header.HeaderType = Type.Al;
                    break;
                case "BY":
                    header.HeaderType = Type.By;
                    break;
                case "OFFSET":
                    header.HeaderType = Type.Offset;
                    break;
                default:
                    header.HeaderType = Type.Unknown;
                    break;
            }

            header.Text = temp[1];
            return header;
        }

        public override string ToString()
        {
            switch (HeaderType)
            {
                case Type.Ar:
                    return "[Ar:" + Text + "]";
                case Type.Ti:
                    return "[Ti:" + Text + "]";
                case Type.Al:
                    return "[Al:" + Text + "]";
                case Type.By:
                    return "[By:" + Text + "]";
                case Type.Offset:
                    return "[Offset:" + Text + "]";
                default:
                    return null;
            }
        }
    }
}