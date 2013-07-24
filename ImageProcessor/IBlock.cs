using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageProcessor
{
    interface IBlock
    {
        BitmapImage Source { get; }
        IPosition Position { get; }
    }

    class Block: IBlock
    {
        public BitmapImage Source { get; private set; }
        public IPosition Position { get; private set; }

        public Block(BitmapImage source, IPosition position)
        {
            Source = source;
            Position = position;
        }
    }
}
