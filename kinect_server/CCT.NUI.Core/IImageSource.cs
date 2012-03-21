using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CCT.NUI.Core
{
    public interface IImageSource : IDataSource<Bitmap>
    {
        IntSize Size { get; }

        int Width { get; }

        int Height { get; }
    }
}
