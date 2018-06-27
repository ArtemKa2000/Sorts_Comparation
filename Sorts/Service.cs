using System;


namespace Sorts
{
    class Service //Методи для порівняння ефективності сортувань
    {
        public static int pairCount(int[] mas, int length)//Вираховує, скільки наразі невірних пар у масиві
        {
            int pairCount = 0;//лічильник невірних пар
            for (int i = 0; i < length-1; i++) //ітерація по елементах
            {
                if (mas[i] > mas[i + 1])//якщо поточних елемент більший за наступний, то така пара є невірною
                    pairCount++;//інкремінуємо лічильник
            }
            return pairCount;
        }
        public static int[] reducePair(int[] mas)//Зменшує на 1 кількість невірних пар у масиві
        {
            int startPairCount = pairCount(mas,mas.Length);//початкова кількість невірних пар

            if (startPairCount != 0)//якщо startPairCount==0, то наступний цикл ніколи не заверщиться
                while (pairCount(mas,mas.Length) >= startPairCount)
                {
                    for (int i = mas.Length - 1; i > 0; i--)//якщо прибирати невірни пари з кінця, то це буде вигідніше для сортувань вставками(бінарними вставками)
                    {
                        //міняємо місцями елементи, що утворюють невірну пару. Це може створити інші невірні пари, тому ми робимо це в циклі while який перевіряє, чи виконали ми задачу 
                        if (mas[i] < mas[i - 1])
                        {
                            int temp = mas[i];
                            mas[i] = mas[i - 1];
                            mas[i - 1] = temp;
                            break;
                        }
                    }
                }


            return mas;
        }
        public static int[] CreateMass(int count)//створює випадковий масив з заданою кількістю елементів
        {
            int[] mas = new int[count];

            Random r = new Random();

            for (int i = 0; i < count; i++)
            {
                mas[i] = r.Next(1000);//діапазон не впливає на результати тих розрахунків, які використорують цей метод
            }

            return mas;
        }
        public static int[] CreateMass(int count, int diapazone)//створення випадкового масиву в заданому діапазоні
        {
            //count>=2 завжди
            int[] mas = new int[count];
            Random r = new Random();
            mas[0] = 1;//гарантує мінімальне значення в масиві
            mas[1] = diapazone;//гарантує максимальне значення в масиві
            for (int i = 2; i < mas.Length; i++)//заповнює вільне місце в масиві
            {
                
                mas[i] = r.Next(1,diapazone);
            }

            return mas;
        }
        public static int[] CreateDifferentMass(int elements,int count)//створення масиву з заданою кількістю різноманітних значень
        {
            int[] mas = new int[elements];
            int pos = 0;
            for(int i=0;i<mas.Length;i+=count)
            {
                for(int j=0;j<count && pos<mas.Length; j++)
                {
                    mas[pos] = j;
                    pos++;
                }
            }
            return mas;
        }
        public static int FindEdgeClassic(int sample, int counte)
        {
            /* За умови одного елемента в масиві, сортування підрахунком виконує 3 операції, що >= операціям інших сортувань,
            * а оскільки для 1 елемента діапазон значень змінити неможливо, то необхідна наступна перевірка*/
            if (counte == 1)
                return 0;


            Sort sort = new Sort();
            int Diapazon = 0;//шукана кількість елементів

            //ітеруємось по сотням, доки не привисимо sample
            for (int i = 100; sample >= sort.countSortClassic(CreateMass(counte, i)); i += 100)
            {
                Diapazon = i;
            }

            //ітеруємось по десяткам доки не превисимо sample
            for (int i = Diapazon + 10; sample >= sort.countSortClassic(CreateMass(counte, i)); i += 10)
            {
                Diapazon = i;
            }

            //ітеруємось по одиницям доки не привисимо sample
            for (int i = Diapazon + 1; sample >= sort.countSortClassic(CreateMass(counte, i)); i++)
            {
                Diapazon = i;
            }

            return Diapazon;//пошук залишає операції <=sample
        }
        public static int FindEdgeImproved(int sample, int counte)
        {
            /* За умови одного елемента в масиві, вдосконалене сортування підрахунком виконує 8 операції, що >= операціям інших сортувань,
            * а оскільки для 1 елемента кількість різних значень значень змінити неможливо, то необхідна наступна перевірка*/
            if (counte == 1)
                return 0;


            Sort sort = new Sort();
            int DifferentValues = 0;//шукана кількість елементів

            //ітеруємось по сотням, доки не привисимо sample
            for (int i = 100; sample >= sort.countSortImproved(CreateDifferentMass(counte, i),sample); i += 100)
            {
               
                DifferentValues = i;
            }

            //ітеруємось по десяткам доки не превисимо sample
            for (int i = DifferentValues + 10; sample >= sort.countSortImproved(CreateDifferentMass(counte, i), sample); i += 10)
            {
                
                DifferentValues = i;
            }

            //ітеруємось по одиницям доки не привисимо sample
            for (int i = DifferentValues + 1; sample >= sort.countSortImproved(CreateDifferentMass(counte, i), sample); i++)
            {
                
                DifferentValues = i;
            }

            return DifferentValues;
        }

    }
}
