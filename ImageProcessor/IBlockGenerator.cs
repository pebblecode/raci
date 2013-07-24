using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessor
{
    interface IBlockGenerator
    {
        IEnumerable<IBlock> GenerateBlocks(Bitmap image);
    }

    public class BasicBlockGenerator : IBlockGenerator
    {
        private readonly BlockSize _size;

        public BasicBlockGenerator(BlockSize size)
        {
            _size = size;
        }

        public IEnumerable<IBlock> GenerateBlocks(Bitmap image)
        {
            var size = new Size(_size.Width, _size.Height);
            var cropArea = new Rectangle {Size = size};
            var blockList = new List<IBlock>();

            for (int x = 0; x <= image.Width - _size.Width; x += (_size.Width))
            {
                for (int y = 0; y <= image.Height - _size.Height; y += (_size.Height))
                {

                    cropArea.Location = new Point(x, y);
                    Bitmap source = image.Clone(cropArea, image.PixelFormat);
                    blockList.Add(new Block(source, new Position(x, y)));
                }
            }

            return blockList;
        }
    }

    public class BlockSize
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
