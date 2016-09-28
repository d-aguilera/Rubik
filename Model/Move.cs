using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubik.Model
{
    public class Move
    {
        Move(Faces face, Collection<double> whichLayers, int times) : this(face, whichLayers, times, false)
        {
        }

        Move(Faces face, Collection<double> whichLayers, int times, bool inverted)
        {
            times = ((times + 1) % 4 + 4) % 4 - 1; // will return -1, 0, 1, 2 only

            if (times < 0)
            {
                times = -times;
                inverted = !inverted;
            }

            Face = face;
            Times = times;
            WhichLayers = whichLayers;
            Inverted = inverted;
        }

        public Faces Face
        {
            get;
            private set;
        }

        public int Times
        {
            get;
            private set;
        }

        public Collection<double> WhichLayers
        {
            get;
            private set;
        }

        public bool Inverted
        {
            get;
            set;
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        public override string ToString()
        {
            var initial = Face.ToString()[0].ToString();
            if (WhichLayers == DoubleLayer)
                initial = initial.ToLowerInvariant();
            var inverted = Inverted ? "'" : "";
            var times = Times == 2 ? "2" : "";
            return initial + inverted + times;
        }

        public static Move Parse(string move)
        {
            Move parsed;
            if (!TryParse(move, out parsed))
                throw new ArgumentOutOfRangeException("move");
            return parsed;
        }

        static IDictionary<string, Func<Move>> __movesDictionary;

        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "The rule is wrong.")]
        static IDictionary<string, Func<Move>> MovesDictionary
        {
            get
            {
                if (__movesDictionary == null)
                {
                    __movesDictionary = new Dictionary<string, Func<Move>>
                    {
                        { "F", () => Front() },
                        { "F2", () => Front2() },
                        { "F'", () => FrontInverted() },
                        { "U", () => Up() },
                        { "U2", () => Up2() },
                        { "U'", () => UpInverted() },
                        { "R", () => Right() },
                        { "R2", () => Right2() },
                        { "R'", () => RightInverted() },
                        { "B", () => Back() },
                        { "B2", () => Back2() },
                        { "B'", () => BackInverted() },
                        { "D", () => Down() },
                        { "D2", () => Down2() },
                        { "D'", () => DownInverted() },
                        { "L", () => Left() },
                        { "L2", () => Left2() },
                        { "L'", () => LeftInverted() },
                        { "M", () => M() },
                        { "M2", () => M2() },
                        { "M'", () => MI() },
                        { "E", () => E() },
                        { "E2", () => E2() },
                        { "E'", () => EI() },
                        { "S", () => S() },
                        { "S2", () => S2() },
                        { "S'", () => SI() },
                        { "f", () => DoubleFront() },
                        { "f2", () => DoubleFront2() },
                        { "f'", () => DoubleFrontInverted() },
                        { "u", () => DoubleUp() },
                        { "u2", () => DoubleUp2() },
                        { "u'", () => DoubleUpInverted() },
                        { "r", () => DoubleRight() },
                        { "r2", () => DoubleRight2() },
                        { "r'", () => DoubleRightInverted() },
                        { "b", () => DoubleBack() },
                        { "b2", () => DoubleBack2() },
                        { "b'", () => DoubleBackInverted() },
                        { "d", () => DoubleDown() },
                        { "d2", () => DoubleDown2() },
                        { "d'", () => DoubleDownInverted() },
                        { "l", () => DoubleLeft() },
                        { "l2", () => DoubleLeft2() },
                        { "l'", () => DoubleLeftInverted() },
                        { "X", () => X() },
                        { "X2", () => X2() },
                        { "X'", () => XInverted() },
                        { "Y", () => Y() },
                        { "Y2", () => Y2() },
                        { "Y'", () => YInverted() },
                        { "Z", () => Z() },
                        { "Z2", () => Z2() },
                        { "Z'", () => ZInverted() },
                    };
                }

                return __movesDictionary;
            }
        }

        public static bool TryParse(string move, out Move parsed)
        {
            if (string.IsNullOrEmpty(move))
            {
                parsed = null;
                return false;
            }

            move = move.Trim();

            Func<Move> func;

            if (MovesDictionary.TryGetValue(move, out func))
            {
                parsed = func();
                return true;
            }

            parsed = null;
            return false;
        }

        public static Move Front()
        {
            return new Move(Faces.Front, Layer1, 1);
        }

        public static Move Front2()
        {
            return new Move(Faces.Front, Layer1, 2);
        }

        public static Move FrontInverted()
        {
            return Invert(Front());
        }

        public static Move Up()
        {
            return new Move(Faces.Up, Layer1, 1);
        }

        public static Move Up2()
        {
            return TimesTwo(Up());
        }

        public static Move UpInverted()
        {
            return Invert(Up());
        }

        public static Move Right()
        {
            return new Move(Faces.Right, Layer1, 1);
        }

        public static Move Right2()
        {
            return TimesTwo(Right());
        }

        public static Move RightInverted()
        {
            return Invert(Right());
        }

        public static Move Back()
        {
            return new Move(Faces.Back, Layer1, 1);
        }

        public static Move Back2()
        {
            return TimesTwo(Back());
        }

        public static Move BackInverted()
        {
            return Invert(Back());
        }

        public static Move Down()
        {
            return new Move(Faces.Down, Layer1, 1);
        }

        public static Move Down2()
        {
            return TimesTwo(Down());
        }

        public static Move DownInverted()
        {
            return Invert(Down());
        }

        public static Move Left()
        {
            return new Move(Faces.Left, Layer1, 1);
        }

        public static Move Left2()
        {
            return TimesTwo(Left());
        }

        public static Move LeftInverted()
        {
            return Invert(Left());
        }

        public static Move M()
        {
            return new Move(Faces.Left, Layer0, 1);
        }

        public static Move M2()
        {
            return TimesTwo(M());
        }

        public static Move MI()
        {
            return Invert(M());
        }

        public static Move E()
        {
            return new Move(Faces.Down, Layer0, 1);
        }

        public static Move E2()
        {
            return TimesTwo(E());
        }

        public static Move EI()
        {
            return Invert(E());
        }

        public static Move S()
        {
            return new Move(Faces.Front, Layer0, 1);
        }

        public static Move S2()
        {
            return TimesTwo(S());
        }

        public static Move SI()
        {
            return Invert(S());
        }

        public static Move DoubleUp()
        {
            return Double(Up());
        }

        public static Move DoubleUp2()
        {
            return TimesTwo(DoubleUp());
        }

        public static Move DoubleUpInverted()
        {
            return Invert(DoubleUp());
        }

        public static Move DoubleLeft()
        {
            return Double(Left());
        }

        public static Move DoubleLeft2()
        {
            return TimesTwo(DoubleLeft());
        }

        public static Move DoubleLeftInverted()
        {
            return Invert(DoubleLeft());
        }

        public static Move DoubleFront()
        {
            return Double(Front());
        }

        public static Move DoubleFront2()
        {
            return TimesTwo(DoubleFront());
        }

        public static Move DoubleFrontInverted()
        {
            return Invert(DoubleFront());
        }

        public static Move DoubleRight()
        {
            return Double(Right());
        }

        public static Move DoubleRight2()
        {
            return TimesTwo(DoubleRight());
        }

        public static Move DoubleRightInverted()
        {
            return Invert(DoubleRight());
        }

        public static Move DoubleBack()
        {
            return Double(Back());
        }

        public static Move DoubleBack2()
        {
            return TimesTwo(DoubleBack());
        }

        public static Move DoubleBackInverted()
        {
            return Invert(DoubleBack());
        }

        public static Move DoubleDown()
        {
            return Double(Down());
        }

        public static Move DoubleDown2()
        {
            return TimesTwo(DoubleDown());
        }

        public static Move DoubleDownInverted()
        {
            return Invert(DoubleDown());
        }

        public static Move X()
        {
            return All(Right());
        }

        public static Move X2()
        {
            return TimesTwo(X());
        }

        public static Move XInverted()
        {
            return Invert(X());
        }

        public static Move Y()
        {
            return All(Up());
        }

        public static Move Y2()
        {
            return TimesTwo(Y());
        }

        public static Move YInverted()
        {
            return Invert(Y());
        }

        public static Move Z()
        {
            return All(Front());
        }

        public static Move Z2()
        {
            return TimesTwo(Z());
        }

        public static Move ZInverted()
        {
            return Invert(Z());
        }

        public static Move Invert(Move move)
        {
            if (move == null)
                throw new ArgumentNullException("move");

            move.Inverted = !move.Inverted;
            return move;
        }

        static Move TimesTwo(Move move)
        {
            move.Times = 2;
            return move;
        }

        static Move Double(Move move)
        {
            move.WhichLayers = DoubleLayer;
            return move;
        }

        static Move All(Move move)
        {
            move.WhichLayers = AllLayers;
            return move;
        }

        public static Collection<double> AllLayers = new Collection<double> { };
        public static Collection<double> Layer0 = new Collection<double> { 0.0 };
        public static Collection<double> Layer1 = new Collection<double> { 1.0 };
        public static Collection<double> DoubleLayer = new Collection<double> { 1.0, 0.0 };
    }
}
