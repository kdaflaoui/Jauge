using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Formulaire
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                for (int i = 0; i <= jauge1.MaxValue; i++)
                {
                    new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(this.ProgressUpGrade)).Start(i);
                    System.Threading.Thread.Sleep(10);
                }
            });

        }

        public void ProgressUpGrade(Object progress)
        {
            jauge1.Invoke((MethodInvoker)delegate {

                jauge1.UpdateProgress(Convert.ToInt32(progress));

            });
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            trackBar1.Maximum = (int)jauge1.MaxValue;
            jauge1.CurrentValue = trackBar1.Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string saisi = textBox1.Text;

            double valeur;  

            if (saisi =="")
            {
                MessageBox.Show("Veuillez saisir une valeur entre " + jauge1.MinValue + " et " + jauge1.MaxValue, " info");

            }
            else
            {
               valeur =  double.Parse(saisi);
                if (valeur >= jauge1.MinValue && valeur <= jauge1.MaxValue)
                {
                     
                    Task.Run(() =>
                    {
                        for (int i = 0; i <= double.Parse(textBox1.Text); i++)
                        {
                            new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(this.ProgressUpGrade)).Start(i);
                            System.Threading.Thread.Sleep(10);
                        }
                    });
                }
                else
                {
    
                    MessageBox.Show("Veuillez saisir une valeur entre "+ jauge1.MinValue +" et "+ jauge1.MaxValue, " info");
                }
            }

              
            
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
