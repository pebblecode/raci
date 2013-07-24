using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessor
{
    public interface IBlock
    {
        Bitmap Source { get; }
        IPosition Position { get; }
    }

    public class Block: IBlock
    {
        public Bitmap Source { get; private set; }
        public IPosition Position { get; private set; }

        public Block(Bitmap source, IPosition position)
        {
            Source = source;
            Position = position;
        }
    }
}
