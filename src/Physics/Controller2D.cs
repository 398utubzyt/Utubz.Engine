using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utubz.Physics
{
    public class Controller2D : Collider2D
    {
        public void Move(Vector2 motion)
        {
            Transform.Translate(motion);
        }
    }
}
