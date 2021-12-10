
using System.Diagnostics;

namespace clFish
{
    public partial class Form1 : Form
    {
        string tmp = string.Empty;        
        bool fl = false;
        bool flm = false;

        public Form1()
        {
            InitializeComponent();
            button2.Visible = false;
            if (!fl)
            {
                button3.Text = "Ïóñê";
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
        string path = @"log.txt";

        private void mainTimer_Tick(object sender, EventArgs e)
        {
           

           try
            {
                string s = File.ReadAllLines(path).Last();
                if (tmp != s)
                {
                    listBox1.Items.Add(s);
                    tmp = s;
                    listBox1.TopIndex = listBox1.Items.Count - 1;
                }
            }
            catch (Exception ex)
            {   
            }

        }

        private void î÷èñòèòüËîãToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File.WriteAllText(path, string.Empty);
            listBox1.Items.Clear();
            listBox1.Items.Add("Ëîã ôàéë î÷èùåí...");
            tmp = String.Empty;
        }

        
        private void button3_Click(object sender, EventArgs e)
        {
            if (!fl)
            {
                Process.Start("main.exe");
                fl = true;
                button3.Text = "Ñòîï";
            }
            else
            {
                fl = false;
                button3.Text = "Ïóñê";
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
        }
    }
}