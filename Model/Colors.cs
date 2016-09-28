using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubik.Model
{
    [Flags]
    public enum Colors
    {
        None = 0,
        White = 1,
        Green = 2,
        Red = 4,
        Yellow = 8,
        Blue = 16,
        Orange = 32,
    }
}
