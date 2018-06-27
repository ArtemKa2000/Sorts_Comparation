using System;
using System.Windows;
using System.Drawing;


namespace Sorts
{
    /// <summary>
    /// Логика взаимодействия для WrongPair.xaml
    /// </summary>
    public partial class WrongPair : Window
    {
        Menu parent;

        public WrongPair(Menu p)
        {
            InitializeComponent();
            parent = p;

            //Налаштування графіку
            chart.ChartAreas[0].AxisX.Minimum = 0;//мінімальні значення осей
            chart.ChartAreas[0].AxisY.Minimum = 0;
            chart.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Far;//вирівнювання підпису осей
            chart.ChartAreas[0].AxisY.TitleAlignment = StringAlignment.Far;
            chart.Legends[0].Font = new Font("Times New Roman", 18);//шрифт легенди
            chart.Series.Add("Сусідні невпорядковані пари");//Створюємо серію даних
            chart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;//обираємо тип графіку
            chart.Series[0].Color = Color.Green;//Встановлюємо колір графіку
            chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;//Вимикаємо сітку
            chart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart.ChartAreas[0].AxisY.Title = "Сусідні невпорядковані пари";//Задаємо підписи координатних осей
            chart.ChartAreas[0].AxisX.Title = "Кількість елементів";
            chart.ChartAreas[0].AxisX.TitleFont = new Font("Times New Roman", 12); //Задаємо шрифт
            chart.ChartAreas[0].AxisY.TitleFont = new Font("Times New Roman", 12);
        }


        private void Button_WrongPair_Click(object sender, RoutedEventArgs e)
        {
            int counte;//кількість елментів
            int countm;//Кількість масивів, за якими будуть вираховуватись середні данні
            try //Якщо користувач не вкаже потрібні дані, буде виведено повідомлення про помилку
            {
                counte = int.Parse(tb_elementCount.Text);//введення кількості елементів
                countm = int.Parse(tb_WrongPairAccuracy.Text);//вводимо точність розрахунку

                int[] y = new int[counte + 1];
                
                for (int i = 0; i < countm; i++)
                {
                    int[] temp = Service.CreateMass(counte);
                    for(int j=0;j<=counte;j++)
                    {
                        y[j] += Service.pairCount(temp, j);
                    }
                }

                //Налаштування Chart
                chart.Series[0].Points.Clear();
                chart.ChartAreas[0].AxisX.Interval = Math.Round((double)counte / 10);//Максимальне значення осі x дорівнює максимальній кількості елементів
                chart.ChartAreas[0].AxisY.Interval = Math.Round((double)y[counte] / (8*countm));//Максимальне значення по осі y буде у останньому елементі cords.y

                //Побудова графіку за отриманими даними(послідовно додаємо точки графіку)
                for (int i = 0; i <=counte; i++)
                {
                    chart.Series[0].Points.AddXY(i, Math.Round((double)y[i]/countm));
                }
            }
            catch (Exception ex)//Користувач не вказав потрібні дані
            {
                MessageBox.Show("Заповніть коректно поля", ex.ToString());//Повідомлення про помилку
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
            chart.Series[0].Points.Clear();
        }
    }
}
