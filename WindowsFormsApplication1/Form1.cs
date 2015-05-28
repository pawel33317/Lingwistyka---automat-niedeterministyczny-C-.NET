using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        string[] tablicaZdan;
        string input = "";

        List<int> currentStates = new List<int>();

        String []statesName = {"SP","S0","S1","S2","S3","S4","S5","S6","S7","S8","S9","SK","SZ"};
        int[,] statesTable = 
        {
            {1, 2, 3, 4, 5, 6, 7, 8, 9, 10}, //SP 1-->0, 2-->1 ...
            {11,12,12,12,12,12,12,12,12,12}, //0
            {12,11,12,12,12,12,12,12,12,12}, //1
            {12,12,11,12,12,12,12,12,12,12}, //2
            {12,12,12,11,12,12,12,12,12,12}, //3
            {12,12,12,12,11,12,12,12,12,12}, //4
            {12,12,12,12,12,11,12,12,12,12}, //5
            {12,12,12,12,12,12,11,12,12,12}, //6
            {12,12,12,12,12,12,12,11,12,12}, //7
            {12,12,12,12,12,12,12,12,11,12}, //8
            {12,12,12,12,12,12,12,12,12,11}, //9
            {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}, //SK dobry koniec znalazlo powtórzenie
            {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}  //SZ zly brak powtórzenia 
        };

        private void bLoadFile_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            List<int> stringList = new List<int> { };

            int size = -1;
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                try
                {
                    input = File.ReadAllText(file);
                    size = input.Length;
                }
                catch (IOException)
                {
                    MessageBox.Show("Error");
                }
                finally
                {
                    label1.Text = openFileDialog1.FileName.ToString();
                }
            }
        }
        private void bReadFile_Click(object sender, EventArgs e)
        {
            label8.Text = "";
            tablicaZdan = input.Split(new string[] { textBox1.Text }, StringSplitOptions.None);
            foreach (string s in tablicaZdan)
            {
                label8.Text += s + "\n";
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            label7.Text = "";
            label6.Text = "";
            //dla każdego zdania
            foreach (string s in tablicaZdan)
            {
                int position = 0;
                bool znalezionoPowtorzenie = false;
                currentStates.Clear();
                currentStates.Add(0);
                //dla każdego znaku
                foreach (char c in s)
                {
                    
                    int podanyZnak = (int)Char.GetNumericValue(c);


                    //dla każdego stanu w którym aktualnie jest
                    List<int> tmpcurrentStates = new List<int>(currentStates);
                    currentStates.Clear();

                    //z każdego starego stanu przechodzi do nowego
                    foreach (int stan in tmpcurrentStates)
                    {
                        //wypisujemy stany w których jest aktualnie
                        label7.Text += statesName[stan] + " ";

                        //przechodzimy do nowych stanów
                        if (statesTable[stan, podanyZnak] != -1)
                            currentStates.Add(statesTable[stan, podanyZnak]);

                        //sprawdzamy czy nie jest w stanie 
                        if (stan == 11)
                            znalezionoPowtorzenie = true;
                    }
                    currentStates.Add(0); if (znalezionoPowtorzenie) break;
                    label7.Text += " --> ";
                    label7.Text += " [" + c + "] ";
                    position++;
                }
                if (znalezionoPowtorzenie)
                {
                    label7.Text += "ZNALEZIONO";
                    label6.Text += s + "  -->  " + (position-1) + "\n";
                }
                else
                {
                    //dopisuje ostatnie stany na koniec w któych się znajduje
                    foreach (int stan in currentStates)
                    {
                        label7.Text += statesName[stan] + " ";
                    }
                    label7.Text += "BRAK";
                }
                label7.Text += "\n";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }
}

