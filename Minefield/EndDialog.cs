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
    public partial class EndDialog : Form
    {
        public EndDialog()
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
            label3.Text = (wins + losses).ToString() + "\n" + wins.ToString() + "\n" + losses.ToString() + "\n" + (ratio).ToString("0.000");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Retry;
            Close();
        }

        public void setText(string labelText)
        {
            label1.Text = labelText;
        }
    }
}
