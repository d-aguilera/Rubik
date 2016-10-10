using Microsoft.VisualStudio.TestTools.UnitTesting;

using Rubik.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

namespace Rubik.UnitTests
{
    [TestClass]
    public class BasicTests
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestInitialize]
        public void Initialize()
        {
            TestContext.Properties["Cube"] = new Cube(3);
        }

        Cube Cube
        {
            get
            {
                return (Cube)testContextInstance.Properties["Cube"];
            }
        }

        [TestMethod]
        public void CycleTest()
        {
            Assert.IsTrue(Cube.Solved);

            for (int i = 0; i < 4; i++)
            {
                Cube.Run("U");
            }

            Assert.IsTrue(Cube.Solved);

            for (int i = 0; i < 10; i++)
            {
                Cube.Run("R U R' F");
            }

            Assert.IsTrue(Cube.Solved);

            for (int i = 0; i < 6; i++)
            {
                Cube.Run("R U R' U R U U R'");
            }

            Assert.IsTrue(Cube.Solved);

            for (int i = 0; i < 6; i++)
            {
                Cube.Run("F U R U' R' F'");
            }

            Assert.IsTrue(Cube.Solved);

            for (int i = 0; i < 6; i++)
            {
                Cube.Run("F R U R' U' F'");
            }

            Assert.IsTrue(Cube.Solved);
        }

        [TestMethod]
        public void UndoTest()
        {
            Assert.IsTrue(Cube.Solved);

            Scramble(Cube);

            Cube.Run("L' F' B2 U' F R D' B' L B' L D2 R' U2 F2 D2 B' R2");

            Assert.IsTrue(Cube.Solved);

            var moves = "Z2 D' L' R' F R D2 U2 L' U' L U' U' U' F' U' F U R U' R' Y R U R' U' Y U' U' R U R' U r U r' R U R' U' r U' r'";

            Cube.Run(moves);

            Cube.Undo(moves.Split().Length);

            Assert.IsTrue(Cube.Solved);
        }

        [TestMethod]
        public void LucasTest()
        {
            Assert.IsTrue(Cube.Solved);

            Assert.IsTrue(TopFaceIsWhite(Cube));

            Scramble(Cube);

            AssertCube(Cube, new[] { "GOGOGGBRW", "RYRWWRGWG", "BOYORGRYO", "WRYYBROWR", "BBYBYBYYW", "OBBWOGOGW" });

            Cube.Run("Z2"); // inspection

            Cube.Run("D' L' R' F R D2"); // cross

            AssertCube(Cube, new[] { "YGGGGRYOO", "BRWBYGROB", "GBWOOBBGW", "BYWBBYYRG", "RWGWWWOWO", "YORRRYRYO" });

            Cube.Run("U2 L' U' L"); // 1st pair

            Cube.Run("U' U' U' F' U' F U R U' R'"); // 2nd pair

            AssertCube(Cube, new[] { "GGGGGYGGB", "YOYYYBBRR", "GBOOOYOOW", "BYOBBGYRW", "RWWWWWOWW", "YOBRRBRRR" });

            Cube.Run("Y R U R'"); // 3rd pair

            AssertCube(Cube, new[] { "OOYOOROOY", "RRGBYBYYG", "BYOBBGBBR", "RRBRRYYOB", "WWWWWWRWW", "GGWGGYGGO" });

            Cube.Run("U' Y U' U' R U R'"); // 4th pair

            AssertCube(Cube, new[] { "BBYBBOBBG", "YBBYYYGRR", "RRORRYRRY", "GGOGGGGGY", "WWWWWWWWW", "OOBOOYOOR" });

            Cube.Run("U r U r' R U R' U' r U' r'"); // OLL(CP)

            Assert.IsTrue(Cube.Solved);
        }

        static bool TopFaceIsWhite(Cube cube)
        {
            var topCenterPosition = new Point3D(0.0, 0.0, 1.0);
            var topCenterCubelet = cube.Cubelets.Single(x => x.Position == topCenterPosition);
            var topCenterColor = topCenterCubelet.FaceColors[Faces.Up];
            
            return topCenterColor == Colors.White;
        }

        static void Scramble(Cube cube)
        {
            cube.Run("R2 B D2 F2 U2 R D2 L' B L' B D R' F' U B2 F L");
        }

        static void AssertCube(Cube cube, string[] colorInitials)
        {
            int index = 0;

            var sequence = colorInitials
                .SelectMany(x => x)
                .Select(x => x.ToString())
                .Select(x => ParseInitial(x))
                .ToArray()
                ;

            AssertFace(cube, sequence, ref index, Faces.Front);
            AssertFace(cube, sequence, ref index, Faces.Up);
            AssertFace(cube, sequence, ref index, Faces.Right);
            AssertFace(cube, sequence, ref index, Faces.Back);
            AssertFace(cube, sequence, ref index, Faces.Down);
        }

        static void AssertFace(Cube cube, Colors[] sequence, ref int index, Faces face)
        {
            double x1, x2, y1, y2, z1, z2;

            switch (face)
            {
                case Faces.Up:
                    x1 = -1.0; x2 = 1.0;
                    y1 = -1.0; y2 = 1.0;
                    z1 = 1.0; z2 = 1.0;
                    break;
                case Faces.Front:
                    x1 = -1.0; x2 = 1.0;
                    y1 = 1.0; y2 = 1.0;
                    z1 = -1.0; z2 = 1.0;
                    break;
                case Faces.Right:
                    x1 = 1.0; x2 = 1.0;
                    y1 = -1.0; y2 = 1.0;
                    z1 = -1.0; z2 = 1.0;
                    break;
                case Faces.Down:
                    x1 = -1.0; x2 = 1.0;
                    y1 = -1.0; y2 = 1.0;
                    z1 = -1.0; z2 = -1.0;
                    break;
                case Faces.Back:
                    x1 = -1.0; x2 = 1.0;
                    y1 = -1.0; y2 = -1.0;
                    z1 = -1.0; z2 = 1.0;
                    break;
                case Faces.Left:
                    x1 = -1.0; x2 = -1.0;
                    y1 = -1.0; y2 = 1.0;
                    z1 = -1.0; z2 = 1.0;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            Cubelet cubelet;

            for (var x = x1; x <= x2; x += 1.0)
                for (var y = y1; y <= y2; y += 1.0)
                    for (var z = z1; z <= z2; z += 1.0)
                    {
                        cubelet = cube.Cubelets.Single(c => c.Position.X == x && c.Position.Y == y && c.Position.Z == z);
                        Assert.AreEqual(sequence[index++], cubelet.FaceColors[face]);
                    }
        }

        static Colors ParseInitial(string initial)
        {
            switch (initial)
            {
                case "B": return Colors.Blue;
                case "G": return Colors.Green;
                case "O": return Colors.Orange;
                case "R": return Colors.Red;
                case "W": return Colors.White;
                case "Y": return Colors.Yellow;
                default: return Colors.None;
            }
        }

        static void DumpFace(Cube cube, Faces face)
        {
            IEnumerable<Cubelet> ring;
            var whichLayers = Move.Layer1;

            switch (face)
            {
                case Faces.Up:
                    ring = cube.Cubelets.Where(x => whichLayers == Move.AllLayers || whichLayers.Contains(x.Position.Z));
                    break;
                case Faces.Front:
                    ring = cube.Cubelets.Where(x => whichLayers == Move.AllLayers || whichLayers.Contains(x.Position.Y));
                    break;
                case Faces.Right:
                    ring = cube.Cubelets.Where(x => whichLayers == Move.AllLayers || whichLayers.Contains(x.Position.X));
                    break;
                case Faces.Down:
                    ring = cube.Cubelets.Where(x => whichLayers == Move.AllLayers || whichLayers.Contains(-x.Position.Z));
                    break;
                case Faces.Back:
                    ring = cube.Cubelets.Where(x => whichLayers == Move.AllLayers || whichLayers.Contains(-x.Position.Y));
                    break;
                case Faces.Left:
                    ring = cube.Cubelets.Where(x => whichLayers == Move.AllLayers || whichLayers.Contains(-x.Position.X));
                    break;
                default:
                    throw new InvalidOperationException();
            }

            var colors = new List<Colors>();
            foreach (var cubelet in ring)
            {
                colors.Add(cubelet.FaceColors[face]);
            }

            System.Diagnostics.Debug.WriteLine(face + ": " + string.Join(" - ", colors));
        }
    }
}
