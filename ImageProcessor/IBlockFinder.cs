using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace ImageProcessor
{
    interface IBlockFinder
    {
        IBlock Find(IBlock source, IEnumerable<IBlock> blockLibrary);
    }

    class HexColourBlockFinder : IBlockFinder
    {

        public IBlock Find(IBlock source, IEnumerable<IBlock> blockLibrary)
        {
            var libary = blockLibrary.ToArray();

            if (libary.Any())
                throw new ArgumentException("Library contains no blocks!");

            IBlock matchingBlock = libary.First();

            double currentMinDistanceToTarget = DistanceToTargetColour(source, matchingBlock);

            foreach (var blockToTest in libary)
            {
                double distanceToTargetColour = DistanceToTargetColour(source, blockToTest);
                if (currentMinDistanceToTarget > distanceToTargetColour)
                {
                    matchingBlock = blockToTest;
                    currentMinDistanceToTarget = distanceToTargetColour;
                }
            }
            return matchingBlock;
        }

        /**
         * Returns the pythagorean distance between the average rgb colours of the blocks
         */
        private double DistanceToTargetColour(IBlock block1, IBlock block2)
        {
            var averageColour1 = GetAverageHexValue(block1.Source);
            var averageColour2 = GetAverageHexValue(block2.Source);
            return Math.Sqrt(
                Math.Pow(averageColour1.R - averageColour2.R, 2)
                + Math.Pow(averageColour1.G - averageColour2.G, 2)
                + Math.Pow(averageColour1.B - averageColour2.B, 2));
        }

        private Color GetAverageHexValue(BitmapImage img)
        {
            // TODO: Refactor using LINQ

            int r = 0;
            int g = 0;
            int b = 0;
            int total = 0;

            foreach (var i in GetPixelData(img))
            {
                Color pixelColor = Color.FromArgb(i);
                r += pixelColor.R;
                g += pixelColor.G;
                b += pixelColor.B;
                total++;
            }

            r /= total;
            g /= total;
            b /= total;

            return Color.FromArgb(r, g, b);
        }

        private static int[] GetPixelData(BitmapImage img)
        {
            int h = img.PixelHeight;
            int w = img.PixelWidth;
            int[] pixelData = new int[w*h];
            int widthInByte = 4*w;
            img.CopyPixels(pixelData, widthInByte, 0);
            return pixelData;
        }
    }
}