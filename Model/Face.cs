using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubik.Model
{
    [Flags]
    public enum Faces
    {
        None = 0,
        Up = 1,
        Front = 2,
        Right = 4,
        Down = 8,
        Back = 16,
        Left = 32,
    }
}
