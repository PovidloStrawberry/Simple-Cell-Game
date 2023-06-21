using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellGame
{
    public  class Cell
    {
        public bool isImmortal;
        public bool isPredator;
        public bool isPlant;
        public int organicLeft;
        public int lifeTime;
        public int startEnergy;

        public Brush cellColor;
        public bool isBorn;
        public int x, y;
        public int energy;
        public int reproductionPrice;
        public int lifePrice;
        public int type;
        public Field field;
        public Random random;
        public Cell(Field field)
        {
            this.field = field;
            random = this.field.random;
            isBorn = true;
        }
        public void InitPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public virtual void LiveActivity()
        {

        }
        protected int GetBornedCellXPosition()
        {
            int randX = random.Next(-1, 2);
            int newX = (x + randX + field.cols) % field.cols;
            return newX;
        }
        protected int GetBornedCellYPosition()
        {
            int randY = random.Next(-1, 2);
            int newY = (y + randY + field.rows) % field.rows;
            return newY;
        }
        public int CountOfNeiboursWithEnergy(int e)
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + field.cols) % field.cols;
                    var row = (y + j + field.rows) % field.rows;
                    var isSelfChecking = col == x && row == y;
                    if (field.cells[col, row] != null && !isSelfChecking && field.cells[col, row].energy >= e) count++;
                }
            }
            return count;
        }
        public int CountOfNotMyTypeNeiboursWithEnergy(int e)
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + field.cols) % field.cols;
                    var row = (y + j + field.rows) % field.rows;
                    var isSelfChecking = col == x && row == y;
                    if (field.cells[col, row] != null && !isSelfChecking && field.cells[col, row].energy >= e && field.cells[col, row].type != field.cells[x, y].type && !field.cells[col, row].isPredator) count++;
                }
            }
            return count;
        }
        public int CountOfNeibours()
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + field.cols) % field.cols;
                    var row = (y + j + field.rows) % field.rows;
                    var isSelfChecking = col == x && row == y;
                    if (field.cells[col, row] != null && !isSelfChecking) count++;
                }
            }
            return count;
        }

        public int CountOfMyTypeNeibours()
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + field.cols) % field.cols;
                    var row = (y + j + field.rows) % field.rows;
                    var isSelfChecking = col == x && row == y;
                    if (field.cells[col, row] != null && field.cells[col, row].type == field.cells[x, y].type && !isSelfChecking) count++;
                }
            }
            return count;
        }
        public void Dead()
        {
            if (!Convert.ToBoolean(energy * lifeTime))
            {
                field.cells[x, y] = null;
                field.organic[x, y] += organicLeft;
                field.organic[x, y] = Math.Min(255, field.organic[x, y]);
            }
        }
    }
}
