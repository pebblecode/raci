using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessor
{
    public class Processor
    {
        public Bitmap ProcessImage(Bitmap original, IEnumerable<Bitmap> libraryBase, int blockHeight, int blockWidth)
        {
            IBlockGenerator generator = new BasicBlockGenerator(new BlockSize(blockWidth, blockHeight));
            var library = LoadLibrary(libraryBase, generator).ToList();
            var decomposedOriginal = generator.GenerateBlocks(original);
            IBlockFinder finder = new HexColourBlockFinder();
            var newBlocks = decomposedOriginal.Select(originalBlock => finder.Find(originalBlock, library));
            IBlockStitcher stitcher = new BlockStitcher();
            return stitcher.Stitch(newBlocks);
        }

        private IEnumerable<IBlock> LoadLibrary(IEnumerable<Bitmap> libraryBase, IBlockGenerator generator)
        {
            return libraryBase.SelectMany(generator.GenerateBlocks);
        }
    }
}
