using CellGame;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellGame
{
    public class Mushroom : Cell
    {
        public int organicEat = 2;
        public Mushroom(Field field) : base(field)
        {
            startEnergy = 5;
            cellColor = Brushes.Aqua;
            organicLeft = 0;
            lifePrice = 1;
            type = 2;
            reproductionPrice = 6;
            lifeTime = 1;
        }
        public override void LiveActivity()
        {
            if (!isBorn)
            {
                if (energy >= reproductionPrice + organicEat && CountOfNeibours() < 8)
                {
                    bool done = false;
                    while (!done)
                    {
                        int newX = GetBornedCellXPosition();
                        int newY = GetBornedCellYPosition();
                        if (field.cells[newX, newY] == null || field.cells[newX, newY].type == 1)
                        {
                            field.cells[x, y].energy -= reproductionPrice;
                            field.cells[newX, newY] = new Mushroom(field);
                            field.cells[newX, newY].InitPosition(newX, newY);

                        }
                        done = true;
                    }
                }
                else
                {
                    if (field.organic[x, y] >= organicEat)
                    {
                        energy += organicEat;
                        field.organic[x, y] -= organicEat;
                        energy -= energy % 255 * (energy / 255);
                    }
                }
                energy -= lifePrice;
                energy = Math.Max(energy, 0);

                Dead();
            }
            isBorn = false;
        }
    }
}
