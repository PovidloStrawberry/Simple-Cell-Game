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
        private Cell cellToSpawn;

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
        int[] civAttack = { 0, 0, 0, 0, 0, 0 };
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
        public CellCreator currentCellCreator;
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
            CellDrawing();
            nudResolution.Enabled = true;
            nudDensity.Enabled = true;
        }
        private void CellDrawing()
        {
            graphics.Clear(Color.Black);
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (watchMode == "cells")
                    {
                        if (gameField.cells[i, j] != null)
                            graphics.FillRectangle(gameField.cells[i, j].cellColor, gameField.cells[i, j].x * resoluton, gameField.cells[i, j].y * resoluton, resoluton, resoluton);
                    }

                    else if (watchMode == "organic")
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
                    if (gameField.cells[i, j] != null)
                    {
                        gameField.cells[i, j].LiveActivity();
                    }
                }
            }
            CellDrawing();
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
            NextGenegation();
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
            var posX = PictureBox.MousePosition.X / resoluton - 287 / resoluton;
            var posY = PictureBox.MousePosition.Y / resoluton - 30 / resoluton;
            var cellPosX = (posX + cols) % cols;
            var cellPosY = (posY + rows) % rows;
            gameField.cells[cellPosX, cellPosY] = currentCellCreator.CreateCell(gameField);
            gameField.cells[cellPosX, cellPosY].InitPosition(cellPosX, cellPosY);
            gameField.cells[cellPosX, cellPosY].energy = gameField.cells[cellPosX, cellPosY].startEnergy;
            CellDrawing();
        }

        //bStart.BackgroundImage = Image.FromFile(@"D:\2.jpg", false);
        //bStart.BackgroundImageLayout = ImageLayout.Center;

        private void bPause_Click(object sender, EventArgs e)
        {
            // if (!timer1.Enabled) return;

            if (timer1.Enabled) timer1.Stop();
            else timer1.Start();
        }

        private void bNextFrame_Click(object sender, EventArgs e)
        {
            NextGenegation();
        }



        private void tocell1_Click(object sender, EventArgs e)
        {
            currentCellCreator = new PlantCreator();
        }

        private void tocell2_Click(object sender, EventArgs e)
        {
            currentCellCreator = new MushroomCreator();
        }

        private void tocell3_Click(object sender, EventArgs e)
        {
            currentCellCreator = new Predator_RedCreator();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            currentCellCreator = new Predator_BlueCreator();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            currentCellCreator = new ParasiteCreator();
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
}
