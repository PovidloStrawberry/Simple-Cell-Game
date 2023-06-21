using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellGame
{
    public abstract class CellCreator
    {
        public abstract Cell CreateCell(Field field);
    }
    public class PlantCreator : CellCreator
    {
        public override Cell CreateCell(Field field)
        {
            return new Plant(field);
        }
    }
    public class MushroomCreator : CellCreator
    {
        public override Cell CreateCell(Field field)
        {
            return new Mushroom(field);
        }
    }
    public class Predator_RedCreator : CellCreator
    {
        public override Cell CreateCell(Field field)
        {
            return new Predator_Red(field);
        }
    }
    public class Predator_BlueCreator : CellCreator
    {
        public override Cell CreateCell(Field field)
        {
            return new Predator_Blue(field);
        }
    }
    public class ParasiteCreator : CellCreator
    {
        public override Cell CreateCell(Field field)
        {
            return new Parasite(field);
        }
    }
}
