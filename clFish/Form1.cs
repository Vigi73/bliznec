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
        List<int> listFox = new List<int>{};
        int tmpFox = int.MaxValue;
        int ffff = 0;




        public Form1()
        {
            InitializeComponent();

            dgEverest.AllowUserToAddRows = false;
            dgRange.AllowUserToDeleteRows = false;


            dgEverest.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            dgRange.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 7, FontStyle.Bold);

            for (int i = 0; i < dgEverest.Columns.Count; i++)
                dgEverest.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            for (int i = 0; i < dgRange.Columns.Count; i++)
                dgRange.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

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
        public SoundPlayer sp = new SoundPlayer(Properties.Resources.bonus);
        

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

        private void t4����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("yrt.exe");
        }
               
        private void tmrFox_Tick(object sender, EventArgs e)
        {
            if (chFox.Checked && listBox1.Items.Count > 0)
            {
                try
                {

                    var lastRowForFox = listBox1.Items[^1].ToString();

                    if (checkBox3.Checked) // ���� � ������ ����
                        fox_fish(lastRowForFox);
                    else
                        fox(lastRowForFox);
                }
                catch
                {
                }

                button5_Click(null, null);
            }
        }

        //������� ������ ����� �� ���

        public int GetFish(string weigh)
        {
            if (weigh.Contains('.'))
            {
                switch (weigh.Split(".")[1].Length)
                {
                    case 3:
                        tVes = weigh.Replace(".", "");
                        break;
                    case 2:
                        tVes = weigh.Replace(".", "") + "0";
                        break;
                    case 1:
                        tVes = weigh.Replace(".", "") + "00";
                        break;
                }
            }
            else
            {
                tVes = weigh;
            }

            int realWeigh = default(int);
            try
            {
                realWeigh = Int32.Parse(tVes);
            }
            catch
            {
            }

            return realWeigh;
        }
// ===================================================================================================================

        string fishTMP = string.Empty;

        private void fox(string? lastRow)
        {
            string currentFish = lastRow.Split(';')[1];
            if (currentFish != fishTMP)
            {
                               

                int realF = GetFish(currentFish);

                switch (cb�onditions.Text)
                {
                    case "=":
                        if (realF == Int32.Parse(txtWeigh.Text))
                        {
                            sp.Play();
                            txtTarget.Text = currentFish;
                        }
                        else
                        {
                        }
                        break;

                    case ">=":
                        if (realF >= Int32.Parse(txtWeigh.Text))
                        {
                            if (realF == Int32.Parse(txtWeigh.Text))
                            {
                                sp.Play();
                                txtTarget.Text = currentFish;
                            }
                            else
                            {

                                listBox4.Items.Add(currentFish);
                                listBox4.TopIndex = listBox4.Items.Count - 1;
                            }
                        }
                        else
                        {
                        }
                        break;

                    case "<=":
                        if (realF <= Int32.Parse(txtWeigh.Text))
                        {
                            if (realF == Int32.Parse(txtWeigh.Text))
                            {
                                sp.Play();
                                txtTarget.Text = currentFish;
                            }
                            else
                            {
                                listBox4.Items.Add(currentFish);
                                listBox4.TopIndex = listBox4.Items.Count - 1;
                            }
                        }
                        else
                        {
                        }
                        break;

                    case "<>":
                        if (realF == Int32.Parse(txtWeigh.Text))
                        {
                            sp.Play();
                            txtTarget.Text = currentFish;
                        }
                        else
                        {
                            listBox4.Items.Add(currentFish);
                            listBox4.TopIndex = listBox4.Items.Count - 1;
                        }
                        break;
                }
                
                //listBox4.Items.Add(currentFish);
                fishTMP = currentFish;


            }
        }

        
        // �������� ����������� � ������ ������������ ���
        private void button5_Click(object sender, EventArgs e)
        {
            var resultFish = default(int);

            if (listBox4.Items.Count > 0)
            {
                foreach (string elm in listBox4.Items)
                {
                    listFox.Add(GetFish(elm));
                }

                listFox.Sort();

                switch (cb�onditions.Text)
                {
                    case "=":
                        break;

                    case "<=":
                        resultFish = listFox.Last();
                        
                        break;
                    case ">=":
                        resultFish = listFox[0];
                        break;
                    case "<>":

                    resultFish = GetNearFish(listFox);
                    break;
                }

                txtB.Text = resultFish.ToString();
                //lblProc.Text = txtB.Text.ToString();
                
                try
                {
                    var rF = double.Parse(txtB.Text);
                    var rT = double.Parse(txtWeigh.Text);

                    if (txtB.Text != String.Empty)
                    {
                        if (rF > rT)
                        {
                            lblProc.Text = Math.Round(rT / rF * 100, 2).ToString() + "%";
                        }
                        else
                        {                            
                            lblProc.Text = Math.Round(rF / rT * 100, 2).ToString() + "%";
                        }
                    }
                }
                catch { }

            }
        }

        // �������� ������ ��� � ������
        private int GetNearFish(List<int> listFox)
        {
            int ideal = int.Parse(txtWeigh.Text);

            foreach (int elm in listFox)
            {
                if (elm < ideal)
                {

                    if (ideal - elm < tmpFox)
                    {
                        tmpFox = ideal - elm;
                        ffff = elm;
                    }
                }
                else
                {
                    if (elm - ideal < tmpFox)
                    {
                        tmpFox = elm -ideal;
                        ffff = elm;
                    }
                }

            }
            return ffff;           
        }

        // ��������� ���� ��� �������
        private void button7_Click(object sender, EventArgs e)
        {
            lbFishE.Items.Add(comboBox3.Text);
        }
        // ������� ��������� ���� �� �������
        private void button8_Click(object sender, EventArgs e)
        {
            lbFishE.Items.Remove(lbFishE.SelectedItem);
        }

        // ��������� ������� �������� ������
        private void button6_Click(object sender, EventArgs e)
        {
            int otE = int.Parse(txtOtEveret.Text);
            int doE = int.Parse(txtDoEverest.Text);
            int stepE = int.Parse(txtStepEverest.Text);
            int maxV = int.MaxValue;

            


            //��������� ������� ������ ���
            dgEverest.Rows.Clear();
            for (var i = otE; i < doE; i+= stepE)
            {
               // int FishRez = currFishRez(i, currentTrunk);  

               dgEverest.Rows.Add(i.ToString(), (i + stepE).ToString(), $"{0}", $"{0.0}%", $"{maxV}");
            }
        }




        private void dgEverest_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            dgEverest.Rows[e.RowIndex].HeaderCell.Value =
       (e.RowIndex + 1).ToString();
        }



        // ������ ��������
        private void tmEverest_Tick(object sender, EventArgs e)
        {
            if (chEverest.Checked && listBox1.Items.Count > 0)
            {
                
                string lastRowForFox = listBox1.Items[^1].ToString();
                int currVes = GetFish(lastRowForFox.Split(";")[1]);
                string currFIshName = lastRowForFox.Split(';')[0];
                int index = lbFishE.FindString(currFIshName); // ����� ������� ���� � �����
                


                // ���� ���� ������������� ������� ������� �� ��������� �� � �������� ������ ���
                try
                {
                    if (currVes >= int.Parse(txtOtEveret.Text) && currVes <= int.Parse(txtDoEverest.Text) && index != -1)
                    {
                        if (listBox5.FindString(currVes.ToString()) == -1)
                        {
                            listBox5.Items.Add(currVes.ToString());
                            listBox5.TopIndex = listBox5.Items.Count - 1;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("�� ������� ��� ������");
                }
                button9_Click(null, null);

            }
        }
        // ��������� ����������� ��� � �������
        private void button9_Click(object sender, EventArgs e)
        {
            float score = 0f;

            foreach (DataGridViewRow row in dgEverest.Rows)
            {
                 score += float.Parse(((string)row.Cells[3].Value).Trim('%')) * 100;
            }
            textBox2.Text = Math.Round(score , 2).ToString(); // ����� ������


                if (listBox5.Items.Count > 0)
            {
                foreach (string rowRez in listBox5.Items)
                {

                    int rez = Int32.Parse(rowRez);


                    foreach (DataGridViewRow row in dgEverest.Rows)
                    {
                        try
                        {
                            if (rez >= Int32.Parse((string)row.Cells[0].Value) &&
                                rez < Int32.Parse((string)row.Cells[1].Value) &&
                                rez < Int32.Parse((string)row.Cells[4].Value))
                            {
                                dgEverest.ClearSelection();
                                row.Cells[2].Value = $"{rez}";
                                row.Cells[4].Value = $"{rez}";
                                row.Cells[3].Value = Math.Round(float.Parse((string)row.Cells[0].Value) / rez * 100, 2).ToString() + "%";
                                row.Selected = true;
                                dgEverest.FirstDisplayedScrollingRowIndex = row.Index;
                                sp.Play();
                            }


                        }
                        catch
                        {
                        }
                        
                    }
                }
            }
        }

        // ��������� ������� �������� ��� ����������
        private void btnDAddTable_Click(object sender, EventArgs e)
        {
            try
            {
                int otD = int.Parse(txtDOt.Text);
                int doD = int.Parse(txtDDo.Text);
                int maxV = int.MaxValue;
                string fishRange = cbDFish.Text;
                string conditionRange = cbDCondition.Text;
               
                //�������� ������ � ������� 
                dgRange.Rows.Add(fishRange, $"{otD}", $"{doD}", conditionRange, $"{0}", $"{0.00}%", $"{maxV}");
            }
            catch
            {
                MessageBox.Show("�� ��� ������ �������...");
            }

        }
        // ������� ������ �� �������
        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                dgRange.Rows.Remove(dgRange.Rows[dgRange.CurrentRow.Index]);
            }
            catch
            {
            }
        }
        // ����������� ���� ���������
        private void tmRange_Tick(object sender, EventArgs e)
        {
            if (chRange.Checked && listBox1.Items.Count > 0)
            {
                var lastRowForFox = listBox1.Items[^1].ToString();

                var dataLogFish = lastRowForFox.Split(";");

                string curFish = dataLogFish[0].Trim();
                int currFishWeight = GetFish(dataLogFish[1]);

               
                foreach (DataGridViewRow row in dgRange.Rows)
                {
                    try
                    {

                        switch (row.Cells[3].Value)
                        {
                            case ">=":
                                if ((string)row.Cells[0].Value == curFish && currFishWeight >= int.Parse((string)row.Cells[1].Value) &&
                                    currFishWeight <= int.Parse((string)row.Cells[2].Value) && currFishWeight > int.Parse((string)row.Cells[4].Value))
                                {
                                    dgRange.ClearSelection();
                                    row.Cells[4].Value = $"{currFishWeight}";
                                    row.Cells[5].Value = $"{Math.Round(currFishWeight / float.Parse((string)row.Cells[2].Value) * 100, 2)}%";
                                    row.Selected = true;
                                    dgRange.FirstDisplayedScrollingRowIndex = row.Index;
                                    sp.Play();
                                }


                                break;
                            case "<=":
                                if ((string)row.Cells[0].Value == curFish && currFishWeight >= int.Parse((string)row.Cells[1].Value) &&
                                    currFishWeight <= int.Parse((string)row.Cells[2].Value) && currFishWeight < int.Parse((string)row.Cells[6].Value))
                                {
                                    dgRange.ClearSelection();
                                    row.Cells[4].Value = $"{currFishWeight}";
                                    row.Cells[6].Value = $"{currFishWeight}";
                                    row.Cells[5].Value = $"{Math.Round(float.Parse((string)row.Cells[1].Value) / currFishWeight  * 100, 2)}%";
                                    row.Selected = true;
                                    dgRange.FirstDisplayedScrollingRowIndex = row.Index;
                                    sp.Play();
                                }

                                break;
                        }
                    }
                    catch { }
                }



            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }



        //================================ if fish =======================================

        private void fox_fish(string? lastRow)
        {
            string currentFish = lastRow.Split(';')[1];
            string currentFishName = lastRow.Split(';')[0];

            if (currentFish != fishTMP)
            {


                int realF = GetFish(currentFish);

                switch (cb�onditions.Text)
                {
                    case "=":
                        if (realF == Int32.Parse(txtWeigh.Text) && cbFishfox.Text == currentFishName) 
                        {
                            sp.Play();
                            txtTarget.Text = currentFish;
                        }
                        else
                        {
                        }
                        break;

                    case ">=":
                        if (realF >= Int32.Parse(txtWeigh.Text) && cbFishfox.Text == currentFishName)
                        {
                            if (realF == Int32.Parse(txtWeigh.Text))
                            {
                                sp.Play();
                                txtTarget.Text = currentFish;
                            }
                            else
                            {

                                listBox4.Items.Add(currentFish);
                                listBox4.TopIndex = listBox4.Items.Count - 1;
                            }
                        }
                        else
                        {
                        }
                        break;

                    case "<=":
                        if (realF <= Int32.Parse(txtWeigh.Text) && cbFishfox.Text == currentFishName)
                        {
                            if (realF == Int32.Parse(txtWeigh.Text))
                            {
                                sp.Play();
                                txtTarget.Text = currentFish;
                            }
                            else
                            {
                                listBox4.Items.Add(currentFish);
                                listBox4.TopIndex = listBox4.Items.Count - 1;
                            }
                        }
                        else
                        {
                        }
                        break;

                    case "<>":
                        if (cbFishfox.Text == currentFishName)
                        {
                            if (realF == Int32.Parse(txtWeigh.Text))
                            {
                                sp.Play();
                                txtTarget.Text = currentFish;
                            }
                            else
                            {
                                listBox4.Items.Add(currentFish);
                                listBox4.TopIndex = listBox4.Items.Count - 1;
                            }
                        }
                        else
                        {
                        }
                        break;
                }
                fishTMP = currentFish;
            }
        }
    }
}