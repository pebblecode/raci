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

    class BlockStitcher : IBlockStitcher
    {
        public Bitmap Stitch(IEnumerable<IBlock> blocks)
        {
            //var image = new Bitmap();
            //Graphics graphics = Graphics.FromImage(image);
            //graphics.DrawImage();
            //return image;
            return null;
        }
    }
}
