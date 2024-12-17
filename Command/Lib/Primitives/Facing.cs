using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Command.Lib.Primitives
{
    public enum Facing
    {
        North,
        East,
        South,
        West
    }

    public static class Facings
    {
        public static Point SouthDisplacement = new Point(0, 1);
        public static Point NorthDisplacement = new Point(0, -1);
        public static Point EastDisplacement = new Point(1, 0);
        public static Point WestDisplacement = new Point(-1, 0);

        public static Facing[] DoubleAxis = [Facing.North, Facing.East, Facing.South, Facing.West];
        public static Point ToDisplacement(Facing facing)
        {
            return facing switch
            {
                Facing.North => NorthDisplacement,
                Facing.East => EastDisplacement,
                Facing.South => SouthDisplacement,
                Facing.West => WestDisplacement,
                _ => throw new Exception()
            };
        }

        public static Facing ToFacing(Point displacement)
        {
            return displacement switch
            {
                { X: 0, Y: -1 } => Facing.North,
                { X: 1, Y: 0 } => Facing.East,
                { X: 0, Y: 1 } => Facing.South,
                { X: -1, Y: 0 } => Facing.West,
                _ => throw new Exception()
            };
        }

        public static Facing Opposing(Facing facing) => facing switch
        {
            Facing.North => Facing.South,
            Facing.East => Facing.West,
            Facing.South => Facing.North,
            Facing.West => Facing.East,
            _ => throw new Exception()
        };
    }
}

