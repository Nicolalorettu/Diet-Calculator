using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diet_Creator
{
    public partial class Form4 : Form
    {
        private Form Form1;

        public Form4(Form Form1)
        {
            InitializeComponent();
            this.Form1 = Form1;

            Showfoodcombobox showfoodcombobox = new Showfoodcombobox();
            EventArgs e = new EventArgs();
            list.AddRange(showfoodcombobox.foodviewer());
            this.comboBox1.Items.AddRange(list.ToArray());
            this.button2.Enabled = false;
            
        }
        List<string> list = new List<string>();
        List<string> listNew = new List<string>();
        string[,] combofood = new string[90,7];
        int counterstring = 0;
        string[] paramfood = new string[5];
        string[] propparamfood = new string[5];
        double [] adder = new double[5];
        private bool button4WasClicked = false;
        private string[,] foodremain = new string[30, 7];
        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            
            this.comboBox1.Items.Clear();
            listNew.Clear();
            foreach (var item in list)
            {
                if (item.ToLower().Contains(this.comboBox1.Text))
                {
                    listNew.Add(item);
                }
            }
            this.comboBox1.Items.AddRange(listNew.ToArray());
            this.comboBox1.SelectionStart = this.comboBox1.Text.Length;
            Cursor = Cursors.Default;
            if (this.comboBox1.Text != null)
            {
                string firstchar = this.comboBox1.Text;
                this.comboBox1.DroppedDown = true;
                this.comboBox1.Text = firstchar;
                this.comboBox1.SelectionStart = this.comboBox1.Text.Length;
            }
            
            return;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            int addercount = 0;
            int valuefiller = 4;
            if (comboBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Alimento o grammatura non inserite!", "Diet Calc", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                groupBox2.Text = comboBox1.Text + ", " + "valore per "+textBox2.Text+" gr/ml";
                catchparameterscombobox param = new catchparameterscombobox();
                paramfood = param.parameterscombobox(comboBox1.Text);
                proportionalfooddata propfooddata = new proportionalfooddata();
                propparamfood = propfooddata.setwithrightproportion(paramfood, textBox2.Text);
                if (button4WasClicked)
                {
                    Array.Clear(adder, 0, adder.Length);
                    button4WasClicked = false;
                }
                foreach(string value in propparamfood)
                {
                    adder[addercount] += Convert.ToDouble(value);
                    addercount++;
                }
                textBox3.Text = adder[0].ToString();
                textBox4.Text = adder[1].ToString();
                textBox5.Text = adder[2].ToString();
                textBox6.Text = adder[3].ToString();
                textBox7.Text = adder[4].ToString();
                
                combofood[counterstring, 0]+= (comboBox1.Text);
                combofood[counterstring, 1] += (textBox2.Text);
                combofood[counterstring, 2] += propparamfood[0];
                combofood[counterstring, 3] += propparamfood[1];
                combofood[counterstring, 4] += propparamfood[2];
                combofood[counterstring, 5] += propparamfood[3];
                combofood[counterstring, 6] += propparamfood[4];
                checkedListBox1.Items.Add(combofood[counterstring, 0] + " " + combofood[counterstring, 1] + " gr/ml" + "\r\n");
                this.comboBox1.SelectedIndex = -1;
                textBox2.Text = "";
                counterstring++;

                
                
                foreach (Control control in groupBox2.Controls)
                {
                    control.Visible = true;
                    for (int i = 13; i < 18; i++)
                    {
                        if (control.Name.Contains(i.ToString()))
                        {
                            control.Text = propparamfood[valuefiller].ToString();
                            valuefiller--;

                        }
                    }
                }
                valuefiller = 4;
            }
                   
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
        static List<string> Split(string str, int chunkSize)
        {
            List<string> listtext = new List<string>();
            int counter = 0;
            try
            {
                
                while(str.Length > 0)
                {
                    if(str.Length>= chunkSize)
                    {
                        var temp = str.Substring(0, chunkSize);
                        str = str.Remove(0, temp.Length);
                        listtext.Add(temp);
                        counter = temp.Length;
                    }
                    else
                    {
                        var temp = str.Substring(0, str.Length);
                        str = str.Remove(0, temp.Length);
                        listtext.Add(temp);
                    }
                    
                }
                
            }
            catch(System.Exception ex) 
            {
                MessageBox.Show(ex.ToString(), "Diet Calc", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return listtext;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form1.Show();
            this.Close();
            
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.checkedListBox1.Items.Clear();
            this.textBox3.Clear();
            this.textBox4.Clear();
            this.textBox5.Clear();
            this.textBox6.Clear();
            this.textBox7.Clear();
            button4WasClicked = true;
            groupBox2.Text = "";           
            foodremain = new string[30, 7];
            combofood = new string[90, 7];
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            label12.Visible = false;
            label13.Visible = false;
            label14.Visible = false;
            label15.Visible = false;
            label16.Visible = false;
            label17.Visible = false;
            counterstring = 0;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string iteminlist = "";
            foreach (object item in checkedListBox1.Items) iteminlist += item.ToString() + "\r\n";
            Clipboard.SetText(iteminlist);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            adder = new double[5];
            foodremain = new string[30, 7];
            int addercounter = 0;
            DialogResult dialogResult = MessageBox.Show("Vuoi veramente eliminare gli elementi selezionati?", "Diet Creator", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                int remainitem = 0;
                for (int i = checkedListBox1.Items.Count - 1; i >= 0; i--)
                {
                    if (checkedListBox1.GetItemChecked(i))
                    {
                        while (combofood[i, 0] != null)
                        {
                            for (int j = 0; j < combofood.GetLength(1); j++)
                            {
                                combofood[i, j] = null;

                            }
                        }
                    }
                        checkedListBox1.Items.RemoveAt(i);
                }
                for (int i=0; i<90; i++)
                {
                    if(combofood[i, 0] != null)
                    {
                        for (int j = 0; j < combofood.GetLength(1); j++)
                        {
                            foodremain[remainitem, j] = combofood[i, j];
                            if (j >= 2)
                            {
                                adder[addercounter] += Convert.ToDouble(foodremain[remainitem, j]);
                                addercounter++;
                            }
                        }
                        addercounter = 0;
                        remainitem++;
                    }
                    
                }
                remainitem = 0;

                combofood = new string[90, 7];
                while (foodremain[remainitem, 0] != null)
                {
                    for (int j = 0; j < foodremain.GetLength(1); j++)
                    {
                        combofood[remainitem, j] = foodremain[remainitem, j];
                    }
                    remainitem++;
                }
                counterstring = remainitem;
                remainitem = 0;
                textBox3.Text = (adder[0]).ToString();
                textBox4.Text = (adder[1]).ToString();
                textBox5.Text = (adder[2]).ToString();
                textBox6.Text = (adder[3]).ToString();
                textBox7.Text = (adder[4]).ToString();
                checkedListBox1.Items.Clear();
                for (int i =0; i< foodremain.GetLength(0);i++)
                {
                    if (foodremain[i, 0] != null)
                    {
                        checkedListBox1.Items.Add(foodremain[i, 0] + " " + foodremain[i, 1] + " gr/ml" + "\r\n");
                    }
                        
                }
                this.button2.Enabled = false;
            }
            else
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i,false);
                    checkedListBox1.Focus();
                }
                this.button2.Enabled = false;

            }
            if(checkedListBox1 == null)
            {
                adder = new double[5];
            }
            
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (this.checkedListBox1.CheckedItems.Count == 1)
            {
                if (e.NewValue == CheckState.Unchecked)
                {
                    this.button2.Enabled = false;
                }
            }
            else
            {
                this.button2.Enabled = true;
            }
        }

        
    }
}
