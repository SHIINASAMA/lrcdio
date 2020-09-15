using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using ToolKits.LrcData;

namespace ToolKits.Adapter
{
    public class LrcAdapter : ILrcAdapter
    {
        string _LrcLine1 = @"\[\d{1,2}\:\d{1,2}\.\d{2,3}\]";
        string _LrcLine2 = @"\[\d{1,2}\:\d{1,2}\]";
        string _LrcHeader = @"^\[\w{2,6}\:\w{1,}\]";

        /// <summary>
        /// 格式化读取 Lrc 文件
        /// </summary>
        /// <param name="lrcObject">LrcObject 对象的引用</param>
        /// <param name="objectPath">Lrc 文件路径</param>
        public void ReadLrcFile(ref LrcObject lrcObject, string objectPath)
        {
            string pattern = @"((\[\d{1,2}\:\d{1,2}\.\d{2,3}\])|(\[\d{1,2}\:\d{1,2}\]))+";
            using (StreamReader reader = new StreamReader(objectPath))
            {
                string temp;
                while((temp = reader.ReadLine()) != null)
                {
                    // 歌词
                    if (Regex.IsMatch(temp, pattern))
                    {
                        foreach (LrcLine line in FormatLrcLine(temp))
                        {
                            lrcObject.Lines.Add(line);
                        }
                    }
                    else if (Regex.IsMatch(temp, _LrcHeader))
                    {
                        LrcHeader header = FormatLrcHeader(temp);
                        lrcObject.Headers.Add(header.Type,header.Text);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// 格式化写入 Lrc 文件
        /// </summary>
        /// <param name="lrcObject">LrcObject 对象的引用</param>
        /// <param name="objectPath">写入的文件路径</param>
        public void WriteLrcFile(ref LrcObject lrcObject, string objectPath)
        {
            using (StreamWriter writer = new StreamWriter(objectPath))
            {
                foreach (LrcHeader header in lrcObject.Headers)
                {
                    writer.WriteLine(header.FormatLrc());
                }
                foreach (LrcLine line in lrcObject.Lines)
                {
                    writer.WriteLine(line.FormatLrc());
                }
                writer.Flush();
            }
        }

        /// <summary>
        /// <para>将时间标签格式转换为 TimeSpan 类型</para>
        /// <para>支持 [xx:xx:xxx]，[xx:xx] 等</para>
        /// </summary>
        /// <param name="str">时间标签</param>
        /// <returns></returns>
        public TimeSpan GetLrcTime(string str) 
        {
            int min;
            int sec;
            int ms;
            // 除去两端[]
            str = str.Substring(1, str.Length - 2);
            // 有毫秒格式
            if (Regex.IsMatch(str, _LrcLine1))
            {
                // 00:00.00
                string[] temp = str.Split(':', ',');
                // temp[0] is min
                // temp[1] is sec
                // temp[2] is ms
                min = Int32.Parse(temp[0]);
                sec = Int32.Parse(temp[1]);
                if (temp[2].Length == 2) ms = 10 * Int32.Parse(temp[2]);
                else ms = Int32.Parse(temp[2]);
                return new TimeSpan(0,0,min,sec,ms);
            }
            else if(Regex.IsMatch(str, _LrcLine2))
            {
                string[] temp = str.Split(':');
                // temp[0] is min
                // temp[1] is sec
                min = Int32.Parse(temp[0]);
                sec = Int32.Parse(temp[1]);
                return new TimeSpan(0, 0, min, sec);
            }
            else
            {
                return TimeSpan.Zero;
            }
        }

        /// <summary>
        /// 计算一行文本中有多少个[xx:xx:xx]标签
        /// </summary>
        /// <param name="str">一行字符串</param>
        /// <returns></returns>
        public int GetCount(string str)
        {
            string[] temp = str.Split(':');
            return temp.Length;
        }

        /// <summary>
        /// 将一行歌词解析为 LrcLine 对象
        /// </summary>
        /// <param name="str">一行歌词</param>
        /// <returns></returns>
        public List<LrcLine> FormatLrcLine(string str)
        {
            string[] temp = str.Split(']');
            string text;
            List<LrcLine> result = new List<LrcLine>();
            // 有歌词
            if(!Regex.IsMatch(temp[temp.Length - 1], @"(\[\d{ 1,2}\:\d{ 1,2}\.\d{ 2,3}\])| (\[\d{ 1,2}\:\d{ 1,2}\])"))
            {
                text = temp[temp.Length - 1];
                for(int i = 0;i< temp.Length - 1; i++)
                {
                    TimeSpan timeSpan = GetLrcTime(temp[i] + "]");
                    result.Add(new LrcLine(timeSpan, text));
                }
            }else
            {
                text = "";
                for(int i = 0; i < temp.Length; i++)
                {
                    TimeSpan timeSpan = GetLrcTime(temp[i] + "]");
                    result.Add(new LrcLine(timeSpan, text));
                }
            }
            return result;
        }

        /// <summary>
        /// <para>将一行信息解析为 LrcHeader 对象</para>
        /// <para>支持ar、ti、al、by、offset</para>
        /// </summary>
        /// <param name="str">一行信息</param>
        /// <returns></returns>
        public LrcHeader FormatLrcHeader(string str)
        {
            str = str.Substring(1, str.Length - 2);
            string[] temp = str.Split(':');
            LrcHeader.TagType tag = LrcHeader.TagType.UnKnow;
            switch (temp[0].ToUpper())
            {
                case "AR":
                    tag = LrcHeader.TagType.Ar;
                    break;
                case "TI":
                    tag = LrcHeader.TagType.Ti;
                    break;
                case "AL":
                    tag = LrcHeader.TagType.Al;
                    break;
                case "BY":
                    tag = LrcHeader.TagType.By;
                    break;
                case "OFFSET":
                    tag = LrcHeader.TagType.Offset;
                    break;
            }
            return new LrcHeader(tag, temp[1]);
        }
    }
}
