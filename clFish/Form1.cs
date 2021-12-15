using System.Collections.Generic;
using System.Diagnostics;
using System.Media;

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
        int wgFish = 0;
        string tVes = string.Empty;
        List<string> listTmp = new List<string>(new[] {""});



        public Form1()
        {
            InitializeComponent();
            button2.Visible = false;
            if (!fl)
            {
                button3.Text = "����";
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
            if (lstTargets.Items.Count != 0)
            {
                pictureBox1.Visible = true;
            }
            else
            {
                pictureBox1.Visible = false;
            }
           try
            {
                //string s = File.ReadAllLines(path).Last();
                string s = File.ReadLines(path).Last(); // read last row in the file
                

                if (tmp != s)
                {
                    if (s.Split(";")[2].Trim() == "������")
                    {
                        small++;
                    }
                    else
                    {
                        standings++;
                    }

                    listBox1.Items.Add(s);


                    mFish = getMaxFish(s);



                    toolStripStatusLabel1.Text = $"�����: {listBox1.Items.Count}";
                    toolStripStatusLabel3.Text = $"������: {small}";
                    toolStripStatusLabel2.Text = $"�������: {standings}";
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

        private void �����������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File.WriteAllText(path, string.Empty);
            listBox1.Items.Clear();
            listBox1.Items.Add("��� ���� ������...");
            tmp = String.Empty;
        }
                
        private void button3_Click(object sender, EventArgs e)
        {
            if (!fl)
            {
                Process.Start("main.exe");
                fl = true;
                button3.Text = "����";
            }
            else
            {
                fl = false;
                button3.Text = "����";
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
                try
                {

                    var lastRow = listBox1.Items[^1].ToString();
                    bliznec(lastRow);
                }
                catch 
                {
                }
                button4_Click(null, null);
            }

            // ���� ����� ����� �� ���
            if (chFox.Checked)
            {
                try
                {

                    var lastRowForFox = listBox1.Items[^1].ToString();
                    fox(lastRowForFox);
                }
                catch
                {
                }

            }
            


            

        }

        string ttmp = string.Empty;

        
        private void getRealFish(string v)
        {
            if (v.Contains('.'))
            {
                switch (v.Split(".")[1].Length)
                {
                    case 3:
                        tVes = v.Replace(".", "");
                        break;
                    case 2:
                        tVes = v.Replace(".", "") + "0";
                        break;
                    case 1:
                        tVes = v.Replace(".", "") + "00";
                        break;
                }   
            }
            else
            {
                tVes = v;
            }

            //txtLog.Text = tVes;
            
        }
        SoundPlayer sp = new SoundPlayer(Properties.Resources.bonus);
        

        private void button4_Click(object sender, EventArgs e)
        {
            //Looking for twins
            if (listBox2.Items.Count > 0 && listBox3.Items.Count > 0)
            {
                foreach (string it in listBox2.Items)
                {
                    int index = listBox3.FindString(it);
                    if (index != -1)
                    {

                        int x = lstTargets.FindString(it);
                        if (x != -1)
                        {
                        }
                        else
                        {
                            sp.Play();
                            lstTargets.Items.Add(it);
                            lstTargets.TopIndex = lstTargets.Items.Count - 1;
                        }
                    }
                }
            }
        }

        private void bliznec(string? lastRow)
        {
            try
            {
                getRealFish(lastRow.Split(";")[1].Trim());
                
            }
            catch 
            { }
            

            if (lastRow.Split(";")[0] == comboBox1.Text && Int32.Parse(tVes) >= Convert.ToInt32(textBox1.Text) || comboBox1.Text == "*")
            {
                if (ttmp != lastRow)
                {
                    listBox2.Items.Add(lastRow.Split(";")[1]);
                    ttmp = lastRow;
                    listBox2.TopIndex = listBox2.Items.Count - 1;
                }
            }
            if (lastRow.Split(";")[0] == comboBox2.Text && Int32.Parse(tVes) >= Convert.ToInt32(textBox1.Text) || comboBox2.Text == "*")
            {
                if (ttmp != lastRow)
                {
                    listBox3.Items.Add(lastRow.Split(";")[1]);
                    ttmp = lastRow;
                    listBox3.TopIndex = listBox3.Items.Count - 1;
                }                
            }
        }

        //������� ������ ����� �� ���
        private void fox(string? lastRow)
        {

        }

     
    }
}