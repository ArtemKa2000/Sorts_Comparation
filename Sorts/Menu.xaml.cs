using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sorts
{
    /// <summary>
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
            sortsStatictic = new SortsStatistics(this);
            wrongPairs = new WrongPair(this);
            countingSorts = new CountingSort(this);
            theorys = new Theory(this);
            visualisations = new Visualization(this);
        }
        //імена не співпадають закінченнями з назвою класів аби запобігти конфлікту імен(всупереч різному за регістрами написанню можливі конфлікти)
        SortsStatistics sortsStatictic;
        WrongPair wrongPairs;
        CountingSort countingSorts;
        Theory theorys;
        Visualization visualisations;

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;//повераємо об'єкт до його початковго типу
            tb.FontSize +=4;//збільшуємо шрифт
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;//повертаємо об'єкт до його початкого типу
            tb.FontSize -= 4;//повертаємо початковий розмір шрифту
        }


        private void tb_SortsStatistics_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            sortsStatictic.Show();
        }

        private void tb_WrongPair_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            wrongPairs.Show();
        }

        private void tb_CountingSort_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            countingSorts.Show();
        }

        private void tb_Theory_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            theorys.Show();
        }

        private void tb_Visualization_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            visualisations.Show();
        }

        //Наступні 2 методи вимикають додаток при 2 різних сценаріях
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
