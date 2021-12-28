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
            
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView2.ColumnHeadersVisible = false;

            dataGridView1.RowHeadersVisible = false;
            dataGridView2.RowHeadersVisible = false;

           
            for (int i = 0; i < 3; i++)
            {
                dataGridView1.Rows.Add("  ", "  ", "  ", "  ", "  ", "  ", "  ", "  ", "  ");
                dataGridView2.Rows.Add("  ", "  ", "  ", "  ", "  ", "  ", "  ", "  ", "  ");
            }

            

            dgEverest.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            dgRange.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 7, FontStyle.Bold);

            for (int i = 0; i < dgEverest.Columns.Count; i++)
                dgEverest.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            for (int i = 0; i < dgRange.Columns.Count; i++)
                dgRange.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
                dataGridView1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;


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

            // Если поиск Охота на лис
            
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

        private void t4ПускToolStripMenuItem_Click(object sender, EventArgs e)
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

                    if (checkBox3.Checked) // если с учетом рыбы
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

        //Функция работы охота на лис

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

                switch (cbСonditions.Text)
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

        
        // Проверка результатов в списке соответствия рыб
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

                switch (cbСonditions.Text)
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

        // Получаем лучший рез к идеалу
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

        // Добавляем рыбу для задания
        private void button7_Click(object sender, EventArgs e)
        {
            lbFishE.Items.Add(comboBox3.Text);
        }
        // Удаляем выбронную рыбу из задания
        private void button8_Click(object sender, EventArgs e)
        {
            lbFishE.Items.Remove(lbFishE.SelectedItem);
        }

        // Заполнить задание основная кнопка
        private void button6_Click(object sender, EventArgs e)
        {
            int otE = int.Parse(txtOtEveret.Text);
            int doE = int.Parse(txtDoEverest.Text);
            int stepE = int.Parse(txtStepEverest.Text);
            int maxV = int.MaxValue;

            


            //заполняем таблицу первый раз
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



        // Таймер эвереста
        private void tmEverest_Tick(object sender, EventArgs e)
        {
            if (chEverest.Checked && listBox1.Items.Count > 0)
            {
                
                string lastRowForFox = listBox1.Items[^1].ToString();
                int currVes = GetFish(lastRowForFox.Split(";")[1]);
                string currFIshName = lastRowForFox.Split(';')[0];
                int index = lbFishE.FindString(currFIshName); // поиск текущей рыбы в садке
                


                // если рыба соответствует условию задания то дабавляем ее в основной список рыб
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
                    MessageBox.Show("Не введены все данные");
                }
                button9_Click(null, null);

            }
        }
        // Вставляем выловленный рез в таблицу
        private void button9_Click(object sender, EventArgs e)
        {
            float score = 0f;

            foreach (DataGridViewRow row in dgEverest.Rows)
            {
                 score += float.Parse(((string)row.Cells[3].Value).Trim('%')) * 100;
            }
            textBox2.Text = Math.Round(score , 2).ToString(); // Вывод баллов


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

        // Заполняем таблицу заданием для диапазонов
        private void btnDAddTable_Click(object sender, EventArgs e)
        {
            try
            {
                int otD = int.Parse(txtDOt.Text);
                int doD = int.Parse(txtDDo.Text);
                int maxV = int.MaxValue;
                string fishRange = cbDFish.Text;
                string conditionRange = cbDCondition.Text;
               
                //Добавить запись в таблицу 
                dgRange.Rows.Add(fishRange, $"{otD}", $"{doD}", conditionRange, $"{0}", $"{0.00}%", $"{maxV}");
            }
            catch
            {
                MessageBox.Show("Не все данные введены...");
            }

        }
        // Удаляем запись из таблицы
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
        // отслеживаем улов Диапазоны
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
        // create new card 1
        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.ClearSelection();


                var od = new List<int>();
                var dv = new List<int>();
                var tr = new List<int>();
                var ch = new List<int>();
                var py = new List<int>();
                var sh = new List<int>();
                var se = new List<int>();
                var vo = new List<int>();
                var de = new List<int>();

                var listTemp = Clipboard.GetText().Split(",");

                List<int> list = new List<int>();

                foreach (string value in listTemp)
                {
                    list.Add(Int32.Parse(value));
                }


                for (int i = 0; i < list.Count; i++)
                {
                    switch (list[i])
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            od.Add(list[i]); // Добавляем в список если до 10
                                             //listBox6.Items.Add(list[i]);
                            break;

                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                            dv.Add(list[i]); // Добавляем в список если до 20
                            break;

                        case 20:
                        case 21:
                        case 22:
                        case 23:
                        case 24:
                        case 25:
                        case 26:
                        case 27:
                        case 28:
                        case 29:
                            tr.Add(list[i]); // Добавляем в список если до 30
                            break;

                        case 30:
                        case 31:
                        case 32:
                        case 33:
                        case 34:
                        case 35:
                        case 36:
                        case 37:
                        case 38:
                        case 39:
                            ch.Add(list[i]); // Добавляем в список если до 40
                            break;

                        case 40:
                        case 41:
                        case 42:
                        case 43:
                        case 44:
                        case 45:
                        case 46:
                        case 47:
                        case 48:
                        case 49:
                            py.Add(list[i]); // Добавляем в список если до 50
                            break;

                        case 50:
                        case 51:
                        case 52:
                        case 53:
                        case 54:
                        case 55:
                        case 56:
                        case 57:
                        case 58:
                        case 59:
                            sh.Add(list[i]); // Добавляем в список если до 60
                            break;

                        case 60:
                        case 61:
                        case 62:
                        case 63:
                        case 64:
                        case 65:
                        case 66:
                        case 67:
                        case 68:
                        case 69:
                            se.Add(list[i]); // Добавляем в список если до 70
                            break;

                        case 70:
                        case 71:
                        case 72:
                        case 73:
                        case 74:
                        case 75:
                        case 76:
                        case 77:
                        case 78:
                        case 79:
                            vo.Add(list[i]); // Добавляем в список если до 80
                            break;

                        case 80:
                        case 81:
                        case 82:
                        case 83:
                        case 84:
                        case 85:
                        case 86:
                        case 87:
                        case 88:
                        case 89:
                        case 90:
                        case 91:
                        case 92:
                        case 93:
                        case 94:
                        case 95:
                        case 96:
                        case 97:
                        case 98:
                        case 99:
                            de.Add(list[i]); // Добавляем в список если до 70
                            break;
                    }
                }
                AddRowRez1(0, od);
                AddRowRez1(1, dv);
                AddRowRez1(2, tr);
                AddRowRez1(3, ch);
                AddRowRez1(4, py);
                AddRowRez1(5, sh);
                AddRowRez1(6, se);
                AddRowRez1(7, vo);
                AddRowRez1(8, de);
                button11.Enabled = false;
            }
            catch { }
            

        }


        // Метод расстановки цифр в карте 1
        private void AddRowRez1(int v, List<int> zn)
        {
            Random rnd = new Random();

            switch (zn.Count)
            {

                case 1:
                    int vlz = rnd.Next(0, 3);
                    dataGridView1[v, vlz].Value = $"{zn[0]}";
                    break;
                case 2:
                    switch (rnd.Next(1, 4))
                    {
                        case 1:
                            dataGridView1[v, 0].Value = $"{zn[0]}";
                            dataGridView1[v, 1].Value = $"{zn[1]}";
                            break;
                        case 2:
                            dataGridView1[v, 0].Value = $"{zn[0]}";
                            dataGridView1[v, 2].Value = $"{zn[1]}";
                            break;
                        case 3:
                            dataGridView1[v, 1].Value = $"{zn[0]}";
                            dataGridView1[v, 2].Value = $"{zn[1]}";
                            break;
                    }

                    break;
                case 3:
                    for (int i = 0; i < 3; i++)
                    {
                        dataGridView1[v, i].Value = $"{zn[i]}";
                    }
                    break;
            }
        }


        // Метод расстановки цифр в карте 2
        private void AddRowRez2(int v, List<int> zn)
        {
            Random rnd = new Random();

            switch (zn.Count)
            {

                case 1:
                    int vlz = rnd.Next(0, 3);
                    dataGridView2[v, vlz].Value = $"{zn[0]}";
                    break;
                case 2:
                    switch (rnd.Next(1, 4))
                    {
                        case 1:
                            dataGridView2[v, 0].Value = $"{zn[0]}";
                            dataGridView2[v, 1].Value = $"{zn[1]}";
                            break;
                        case 2:
                            dataGridView2[v, 0].Value = $"{zn[0]}";
                            dataGridView2[v, 2].Value = $"{zn[1]}";
                            break;
                        case 3:
                            dataGridView2[v, 1].Value = $"{zn[0]}";
                            dataGridView2[v, 2].Value = $"{zn[1]}";
                            break;
                    }

                    break;
                case 3:
                    for (int i = 0; i < 3; i++)
                    {
                        dataGridView2[v, i].Value = $"{zn[i]}";
                    }
                    break;
            }
        }




        // creat new card 2
        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView2.ClearSelection();


                var od = new List<int>();
                var dv = new List<int>();
                var tr = new List<int>();
                var ch = new List<int>();
                var py = new List<int>();
                var sh = new List<int>();
                var se = new List<int>();
                var vo = new List<int>();
                var de = new List<int>();

                //string buff = Clipboard.GetText().Split(",");


                var listTemp = Clipboard.GetText().Split(",");

                List<int> list = new List<int>();

                foreach (string value in listTemp)
                {
                    list.Add(Int32.Parse(value));
                }


                for (int i = 0; i < list.Count; i++)
                {
                    switch (list[i])
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            od.Add(list[i]); // Добавляем в список если до 10
                                             //listBox6.Items.Add(list[i]);
                            break;

                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                            dv.Add(list[i]); // Добавляем в список если до 20
                            break;

                        case 20:
                        case 21:
                        case 22:
                        case 23:
                        case 24:
                        case 25:
                        case 26:
                        case 27:
                        case 28:
                        case 29:
                            tr.Add(list[i]); // Добавляем в список если до 30
                            break;

                        case 30:
                        case 31:
                        case 32:
                        case 33:
                        case 34:
                        case 35:
                        case 36:
                        case 37:
                        case 38:
                        case 39:
                            ch.Add(list[i]); // Добавляем в список если до 40
                            break;

                        case 40:
                        case 41:
                        case 42:
                        case 43:
                        case 44:
                        case 45:
                        case 46:
                        case 47:
                        case 48:
                        case 49:
                            py.Add(list[i]); // Добавляем в список если до 50
                            break;

                        case 50:
                        case 51:
                        case 52:
                        case 53:
                        case 54:
                        case 55:
                        case 56:
                        case 57:
                        case 58:
                        case 59:
                            sh.Add(list[i]); // Добавляем в список если до 60
                            break;

                        case 60:
                        case 61:
                        case 62:
                        case 63:
                        case 64:
                        case 65:
                        case 66:
                        case 67:
                        case 68:
                        case 69:
                            se.Add(list[i]); // Добавляем в список если до 70
                            break;

                        case 70:
                        case 71:
                        case 72:
                        case 73:
                        case 74:
                        case 75:
                        case 76:
                        case 77:
                        case 78:
                        case 79:
                            vo.Add(list[i]); // Добавляем в список если до 80
                            break;

                        case 80:
                        case 81:
                        case 82:
                        case 83:
                        case 84:
                        case 85:
                        case 86:
                        case 87:
                        case 88:
                        case 89:
                        case 90:
                        case 91:
                        case 92:
                        case 93:
                        case 94:
                        case 95:
                        case 96:
                        case 97:
                        case 98:
                        case 99:
                            de.Add(list[i]); // Добавляем в список если до 70
                            break;
                    }
                }
                AddRowRez2(0, od);
                AddRowRez2(1, dv);
                AddRowRez2(2, tr);
                AddRowRez2(3, ch);
                AddRowRez2(4, py);
                AddRowRez2(5, sh);
                AddRowRez2(6, se);
                AddRowRez2(7, vo);
                AddRowRez2(8, de);
                button12.Enabled = false;
            }
            catch { }

            
        }



        //================================ if fish =======================================

        private void fox_fish(string? lastRow)
        {
            string currentFish = lastRow.Split(';')[1];
            string currentFishName = lastRow.Split(';')[0];

            if (currentFish != fishTMP)
            {


                int realF = GetFish(currentFish);

                switch (cbСonditions.Text)
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