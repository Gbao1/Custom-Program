//Positions.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRules
{
    public class Positions
    {
        //Attributes
        public int Column {  get; }
        public int Row { get; }


        //Functions
        public Positions(int row, int column)
        {
            Column = column;
            Row = row;
        }

        public override bool Equals(object obj)
        {
            return obj is Positions positions &&
                   Column == positions.Column &&
                   Row == positions.Row;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Column, Row);
        }

        /// <summary>
        /// Default operators like +, -, ==, and != don’t know how to handle user-defined types.
        /// So we need to customise them for convenience.
        /// </summary>

        //checks if two Positions objects are equal by comparing their Column and Row properties
        public static bool operator ==(Positions left, Positions right)
        {
            return EqualityComparer<Positions>.Default.Equals(left, right);
        }

        // checks if two Positions objects are not equal
        public static bool operator !=(Positions left, Positions right)
        {
            return !(left == right);
        }

        //add a Direction object to a Positions object,
        //creates a new Positions object with Row and Column being the sum of the corresponding properties from pos and dir
        public static Positions operator +(Positions pos, Direction dir)
        {
            return new Positions(pos.Row + dir.RowDir, pos.Column + dir.ColumnDir);
        }
    }
}
