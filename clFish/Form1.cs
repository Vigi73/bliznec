
using System.Diagnostics;

namespace clFish
{
    public partial class Form1 : Form
    {
        string tmp = string.Empty;        
        bool fl = false;
        bool flm = false;
        int small = default, standings = default;
        int maxFish = 0;
        string mFish = "0";
        string path = @"log.txt";


        public Form1()
        {
            InitializeComponent();
            button2.Visible = false;
            if (!fl)
            {
                button3.Text = "Пуск";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            listBox1.Items.Clear();
            mainTimer.Enabled = true;
            button1.Visible = false;
            button2.Visible = true;
        }

        private void button2_Click(object? sender, EventArgs? e)
        {
            mainTimer.Enabled=false;
            button2.Visible=false;
            button1.Visible=true;
        }
                
        private void mainTimer_Tick(object sender, EventArgs e)
        {
           try
            {
                //string s = File.ReadAllLines(path).Last();
                string s = File.ReadLines(path).Last(); // read last row in the file
                

                if (tmp != s)
                {
                    if (s.Split(";")[2].Trim() == "мелочь")
                    {
                        small++;
                    }
                    else
                    {
                        standings++;
                    }

                    listBox1.Items.Add(s);


                    mFish = getMaxFish(s);



                    toolStripStatusLabel1.Text = $"Всего: {listBox1.Items.Count}";
                    toolStripStatusLabel3.Text = $"Мелочь: {small}";
                    toolStripStatusLabel2.Text = $"Зачётов: {standings}";
                    if (mFish != "")
                    {
                        toolStripStatusLabel4.Text = $"Max: {mFish}";
                    }
                    
                    tmp = s;
                    listBox1.TopIndex = listBox1.Items.Count - 1; // for autoscrolling
                }
            }
            catch (Exception ex)
            {   
            }
        }

        private string getMaxFish(string s)
        {
            string widthFish = s.Split(";")[1];
            if (widthFish.Contains('.'))
            {
                if (widthFish.Split(".")[1].Length == 1) widthFish += "00";
                if (widthFish.Split(".")[1].Length == 2) widthFish += "0";
            }
            int result = int.Parse(widthFish.Replace(".", ""));

            if (result > maxFish)
            {
                maxFish = result;
                return widthFish;
            }   
            else
            {
                return string.Empty;
            }

        }

        private void очиститьЛогToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File.WriteAllText(path, string.Empty);
            listBox1.Items.Clear();
            listBox1.Items.Add("Лог файл очищен...");
            tmp = String.Empty;
        }
                
        private void button3_Click(object sender, EventArgs e)
        {
            if (!fl)
            {
                Process.Start("main.exe");
                fl = true;
                button3.Text = "Стоп";
            }
            else
            {
                fl = false;
                button3.Text = "Пуск";
                try
                {
                    Process[] proc = Process.GetProcessesByName("main");

                    proc[0].Kill();
                    proc[1].Kill();
                    
                }
                catch
                {

                }
            }
            
        }

        private void chTimer_Tick(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                this.TopMost = true;
            }
            else
            {
                this.TopMost = false;
            }

            if (checkBox2.Checked)
            {
                var lastRow = listBox1.Items[^1].ToString();
                bliznec(lastRow);
            }
            else
            {
                listBox2.Items.Clear();
                listBox3.Items.Clear();
            }
            


        }

        string ttmp = string.Empty;


        private int getRealFish(string v)
        {
            string tVes = string.Empty;

            if (v.Contains('.'))
            {
                if (v.Split(".")[1].Length < 2) tVes = v.Replace(".", "") + "00";
                if (v.Split(".")[1].Length < 3) tVes = v.Replace(".", "") + "0";
            }
            else
            {
                tVes = v;
            }
            return int.Parse(tVes);
        }


        private void bliznec(string? lastRow)
        {
            int wgFish = getRealFish(lastRow.Split(";")[1]); 

            if (lastRow.Split(";")[0] == comboBox1.Text && int.Parse(textBox1.Text) >= wgFish)
            {
                if (ttmp != lastRow)
                {
                    listBox2.Items.Add(lastRow.Split(";")[1]);
                    ttmp = lastRow;
                    listBox2.TopIndex = listBox2.Items.Count - 1;
                }
            }
            if (lastRow.Split(";")[0] == comboBox2.Text && int.Parse(textBox1.Text) >= wgFish)
            {
                if (ttmp != lastRow)
                {
                    listBox3.Items.Add(lastRow.Split(";")[1]);
                    ttmp = lastRow;
                    listBox3.TopIndex = listBox3.Items.Count - 1;
                }                
            }
        }

     
    }
}