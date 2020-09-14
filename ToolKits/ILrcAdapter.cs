using System;
using System.Collections.Generic;
using System.Text;
using ToolKits.LrcData;

namespace ToolKits
{
    interface ILrcAdapter
    {
        // 文件操作相关
        void ReadLrcFile(ref LrcObject lrcObject,string objectPath);
        void WriteLrcFile(ref LrcObject lrcObjec, string objectPath);
    }
}
