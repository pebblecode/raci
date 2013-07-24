using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace ImageProcessor
{
    interface IBlockFinder
    {
        IBlock Find(IBlock source, IEnumerable<IBlock> blockLibrary);
    }

    internal class HexColourBlockFinder : IBlockFinder
    {

        public IBlock Find(IBlock source, IEnumerable<IBlock> blockLibrary)
        {
            var libary = blockLibrary.ToArray();

            if (!libary.Any())
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

            return new Block(matchingBlock.Source, source.Position);
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

        private Color GetAverageHexValue(Bitmap img)
        {
            // TODO: Refactor using LINQ
            // TODO: Might need to refactor to only check a sample of pixels

            int r = 0;
            int g = 0;
            int b = 0;
            int total = 0;

            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    Color pixelColor = img.GetPixel(x, y);
                    r += pixelColor.R;
                    g += pixelColor.G;
                    b += pixelColor.B;
                    total++;
                }
            }

            r /= total;
            g /= total;
            b /= total;

            return Color.FromArgb(r, g, b);
        }
    }
}