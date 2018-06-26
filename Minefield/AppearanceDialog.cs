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
    public partial class AppearanceDialog : Form
    {
        int flagIndex = Properties.Settings.Default.FlagIndex;
        int mineIndex = Properties.Settings.Default.MineIndex;

        public AppearanceDialog()
        {
            InitializeComponent();
            button3.BackColor = Properties.Settings.Default.TileColour;
            button3.ForeColor = Properties.Settings.Default.GridColour;
            switch(flagIndex)
            {
                case 8:
                    radioButton1.Select();
                    break;
                case 9:
                    radioButton2.Select();
                    break;
                case 10:
                    radioButton3.Select();
                    break;
                case 11:
                    radioButton4.Select();
                    break;
            }
            switch(mineIndex)
            {
                case 12:
                    radioButton5.Select();
                    break;
                case 13:
                    radioButton6.Select();
                    break;
                case 14:
                    radioButton7.Select();
                    break;
                case 15:
                    radioButton8.Select();
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TileColour = button3.BackColor;
            Properties.Settings.Default.GridColour = button3.ForeColor;
            Properties.Settings.Default.FlagIndex = flagIndex;
            Properties.Settings.Default.MineIndex = mineIndex;
            Properties.Settings.Default.Save();
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            button3.BackColor = colorDialog1.Color;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            colorDialog2.ShowDialog();
            button3.ForeColor = colorDialog2.Color;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult confirmClear = MessageBox.Show("Are you sure you want to delete your appearance settings?\nAll your appearance settings will be lost forever!", "Clear settings?", MessageBoxButtons.YesNo);
            if (confirmClear == DialogResult.Yes)
            {
                Properties.Settings.Default.TileColour = Color.FromArgb(255, 0, 212, 0);
                Properties.Settings.Default.GridColour = Color.Green;
                Properties.Settings.Default.FlagIndex = 8;
                Properties.Settings.Default.MineIndex = 12;
                button3.BackColor = Properties.Settings.Default.TileColour;
                button3.ForeColor = Properties.Settings.Default.GridColour;
                radioButton1.Select();
                radioButton5.Select();
                Properties.Settings.Default.Save();
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            flagIndex = 8;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            flagIndex = 9;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            flagIndex = 10;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            flagIndex = 11;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            mineIndex = 12;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            mineIndex = 13;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            mineIndex = 14;
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            mineIndex = 15;
        }
    }
}
