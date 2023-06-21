using CellGame;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellGame
{
    public class Predator : Cell
    {
        public Predator(Field field) : base(field)
        {
            startEnergy = 50;
            cellColor = Brushes.Red;
            organicLeft = 15;
            lifePrice = 1;
            reproductionPrice = 12;
            lifeTime = random.Next(50, 71);
            isPredator = true;
        }
        public virtual void makeChild(int x, int y, int energyFromCell)
        {
            field.cells[x, y] = new Predator(field);
            field.cells[x, y].energy = energyFromCell + lifePrice * 2;
        }
        public void Reproduct(int newX, int newY)
        {
            int energyFromCell = lifePrice * 2;
            if (field.cells[newX, newY] != null)
                energyFromCell = field.cells[newX, newY].energy;
            field.cells[x, y].energy -= reproductionPrice + lifePrice * 2;
            makeChild(newX, newY, energyFromCell);
        }
        public override void LiveActivity()
        {
            if (!isBorn)
            {
                if (energy >= reproductionPrice + lifePrice * 3 && CountOfMyTypeNeibours() < 8)
                {
                    bool done = false;
                    while (!done)
                    {
                        int newX = GetBornedCellXPosition();
                        int newY = GetBornedCellYPosition();
                        if (field.cells[newX, newY] != null && field.cells[newX, newY].GetType() != field.cells[x, y].GetType() || field.cells[newX, newY] == null)
                        {
                            if (field.cells[newX, newY] != null && field.cells[newX, newY].isPredator)
                            {
                                if (field.cells[newX, newY].energy < field.cells[x, y].energy)
                                {
                                    Reproduct(newX, newY);
                                }
                            }
                            else
                            {
                                Reproduct(newX, newY);
                            }

                        }
                        done = true;
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
