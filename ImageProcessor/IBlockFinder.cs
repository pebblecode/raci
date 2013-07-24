using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace ImageProcessor
{
    interface IBlockFinder
    {
        IBlock Find(IBlock source);
    }

    internal class HexColourBlockFinder : IBlockFinder
    {
        private IDictionary<Color, IBlock> libraryColorCache = new Dictionary<Color, IBlock>();
 
        public HexColourBlockFinder(IEnumerable<IBlock> blockLibrary)
        {
            foreach (var block in blockLibrary)
            {
                libraryColorCache[GetAverageHexValue(block.Source)] = block;
            }
        }

        public IBlock Find(IBlock source)
        {
            IBlock matchingBlock = libraryColorCache.First().Value;

            double currentMinDistanceToTarget = DistanceToTargetColour(source, libraryColorCache.First().Key);

            foreach (var colour in libraryColorCache.Keys)
            {
                double distanceToTargetColour = DistanceToTargetColour(source, colour);
                if (currentMinDistanceToTarget > distanceToTargetColour)
                {
                    matchingBlock = libraryColorCache[colour];
                    currentMinDistanceToTarget = distanceToTargetColour;
                }
            }

            return new Block(matchingBlock.Source, source.Position);
        }

        /**
         * Returns the pythagorean distance between the average rgb colours of the blocks
         */
        private double DistanceToTargetColour(IBlock block1, Color colour)
        {
            var averageColour1 = GetAverageHexValue(block1.Source);
            return Math.Sqrt(
                Math.Pow(averageColour1.R - colour.R, 2)
                + Math.Pow(averageColour1.G - colour.G, 2)
                + Math.Pow(averageColour1.B - colour.B, 2));
        }

        private Color GetAverageHexValue(Bitmap img)
        {

            // TODO: Refactor using LINQ
            // TODO: Might need to refactor to only check a sample of pixels

            int r = 0;
            int g = 0;
            int b = 0;
            int total = 0;

            for (int i = 0; i < img.Width && i < img.Height; i++)
            {
                Color pixelColor = img.GetPixel(i, i);
                r += pixelColor.R;
                g += pixelColor.G;
                b += pixelColor.B;
                total++;
            }

            for (int i = 0; i < img.Width && i < img.Height; i++)
            {
                Color pixelColor = img.GetPixel(i, img.Height - i - 1);
                r += pixelColor.R;
                g += pixelColor.G;
                b += pixelColor.B;
                total++;
            }

                //for (int x = 0; x < img.Width; x++)
                //{
                //    for (int y = 0; y < img.Height; y++)
                //    {
                //        Color pixelColor = img.GetPixel(x, y);
                //        r += pixelColor.R;
                //        g += pixelColor.G;
                //        b += pixelColor.B;
                //        total++;
                //    }
                //}

                r /= total;
            g /= total;
            b /= total;

            return Color.FromArgb(r, g, b);
        }
    }
}