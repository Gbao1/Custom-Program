//Direction.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRules
{
    public class Direction
    {
        //Attributes
        public readonly static Direction Up = new Direction(-1, 0);
        public readonly static Direction Down = new Direction(1, 0);
        public readonly static Direction Right = new Direction(0, 1);
        public readonly static Direction Left = new Direction(0, -1);

        public readonly static Direction UpLeft = Up + Left;
        public readonly static Direction UpRight = Up + Right;
        public readonly static Direction DownLeft = Down + Left;
        public readonly static Direction DownRight = Down + Right;

        public int RowDir { get; }
        public int ColumnDir { get; }


        //Functions
        public Direction(int rowDir, int columnDir)
        {
            RowDir = rowDir;
            ColumnDir = columnDir;
        }

        //add two Direction objects together
        public static Direction operator +(Direction dir1, Direction dir2)
        {
            return new Direction(dir1.RowDir + dir2.RowDir, dir1.ColumnDir + dir2.ColumnDir);
        }

        //scale a Direction object by a given integer scale
        public static Direction operator *(int scale, Direction dir)
        {
            return new Direction(scale * dir.RowDir, scale * dir.ColumnDir);
        }

    }
}
