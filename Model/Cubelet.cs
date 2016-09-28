using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Rubik.Model
{
    public class Cubelet
    {
        public Cubelet()
        {
            FaceColors = new Dictionary<Faces, Colors>
            {
                { Faces.Up, Colors.None },
                { Faces.Front, Colors.None },
                { Faces.Right, Colors.None },
                { Faces.Down, Colors.None },
                { Faces.Back, Colors.None },
                { Faces.Left, Colors.None },
            };
        }

        public Point3D Position
        {
            get;
            set;
        }

        public IDictionary<Faces, Colors> FaceColors
        {
            get;
            private set;
        }
    }
}
