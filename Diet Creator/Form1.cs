using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diet_Creator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
             
            checkclockDB clocklabel = new checkclockDB();
            try
            {
                toolStripStatusLabel2.Text = clocklabel.clocktaker();
            }
            catch (System.Exception e) 
            {
                toolStripStatusLabel2.Text = "Database vuoto";
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 loader = new Form2();
            loader.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form4 loader = new Form4(this);
            loader.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Programma scritto e compilato da Lorettu Nicola in C#", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
