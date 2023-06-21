using CellGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellGame
{
    public class Field
    {
        public Cell[,] cells;
        public int[,] organic;
        public int cols, rows;
        public Random random = new Random();
        public Field(int cols, int rows)
        {
            this.cols = cols;
            this.rows = rows;
            this.cells = new Cell[this.cols, this.rows];
            this.organic = new int[this.cols, this.rows];
        }
    }
}
