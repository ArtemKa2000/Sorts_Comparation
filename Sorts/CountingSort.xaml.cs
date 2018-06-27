using System;
using System.Windows;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Sorts
{
    /// <summary>
    /// Логика взаимодействия для CountingSort.xaml
    /// </summary>
    public partial class CountingSort : Window
    {
        Menu parent;
        public CountingSort(Menu p)
        {
            InitializeComponent();
            parent = p;

            chart.ChartAreas[0].AxisX.Minimum = 0;
            chart.ChartAreas[0].AxisY.Minimum = 0;
            chart.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Far;
            chart.ChartAreas[0].AxisY.TitleAlignment = StringAlignment.Far;
            chart.Legends[0].Font = new Font("Times New Roman", 18);//шрифт легенди
            chart.ChartAreas[0].AxisX.TitleFont = new Font("Times New Roman", 12); //Задаємо шрифт
            chart.ChartAreas[0].AxisY.TitleFont = new Font("Times New Roman", 12);
            chart.ChartAreas[0].AxisX.Title = "Кількість елементів";
            chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;//вимикаємо сітку
            chart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart.Series.Add("Верхня межа ефективності сортування підрахунком");//додаємо нову серію даних
            chart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;//встановлюємо тип графіку
        }


        private void Button_CountingSort_Click(object sender, RoutedEventArgs e)
        {
            int elementCount;//максимальна кількість елементів
            int SortCheckMarker = cb_Type.SelectedIndex;//Визначення сортування, з яким буде проведене порівняння
            Color[] color = new Color[] { Color.Red, Color.Purple};//зберігає 3 кольори для графіків


            try//Якщо користувач не вкаже кількість елементів, буде виведено повідомлення про помилку
            {
                elementCount = int.Parse(tb_elementCount.Text);


                Sort sorts = new Sort();//Створення об'єкту який який буде проводити сортування
                Coordinates cords = new Coordinates(elementCount);//Створення об'єкту який буде зберігати отримані дані
                if(SortCheckMarker==0)
                 for (int i = 1; i <= elementCount; i++)
                 {
                    cords.x[i - 1] = i;//Заповнюємо значення х
                                
                    sorts.copyMas(Service.CreateMass(i));
                    sorts.QSort(0, i - 1);//Еталон

                    cords.y[i - 1] = Service.FindEdgeClassic(sorts.qComparation, i);//порівняння ведеться за загальною кількістю операцій
                       
                    sorts.qComparation = 0;
                    sorts.qInversin = 0;

                    }
                else//SortCheckMarker==1
                    for (int i = 1; i <= elementCount; i++)
                    {
                        cords.x[i - 1] = i;//Заповнюємо значення х
                        int[] temp = Service.CreateDifferentMass(i,i);//створення масиву з усіма унікальними значеннями, який повністю впорядковано
                        sorts.copyMas(temp);
                        sorts.QSort(0, i-1);//Еталон

                        
                        cords.y[i - 1] = Service.FindEdgeImproved(sorts.qComparation, i);//порівняння ведеться за загальною кількістю операцій
                        
                        sorts.qComparation = 0;
                        sorts.qInversin = 0;

                    }


                //Налаштування Chart
                chart.Series[0].Points.Clear();
                chart.ChartAreas[0].AxisX.Interval = Math.Round((double)elementCount / 10);//Максимальне значення осі x дорівнює максимальній кількості елементів
                chart.ChartAreas[0].AxisY.Interval = Math.Round((double)cords.y[elementCount-1] / 8);//Максимальне значення по осі y буде у останньому елементі cords.y
                chart.ChartAreas[0].AxisX.Maximum = elementCount;//встановлюємо параметри осей

                
                if (SortCheckMarker == 0)
                {
                    chart.Series[0].Color = color[1];//встановлюємо колір графіку
                    chart.ChartAreas[0].AxisY.Title = "Діапазон значень";
                }
                else
                {
                    chart.Series[0].Color = color[0];
                    chart.ChartAreas[0].AxisY.Title = "Кількість різних значень";
                }



                chart.Series[0].Points.AddXY(0, 0);//початкова точка(отримати ці данні з розрахунків проблематично)
                for (int i = 0; i < elementCount; i++)//виводимо дані
                {
                    chart.Series[0].Points.AddXY(cords.x[i], cords.y[i]);
                }


            }
            catch (Exception ex)//Користувач не вказав кількість елементів для розрахунку
            {
                MessageBox.Show("Помилка у вхідних даних",ex.ToString());//Повідомлення про помилку
            }
        }


        private void button_Back_Click(object sender, RoutedEventArgs e)//Повердаємось до головного меню
        {
            this.Hide();
            parent.Show();
        }

        private void Window_Closed(object sender, EventArgs e)//Повердаємось до головного меню
        {
            Application.Current.Shutdown();
        }

        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            try//Може ще не бути жодної створеної серії
            {
                chart.Series[0].Points.Clear();
            }
            catch { }
        }

        private void cb_Type_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            tb_DifferentValues.Width = cb_Type.ActualWidth;
        }
    }
}
