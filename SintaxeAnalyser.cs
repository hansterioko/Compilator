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
            step = 0;

            sdvig();
            

            
            while (!isError)
            {
                //MessageBox.Show("Стадия  " + state.ToString() + "   Стек   " + stack.Peek());
                switch (state)
                {
                    case 0:
                        
                        
                        if (stack.Peek() == "<список_действий>")
                        {               
                            goState(1);
                        }
                        else
                        if (stack.Peek() == "<действие>")
                        {
                            goState(2);

                        }else
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
                        if (step + 1 > standartTable.Count)
                        {
                            isError = true;
                            breakWhile = true;
                            continue;
                        }
                        else
                        if (getVerhina(step).Equals(Convert.ToChar(10).ToString()))
                        {
                            sdvig();
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
                        if (step + 1 > standartTable.Count)     // если после /- ничё нет, то конец
                        {
                            isError = true;
                            breakWhile = true;
                            continue;
                        }

                        if (stack.Peek().Equals(Convert.ToChar(10).ToString()))
                        {
                            if (step + 1 > standartTable.Count)
                            {
                                isError = true;
                                MessageBox.Show("После переноса ожидалось: if or variable of переход на другую строку");
                                continue;
                            }
                            sdvig();
                        }

                        if (stack.Peek() == "<действие>")
                        {
                            goState(39);
                        }
                        else
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
                        if (variables.Contains(stack.Peek()))
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
                            messengeShow("if или variable или переход на другую строку");
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
                        if (variables.Contains(stack.Peek()))
                        {
                            if (step + 1 > standartTable.Count)
                            {
                                isError = true;
                                MessageBox.Show("После числа ожидалось: =");
                                continue;
                            }
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
                            if (step + 1 > standartTable.Count)
                            {
                                isError = true;
                                MessageBox.Show("После = ожидалось: Переменная или число");
                                continue;
                            }
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
                            if (step + 1 > standartTable.Count)
                            {
                                isError = true;
                                stack.Pop();
                                MessageBox.Show("После " + stack.Peek() + " ожидалось: ; or знак");
                                continue;
                            }
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
                            if (step + 1 > standartTable.Count)
                            {
                                isError = true;
                                MessageBox.Show("Ожидалось: переменная или число");
                                continue;
                            }
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
                            if (step + 1 > standartTable.Count)
                            {
                                isError = true;
                                MessageBox.Show("Ожидалось: \";\"");
                                continue;
                            }
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
                            isError = plug();// Заглушка
                            if (isError)
                            {
                                MessageBox.Show("then не нашлось");
                                continue;
                            }
                            stack.Push("then");
                            goState(23);
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

                    //case 22:
                    //    if (stack.Peek() == "\n")
                    //    {
                    //        if (step + 1 > standartTable.Count)
                    //        {
                    //            isError = true;
                    //            MessageBox.Show("Ожидалось: then");
                    //            continue;
                    //        }
                    //        sdvig();
                    //    }

                    //    if (stack.Peek() == "then")
                    //    {
                    //        goState(23);
                    //    }
                    //    else
                    //    {
                    //        messengeShow("then");
                    //        isError = true;
                    //        continue;
                    //    }
                    //    break;

                    case 23:
                        if (stack.Peek() == "then")
                        {
                            if (step + 1 > standartTable.Count)
                            {
                                isError = true;
                                MessageBox.Show("Ожидалось: переход на другую строку");
                                continue;
                            }
                            sdvig();
                        }

                        if (stack.Peek().Equals(Convert.ToChar(10).ToString()))
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
                        if (stack.Peek().Equals(chars[0]))
                        {
                            if (step + 1 > standartTable.Count)
                            {
                                isError = true;
                                MessageBox.Show("Ожидалось: if or variable");
                                continue;
                            }
                            sdvig();
                        }

                        if (stack.Peek() == "<список_действий>")
                        {
                            goState(26);
                        }
                        else
                        if (stack.Peek() == "<действие>")
                        {
                            goState(2);
                        }
                        else
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
                        if (variables.Contains(stack.Peek()))
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
                            messengeShow("переменная или if");
                            isError = true;
                            continue;
                        }
                        break;

                    case 26:
                        if (stack.Peek() == "<список_действий>")
                        {
                            if (step + 1 > standartTable.Count)
                            {
                                isError = true;
                                MessageBox.Show("Ожидалось: переход на другую строку");
                                continue;
                            }
                            sdvig();
                        }

                        if (stack.Peek().Equals(chars[0]))
                        {
                            goState(27);
                        }
                        else
                        {
                            messengeShow("перенос на друг. строку");
                            isError = true;
                            continue;
                        }
                        break;

                    case 27:
                        if (stack.Peek().Equals(chars[0]))
                        {
                            if (step + 1 > standartTable.Count)
                            {
                                isError = true;
                                MessageBox.Show("Ожидалось: end or else");
                                continue;
                            }
                            sdvig();
                        }

                        
                        if (stack.Peek() == "<условный_оператор>")
                        {
                            goState(6);
                        }
                        else
                        if (stack.Peek() == "<присваивание>")
                        {
                            goState(5);
                        }
                        else
                       if (stack.Peek() == "<действие>")
                        {
                            goState(39);
                        }
                        else
                        if (stack.Peek() == "end")
                        {
                            goState(28);
                        }
                        else
                        if (stack.Peek() == "else")
                        {
                            goState(30);
                        }
                        else
                        if (variables.Contains(stack.Peek()))
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
                            messengeShow("end or else");
                            isError = true;
                            continue;
                        }
                        break;

                    case 28:
                        if (stack.Peek() == "end")
                        {
                            if (step + 1 > standartTable.Count)
                            {
                                isError = true;
                                MessageBox.Show("Ожидалось: \";\"");
                                continue;
                            }
                            sdvig();
                        }

                        if (stack.Peek() == ";")
                        {
                            goState(37);
                        }
                        else
                        {
                            messengeShow(";");
                            isError = true;
                            continue;
                        }
                        break;

                    case 30:
                        if (stack.Peek() == "else")
                        {
                            if (step + 1 > standartTable.Count)
                            {
                                isError = true;
                                MessageBox.Show("Ожидалось: переход на другую строку");
                                continue;
                            }
                            sdvig();
                        }

                        if (stack.Peek().Equals(chars[0]))
                        {
                            goState(31);
                        }
                        else
                        {
                            messengeShow("переход на другую строку");
                            isError = true;
                            continue;
                        }
                        break;

                    case 31:
                        if (stack.Peek().Equals(chars[0]))
                        {
                            if (step + 1 > standartTable.Count)
                            {
                                isError = true;
                                MessageBox.Show("Ожидалось: variable or if");
                                continue;
                            }
                            sdvig();
                        }

                        if (stack.Peek() == "<список_действий>")
                        {
                            goState(32);
                        }
                        else
                        if (stack.Peek() == "<действие>")
                        {
                            goState(2);
                        }
                        else
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
                        if (variables.Contains(stack.Peek()))
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
                            messengeShow("variable or if");
                            isError = true;
                            continue;
                        }
                        break;

                    case 32:
                        if (stack.Peek() == "<список_действий>")
                        {
                            if (step + 1 > standartTable.Count)
                            {
                                isError = true;
                                MessageBox.Show("Ожидалось: переход на другую строку");
                                continue;
                            }
                            sdvig();
                        }

                        if (stack.Peek().Equals(chars[0]))
                        {
                            goState(33);
                        }
                        else
                        {
                            messengeShow("переход на другую строку");
                            isError = true;
                            continue;
                        }
                        break;

                    case 33:
                        if (stack.Peek().Equals(chars[0]))
                        {
                            if (step + 1 > standartTable.Count)
                            {
                                isError = true;
                                MessageBox.Show("Ожидалось: end");
                                continue;
                            }
                            sdvig();
                        }

                        if (stack.Peek() == "end")
                        {
                            goState(34);
                        }
                        else
                        if (stack.Peek() == "<действие>")
                        {
                            goState(39);
                        }
                        else
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
                        if (variables.Contains(stack.Peek()))
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
                            messengeShow("end");
                            isError = true;
                            continue;
                        }
                        break;

                    case 34:
                        if (stack.Peek() == "end")
                        {
                            if (step + 1 > standartTable.Count)
                            {
                                isError = true;
                                MessageBox.Show("Ожидалось: ;");
                                continue;
                            }
                            sdvig();
                        }

                        
                        if (stack.Peek() == ";")
                        {
                            goState(38);
                        }
                        else
                        {
                            messengeShow(";");
                            isError = true;
                            continue;
                        }
                        break;

                    case 35:
                        if (stack.Peek() == ";")
                        {
                            svertka(6, "<присваивание>");
                        }
                        else
                        {
                            messengeShow(";");
                            isError = true;
                            continue;
                        }
                        break;

                    case 37:
                        if (stack.Peek() == ";")
                        {
                            svertka(7, "<условный_оператор>");
                        }
                        else
                        {
                            messengeShow(";");
                            isError = true;
                            continue;
                        }
                        break;

                    case 38:
                        if (stack.Peek() == ";")
                        {
                            svertka(11, "<условный_оператор>");
                        }
                        else
                        {
                            messengeShow(";");
                            isError = true;
                            continue;
                        }
                        break;

                    case 39:
                        if (stack.Peek() == "<действие>")
                        {
                            svertka(3, "<список_действий>");
                        }
                        else
                        {
                            messengeShow("");
                            isError = true;
                            continue;
                        }
                        break;
                }

            }
            if (!isError)
            {
                MessageBox.Show("Без ошибок");
            }
        }

        private void goState(int stateNum)
        {
           // MessageBox.Show("В Гостате приходит " + stateNum);
          
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
               // MessageBox.Show("Удаляет " + sortStack.Peek());
                state = sortStack.Pop();
            }
            

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

        private bool plug()
        {
            bool findOK = false;
            for (int i = step; i < standartTable.Count; i++)
            {
                if (getVerhina(i).Equals("then"))
                {
                    findOK = true;
                    step = i+1;
                    return false;
                }
            }
            if (findOK)
            {
                MessageBox.Show("Не нашлось then");
            }
            return true;
        }
    }
}
