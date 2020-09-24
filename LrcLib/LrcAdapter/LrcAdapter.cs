﻿using LrcLib.LrcData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LrcLib.LrcAdapter
{
    public class LrcAdapter
    {
        public static void WriteToFile(ref LrcObject lrc,string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                foreach(LrcHeader header in lrc.LrcHeaders)
                {
                    writer.WriteLine(header.ToString());
                }

                foreach (LrcLine line in lrc.LrcLines)
                {
                    writer.WriteLine(line.ToString());
                }
                writer.Flush();
            }
        }

        public static void ReadFromFile(ref LrcObject lrc,string path)
        {
            string pattern = @"((\[\d{1,2}\:\d{1,2}\.\d{2,3}\])|(\[\d{1,2}\:\d{1,2}\]))+";
            using (StreamReader reader = new StreamReader(path))
            {
                string temp;
                while ((temp = reader.ReadLine()) != null)
                {
                    // 歌词
                    if (Regex.IsMatch(temp, pattern))
                    {
                        foreach (LrcLine line in LrcLine.Pause(temp))
                        {
                            lrc.LrcLines.Add(line);
                        }
                    }
                    // 信息
                    else if (Regex.IsMatch(temp, LrcHeader.FormatString))
                    {
                        LrcHeader header = LrcHeader.Pause(temp);
                        lrc.LrcHeaders[(int)header.HeaderType].Text = header.Text;
                    }
                    // 其他则跳过
                    else
                    {
                        continue;
                    }
                }
            }
        }
    }
}
