using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utubz
{
    public enum Button
    {
        South,
        East,
        West,
        North,
        LB,
        RB,
        Back,
        Start,
        Guide,
        LeftStick,
        RightStick,
        Up,
        Right,
        Down,
        Left,

        A = South,
        B = East,
        X = West,
        Y = North,

        Cross = South,
        Circle = East,
        Square = West,
        Triangle = North,

        Max = Left + 1
    }
}
