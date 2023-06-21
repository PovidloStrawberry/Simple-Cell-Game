using CellGame;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellGame
{
    public class Plant : Cell
    {
        public int photosynthesis = 2;

        public Plant(Field field) : base(field)
        {
            startEnergy = 1;
            cellColor = Brushes.Green;
            organicLeft = 10;
            lifePrice = 0;
            type = 1;
            reproductionPrice = 20;
            lifeTime = random.Next(30, 91);
        }
        public override void LiveActivity()
        {
            if (!isBorn)
            {
                if (energy >= reproductionPrice + photosynthesis && CountOfNeibours() < 8)
                {
                    bool done = false;
                    while (!done)
                    {
                        int newX = GetBornedCellXPosition();
                        int newY = GetBornedCellYPosition();
                        if ((field.cells[newX, newY] == null))
                        {
                            field.cells[x, y].energy -= reproductionPrice;
                            field.cells[newX, newY] = new Plant(field);
                            field.cells[newX, newY].InitPosition(newX, newY);

                        }
                        done = true;
                    }
                }
                else
                {
                    energy += photosynthesis;
                    energy -= energy % 255 * (energy / 255);
                }
                energy -= lifePrice;
                energy = Math.Max(energy, 0);
                lifeTime--;

                Dead();
            }
            isBorn = false;
        }
    }
}
