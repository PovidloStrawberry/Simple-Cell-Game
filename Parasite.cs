using CellGame;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellGame
{
    public class Parasite : Cell
    {
        public int parasitism = 3;
        public Parasite(Field field) : base(field)
        {
            startEnergy = 1;
            cellColor = Brushes.Violet;
            type = 5;
            organicLeft = 5;
            lifePrice = 1;
            reproductionPrice = 100;
            lifeTime = random.Next(1000, 2001);
        }
        public virtual void makeChild(int x, int y, int energyFromCell)
        {
            field.cells[x, y] = new Parasite(field);
            field.cells[x, y].InitPosition(x, y);
            field.cells[x, y].energy = energyFromCell + lifePrice * 2;
        }
        public void Reproduct(int newX, int newY)
        {
            int energyFromCell = lifePrice * 2;
            field.cells[x, y].energy -= reproductionPrice + lifePrice * 2;
            makeChild(newX, newY, energyFromCell);
        }
        public override void LiveActivity()
        {
            if (!isBorn)
            {
                if (energy >= reproductionPrice + lifePrice * 3 + 1 && CountOfNeibours() < 8)
                {
                    bool done = false;
                    while (!done)
                    {
                        int a = random.Next(-1, 2);
                        int b = random.Next(-1, 2);
                        int newX = GetBornedCellXPosition();
                        int newY = GetBornedCellYPosition();
                        if (field.cells[newX, newY] == null)
                        {
                            Reproduct(newX, newY); 
                            done = true;
                        }
                    }
                }
                else
                {
                    if (CountOfNotMyTypeNeiboursWithEnergy(parasitism) > 0)
                    {
                        bool done = false;
                        while (!done)
                        {
                            int a = random.Next(-1, 2);
                            int b = random.Next(-1, 2);
                            if (field.cells[(x + a + field.cols) % field.cols, (y + b + field.rows) % field.rows] != null && field.cells[(x + a + field.cols) % field.cols, (y + b + field.rows) % field.rows].type != type)
                            {
                                if (field.cells[(x + a + field.cols) % field.cols, (y + b + field.rows) % field.rows].energy >= parasitism)
                                {
                                    field.cells[(x + a + field.cols) % field.cols, (y + b + field.rows) % field.rows].energy -= parasitism;
                                    energy += parasitism;

                                }
                                done = true;
                            }
                        }
                    }
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
