using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utubz.Internal.Platforms
{
    public sealed class GlfwPlatform : Platform
    {
        public override string Name => "GLFW";
        
        public override Window CreateWindow(string title, int width, int height, bool vsync)
        {
            return new Window(title, width, height, vsync);
        }
    }
}
