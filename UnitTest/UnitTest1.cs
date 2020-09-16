using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using LrcLib;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        // 测试写入模块
        [TestMethod]
        public void TestWrite()
        {
            LrcObject lrc = new LrcObject();
            lrc.Headers.Add(LrcHeader.TagType.By, "Kaoru");
            lrc.Lines.Add(new LrcLine(new System.TimeSpan(0, 0, 1, 2, 100), "This is a Test"));

            LrcAdapter adapter = new LrcAdapter();
            adapter.WriteLrcFile(ref lrc, @"..\..\..\..\ToolKits\WriteTest.lrc");
        }

        // 测试读取模块
        [TestMethod]
        public void TestRead()
        {
            LrcObject lrc = new LrcObject();
            LrcAdapter adapter = new LrcAdapter();
            adapter.ReadLrcFile(ref lrc, @"..\..\..\..\ToolKits\ReadTest.lrc");
            Console.WriteLine("歌手是：" + lrc.Headers[LrcHeader.TagType.Ar]);
            Console.WriteLine("专辑是：" + lrc.Headers[LrcHeader.TagType.Al]);
            Console.WriteLine("歌名是：" + lrc.Headers[LrcHeader.TagType.Ti]);
            Console.WriteLine("Lrc编者是：" + lrc.Headers[LrcHeader.TagType.By]);
            Console.WriteLine("补偿值是：" + lrc.Headers[LrcHeader.TagType.Offset]);
            lrc.Lines.Sort();
            foreach (LrcLine line in lrc.Lines)
            {
                Console.WriteLine("min:{0},sec:{1},ms:{2},text:{3}", line.Time.Minutes, line.Time.Seconds, line.Time.Milliseconds, line.Text);
            }
        }
    }
}
