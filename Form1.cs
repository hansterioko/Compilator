using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Laba1
{

    public partial class Form1 : Form
    { 
        public List<string> chars = new List<string> { Convert.ToChar(10).ToString(), "*", ";", "-", "+", "/", "(", ")", "=", "==", "<", ">", ",", "!=", "&", "|", "^" };
        public List<string> keyWord = new List<string> { "if", "then", "else", "end"};
        public List<String> variables = new List<string>();
        public List<String> literals = new List<string>();

        public ListWithDuplicates standartTable = new ListWithDuplicates();

        public Form1()
        {
            InitializeComponent();
        }

        
      
        private void button1_Click(object sender, EventArgs e)
        {
            variables.Clear();
            literals.Clear();
            standartTable.Clear();


            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
           
            dataGridView4.Rows.Clear();
            dataGridView4.Refresh();
            dataGridView5.Rows.Clear();
            dataGridView5.Refresh();
            dataGridView6.Rows.Clear();
            dataGridView6.Refresh();


            string code = rText.Text.TrimStart();
          
            string buffer = "";
            if (code.Length != 0)
            {
                for (int i = 0; i < code.Length; i++)
                {
                    if (!((code[i] >= 48 && code[i] <= 62) || (code[i] >= 65 && code[i] <= 90) || (code[i] == 32) || (code[i] == 94) || (code[i] == 33) || (code[i] >= 97 && code[i] <= 122) || (code[i] >= 60 && code[i] <= 62) || (code[i] >= 40 && code[i] <= 47) || (code[i] == 44) || (code[i] == 46) || code[i] == 10))
                    {
                        MessageBox.Show($"Ошибка компиляции в символе {code[i]}");
                        break;
                    }

                    //MessageBox.Show(code[i].ToString());

                    if (code[i] == 10)
                    {
                        if (code[i - 1] == 10)
                        {
                            continue;
                        }
                        addToList(buffer);
                        buffer = "";
                        addToList(code[i].ToString());
                        continue;
                    }

                    //if (code[i] == 32)
                    //{
                    //    if(code[i-1] == 32)
                    //    {
                    //        continue;
                    //    }
                    //    if (i + 1 > code.Length)
                    //    {
                    //        addToList(buffer);
                    //        buffer = "";
                    //        continue;
                    //    }
                    //}
                    if ((code[i].Equals('!')) && (code.Length >= i+1) && (code[i + 1].Equals('=')))
                    {
                        addToList(buffer);
                        buffer = "";
                        buffer += code[i];
                        continue;
                    }
                    if ((code[i].Equals('=')) && (i >= 0) && (code[i - 1].Equals('!')))
                    {
                        buffer += code[i];
                        addToList(buffer);
                        buffer = "";
                        continue;
                    }

                    if (buffer != "")
                    {
                        if (Convert.ToString(code[i]).Trim().Equals("")) //если нет пробелов
                        {
                            addToList(buffer); //добавляем лексемы в список
                            buffer = ""; //очищаем буфер от лексемы
                        }
                        else if (chars.Contains(Convert.ToString(code[i]))) // если лексема относится к списку знаков
                        {
                            if (chars.Contains(Convert.ToString(buffer) + Convert.ToString(code[i]))) // если в буфере есть символы знаков и любой другой символ
                            {
                                buffer += code[i]; //сложение к буферу
                            }
                            else
                            {
                                addToList(buffer); // добавление лексемы в список
                                buffer = Convert.ToString(code[i]); // буфер становится равным символу
                            }
                        }
                        else
                        {
                            if (chars.Contains(Convert.ToString(buffer[0])))
                            {//если первый элемент буфера относится к символу
                                addToList(buffer); // добавление лексемы в список
                                buffer = ""; //обнуление буфера
                            }
                            buffer += code[i];// сложение элементов к буферу
                        }
                    }
                    else
                    {
                        buffer += code[i];// сложение элементов к буферу
                    }

                    if (i == code.Length - 1)
                    {
                        addToList(buffer);
                    }

                }



                PrintWord(dataGridView4, literals);
                PrintWord(dataGridView5, variables);
                PrintLeksemWithCode("4", dataGridView4);
                PrintLeksemWithCode("2", dataGridView2);
                PrintLeksemWithCode("3", dataGridView3);
               
                PrintLeksemWithCode("5", dataGridView5);

                SaveStandartTable();

                //variables.Clear();
                //literals.Clear();

                //SintaxeAnalyser analyser = new SintaxeAnalyser(chars, keyWord, variables, literals, standartTable);
                //analyser.checkEngine();

                foreach (string item in literals)
                {
                    richTextBox1.Text += item + "               ";
                }
            }

            
        }

        
        public void PrintLeksem(ListWithDuplicates list)
        {
            foreach (KeyValuePair<string, string> listItem in list)
            {
                dataGridView1.Rows.Add(listItem.Key, listItem.Value);

            }
        }

        public void SaveStandartTable()
        {
            foreach(DataGridViewRow row in dataGridView6.Rows)
            {
                standartTable.Add(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString());
            }
        }

        public void PrintLeksemWithCode(String numberTable, DataGridView dataGridView)
        {
            foreach(DataGridViewRow row in dataGridView6.Rows)
            {
                foreach(DataGridViewRow row2 in dataGridView.Rows)
                {
                    if (row.Cells[1].Value.ToString() == row2.Cells[1].Value.ToString())
                    {
                        row.Cells[1].Value = row2.Cells[0].Value;
                        row.Cells[0].Value = numberTable;
                    }
                }
            }
        }

        public void PrintWord(DataGridView dataGridView, List<String> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                dataGridView.Rows.Add(i, list[i]);
            }
        }

        public void addToList(string lexem)
        {
            if (keyWord.Contains(lexem.ToLower()))
                lexem = lexem.ToLower();

            if (int.TryParse(lexem, out int numericValue)) //если число
            {
                var list = new ListWithDuplicates();
                list.Add(lexem, "L");
                PrintLeksem(list);  // вывод лексемы в 1-ую таблицу

                literals.Add(lexem);    // добавление в список чисел

                dataGridView6.Rows.Add("", lexem);
            }
            else
            {
                
                if (chars.Contains(Convert.ToString(lexem)))
                {
                    var list = new ListWithDuplicates();
                    list.Add(lexem, "R");
                    PrintLeksem(list);

                    dataGridView6.Rows.Add("", lexem);
                }
                else
                {
                    if (lexem.Length == 0)
                    {
                        return;
                    }
                    else
                    {
                        if (lexem[0] >= 48 && lexem[0] <= 57)
                        {
                            MessageBox.Show($"Ошибка в лексеме: {lexem}", "Вывод лексем", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                            return;
                        }
                        else
                        {
                            if (lexem == " ")
                            {
                                return;
                            }

                            lexem = lexem.Trim();

                            var list = new ListWithDuplicates();
                            list.Add(lexem, "I");
                            PrintLeksem(list);

                            if (!variables.Contains(lexem) && !keyWord.Contains(lexem.ToLower()))
                                variables.Add(lexem);

                            dataGridView6.Rows.Add("", lexem);
                        }
                    }
                    
                }
                
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PrintWord(dataGridView2, keyWord);
            PrintWord(dataGridView3, chars);
        }



    }
}
