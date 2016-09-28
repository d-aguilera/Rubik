using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media.Media3D;

namespace Rubik.Model
{
    public class Cube
    {
        public Cube(int size)
        {
            InitializeMemory();
            InitializeCubelets(size);
        }

        public int Size
        {
            get;
            private set;
        }

        public Collection<Cubelet> Cubelets
        {
            get;
            private set;
        }

        public bool Solved
        {
            get
            {
                var n = Size / 2.0 - 0.5;

                return true
                    && FrontFaceSolved(n)
                    && UpFaceSolved(n)
                    && RightFaceSolved(n)
                    && BackFaceSolved(n)
                    && DownFaceSolved(n)
                    && LeftFaceSolved(n)
                    ;
            }
        }

        bool LeftFaceSolved(double n)
        {
            var l = Cubelets
                .Single(c => c.Position == new Point3D(-n, 0.0, 0.0))
                .FaceColors[Faces.Left]
                ;

            return Cubelets
                .Where(x => x.Position.X == -n)
                .Select(x => x.FaceColors[Faces.Left])
                .All(color => color == l)
                ;
        }

        bool DownFaceSolved(double n)
        {
            var d = Cubelets
                .Single(c => c.Position == new Point3D(0.0, 0.0, -n))
                .FaceColors[Faces.Down]
                ;

            return Cubelets
                .Where(x => x.Position.Z == -n)
                .Select(x => x.FaceColors[Faces.Down])
                .All(color => color == d)
                ;
        }

        bool BackFaceSolved(double n)
        {
            var b = Cubelets
                .Single(c => c.Position == new Point3D(0.0, -n, 0.0))
                .FaceColors[Faces.Back]
                ;

            return Cubelets
                .Where(x => x.Position.Y == -n)
                .Select(x => x.FaceColors[Faces.Back])
                .All(color => color == b)
                ;
        }

        bool RightFaceSolved(double n)
        {
            var r = Cubelets
                .Single(c => c.Position == new Point3D(n, 0.0, 0.0))
                .FaceColors[Faces.Right]
                ;

            return Cubelets
                .Where(x => x.Position.X == n)
                .Select(x => x.FaceColors[Faces.Right])
                .All(color => color == r)
                ;
        }

        bool UpFaceSolved(double n)
        {
            var u = Cubelets
                .Single(c => c.Position == new Point3D(0.0, 0.0, n))
                .FaceColors[Faces.Up]
                ;

            return Cubelets
                .Where(x => x.Position.Z == n)
                .Select(x => x.FaceColors[Faces.Up])
                .All(color => color == u)
                ;
        }

        bool FrontFaceSolved(double n)
        {
            var f = Cubelets
                .Single(c => c.Position == new Point3D(0.0, n, 0.0))
                .FaceColors[Faces.Front]
                ;

            return Cubelets
                .Where(x => x.Position.Y == n)
                .Select(x => x.FaceColors[Faces.Front])
                .All(color => color == f)
                ;
        }

        public void Run(string sequence)
        {
            if (sequence == null)
                throw new ArgumentNullException("sequence");

            var split = sequence.Split()
                .Where(x => !string.IsNullOrEmpty(x))
                ;

            var moves = new List<Move>();
            foreach (var s in split)
                moves.Add(Move.Parse(s));

            Run(moves);
        }

        public void Undo()
        {
            Undo(1);
        }

        public void Undo(int moves)
        {
            for (int i = 0; i < moves; i++)
            {
                Run(new[] { Move.Invert(Memory.Pop()) }, false);
            }
        }

        void Run(IEnumerable<Move> moves, bool memorize = true)
        {
            foreach (var move in moves)
            {
                PerformMove(move);
                if (memorize)
                    Memory.Push(move);
            }
        }

        Stack<Move>Memory
        {
            get;
            set;
        }

        void PerformMove(Move move)
        {
            Cubelet[] ring;
            Vector3D axis;
            Faces[] colorRotationSequence;

            var times = move.Times;
            var inverted = move.Inverted;

            GetMoveParameters(move, out ring, out axis, out colorRotationSequence);

            for (var i = 0; i < ring.Length; i++)
            {
                var cubelet = ring[i];
                var colors = cubelet.FaceColors;

                for (int k = 0; k < times; k++)
                    if (inverted)
                    {
                        var j = colorRotationSequence.Length - 1;
                        var first = colors[colorRotationSequence[j]];
                        for (; j > 0; j--)
                            colors[colorRotationSequence[j]] = colors[colorRotationSequence[j - 1]];
                        colors[colorRotationSequence[j]] = first;
                    }
                    else
                    {
                        var j = 0;
                        var first = colors[colorRotationSequence[j]];
                        for (; j < colorRotationSequence.Length - 1; j++)
                            colors[colorRotationSequence[j]] = colors[colorRotationSequence[j + 1]];
                        colors[colorRotationSequence[j]] = first;
                    }

                var angle = times * (inverted ? -90.0 : 90.0);
                var axisAngle = new AxisAngleRotation3D(axis, angle);
                var transform = new RotateTransform3D(axisAngle);
                cubelet.Position = Round(transform.Transform(cubelet.Position));
            }
        }

        void GetMoveParameters(Move move, out Cubelet[] ring, out Vector3D axis, out Faces[] colorRotationSequence)
        {
            var whichLayers = move.WhichLayers;
            IEnumerable<Cubelet> cubelets;

            switch (move.Face)
            {
                case Faces.Up:
                    cubelets = Cubelets.Where(x => whichLayers == Move.AllLayers || whichLayers.Contains(x.Position.Z));
                    axis = new Vector3D(0.0, 0.0, 1.0);
                    colorRotationSequence = new[] { Faces.Front, Faces.Right, Faces.Back, Faces.Left };
                    break;

                case Faces.Front:
                    cubelets = Cubelets.Where(x => whichLayers == Move.AllLayers || whichLayers.Contains(x.Position.Y));
                    axis = new Vector3D(0.0, 1.0, 0.0);
                    colorRotationSequence = new[] { Faces.Up, Faces.Left, Faces.Down, Faces.Right };
                    break;

                case Faces.Right:
                    cubelets = Cubelets.Where(x => whichLayers == Move.AllLayers || whichLayers.Contains(x.Position.X));
                    axis = new Vector3D(1.0, 0.0, 0.0);
                    colorRotationSequence = new[] { Faces.Up, Faces.Front, Faces.Down, Faces.Back };
                    break;

                case Faces.Down:
                    cubelets = Cubelets.Where(x => whichLayers == Move.AllLayers || whichLayers.Contains(-x.Position.Z));
                    axis = new Vector3D(0.0, 0.0, -1.0);
                    colorRotationSequence = new[] { Faces.Front, Faces.Left, Faces.Back, Faces.Right };
                    break;

                case Faces.Back:
                    cubelets = Cubelets.Where(x => whichLayers == Move.AllLayers || whichLayers.Contains(-x.Position.Y));
                    axis = new Vector3D(0.0, -1.0, 0.0);
                    colorRotationSequence = new[] { Faces.Up, Faces.Right, Faces.Down, Faces.Left };
                    break;

                case Faces.Left:
                    cubelets = Cubelets.Where(x => whichLayers == Move.AllLayers || whichLayers.Contains(-x.Position.X));
                    axis = new Vector3D(-1.0, 0.0, 0.0);
                    colorRotationSequence = new[] { Faces.Up, Faces.Back, Faces.Down, Faces.Front };
                    break;

                default:
                    throw new InvalidOperationException();
            }

            ring = cubelets.ToArray();
        }

        static Point3D Round(Point3D point3D)
        {
            return new Point3D(Math.Round(point3D.X, 1), Math.Round(point3D.Y, 1), Math.Round(point3D.Z, 1));
        }

        void InitializeMemory()
        {
            Memory = new Stack<Move>();
        }

        void InitializeCubelets(int size)
        {
            Size = size;

            Cubelets = new Collection<Cubelet>();

            var n = size / 2.0 - 0.5;

            for (var x = -n; x <= n; x += 1.0)
                for (var y = -n; y <= n; y += 1.0)
                    for (var z = -n; z <= n; z += 1.0)
                        if (x == -n || x == n || y == -n || y == n || z == -n || z == n)
                        {
                            var cubelet = new Cubelet();

                            cubelet.Position = new Point3D(x, y, z);

                            if (x == -n)
                                cubelet.FaceColors[Faces.Left] = Colors.Orange;
                            if (x == n)
                                cubelet.FaceColors[Faces.Right] = Colors.Red;
                            if (y == -n)
                                cubelet.FaceColors[Faces.Back] = Colors.Blue;
                            if (y == n)
                                cubelet.FaceColors[Faces.Front] = Colors.Green;
                            if (z == -n)
                                cubelet.FaceColors[Faces.Down] = Colors.Yellow;
                            if (z == n)
                                cubelet.FaceColors[Faces.Up] = Colors.White;

                            Cubelets.Add(cubelet);
                        }
        }
    }
}
