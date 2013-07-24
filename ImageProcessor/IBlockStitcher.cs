using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageProcessor
{
    interface IBlockStitcher
    {
        BitmapImage Stitch(IEnumerable<IBlock> blocks);
    }

    class BlockStitcher : IBlockStitcher
    {
        public BitmapImage Stitch(IEnumerable<IBlock> blocks)
        {
            throw new NotImplementedException();
        }
    }
}
