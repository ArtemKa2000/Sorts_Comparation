using System;
using System.Windows;
using System.IO;
using System.Drawing;

namespace Sorts
{
    /// <summary>
    /// Логика взаимодействия для SortsStatistics.xaml
    /// </summary>
    public partial class SortsStatistics : Window
    {
        Menu parent;
        public SortsStatistics(Menu p)//повинен отримати об'єкт меню для повернення до нього 
        {
            InitializeComponent();
            parent = p;

            //Налаштування Chart
            chart.ChartAreas[0].AxisX.Minimum = 0;//мінімальні значення осей
            chart.ChartAreas[0].AxisY.Minimum = 0;
            chart.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Far;//вирівнювання підпису осей
            chart.ChartAreas[0].AxisY.TitleAlignment = StringAlignment.Far;
            chart.Legends[0].Font =new Font("Times New Roman", 18);//шрифт легенди
            chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;//вимикаємо сітку
            chart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart.ChartAreas[0].AxisX.Title = "Сусідні невпорядковані пари";//задаємо підписи координатних осей
            chart.ChartAreas[0].AxisX.TitleFont = new Font("Times New Roman", 12); //Задаємо шрифт
            chart.ChartAreas[0].AxisY.Title = "Кількість операцій";
            chart.ChartAreas[0].AxisY.TitleFont = new Font("Times New Roman", 12);
            chart.Series.Add("Вставки");//створюємо серії 3-х графіків
            chart.Series.Add("Бінарні вставки");
            chart.Series.Add("QSort");
            chart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;//задаємо тип графіку
            chart.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart.Series[2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart.Series[0].Color = Color.Red;//задаємо колір графіків
            chart.Series[1].Color = Color.Green;
            chart.Series[2].Color = Color.Purple;
        }


        int[] date;//зразок масива, який буде копіюватись для сортувань
        private void Button_SortsStatistics_Click(object sender, RoutedEventArgs e)
        {
            //визначаємо обраний радіо-баттон. За замовчуванням обрану перший(з індексом 0).
            int CheckMarker = cb_CompareType.SelectedIndex;

            try
            {
                if (date == null)//Якщо користувач ще жодного разу не ввів дані
                    button_DataUpdate_Click(button_DataUpdate, new RoutedEventArgs());

                int[] mas = new int[date.Length];
                date.CopyTo(mas,0);
                int count = Service.pairCount(mas, mas.Length);//визначаємо початкову кількість невірних пар

                /* 
                 * Створюємо об'єкти для зберігання даних
                 * Значення +1, бо ми враховуємо випадок з 0 невірних пар, тобто коли масив буде повністю впорядкований
                */
                Coordinates cords1 = new Coordinates(count + 1);//ефективність вставок
                Coordinates cords2 = new Coordinates(count + 1);//ефективність бінарних вставок
                Coordinates cords3 = new Coordinates(count + 1);//ефективність QSort
                Sort sorts = new Sort();//створюємо об'єкт який буде виконувати сортування

                /* Збираємо дані
                 * x-невірні пари, y-кількість */
                for (int i = count; i >= 0; i--)//Рух у зворотньому порядку, бо кількість невірних пар легше зменшувати
                {
                    //Тимчасово зберігають виконані операції
                    int[] counters1 = null;//Вставки
                    int[] counters2 = null;//Бінарні вставки

                    //Зберігаємо поточну кількість невірних пар
                    cords1.x[i] = i;
                    cords2.x[i] = i;
                    cords3.x[i] = i;

                    sorts.copyMas(mas);//завантажує наш масив в об'єкт для обробки його різними сортуваннями
                    //Виконуємо 3 види різних сортувань і зберігаємо результати
                    counters1 = sorts.Insert();
                    counters2 = sorts.binInsert();
                    sorts.QSort(0, mas.Length - 1);

                    //ініціалізація у в залежності від обраних radioButton-ів
                    if (CheckMarker == 0)//порівняння виконується за усіма видами операцій(порівняння+обміни)
                    {
                        cords1.y[i] = counters1[0] + counters1[1];
                        cords2.y[i] = counters2[0] + counters2[1];
                        cords3.y[i] = sorts.qComparation + sorts.qInversin;
                    }
                    else
                         if (CheckMarker == 1)//порівняння виконується лише за порівняннями
                    {
                        cords1.y[i] = counters1[0];
                        cords2.y[i] = counters2[0];
                        cords3.y[i] = sorts.qComparation;
                    }
                    else
                        if (CheckMarker == 2)//порівняння виконується лише за обмінами
                    {
                        cords1.y[i] = counters1[1];
                        cords2.y[i] = counters2[1];
                        cords3.y[i] = sorts.qInversin;
                    }

                    //Необхідно обнулити попередні результати роботи QSort
                    sorts.qComparation = 0;
                    sorts.qInversin = 0;

                    Service.reducePair(mas);//зменшує на 1 кількість невірних пар
                }

                //Налаштування Chart
                chart.ChartAreas[0].AxisX.Interval = Math.Round((double)count / 10);//Максимальне значення осі x дорівнює максимальній кількості елементів
                chart.ChartAreas[0].AxisY.Interval = Math.Round((double)cords1.y[count] / 8);//Максимальне значення по осі y буде у останньому елементі cords1.y(кількість операцій сортування вставками)
                chart.Series[0].Points.Clear();//очищуємо робочу поверхню.
                chart.Series[1].Points.Clear();
                chart.Series[2].Points.Clear();
                chart.ChartAreas[0].AxisX.Maximum = count;//обмежуємо максимум, бо chart робить вісь х довше на 1 пунк ніж потрібно


                //Побудова графіку за отриманими даними(послідовно додаємо точки графіків до відповідних серій)
                for (int i = 0; i < cords1.x.Length; i++)//length однакова у всіх масивів - можемо обрати будь-яку
                {
                    chart.Series[0].Points.AddXY(cords1.x[i], cords1.y[i]);
                    chart.Series[1].Points.AddXY(cords2.x[i], cords2.y[i]);
                    chart.Series[2].Points.AddXY(cords3.x[i], cords3.y[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Введіть коректні дані.", ex.ToString());
            }
        }


        private void button_DataUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            { //Якщо користувач вказав кількість елементів для збору статистики
                int elements = int.Parse(tb_elementCount.Text);

                if(elements!=0)//опрацьовувати данні можна лише якщо кількість елементів не == 0
                    date = Service.CreateMass(elements);
                else
                    MessageBox.Show("Введіть іншу кількість елементів");//Була вказана нульова довжина масиву
            }
            catch (Exception ex)
            {   //Якщо користувач не вказав кількість елементів для збору статистики
                MessageBoxResult result = MessageBox.Show("Кількість елементів не коректна. Зчитати масив з фалу?", ex.ToString(), MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    StreamReader sr = new StreamReader("mas.txt");
                    string[] mass = sr.ReadLine().Split(' ');
                    date = new int[mass.Length];

                    for (int i = 0; i < mass.Length; i++)
                    {
                        date[i] = int.Parse(mass[i]);
                    }
                    sr.Close();
                }
                else //якщо користувач не погодився зчитувати результат з файлу
                {
                    chart.Series[0].Points.Clear();//очищуємо робочу поверхню.
                    chart.Series[1].Points.Clear();
                    chart.Series[2].Points.Clear();
                    return;
                }
            }
        }

        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            chart.Series[0].Points.Clear();//очищуємо робочу поверхню.
            chart.Series[1].Points.Clear();
            chart.Series[2].Points.Clear();
        }

        private void button_Back_Click(object sender, RoutedEventArgs e)//повертаємось до головного меню
        {
            this.Hide();
            parent.Show();  
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
