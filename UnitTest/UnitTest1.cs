using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolKits.Adapter;
using ToolKits.LrcData;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        // ≤‚ ‘–¥»Îƒ£øÈ
        [TestMethod]
        public void TestWrite()
        {
            LrcObject lrc = new LrcObject();
            lrc.Headers.Add(new LrcHeader(LrcHeader.TagType.By, "Kaoru"));
            lrc.Lines.Add(new LrcLine(new System.TimeSpan(0, 0, 1, 2, 100), "This is a Test"));

            LrcAdapter adapter = new LrcAdapter();
            adapter.WriteLrcFile(ref lrc, "E:/1.lrc");
        }

        [TestMethod]
        public void TestRead()
        {

        }
    }
}
