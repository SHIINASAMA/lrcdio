using System;
using System.Collections.Generic;
using System.Text;

namespace LrcLib
{
    interface ILrcAdapter
    {
        // 文件操作相关
        void ReadLrcFile(ref LrcObject lrcObject,string objectPath);
        void WriteLrcFile(ref LrcObject lrcObjec, string objectPath);
    }
}
