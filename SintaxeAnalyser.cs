using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba1
{
    class SintaxeAnalyser
    {
        int state = 0;
        int step = 0;
        int lastIn = 0;

        List<string> sost = new List<string> { };

        Stack<int> sortStack = new Stack<int>();
        Stack<String> stack = new Stack<string>();

        public List<string> chars = new List<string>();
        public List<string> keyWord = new List<string>();
        public List<String> variables = new List<string>();
        public List<String> literals = new List<string>();
        public ListWithDuplicates standartTable = new ListWithDuplicates();

        public SintaxeAnalyser(List<string> сhars, List<string> keyWord, List<String> variables, List<String> literals, ListWithDuplicates standartTable)
        {
            this.chars = сhars;
            this.keyWord = keyWord;
            this.variables = variables;
            this.literals = literals;
            this.standartTable = standartTable;
        }

        public void checkEngine()
        {
            bool isError = false;
            bool breakWhile = false;
            state = 0;
            String element = getVerhina(0);

            sdvig();

            while (!breakWhile)
            {
                switch (state)
                {
                    case 0:
                        if (stack.Peek() == "<программа>")
                        {
                            isError = false;
                            breakWhile = true;
                            continue;
                        }else
                        if (stack.Peek() == "<список_действий>")
                        {
                            
                                goState(1);
                            
                        }
                        else
                        if (stack.Peek() == "<действие>")
                        {
                            goState(2);
                        }
                        if (stack.Peek() == "<присваивание>")
                        {
                            goState(5);
                        }
                        else
                        if (stack.Peek() == "<условный_оператор>")
                        {
                            goState(6);
                        }
                        else
                        if (variables.Contains(stack.Peek())) // Переменная
                        {
                            goState(7);
                        }
                        else
                        if (stack.Peek() == "if")
                        {
                            goState(20);
                        }
                        else
                        {
                            messengeShow("Переменная или if");
                            isError = true;
                            continue;
                        }
                            break;

                    case 1:
                        if(step + 1 > standartTable.Count)
                        {
                            isError = false;
                            breakWhile = true;
                            continue;
                        }
                        else
                        if (getVerhina(step+1) == "\n")
                        {
                            goState(4);
                        }
                        else
                        {
                            messengeShow("Переход на другую строку");
                            isError = true;
                            continue;
                        }
                            break;

                    case 2:
                        if (stack.Peek() == "<действие>")
                        {
                            svertka(1, "<список_действий>");
                        }
                        else
                        {
                            isError = true;
                            continue;
                        }
                        break;

                    case 4:
                        if (stack.Peek() == "<действие>")
                        {
                            goState(39);
                        }
                        else
                        if (stack.Peek() == "\n")
                        {
                            sdvig();
                        }
                        
                        if (stack.Peek() == "<присваивание>")
                        {
                            goState(5);
                        }
                        else
                        if (stack.Peek() == "<условный оператор>")
                        {
                            goState(6);
                        }
                        else
                        if (stack.Peek() == "id")
                        {
                            goState(8);
                        }
                        else
                        if (stack.Peek() == "if")
                        {
                            goState(20);
                        }
                        else
                        {
                            messengeShow("if или id или переход на другую строку");
                            isError = true;
                            continue;
                        }
                            break;

                    case 5:
                        if (stack.Peek() == "<присваивание>")
                        {
                            svertka(1, "<действие>");
                        }
                        else
                        {
                            isError = true;
                            continue;
                        }
                            break;

                    case 6:
                        if (stack.Peek() == "<условный_оператор>")
                        {
                            svertka(1, "<действие>");
                        }
                        else
                        {
                            isError = true;
                            continue;
                        }
                        break;

                    case 7:
                        if (stack.Peek() == "id")
                        {
                            sdvig();
                        }
                        if (stack.Peek() == "=")
                        {
                            goState(8);
                        }
                        else
                        {
                            messengeShow("=");
                            isError = true;
                            continue;
                        }
                        break;

                    case 8:
                        if (stack.Peek() == "=")
                        {
                            sdvig();
                        }
                        
                        if (stack.Peek() == "<операнд>")
                        {
                            goState(11);
                        }
                        else
                        if (variables.Contains(stack.Peek()))
                        {
                            goState(9);
                        }
                        else
                        if (literals.Contains(stack.Peek()))
                        {
                            goState(10);
                        }
                        else
                        {
                            messengeShow("переменная или число");
                            isError = true;
                            continue;
                        }
                        break;

                    case 9:
                        if (variables.Contains(stack.Peek()))
                        {
                            svertka(1, "<операнд>");
                        }
                        else
                        {
                            isError = true;
                            continue;
                        }
                        break;

                    case 10:
                        if (literals.Contains(stack.Peek()))
                        {
                            svertka(1, "<операнд>");
                        }
                        else
                        {
                            isError = true;
                            continue;
                        }
                        break;

                    case 11:
                        if (stack.Peek() == "<операнд>")
                        {
                            sdvig();
                        }
                        
                        if (stack.Peek() == ";")
                        {
                            goState(12);
                        }
                        else
                        if (stack.Peek() == "<знак>")
                        {
                            goState(18);
                        }
                        else
                        if (stack.Peek() == "+")
                        {
                            goState(13);
                        }
                        else
                        if (stack.Peek() == "-")
                        {
                            goState(14);
                        }
                        else
                        if (stack.Peek() == "*")
                        {
                            goState(15);
                        }
                        else
                        if (stack.Peek() == "/")
                        {
                            goState(16);
                        }
                        else
                        if (stack.Peek() == "//")
                        {
                            goState(17);
                        }
                        else
                        {
                            messengeShow("знак или ;");
                            isError = true;
                            continue;
                        }
                        break;

                    case 12:
                        if (stack.Peek() == ";")
                        {
                            svertka(4, "<присваивание>");
                        }
                        else
                        {
                            isError = true;
                            continue;
                        }
                        break;

                    case 13:
                        if (stack.Peek() == "+")
                        {
                            svertka(1, "<знак>");
                        }
                        else
                        {
                            isError = true;
                            continue;
                        }
                        break;

                    case 14:
                        if (stack.Peek() == "-")
                        {
                            svertka(1, "<знак>");
                        }
                        else
                        {
                            isError = true;
                            continue;
                        }
                        break;

                    case 15:
                        if (stack.Peek() == "*")
                        {
                            svertka(1, "<знак>");
                        }
                        else
                        {
                            isError = true;
                            continue;
                        }
                        break;

                    case 16:
                        if (stack.Peek() == "/")
                        {
                            svertka(1, "<знак>");
                        }
                        else
                        {
                            isError = true;
                            continue;
                        }
                        break;

                    case 17:
                        if (stack.Peek() == "//")
                        {
                            svertka(1, "<знак>");
                        }
                        else
                        {
                            isError = true;
                            continue;
                        }
                        break;

                    case 18:
                        if (stack.Peek() == "<знак>")
                        {
                            sdvig();
                        }
                        
                        if (stack.Peek() == "<операнд>")
                        {
                            goState(19);
                        }
                        else
                        if (variables.Contains(stack.Peek()))
                        {
                            goState(9);
                        }
                        else
                        if (literals.Contains(stack.Peek()))
                        {
                            goState(10);
                        }
                        else
                        {
                            messengeShow("переменная или число");
                            isError = true;
                            continue;
                        }
                        break;

                    case 19:
                        if (stack.Peek() == "<операнд>")
                        {
                            sdvig();
                        }

                        if (stack.Peek() == ";")
                        {
                            goState(35);
                        }
                        else
                        {
                            messengeShow(";");
                            isError = true;
                            continue;
                        }
                        break;

                    case 20:
                        if (stack.Peek() == "if")           // Втавка заглушки !!!!!!!!
                        {
                            //sdvig();
                            plug();// Заглушка

                            // временно из-за заглушки go
                        }

                        //if (stack.Peek() == ";")
                        //{
                        //    goState(35);
                        //}
                        //else
                        //{
                        //    messengeShow(";");
                        //    isError = true;
                        //    continue;
                        //}
                        break;

                    // Должен быть код вместо заглушки!!!!!

                    case 22:
                        if (stack.Peek() == "\n")
                        {
                            sdvig();
                        }

                        if (stack.Peek() == "then")
                        {
                            goState(23);
                        }
                        else
                        {
                            messengeShow("then");
                            isError = true;
                            continue;
                        }
                        break;

                    case 23:
                        if (stack.Peek() == "then")
                        {
                            sdvig();
                        }

                        if (stack.Peek() == "\n")
                        {
                            goState(24);
                        }
                        else
                        {
                            messengeShow("перенос на друг. строку");
                            isError = true;
                            continue;
                        }
                        break;

                    case 24:
                        if (stack.Peek() == "\n")
                        {
                            sdvig();
                        }

                        if (stack.Peek() == "then")
                        {
                            goState(23);
                        }
                        else
                        {
                            messengeShow("then");
                            isError = true;
                            continue;
                        }
                        break;
                }

            }
        }

        private void goState(int stateNum)
        {
            sortStack.Push(state);
            state = stateNum;
        }

        private void svertka(int num, String neterm)
        {
            for (int i = 0; i < num; i++)
            {
                stack.Pop();
            }
            stack.Push(neterm);
            for (int i = 0; i < num; i++)
            {
                sortStack.Pop();
            }
            state = sortStack.Peek();
        }
        
        private void sdvig()
        {
            stack.Push(getVerhina(step));
            step++;
        }

        private string getVerhina(int numb)
        {
            String element = "";

            switch (Convert.ToInt32(standartTable[numb].Key))
            {
                case 2:
                    element = keyWord[Convert.ToInt32(standartTable[numb].Value)];
                    break;
                case 3:
                    element = chars[Convert.ToInt32(standartTable[numb].Value)];
                    break;
                case 4:
                    element = literals[Convert.ToInt32(standartTable[numb].Value)];
                    break;
                case 5:
                    element = variables[Convert.ToInt32(standartTable[numb].Value)];
                    break;
            }
            return element;
        }

        private void messengeShow(string expected)
        {
            MessageBox.Show("Встретилось: " + stack.Peek() + "  Ожидалось: " + expected); ;
        }

        private void plug()
        {
            while (getVerhina(step) != "\n")
            {
                step++;
                if (step > standartTable.Count)
                {
                    messengeShow("перенос строки после expr");
                }
            }
            
        }
    }
}
