using System;
using System.Collections.Generic;
using System.Linq;
namespace Sorts
{
    public class Sort
    {

        private int[] mas1;//масив для Insert()(сортування вставками)
        private int[] mas2;//масив для binInsert()(сортування бінарними вставками)
        private int[] mas3;//масив для quickSort(l,r,average)(QSort)


        //Дані операцій QSort
        private  int Comparation = 0;//порівняння QSort
        public int qComparation//властивість для роботи з Comparation
        {
            get { return Comparation; }
            set { Comparation = 0; } //передача значенняя відмінного від 0 - помилка
        }

        private int Inversin = 0;//обміни QSort
        public int qInversin//властивість для роботи з Inversin
        {
            get { return Inversin; }
            set { Inversin = 0; } //передача значенняя відмінного від 0 - помилка
        }



        public int[] Insert()//Сортування вставками
        {
            //Лічильники виконаних операцій
            int compareCount = 0;//порівняння
            int inversionCount = 0;//зміни в масиві

            for (int i = 1; i < mas1.Length; i++)//знаходимо місце для вставки кожного елементу
            {
                if (mas1[i] < mas1[i - 1])//якщо j+1==i, то ми повертаємо елемент на початкову позицію і робимо зайву операцію
                {
                    int temp = mas1[i];//поточне значення для вставки
                    mas1[i] = mas1[i - 1];
                    inversionCount+=2;
                    int j = i - 2;//індекс елемента в масиві з яким буде вестися порівняння

                    while (j >= 0 && mas1[j] > temp)//"проштовхуємо" елемент до початку масиву, доки він не буде на своїй позиції
                    {
                        //зсуваємо поточне значення на 1 позицію праворуч
                        mas1[j + 1] = mas1[j];
                        j--;
                        compareCount++;
                        inversionCount++;
                    }

                    compareCount++;     //цикл припинив роботу, отже порівняння у while завершилось помилкою
                    mas1[j + 1] = temp;//вставляємо наш елемент на знайдену позицію
                    inversionCount++;
                }
                compareCount++;
            }

            return new int[] { compareCount, inversionCount };
        }
        public  int[] binInsert()//Сортування бінарними вставками
        {
            //Лічильники виконаних операцій
            int compareCount = 0;//порівняння
            int inversoinCount = 0;//зміни в масиві
            for (int i = 1; i < mas2.Length; i++)
            {
                if (mas2[i]<mas2[i-1])//якщо mas2[i]<mas2[i-1] то елемент стоїть на своєму місці
                {
                 compareCount++;
                 int left = 0;//ліва межа
                 int right = i - 2;//права межа
                 int numToInsert = mas2[i];
                 inversoinCount++;

                //Знаходимо місце для вставки шляхом бінарного пошуку
                 while (left <= right)//виконуємо дії доки межі не розійшлись
                 {
                    int middle = (left + right) / 2;//знаходимо центральний елемент з вже відсортованих
                    if (numToInsert >= mas2[middle])//Якщо елемент для вставки більший за middle, далі працюємо з правою половиною відсортованого масиву
                    {
                        left = middle + 1;//зсуваємо ліву межу
                        compareCount++;
                    }
                    if (numToInsert < mas2[middle])//Якщо елемент для вставки менший за middle, далі працюємо з лівою половиною відсортованого масиву
                    {
                        right = middle - 1;//зсуваємо праву межу
                        compareCount++;
                    }
                    
                  }
                 // зсуваємо елементи праворуч на 1 позицію
                 for (int j = i; j > left; j--)
                 {
                     mas2[j] = mas2[j - 1];
                     inversoinCount++;
                 }
                  mas2[left] = numToInsert;
                  inversoinCount++;
               }
                compareCount++;
            }
            return new int[] { compareCount, inversoinCount };
        }
        public int countSortClassic(int[] mas)
        {
            int max = mas[0];
            int min = mas[0];
            int operation = 0;
            for (int i = 1; i < mas.Length; i++)//шукаємо мінімальне та максимальне значення
            {
                if (mas[i] > max)
                    max = mas[i];
                if (mas[i] < min)
                    min = mas[i];
            }


            //Сортування
            int[] temp = new int[max - min + 1];//створюємо службовий масив довжиною рівною діапазону значень

            //При початковому масиві [1,1,3,3,5,5] службовий масив заповнюється так: [0,2,0,2,0,2]
            for (int i = 0; i < mas.Length; i++)//підраховуємо кількість кожного з елентів в масиві
            {
                temp[mas[i] - min]++;//інкремінуємо відповідний елемент службового масиву
                operation++;
            }
            int pos = 0;//маркує позицію, на яку потрібно ставити поточне число
            for (int i = 0; i < temp.Length; i++)//ітеруємось по службовому масиву
            {

                    operation++;
                for (int j = 0; j < temp[i]; j++)//вставляє в початковий масив поточне число відповідну кількість разів
                {
                    mas[pos] = i + min;//відновлюємо справжнє значення
                    pos++;//зсуваємо позицію для наступного вставлення на 1
                    operation++;
                }
            }

            return operation;
        }
        public int countSortImproved(int[] mas,int sample)
        {

            int operation = 0;
            Dictionary<int, int> values = new Dictionary<int, int>();
            //При початковому масиві [1,1,3,3,5,5] службовий масив заповнюється так: [0,2,0,2,0,2]
            for (int i = 0; i < mas.Length; i++)//підраховуємо кількість кожного з елентів в масиві
            {
                if(values.ContainsKey(mas[i]))
                    values[mas[i]] = values[mas[i]] + 1;
               else
                    values.Add(mas[i], 1);

                operation++;
            }

            //наступна група операцій не має безпосереднього відношення до сортування, тому ці операції не враховуються
            int pos = 0;//маркує позицію, на яку потрібно ставити поточне число
            mas3 = new int[values.Count];
            foreach (int i in values.Keys)
            {
                mas3[pos] = i;
                pos++;
            }
            QSort(0, mas3.Length - 1);
            operation += qComparation;
            operation += qInversin;
            qInversin = 0;
            qComparation = 0;

            pos = 0;
            for (int i = 0; i < mas3.Length; i++)//ітеруємось по службовому масиву
            {
                operation++;
                for (int j = 0; j < values[mas3[i]]; j++)//вставляє в початковий масив поточне число відповідну кількість разів
                {
                    mas[pos] = mas3[i];//відновлюємо справжнє значення
                    pos++;//зсуваємо позицію для наступного вставлення на 1
                    operation++;
                }
            }

            return operation;
        }



        public void QSort(int l, int r)//метод-прокладка для виклику quickSort(l,r,m).Шукає середнє значення для запобігання надмірної рекурсії
        {
            int min = mas3[l];
            int max = mas3[r];

            for (int i = l; i <= r; i++)//пошук максимального і мінімального значень
            {
                if (min > mas3[i])
                    min = mas3[i];
                if (max < mas3[i])
                    max = mas3[i];
            }

            quickSort(l, r, (min + max) / 2);//виклик справжнього QSort();

        }
        private void quickSort(int left, int right, int average)
        {
            /*
             * l,r - визначають межі масиву, з якими наразі йде сортування; 
             * average - середнє значення елементів в масиві, служить для запобігання надмірної рекурсії і переповнення стеку
            */

            //робимо копії переданих меж, для операцій з ними
            int i = left;
            int j = right;

            while (i <= j)
            {
                while (mas3[i] < average) { i++; Comparation++; }//рухаємо ліву межу праворуч, доки не знайдемо елемент >= average
                while (mas3[j] > average) { j--; Comparation++; }//рухаємо праву межу ліворуч, доки не знайдемо елемент <= за average
                Comparation += 2;//2 цикли завершились помилкою.

                if (i <= j)//Якщо межі не розійшлись, то міняємо елементі місцями та зсуваємо межі у відповідних напрямках
                {
                    int temp = mas3[i];
                    mas3[i] = mas3[j];
                    mas3[j] = temp;
                    i++;
                    j--;
                    Inversin+=3;
                }

            }
            if (i < right)//Якщо права частина складається не менш як з 2 елементів, сортуємо її
                QSort(i, right);

            if (left < j)//Якщо ліва частина складається не менш як з 2 елементів, сортуємо її
                QSort(left, j);
        }

        public void copyMas(int[] mas)//копіює поточний масив у змінні даного об'єкту, для подальшої роботи сортувань з ними.
        {
            mas1 = new int[mas.Length];
            mas2 = new int[mas.Length];
            mas3 = new int[mas.Length];
            for (int i = 0; i < mas.Length; i++)
            {
                mas1[i] = mas[i];
                mas2[i] = mas[i];
                mas3[i] = mas[i];
            }
        }

    }
}
