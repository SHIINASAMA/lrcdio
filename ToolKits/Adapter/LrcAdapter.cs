using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ToolKits.LrcData;

namespace ToolKits.Adapter
{
    public class LrcAdapter : ILrcAdapter
    {
        public void ReadLrcFile(ref LrcObject lrcObject, string objectPath)
        {
            
        }

        public void WriteLrcFile(ref LrcObject lrcObject, string objectPath)
        {
            using (StreamWriter writer = new StreamWriter(objectPath))
            {
                foreach (LrcHeader header in lrcObject.Headers)
                {
                    writer.WriteLine(header.FormatLrc() + "\r\n");
                }
                foreach (LrcLine line in lrcObject.Lines)
                {
                    writer.WriteLine(line.FormatLrc() + "\r\n");
                }
                writer.Flush();
            }
        }
    }
}
