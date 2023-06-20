using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CellGame
{
    
    public partial class Form1 : Form
    {
        //List<Cell> cells;

        public Cell[,] cells;

        string watchMode;

        //############################################
        int isAlive = 0;
        int r = 1;
        int g = 2;
        int b = 3;
        int worldEnergy = 4;
        int cellEnergy = 5;
        int cellx1 = 6;
        int celly1 = 7;
        int cellx2 = 8;
        int celly2 = 9;
        int worldOrganic = 10;
        int isBorn = 6;

        int red = 1;
        int green = 2;
        int blue = 3;
        //############################################
        int[] cellReproductionPrice = { -1, 300, 2, 1 };
        int[] cellEatCan = { -1, 0, 2, 0 };
        int[] cellOrgaicLeft = { 0, 0, 0, 0 };
        int[] civAttack = {0, 0, 0, 0, 0, 0};
        //############################################

        public Random random; 
        //public Random random = new Random();

        int selectedcelltype = 1;

        double worldEnegryCount = 0;
        double worldOrganicCount = 0;
        private Graphics graphics;
        private int resoluton;
        private int[,,] field;
        private int rows;
        private int cols;
        private int cellCounter = 0;
        //private int[,] cells;

        double middleWorldEnergy = 0;
        double middleWorldOrganic = 0;

        int aPrice = 5;

        public Form1()
        {
            InitializeComponent();
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void Stop()
        {
            if (!timer1.Enabled) return;
            timer1.Stop();
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    field[i, j, 0] = 0;
                }
            }
            NewCellDrawing();
            nudResolution.Enabled = true;
            nudDensity.Enabled = true;
        }

        private void CellDrawing()
        {
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (watchMode == "cells")
                    {
                        switch (field[i, j, 0])
                        {
                            case 0:
                                graphics.FillRectangle(Brushes.Black, i * resoluton, j * resoluton, resoluton, resoluton);
                                break;

                            case 1:
                                graphics.FillRectangle(Brushes.Green, i * resoluton, j * resoluton, resoluton, resoluton);
                                break;

                            case 2:
                                graphics.FillRectangle(Brushes.Red, i * resoluton, j * resoluton, resoluton, resoluton);
                                break;

                            case 3:
                                graphics.FillRectangle(Brushes.YellowGreen, i * resoluton, j * resoluton, resoluton, resoluton);
                                break;
                            case 4:
                                graphics.FillRectangle(Brushes.Aqua, i * resoluton, j * resoluton, resoluton, resoluton);
                                break;
                            case 5:
                                graphics.FillRectangle(Brushes.Peru, i * resoluton, j * resoluton, resoluton, resoluton);
                                break;
                        }
                    }
                    else if(watchMode == "organic") { }
                }
            }
            pictureBox1.Refresh();
        }
        private void NewCellDrawing()
        {
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (watchMode == "cells")
                    {
                        if (gameField.cells[i, j] != null)
                        {
                            switch (gameField.cells[i, j].type)
                            {
                                case 0:
                                    graphics.FillRectangle(Brushes.Black, i * resoluton, j * resoluton, resoluton, resoluton);
                                    break;

                                case 1:
                                    graphics.FillRectangle(Brushes.Green, i * resoluton, j * resoluton, resoluton, resoluton);
                                    break;

                                case 2:
                                    graphics.FillRectangle(Brushes.Aqua, i * resoluton, j * resoluton, resoluton, resoluton);
                                    break;

                                case 3:
                                    graphics.FillRectangle(Brushes.Red, i * resoluton, j * resoluton, resoluton, resoluton);
                                    break;
                                case 4:
                                    graphics.FillRectangle(Brushes.Blue, i * resoluton, j * resoluton, resoluton, resoluton);
                                    break;
                                case 5:
                                    graphics.FillRectangle(Brushes.Violet, i * resoluton, j * resoluton, resoluton, resoluton);
                                    break;
                            }
                        }
                        else
                            graphics.FillRectangle(Brushes.Black, i * resoluton, j * resoluton, resoluton, resoluton);
                    }
                    
                    else if(watchMode == "organic")
                    {
                        int o = gameField.organic[i, j];
                        Color organicColor = new Color();
                        organicColor = Color.FromArgb(0, o, 0);
                        graphics.FillRectangle(new SolidBrush(organicColor), i * resoluton, j * resoluton, resoluton, resoluton);
                    }

                    else if (watchMode == "energy")
                    {
                        if (gameField.cells[i, j] != null)
                        {
                            int o = gameField.cells[i, j].energy;
                            Color energyColor = new Color();
                            energyColor = Color.FromArgb(0, 0, o);
                            graphics.FillRectangle(new SolidBrush(energyColor), i * resoluton, j * resoluton, resoluton, resoluton);
                        }
                        else
                        {
                            graphics.FillRectangle(Brushes.Black, i * resoluton, j * resoluton, resoluton, resoluton);
                        }
                    }
                }
            }
            pictureBox1.Refresh();
        }

        private void NextGenegation()
        {
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if(field[i, j, 0] >= 0) 
                    {
                        random = new Random(DateTime.UtcNow.Millisecond);
                        /*
                        for (int a = -1; a < 2; a++)
                        {
                            for (int b = -1; b < 2; b++)
                            {
                                var col = (i + a + cols) % cols;
                                var row = (j + b + rows) % rows;
                                var isSelfChecking = col == i && row == j;
                                var hasLife = field[col, row, 0];
                                if(hasLife != 0)
                                    field[i, j, Convert.ToInt32(hasLife)]++;
                            }
                        }
                        int count = Convert.ToInt32(Math.Max(field[i, j, 3], Math.Max(field[i, j, 2], field[i, j, 1])));
                        if (field[i, j, 1] > 0 || field[i, j, 2] > 0 || field[i, j, 3] > 0) 
                        { 
                            for (int h = 1; h < 4; h++)
                            {
                                if (Convert.ToInt32(field[i, j, h]) == count)
                                    field[i, j, 0] = h;
                            }
                        }
                        */

                        bool isReproduct = false;
                        /*
                        if (field[i, j, cellEnergy] <= 0)
                        {
                            field[i, j, 0] = 0;
                            field[i, j, cellEnergy] = 0;
                        }
                        */
                        if (field[i, j, isBorn] == 0)
                        {
                            if ( /* field[i, j, cellEnergy] > 0 && */ field[i, j, 0] > 0 && field[i, j, cellEnergy] >= aPrice && CountOfMyNeighbours(i, j) < 8)
                            {
                                bool done = false;
                                while (CountOfMyNeighbours(i, j) < 8 && !done && field[i, j, cellEnergy] >= aPrice)
                                {
                                    int a = random.Next(-1, 2);
                                    int b = random.Next(-1, 2);
                                    if (field[(i + a + cols) % cols, (j + b + rows) % rows, 0] != field[i, j, 0])
                                    {
                                        /*
                                        field [(i + a + cols) % cols, (j + b + rows) % rows, cellEnergy] -= 2;
                                            field[i, j, cellEnergy] -= 2;
                                            if (field[(i + a + cols) % cols, (j + b + rows) % rows, cellEnergy] <= 0)
                                            {
                                                field[(i + a + cols) % cols, (j + b + rows) % rows, 0] = field[i, j, 0];
                                                field[(i + a + cols) % cols, (j + b + rows) % rows, cellEnergy] = 1;
                                            }
                                        */

                                        field[i, j, cellEnergy] -= aPrice;
                                        field[(i + a + cols) % cols, (j + b + rows) % rows, 0] = field[i, j, 0];
                                        field[(i + a + cols) % cols, (j + b + rows) % rows, isBorn] = 1;
                                        //field[(i + a + cols) % cols, (j + b + rows) % rows, cellEnergy] = 1;
                                        done = true;
                                    }
                                }
                            }
                            else
                            {
                                //field[i, j, cellEnergy]++;
                                //civAttack[field[i, j, 0]]++;
                                field[i, j, cellEnergy]++;
                            }
                        }
                        field[i, j, isBorn] = 0; 
                    }
                }
            }
            CellDrawing();
        }
        private void NewNextGenegation()
        {
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (gameField.cells[i, j] != null)
                    {
                        gameField.cells[i, j].LiveActivity(i, j);
                    }
                }
            }
            NewCellDrawing();
        }

        private void cellReproduction(int x, int y)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + cols) % cols;
                    var row = (y + j + rows) % rows;
                    if (field[col, row, 0] == 0)
                    {
                        field[col, row, 0] = 1;
                        field[col, row, 1] = 2;
                        field[x, y, 1] -= 1;
                        return;
                    }
                }
            }
        }

        private int CountNeighbours(int x, int y)
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + cols) % cols;
                    var row = (y + j + rows) % rows;
                    var isSelfChecking = col == x && row == y;
                    var hasLife = field[col, row, 0];
                    if (hasLife > 0 && !isSelfChecking) count++;
                }
            }
            return count;
        }

        private int NewCountNeighbours(int x, int y)
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + cols) % cols;
                    var row = (y + j + rows) % rows;
                    var isSelfChecking = col == x && row == y;
                    var hasLife = cells[col, row].type;
                    if (hasLife > 0 && !isSelfChecking) count++;
                }
            }
            return count;
        }

        private int CountOfMyNeighbours(int x, int y)
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + cols) % cols;
                    var row = (y + j + rows) % rows;
                    var isSelfChecking = col == x && row == y;
                    //var hasLife = field[col, row, 0];
                    if (field[col, row, 0] == field[x, y, 0] && !isSelfChecking) count++;
                }
            }
            return count;
        }
        public Field gameField;
        private void StartGame()
        {
            watchMode = "cells";
            //if (timer1.Enabled) return;
            nudResolution.Enabled = false;
            nudDensity.Enabled = false;
            resoluton = (int)nudResolution.Value;
            rows = pictureBox1.Height / resoluton;
            cols = pictureBox1.Width / resoluton;
            gameField = new Field(cols, rows);
            field = new int[cols, rows, 11];
            //cells = new int[cols * rows, 4];
            Random random = new Random(2312312);
            int counter = 0;

            cells = new Cell[cols, rows]; 
            //field[40, 30, 0] = 1;
            //field[40, 30, cellEnergy] = 1;
            //field[100, 20, 0] = 2;
            //for (int i = 0; i < cols; i++)
            //{
            //    for (int j = 0; j < rows; j++)
            //    {
            //        field[i, j, 0] = random.Next(0, 3);
            //        if (field[i, j, 0] != 0)
            //            field[i, j, cellEnergy] = 10;
            //    }
            //}
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            worldEnegryCount = 0;
            worldOrganicCount = 0;
            textBox1.Text = worldEnegryCount + "";
            textBox2.Text = worldOrganicCount + "";
            StartGame();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            NewNextGenegation();
            //graphics.FillRectangle(Brushes.White, 0, 970, resoluton, resoluton);
            //pictureBox1.Refresh();
        }

        private void breload_Click(object sender, EventArgs e)
        {
            if (!timer1.Enabled) return;
            Stop();
            StartGame();
            //graphics.FillRectangle(Brushes.Black);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var pos1 = PictureBox.MousePosition.X / resoluton - 287 / resoluton;
            var pos2 = PictureBox.MousePosition.Y / resoluton - 30 / resoluton;
            //field[(pos1 + cols) % cols, (pos2 + rows) % rows, 0] = 1;
            switch (selectedcelltype)
            {
                case 1:
                    gameField.cells[(pos1 + cols) % cols, (pos2 + rows) % rows] = new Plant(gameField);//.type = selectedcelltype;
                    gameField.cells[(pos1 + cols) % cols, (pos2 + rows) % rows].energy = 1;
                    break;
                case 2:
                    gameField.cells[(pos1 + cols) % cols, (pos2 + rows) % rows] = new Mushroom(gameField);//.type = selectedcelltype;
                    gameField.cells[(pos1 + cols) % cols, (pos2 + rows) % rows].energy = 5;
                    break;
                case 3:
                    gameField.cells[(pos1 + cols) % cols, (pos2 + rows) % rows] = new Predator_Red(gameField);//.type = selectedcelltype;
                    gameField.cells[(pos1 + cols) % cols, (pos2 + rows) % rows].energy = 50;
                    break;
                case 4:
                    gameField.cells[(pos1 + cols) % cols, (pos2 + rows) % rows] = new Predator_Blue(gameField);//.type = selectedcelltype;
                    gameField.cells[(pos1 + cols) % cols, (pos2 + rows) % rows].energy = 50;
                    break;
                case 5:
                    gameField.cells[(pos1 + cols) % cols, (pos2 + rows) % rows] = new Parasite(gameField);//.type = selectedcelltype;
                    gameField.cells[(pos1 + cols) % cols, (pos2 + rows) % rows].energy = 1;
                    break;
            }

            //field[(pos1 + cols) % cols, (pos2 + rows - 1) % rows] = 1;
            //field[(pos1 + cols + 1) % cols, (pos2 + rows) % rows] = 1;
            //field[(pos1 + cols) % cols, (pos2 + rows + 1) % rows] = 1;
            //field[(pos1 + cols - 1) % cols, (pos2 + rows + 1) % rows] = 1;
            //field[(pos1 + cols + 1) % cols, (pos2 + rows + 1) % rows] = 1;

            //if (field[pos1, pos2] == 0) field[pos1, pos2] = 1;
            //else field[pos1, pos2] = 0;
            NewCellDrawing();
        }

        //bStart.BackgroundImage = Image.FromFile(@"D:\2.jpg", false);
        //bStart.BackgroundImageLayout = ImageLayout.Center;

        //для заполнения пикселя
        //field[(pos1 + cols) % cols, (pos2 + rows) % rows] = 1;

        private void bPause_Click(object sender, EventArgs e)
        {
            // if (!timer1.Enabled) return;

            if (timer1.Enabled) timer1.Stop();
            else timer1.Start();
        }

        private void bNextFrame_Click(object sender, EventArgs e)
        {
            NewNextGenegation();
        }

       

        private void tocell1_Click(object sender, EventArgs e)
        {
            selectedcelltype = 1;
            textBox3.Text = "1";
        }

        private void tocell2_Click(object sender, EventArgs e)
        {
            selectedcelltype = 2;
            textBox3.Text = "2";
        }

        private void tocell3_Click(object sender, EventArgs e)
        {
            selectedcelltype = 3;
            textBox3.Text = "3";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            selectedcelltype = 4;
            textBox3.Text = "4";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            selectedcelltype = 5;
            textBox3.Text = "5";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            watchMode = "cells";
            textBox6.Text = "cells";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            watchMode = "energy";
            textBox6.Text = "energy";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            watchMode = "organic";
            textBox6.Text = "organic";
        }
    }
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
        public int CountOfNeibours(Cell cell)
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (cell.x + i + cols) % cols;
                    var row = (cell.y + j + rows) % rows;
                    var isSelfChecking = col == cell.x && row == cell.y;
                    //var hasLife = field[col, row, 0];
                    if (cells[col, row] != null && !isSelfChecking) count++;
                }
            }
            return count;
        }
        public int CountOfMyNeibours(Cell cell)
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (cell.x + i + cols) % cols;
                    var row = (cell.y + j + rows) % rows;
                    var isSelfChecking = col == cell.x && row == cell.y;
                    //var hasLife = field[col, row, 0];
                    if (cells[col, row] == cells[cell.x, cell.y] && !isSelfChecking) count++;
                }
            }
            return count;
        }
    }
    public abstract class Cell
    {
        public bool isImmortal;
        public bool isPredator;
        public bool isPlant;
        public int organicLeft;
        public int lifeTime;
        
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
        public abstract void LiveActivity(int p1, int p2);
        public int CountOfWithEnergyNeibours(int e)
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
        public int CountOfWithEnergyNeiboursNoMyType(int e)
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

        public int CountOfMyNeibours()
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + field.cols) % field.cols;
                    var row = (y + j + field.rows) % field.rows;
                    var isSelfChecking = col == x && row == y;
                    //var hasLife = field[col, row, 0];
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
                field.organic[x, y] -= field.organic[x, y] % 255 * (field.organic[x, y] / 255);
            }
        }
    }
    public class Plant : Cell
    {
        public int photosynthesis = 2;
        
        public Plant(Field field) : base(field)
        {
            organicLeft = 10;
            lifePrice = 0;
            type = 1;
            reproductionPrice = 10;
            lifeTime = random.Next(30, 91);
        }
        public override void LiveActivity(int p1, int p2)
        {
            //if (!isBorn)
            //{
                x = p1;
                y = p2;
                if (energy >= reproductionPrice + photosynthesis && CountOfNeibours() < 8)
                {
                    bool done = false;
                    while (!done)
                    {
                        
                        int a = random.Next(-1, 2), b = random.Next(-1, 2);
                        if ((field.cells[(x + a + field.cols) % field.cols, (y + b + field.rows) % field.rows] == null))
                        {
                            field.cells[x, y].energy -= reproductionPrice;
                            field.cells[(x + a + field.cols) % field.cols, (y + b + field.rows) % field.rows] = new Plant(field);
                            
                        }done = true;
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
            //}
            //isBorn = false;
        }
    }

    public class Parasite : Cell
    {
        public int parasitism = 3;
        public Parasite(Field field) : base(field)
        {
            type = 5;
            organicLeft = 5;
            lifePrice = 1;
            reproductionPrice = 100;
            lifeTime = random.Next(1000, 2001); 
        }
        public virtual void makeChild(int x, int y, int energyFromCell)
        {
            field.cells[x, y] = new Parasite(field);
            field.cells[x, y].energy = energyFromCell + lifePrice * 2;
        }
        public void Reproduct(int a, int b)
        {
            int energyFromCell = lifePrice * 2;
            field.cells[x, y].energy -= reproductionPrice + lifePrice * 2;
            makeChild((x + a + field.cols) % field.cols, (y + b + field.rows) % field.rows, energyFromCell);
        }
        public override void LiveActivity(int p1, int p2)
        {
            if (!isBorn)
            {
                x = p1;
                y = p2;
                if (energy >= reproductionPrice + lifePrice * 3 + 1 && CountOfNeibours() < 8)
                {
                    bool done = false;
                    while (!done)
                    {
                        int a = random.Next(-1, 2);
                        int b = random.Next(-1, 2);
                        if (field.cells[(x + a + field.cols) % field.cols, (y + b + field.rows) % field.rows] == null)
                        {
                            Reproduct(a, b);done = true;
                        }
                        
                    }
                }
                else
                {
                    if (CountOfWithEnergyNeiboursNoMyType(parasitism) > 0)
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
                                    
                                }done = true;
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
   

    public class Mushroom : Cell
    {
        public int organicEat = 2;
        public Mushroom(Field field) : base(field)
        {
            organicLeft = 0;
            lifePrice = 1;
            type = 2;
            reproductionPrice = 6;
            lifeTime = 1;
        }
        public override void LiveActivity(int p1, int p2)
        {
            if (!isBorn)
            {
                x = p1;
                y = p2;
                if (energy >= reproductionPrice + organicEat && CountOfNeibours() < 8)
                {
                    bool done = false;
                    while (!done)
                    {
                        int a = random.Next(-1, 2);
                        int b = random.Next(-1, 2);
                        if (field.cells[(x + a + field.cols) % field.cols, (y + b + field.rows) % field.rows] == null || field.cells[(x + a + field.cols) % field.cols, (y + b + field.rows) % field.rows].type == 1) 
                        {
                            field.cells[x, y].energy -= reproductionPrice;
                            field.cells[(x + a + field.cols) % field.cols, (y + b + field.rows) % field.rows] = new Mushroom(field);
                            
                        }done = true;
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
    public class Predator : Cell
    {
        public Predator(Field field) : base(field)
        {
            organicLeft = 15;
            lifePrice = 1;
            reproductionPrice = 12;
            lifeTime = random.Next(50, 71);
            isPredator = true;
        }
        public virtual void makeChild(int x, int y, int energyFromCell)
        {
            field.cells[x, y] = new Predator(field);
            field.cells[x ,y].energy = energyFromCell + lifePrice * 2;
        }
        public void Reproduct(int a, int b)
        {
            int energyFromCell = lifePrice * 2;
            if (field.cells[(x + a + field.cols) % field.cols, (y + b + field.rows) % field.rows] != null)
                energyFromCell = field.cells[(x + a + field.cols) % field.cols, (y + b + field.rows) % field.rows].energy;
            field.cells[x, y].energy -= reproductionPrice + lifePrice * 2;
            makeChild((x + a + field.cols) % field.cols, (y + b + field.rows) % field.rows, energyFromCell);
        }
        public override void LiveActivity(int p1, int p2)
        {
            if (!isBorn)
            {
                x = p1;
                y = p2;
                if (energy >= reproductionPrice + lifePrice * 3 && CountOfMyNeibours() < 8)
                {
                    bool done = false;
                    while (!done)
                    {
                        int a = random.Next(-1, 2);
                        int b = random.Next(-1, 2);
                        if (field.cells[(x + a + field.cols) % field.cols, (y + b + field.rows) % field.rows] != null && field.cells[(x + a + field.cols) % field.cols, (y + b + field.rows) % field.rows].type != field.cells[x, y].type || field.cells[(x + a + field.cols) % field.cols, (y + b + field.rows) % field.rows] == null)
                        {
                            if (field.cells[(x + a + field.cols) % field.cols, (y + b + field.rows) % field.rows] != null && field.cells[(x + a + field.cols) % field.cols, (y + b + field.rows) % field.rows].isPredator)
                            {
                                if (field.cells[(x + a + field.cols) % field.cols, (y + b + field.rows) % field.rows].energy < field.cells[x, y].energy)
                                {
                                    Reproduct(a, b);
                                }
                            }
                            else
                            {
                                Reproduct(a, b);
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
    public class Predator_Red : Predator
    {
        public Predator_Red(Field field) : base(field)
        {
            type = 3;
        }
        public override void makeChild(int x, int y, int energyFromCell)
        {
            field.cells[x, y] = new Predator_Red(field);
            field.cells[x, y].energy = energyFromCell + lifePrice * 2;
        }

    }
    public class Predator_Blue : Predator
    {
        public Predator_Blue(Field field) : base(field)
        {
            type = 4;
        }
        public override void makeChild(int x, int y, int energyFromCell)
        {
            field.cells[x, y] = new Predator_Blue(field);
            field.cells[x, y].energy = energyFromCell + lifePrice * 2;
        }

    }
    public class Plant1 : Cell
    {
        public int photosynthesis = 2;

        public Plant1(Field field) : base(field)
        {
            organicLeft = 10;
            lifePrice = 0;
            type = 5;
            reproductionPrice = 5;
            this.lifeTime = random.Next(30, 61);
        }
        public override void LiveActivity(int p1, int p2)
        {
            if (!isBorn)
            {
                x = p1;
                y = p2;
                if (energy >= reproductionPrice + photosynthesis && CountOfNeibours() < 8)
                {
                    bool done = false;
                    while (!done)
                    {

                        int a = random.Next(-1, 2), b = random.Next(-1, 2);
                        if ((field.cells[(x + a + field.cols) % field.cols, (y + b + field.rows) % field.rows] == null))
                        {
                            field.cells[x, y].energy -= reproductionPrice;
                            field.cells[(x + a + field.cols) % field.cols, (y + b + field.rows) % field.rows] = new Plant1(field);

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
