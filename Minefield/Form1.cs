using System;
using System.Drawing;
using System.Windows.Forms;

namespace Minefield
{
    public partial class Form1 : Form
    {
        bool[,] mines;
        bool[,] flags;
        Button[,] tiles;
        int minesNotFlagged;
        int tilesLeft;
        bool inGame;
        bool gameStarted;
        int clicks;
        int currentX;
        int currentY;

        public Form1()
        {
            InitializeComponent();
            newGame();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        public void newGame()
        {
            int x = Properties.Settings.Default.GridWidth;
            int y = Properties.Settings.Default.GridHeight;
            int count = Properties.Settings.Default.NumberOfMines;
            currentX = x;
            currentY = y;
            tilesLeft = x * y - count;
            minesNotFlagged = count;
            mines = new bool[x, y];
            flags = new bool[x, y];
            tiles = new Button[x, y];
            Array.Clear(mines, 0, mines.Length);
            Array.Clear(flags, 0, flags.Length);
            Array.Clear(tiles, 0, tiles.Length);
            panel1.Controls.Clear();
            clicks = 0;
            inGame = true;
            gameStarted = false;
            if (count >= x * y)
            {
                return;
            }
            Random rand = new Random();
            do
            {
                bool tempDone = false;
                do
                {
                    int pos = rand.Next(0, mines.Length);
                    if (mines[pos % x, pos / x] == false)
                    {
                        mines[pos % x, pos / x] = true;
                        tempDone = true;
                    } else
                    {
                        continue;
                    }
                } while (tempDone == false);
                count--;
            } while (count > 0);
            Width = (48 + 32 * x);
            Height = (96 + 32 * y);
            panel1.Width = (32 + 32 * x);
            panel1.Height = (56 + 32 * y);
            label1.Location = new Point(16, 36 + 32 * y);
            label1.Text = "Mines Left: " + minesNotFlagged.ToString();
            for (int i = 0; i < x; i++)
            {
                for(int j = 0; j < y; j++)
                {
                    Button tile = new Button();
                    tile.Visible = true;
                    tile.Location = new Point(4 + 32*i, 4 + 32*j);
                    tile.Height = 32;
                    tile.Width = 32;
                    tile.ForeColor = Properties.Settings.Default.GridColour;
                    tile.BackColor = Properties.Settings.Default.TileColour;
                    tile.FlatStyle = FlatStyle.Flat;
                    tile.TabStop = false;
                    tile.Show();
                    int a = i, b = j;
                    tile.MouseDown += ((o, e) => {
                        if(e.Button == MouseButtons.Right)
                        {
                            toggleFlag(a, b); 
                        } else if (e.Button == MouseButtons.Left)
                        {
                            clicks++;
                        }
                    });
                    if (mines[i, j] == true)
                    {
                        tile.Click += (sender, e) => clickBomb(a, b, x, y);
                    } else
                    {
                        tile.Click += (sender, e) => clickEmpty(a, b, x, y);
                    }
                    tiles[a, b] = tile;
                    panel1.Controls.Add(tile);
                }
            }
        }

        private void clickBomb(int a, int b, int x, int y)
        {
            if (!flags[a, b] && inGame)
            {
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        if (mines[i, j] == true)
                        {
                            tiles[i, j].Visible = false;
                            PictureBox mineImg = new PictureBox();
                            mineImg.Image = imageList1.Images[Properties.Settings.Default.MineIndex];
                            mineImg.Location = new Point(4 + 32 * i, 4 + 32 * j);
                            mineImg.Width = 32;
                            mineImg.Height = 32;
                            panel1.Controls.Add(mineImg);
                        }
                    }
                }
                loseGame();
            }
        }

        private void clickEmpty(int a, int b, int x, int y)
        {
            if (inGame)
            {
                if (!flags[a, b])
                {
                    if (!gameStarted)
                    {
                        gameStarted = true;
                    }
                    tiles[a, b].Visible = false;
                    tilesLeft--;
                    if (a == 0)
                    {
                        if (b == 0)
                        {
                            sweepTiles(a, b, 0, 0, 2, 2, x, y);
                        }
                        else if (b == y - 1)
                        {
                            sweepTiles(a, b, 0, 1, 2, 2, x, y);
                        }
                        else
                        {
                            sweepTiles(a, b, 0, 1, 2, 3, x, y);
                        }
                    }
                    else if (b == 0)
                    {
                        if (a == x - 1)
                        {
                            sweepTiles(a, b, 1, 0, 2, 2, x, y);
                        }
                        else
                        {
                            sweepTiles(a, b, 1, 0, 3, 2, x, y);
                        }
                    }
                    else if (a == x - 1)
                    {
                        if (b == y - 1)
                        {
                            sweepTiles(a, b, 1, 1, 2, 2, x, y);
                        }
                        else
                        {
                            sweepTiles(a, b, 1, 1, 2, 3, x, y);
                        }
                    }
                    else if (b == y - 1)
                    {
                        sweepTiles(a, b, 1, 1, 3, 2, x, y);
                    }
                    else
                    {
                        sweepTiles(a, b, 1, 1, 3, 3, x, y);
                    }
                }
                if (tilesLeft == 0)
                {
                    if(clicks > 1) {
                        winGame();
                    } else
                    {
                        MessageBox.Show("Invalid game, exiting...");
                        Properties.Settings.Default.GridWidth = 16;
                        Properties.Settings.Default.GridHeight = 16;
                        Properties.Settings.Default.NumberOfMines = 40;
                        Properties.Settings.Default.Save();
                        Close();
                    }
                }
            }
        }

        private void sweepTiles(int a, int b, int xOff, int yOff, int xSteps, int ySteps, int x, int y)
        {
            int mineCount = 0;
            for (int i = 0; i < xSteps; i++)
            {
                for (int j = 0; j < ySteps; j++)
                {
                    if (mines[a + (i - xOff), b + (j - yOff)] == true)
                    {
                        mineCount++;
                    }
                }
            }
            if (mineCount == 0)
            {
                for (int i = 0; i < xSteps; i++)
                {
                    for (int j = 0; j < ySteps; j++)
                    {
                        if (tiles[a + (i - xOff), b + (j - yOff)].Visible == true)
                        {
                            clickEmpty(a + (i - xOff), b + (j - yOff), x, y);
                        }
                    }
                }
            }
            else
            {
                addMineLabel(a, b, mineCount);
            }
        }

        private void addMineLabel(int a, int b, int mineCount)
        {
            PictureBox mineNum = new PictureBox();
            mineNum.Image = imageList1.Images[mineCount - 1];
            mineNum.Location = new Point(4 + 32 * a, 4 + 32 * b);
            mineNum.Width = 32;
            mineNum.Height = 32;
            panel1.Controls.Add(mineNum);
        }

        private void toggleFlag(int a, int b)
        {
            if (inGame)
            {
                if (flags[a, b] == true)
                {
                    tiles[a, b].Image = null;
                    flags[a, b] = false;
                    minesNotFlagged++;
                    label1.Text = "Mines Left: " + minesNotFlagged.ToString();
                }
                else
                {
                    if (minesNotFlagged > 0)
                    {
                        tiles[a, b].Image = imageList1.Images[Properties.Settings.Default.FlagIndex];
                        flags[a, b] = true;
                        minesNotFlagged--;
                        label1.Text = "Mines Left: " + minesNotFlagged.ToString();
                    }
                }
            }
        }

        private void loseGame()
        {
            Properties.Settings.Default.GameLosses += 1;
            Properties.Settings.Default.Save();
            EndDialog loseDialog = new EndDialog();
            loseDialog.Text = "Bad luck!";
            loseDialog.setText("You lost! Better luck next time.");
            loseDialog.ShowDialog();
            if (loseDialog.DialogResult == DialogResult.Retry)
            {
                newGame();
            }
            else if (loseDialog.DialogResult == DialogResult.OK)
            {
                inGame = false;
            }
        }

        private void winGame()
        {
            Properties.Settings.Default.GameWins += 1;
            Properties.Settings.Default.Save();
            EndDialog winDialog = new EndDialog();
            winDialog.Text = "Well done!";
            winDialog.setText("Congratulations, You won!");
            winDialog.ShowDialog();
            if (winDialog.DialogResult == DialogResult.Retry)
            {
                newGame(); //You have to clear the existing clickempties and sweeptiles first
            }
            else if (winDialog.DialogResult == DialogResult.OK)
            {
                inGame = false;
            }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newGame();
        }

        private void appearanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppearanceDialog appearDialog = new AppearanceDialog();
            appearDialog.ShowDialog();
            if(appearDialog.DialogResult == DialogResult.Yes)
            {
                for (int i = 0; i < currentX; i++)
                {
                    for (int j = 0; j < currentY; j++)
                    {
                        tiles[i, j].ForeColor = Properties.Settings.Default.GridColour;
                        tiles[i, j].BackColor = Properties.Settings.Default.TileColour;
                        if (flags[i, j] == true)
                        {
                            tiles[i, j].Image = imageList1.Images[Properties.Settings.Default.FlagIndex];
                        }
                    }
                }
            }
        }

        private void statisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StatisticsDialog statsDialog = new StatisticsDialog();
            statsDialog.Show();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsDialog setDialog = new SettingsDialog();
            setDialog.ShowDialog();
            if(setDialog.DialogResult == DialogResult.Yes)
            {
                if (gameStarted)
                {
                    DialogResult changeNow = MessageBox.Show("Do you want to start a new game with your new settings?", "Settings", MessageBoxButtons.YesNo);
                    if(changeNow == DialogResult.Yes)
                    {
                        newGame();
                    }
                } else
                {
                    newGame();
                }
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void instructionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Minefield is a game consisting in a set of mines randomly places in a grid, it is your job to find all tiles in the grid that do not have a mine underneath them.\n\nIn order to find out which tiles have mines underneath them when you click on a tile which does not have a tile it will reveal the number of mines in the surrounding area, if you believe you know the location of a mine you can right click on a tile to mark it as dangerous, and to prevent you from accidentaly clicking on it later on. You must use your intuition and the numbers you are given to find all the empty tiles. When you click on a mine, you lose the game, however if you reveal all the empty tiles you win the game.\n\nGood luck!", "Instructions");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutBox = new AboutBox1();
            aboutBox.Show();
        }
    }
}
