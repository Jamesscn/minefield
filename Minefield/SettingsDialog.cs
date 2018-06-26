using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minefield
{
    public partial class SettingsDialog : Form
    {
        public SettingsDialog()
        {
            InitializeComponent();
        }

        private void SettingsDialog_Load(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Settings.Default.GridWidth.ToString();
            textBox2.Text = Properties.Settings.Default.GridHeight.ToString();
            textBox3.Text = Properties.Settings.Default.NumberOfMines.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int gridWidth;
            int gridHeight;
            int numberOfMines;
            bool failed = false;
            try
            {
                gridWidth = Int32.Parse(textBox1.Text);
                Properties.Settings.Default.GridWidth = gridWidth;
                Properties.Settings.Default.Save();
            }
            catch
            {
                gridWidth = Properties.Settings.Default.GridWidth;
                textBox1.Text = Properties.Settings.Default.GridWidth.ToString();
                failed = true;
            }
            try
            {
                gridHeight = Int32.Parse(textBox2.Text);
                Properties.Settings.Default.GridHeight = gridHeight;
                Properties.Settings.Default.Save();
            }
            catch
            {
                gridHeight = Properties.Settings.Default.GridHeight;
                textBox2.Text = Properties.Settings.Default.GridHeight.ToString();
                failed = true;
            }
            try
            {
                numberOfMines = Int32.Parse(textBox3.Text);
                Properties.Settings.Default.NumberOfMines = numberOfMines;
                Properties.Settings.Default.Save();
            }
            catch
            {
                numberOfMines = Properties.Settings.Default.NumberOfMines;
                textBox3.Text = Properties.Settings.Default.NumberOfMines.ToString();
                failed = true;
            }
            if (gridWidth <= 1)
            {
                textBox1.Text = "2";
                failed = true;
            }
            if (gridHeight <= 1)
            {
                textBox2.Text = "2";
                failed = true;
            }
            if (numberOfMines >= gridWidth * gridHeight)
            {
                textBox3.Text = (gridWidth * gridHeight - 1).ToString();
                failed = true;
            } else if (numberOfMines <= 0)
            {
                textBox3.Text = "1";
                failed = true;
            }
            if (!failed) {
                Properties.Settings.Default.Save();
                DialogResult = DialogResult.Yes;
                Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(comboBox1.Text)
            {
                case "Easy":
                    textBox1.Text = "9";
                    textBox2.Text = "9";
                    textBox3.Text = "10";
                    break;
                case "Normal":
                    textBox1.Text = "16";
                    textBox2.Text = "16";
                    textBox3.Text = "40";
                    break;
                case "Hard":
                    textBox1.Text = "30";
                    textBox2.Text = "16";
                    textBox3.Text = "99";
                    break;
            }
        }
    }
}
