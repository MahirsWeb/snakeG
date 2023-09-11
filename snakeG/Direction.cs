
using System;
using System.Collections.Generic;

namespace snakeG
{
    public class Direction
    {
        public readonly static Direction left = new Direction(0, -1);
        public readonly static Direction right = new Direction(0, 1);
        public readonly static Direction up = new Direction(-1, 0);
        public readonly static Direction down = new Direction(1, 0);
        public int RowOffSet { get; }
        public int ColOffSet { get; }

        private Direction(int row,int col)
        {
            RowOffSet = row;
            ColOffSet = col;
        }

        public Direction opposite()
        {
            return new Direction(-RowOffSet, -ColOffSet);
        }

        public override bool Equals(object obj)
        {
            return obj is Direction direction &&
                   RowOffSet == direction.RowOffSet &&
                   ColOffSet == direction.ColOffSet;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RowOffSet, ColOffSet);
        }

        public static bool operator ==(Direction left, Direction right)
        {
            return EqualityComparer<Direction>.Default.Equals(left, right);
        }

        public static bool operator !=(Direction left, Direction right)
        {
            return !(left == right);
        }

    }
}
