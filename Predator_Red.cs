using CellGame;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellGame
{
    public class Predator_Red : Predator
    {
        public Predator_Red(Field field) : base(field)
        {
            type = 3;
            cellColor = Brushes.Red;
        }
        public override void makeChild(int x, int y, int energyFromCell)
        {
            field.cells[x, y] = new Predator_Red(field);
            field.cells[x, y].InitPosition(x, y);
            field.cells[x, y].energy = energyFromCell + lifePrice * 2;
        }

    }
}
