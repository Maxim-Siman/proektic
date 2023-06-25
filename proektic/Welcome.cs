using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proektic
{
    public partial class Welcome : Form
    {
        public Welcome()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Log_in f2 = new Log_in();
            f2.Owner = this;
            f2.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Log_up f2 = new Log_up();
            f2.Owner = this;
            f2.Show();
        }
    }
}
