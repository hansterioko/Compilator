using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Laba1
{

    public partial class Form1 : Form
    { 
        public List<string> chars = new List<string> {"\n", "<=",">=", "&", "&&", "*", ";", "-", "+", "/", "{", "}", "(", ")", "=", "==", "<", ">", ","};
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


            string code = rText.Text.TrimStart().TrimEnd();
          
            string buffer = "";
            for(int i = 0; i < code.Length; i++)
            {
                if (!((code[i] >= 48 && code[i] <= 62) || (code[i] >= 65 && code[i] <= 90) || (code[i] == 32) || (code[i] >= 97 && code[i] <= 122) || (code[i] >= 60 && code[i] <= 62) || (code[i] >= 40 && code[i] <= 47) || (code[i] == 44) || (code[i] == 46) || code[i] == 10))
                {
                    MessageBox.Show($"Ошибка компиляции в символе {code[i]}");
                    break;
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
                        if (chars.Contains(Convert.ToString(buffer[0]))){//если первый элемент буфера относится к символу
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
            PrintLeksemWithCode("2", dataGridView2);
            PrintLeksemWithCode("3", dataGridView3);
            PrintLeksemWithCode("4", dataGridView4);
            PrintLeksemWithCode("5", dataGridView5);

            SaveStandartTable();

            //variables.Clear();
            //literals.Clear();
            SintaxeAnalyser analyser = new SintaxeAnalyser(chars, keyWord, variables, literals, standartTable);
            analyser.checkEngine();
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
                    if (lexem[0] >= 48 && lexem[0] <= 57)
                    {
                        MessageBox.Show($"Ошибка в лексеме: {lexem}", "Вывод лексем", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        var list = new ListWithDuplicates();
                        list.Add(lexem, "I");
                        PrintLeksem(list);

                        if (!variables.Contains(lexem) && !keyWord.Contains(lexem))
                        variables.Add(lexem);

                        dataGridView6.Rows.Add("", lexem);
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
