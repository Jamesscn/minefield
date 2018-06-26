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
    public partial class StatisticsDialog : Form
    {
        public StatisticsDialog()
        {
            InitializeComponent();
            int wins = Properties.Settings.Default.GameWins;
            int losses = Properties.Settings.Default.GameLosses;
            float ratio = 0;
            if(losses == 0)
            {
                ratio = wins;
            } else
            {
                ratio = (float)wins / losses;
            }
            label2.Text = (wins + losses).ToString() + "\n" + wins.ToString() + "\n" + losses.ToString() + "\n" + (ratio).ToString("0.000");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult confirmClear = MessageBox.Show("Are you sure you want to delete your statistics?\nAll your statistics will be lost forever!", "Clear statistics?", MessageBoxButtons.YesNo);
            if(confirmClear == DialogResult.Yes)
            {
                Properties.Settings.Default.GameWins = 0;
                Properties.Settings.Default.GameLosses = 0;
                Properties.Settings.Default.Save();
                label2.Text = "0\n0\n0\n0.000";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
