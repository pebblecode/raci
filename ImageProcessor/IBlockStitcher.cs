using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessor
{
    interface IBlockStitcher
    {
        Bitmap Stitch(IEnumerable<IBlock> blocks);
    }

    public class BlockStitcher : IBlockStitcher
    {
        public Bitmap Stitch(IEnumerable<IBlock> blocks)
        {
            var blocksCollection = blocks.ToList();
            CheckStitchingParameters(blocksCollection);
            var exampleBlock = blocksCollection.First().Source;
            var blockWidth = exampleBlock.Width;
            var blockHeight = exampleBlock.Height;
            var baseImageForSettings = blocksCollection.First().Source;
            var image = new Bitmap(CalculateImageWidth(blocksCollection), CalculateImageHeight(blocksCollection), baseImageForSettings.PixelFormat);
            image.SetResolution(baseImageForSettings.HorizontalResolution, baseImageForSettings.VerticalResolution);
            var graphics = Graphics.FromImage(image);
            
            
            foreach (var block in blocksCollection)
            {
                graphics.DrawImage(block.Source, block.Position.X, block.Position.Y);
            }
            return image;
        }

        private int CalculateImageWidth(IEnumerable<IBlock> blocksCollection)
        {
            return blocksCollection.Max(block => block.Position.X + block.Source.Width);
        }

        private int CalculateImageHeight(IEnumerable<IBlock> blocksCollection)
        {
            return blocksCollection.Max(block => block.Position.Y + block.Source.Height);
        }

        private static void CheckStitchingParameters(List<IBlock> blocksCollection)
        {
            if (!blocksCollection.Any())
            {
                throw new ArgumentException("No blocks were provided to build an image with");
            }
            // Also check all blocks have the same width
        }
    }
}
