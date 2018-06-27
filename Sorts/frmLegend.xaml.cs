using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sorts
{
    /// <summary>
    /// Логика взаимодействия для frmLegend.xaml
    /// </summary>
    public partial class frmLegend : Window
    {
        Color[] colors;
        public frmLegend(Color[] c)
        {
            InitializeComponent();
            colors = c;
            First();
            Second();
            Third();
        }

        private void First()
        {
            R1.Fill = new SolidColorBrush(colors[1]);
            R2.Fill = new SolidColorBrush(colors[3]);
            R3.Fill = new SolidColorBrush(colors[0]);
            R4.Fill = new SolidColorBrush(colors[2]);
        }
        private void Third()
        {
            l1.Stroke = new SolidColorBrush(colors[2]);
            B1.BorderBrush = new SolidColorBrush(colors[3]);
        }
        private void Second()
        {
            R5.Fill = new SolidColorBrush(colors[1]);
            R6.Fill = new SolidColorBrush(colors[3]);
            R7.Fill = new SolidColorBrush(colors[2]);
            R8.Fill = new SolidColorBrush(colors[4]);
            R8.Fill.Opacity = 0.5;
            R9.Fill = new SolidColorBrush(colors[0]);
            R9.Fill.Opacity = 0.5;
            R10.Fill = new SolidColorBrush(colors[5]);
            R10.Fill.Opacity = 0.5;
            R11.Fill = new SolidColorBrush(colors[6]);
        }
    }
}
