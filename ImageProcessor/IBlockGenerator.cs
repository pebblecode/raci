using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageProcessor
{
    interface IBlockGenerator
    {
        IEnumerable<IBlock> GenerateBlocks(Bitmap image);
    }

    class BasicBlockGenerator : IBlockGenerator
    {
        private readonly BlockSize _size;

        public BasicBlockGenerator(BlockSize size)
        {
            _size = size;
        }

        public IEnumerable<IBlock> GenerateBlocks(Bitmap image)
        {
            var size = new Size(_size.Width, _size.Height);
            var blockList = new List<IBlock>();
            for (int x = 0; x < image.Width; x += _size.Width)
            {
                for (int y = 0; y < image.Height; y += _size.Width)
                {
                    var cropArea = new Rectangle(new Point(x, y), size);
                    Bitmap blockImg = image.Clone(cropArea, image.PixelFormat);
                    blockList.Add(new Block(blockImg, new Position(x, y)));
                }
            }
            return blockList;
        }
    }

    class BlockSize
    {
        public int Height { get; private set; }
        public int Width { get; private set; }

        public BlockSize(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
