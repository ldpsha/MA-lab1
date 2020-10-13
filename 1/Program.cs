using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1
{
    class matr
    {
        public const int dim = 23;//максимальная длина числа
        public int[] number = new int[dim];//само число, представлено в виде массива
        int numlength;//его длина
        bool isneg = false;//знак
        matr() { }//создание матрицы, конструктор.
        //ввод числа с клавиатуры
        public void ReadNum()
        {
            int i = 0;
            int buff;
            while (true)//считываем по одному символу
            {
                buff = Console.Read();
                if (buff == 13) break;//enter - пора прекратить считывать
                else if (buff == '-') { this.isneg = true; i--; }//считываем знак
                else this.number[i] = buff - 48;//перевод кодировки
                i++;
            }
            Console.Read();//пропустить переход на новую строку
            //сдвиг в конец массива
            this.numlength = i;
            i--;
            buff = 1;
            while (i >= 0)
            {
                this.number[dim - buff] = this.number[i];
                buff++;
                i--;
            }
            for (i = 0; i < (dim - buff); i++) number[i] = 0;

        }
        //вывод числа
        public void WriteNum()
        {
            for (int i = dim-this.numlength; i < dim; i++) Console.Write(this.number[i]);
            Console.WriteLine();
        }

        public static bool operator >(matr left, matr right)
        {
            bool c = false;
            c =  (left.numlength > right.numlength);//если длина левого больше, то левое больше
            if (!c) c =(left.number[dim - right.numlength] > right.number[dim - right.numlength]);//или они равны по длине, но первая цифра левого больше
            return c;
        }

        public static bool operator <(matr left, matr right)
        {

            bool c = false;
            c = (right.numlength > left.numlength);//если длина правого больше, то левое меньше
            if (!c) c=(right.number[dim - left.numlength] > left.number[dim - left.numlength]);//или они равны по длине, но первая цифра левого меньше
            return c;
        }
        
        //перегрузка +
        public static matr operator +(matr A, matr B)
        {
            matr C= new matr();//результирующий вектор
            int carry = 0;//перенос
            //складываем по разрядам с учетом переноса
            for (int i=dim-1; i>0; i--)
            {
                C.number[i] = (A.number[i] + B.number[i]+carry) % 10;
                carry = (A.number[i] + B.number[i] + carry) / 10;
            }
            //вычисляем длину получившегося числа
            int len = 0;
            while (len < dim && (C.number[len] == 0)) len++;
            C.numlength = dim-len;
            return C;
        }
        //перегрузка -
        public static matr operator -(matr A, matr B)
        {
            matr C = new matr();//результирующая матрица
            int carry = 0;//флаг переноса
            int i = dim-1;//идем с конца
            while (i >= 0)
            {//если уменьшаемое больше, чем вычитаемое, то вычитаем с учетом переноса
                if (A.number[i] >= B.number[i] + carry)
                {
                    C.number[i] = A.number[i] - B.number[i] - carry;
                    carry = 0;
                }
                else//или занимаем разряд и вычитаем
                {
                    C.number[i] = A.number[i]+10 - B.number[i] - carry;
                    carry = 1;
                }
                i--;
            }
            //вычисляем длину получившегося числа
            int len = 0;
            while (len < dim && (C.number[len] == 0)) len++;
            C.numlength = dim - len;
            return C;
        }
        //перегрузка умножения
        public static matr operator *(matr A, matr B)
        {
            matr C = new matr();//результирующий вектор
            int carry = 0;//перенос
            int k = 0;//сдвиг влево, чтобы писать в нужный разряд результата
            //умножение как в столбик
            for (int i = dim - 1; i > 0; i--)
            {
                for (int j = dim - 1; j > 0; j--)
                {
                    if (j - k >= 0)
                    {
                        C.number[j - k] += (A.number[i] * B.number[j] + carry) % 10;
                        carry = (A.number[i] * B.number[j] + carry) / 10 + C.number[j - k]/10;
                        C.number[j - k] = C.number[j - k] % 10;
                    }
                }
                k++;
            }
            //вычисляем длину получившегося числа
            int len = 0;
            while (len < dim && (C.number[len] == 0)) len++;
            C.numlength = dim - len;
            return C;
        }
        //перегрузка деления
        public static matr operator /(matr A, matr B)
        {
            matr C = new matr();//результирующий вектор
            matr ed = new matr();//единичный вектор
            for (int i = 0; i < dim - 1; i++)//С и единичный пустые
            { ed.number[i] = 0;
                C.number[i] = 0;
            }
            ed.number[dim - 1] = 1;//единичный=00..01
            while (!(A<B))//вычитаем, пока число не закончится и к результату прибавляем 1
            {
                A -= B;
                C += ed;
            }
            //вычисляем длину получившегося числа
            int len = 0;
            while (len < dim && (C.number[len] == 0)) len++;
            C.numlength = dim - len;
            return C;
        }

        static void Main(string[] args)
        {
            //input A
            matr A = new matr();
            Console.WriteLine("Введите первое число");
            A.ReadNum();
            //input B
            matr B = new matr();
            Console.WriteLine("Введите второе число");
            B.ReadNum();
            //выбор операции
            matr Result = new matr();
            Console.WriteLine("Введите операцию или любую букву для завершения программы:");
            bool end = true;
            while (end)
            {
                int buff = Console.Read();
                Console.ReadLine();
                switch (buff)
                {
                    case '>':
                        if (A > B) Console.WriteLine("A>b");
                        else Console.WriteLine("A<=B");
                        break;
                    case '<':
                        if (A < B) Console.WriteLine("A<b");
                        else Console.WriteLine("A>=B");
                        break;
                    case '-':
                        if (A > B)
                            {
                            if ((B.isneg&&A.isneg)|| (!B.isneg && !A.isneg)) Result = A - B;
                            else Result = A + B;
                            Result.isneg = A.isneg;
                            }
                        else
                        {
                            if ((B.isneg && A.isneg) || (!B.isneg && !A.isneg))
                            { Result = B - A;
                              Result.isneg = !B.isneg;
                            }
                            else
                            {
                                Result = B + A;
                                Result.isneg = B.isneg;
                            }
                        }
                        Console.WriteLine("Результат");
                        if (Result.isneg) Console.Write("-");
                        Result.WriteNum();
                        break;
                    case '+':
                        if (A > B)
                        {
                            if ((B.isneg && A.isneg) || (!B.isneg && !A.isneg)) Result = A + B;
                            else Result = A - B;
                            Result.isneg = A.isneg;
                        }
                        else
                        {
                            if ((B.isneg && A.isneg) || (!B.isneg && !A.isneg))
                            {
                                Result = B + A;
                                Result.isneg = B.isneg;
                            }
                            else
                            {
                                Result = B - A;
                                Result.isneg = B.isneg;
                            }
                        }
                        Console.WriteLine("Результат");
                        if (Result.isneg) Console.Write("-");
                        Result.WriteNum();
                        break;
                    case '*':
                        Result = A * B;
                        if ((B.isneg && A.isneg) || (!B.isneg && !A.isneg))Result.isneg = false;//-*- или +*+ это плюс
                        else Result.isneg = true;
                        Console.WriteLine("Результат");
                        if (Result.isneg) Console.Write("-");
                        Result.WriteNum();
                        break;
                    case '/':
                        Result = A / B;
                        if ((B.isneg && A.isneg) || (!B.isneg && !A.isneg)) Result.isneg = false;//-*- или +*+ это плюс
                        else Result.isneg = true;
                        Console.WriteLine("Результат");
                        if (Result.isneg) Console.Write("-");
                        Result.WriteNum();
                        break;
                    default:
                        end = false;
                        break;
                }
            }
            Console.WriteLine("Конец программы");
            Console.ReadLine();
        }
    }
}
