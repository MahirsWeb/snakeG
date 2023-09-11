
using System;
using System.Collections.Generic;

namespace snakeG
{
    public class position
    {
        public int Row { get; }
        public int Col { get; }

        public position(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public position Translate(Direction dirc)
        {
            return new position(Row + dirc.RowOffSet, Col + dirc.ColOffSet);
        }

        public override bool Equals(object obj)
        {
            return obj is position position &&
                   Row == position.Row &&
                   Col == position.Col;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Col);
        }

        public static bool operator ==(position left, position right)
        {
            return EqualityComparer<position>.Default.Equals(left, right);
        }

        public static bool operator !=(position left, position right)
        {
            return !(left == right);
        }
    }
}
