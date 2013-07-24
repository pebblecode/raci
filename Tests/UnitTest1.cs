using System;
using System.Drawing;
using System.Drawing.Imaging;
using ImageProcessor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestGenAndStitch()
        {
            var testImage = Image.FromFile(@"..\..\testimg.bmp") as Bitmap;

            BasicBlockGenerator generator = new BasicBlockGenerator(new BlockSize(20,20));

            var blocks = generator.GenerateBlocks(testImage);

            var blockStitcher = new BlockStitcher();
            var newImage = blockStitcher.Stitch(blocks);
            newImage.Save("testOut.bmp", ImageFormat.Bmp);
            


        }
    }
}
