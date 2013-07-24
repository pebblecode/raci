using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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
            var testImage = Image.FromFile(@"C:\dev\Code\raci\Tests\testbmp.bmp") as Bitmap;

            BasicBlockGenerator generator = new BasicBlockGenerator(new BlockSize(30,30));

            var blocks = generator.GenerateBlocks(testImage);

            var blockStitcher = new BlockStitcher();
            var newImage = blockStitcher.Stitch(blocks);
            newImage.Save("c:\\testOut.bmp", ImageFormat.Bmp);
            


        }

        [TestMethod]
        public void TestProcessor()
        {
            var testImage = Image.FromFile(@"C:\dev\Code\raci\Tests\testbmp.bmp") as Bitmap;
            var libraryDirectory = @"C:\dev\Code\raci\Tests\LibraryImages\";

            var libraryImages = Directory.GetFiles(libraryDirectory).Select(file => Image.FromFile(file) as Bitmap).ToList();

            var processor = new Processor();

            var output = processor.ProcessImage(testImage, libraryImages, 5, 5);

            output.Save(@"C:\dev\Code\raci\Tests\output.bmp", ImageFormat.Bmp);
        }
    }
}
