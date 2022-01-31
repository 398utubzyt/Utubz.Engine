using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utubz.Internal.Platforms
{
    public abstract class Platform
    {
        public abstract string Name { get; }

        public abstract Window CreateWindow(string title, int width, int height, bool vsync);
    }
}
