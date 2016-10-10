using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubik.Model
{
    static class Extensions
    {
        public static IEnumerable<Cubelet> FrontFace(this IEnumerable<Cubelet> cubelets, double whichLayer)
        {
            return cubelets.Where(x => x.Position.Y == whichLayer);
        }

        public static IEnumerable<Cubelet> UpFace(this IEnumerable<Cubelet> cubelets, double whichLayer)
        {
            return cubelets.Where(x => x.Position.Z == whichLayer);
        }

        public static IEnumerable<Cubelet> RightFace(this IEnumerable<Cubelet> cubelets, double whichLayer)
        {
            return cubelets.Where(x => x.Position.X == whichLayer);
        }

        public static IEnumerable<Cubelet> BackFace(this IEnumerable<Cubelet> cubelets, double whichLayer)
        {
            return cubelets.Where(x => x.Position.Y == -whichLayer);
        }

        public static IEnumerable<Cubelet> DownFace(this IEnumerable<Cubelet> cubelets, double whichLayer)
        {
            return cubelets.Where(x => x.Position.Z == -whichLayer);
        }

        public static IEnumerable<Cubelet> LeftFace(this IEnumerable<Cubelet> cubelets, double whichLayer)
        {
            return cubelets.Where(x => x.Position.X == -whichLayer);
        }
    }
}
