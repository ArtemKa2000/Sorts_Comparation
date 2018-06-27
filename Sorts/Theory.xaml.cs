using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using System.IO;

namespace Sorts
{
    /// <summary>
    /// Логика взаимодействия для Theory.xaml
    /// </summary>
    public partial class Theory : Window
    {
        Menu parent;
        public Theory(Menu p)
        {
            InitializeComponent();
            parent = p;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void button_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            parent.Show();
        }
        FileStream lastStream;
        private void Node_Selected(object sender, RoutedEventArgs e)
        {
            if (lastStream != null)
                lastStream.Close();
                string s = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string path= s.Substring(0, s.LastIndexOf('\\')+1) + "Information\\" + ((TreeViewItem)sender).Name.Split('_')[1] + ".html";
                lastStream = new FileStream(path, FileMode.Open);
                webBrowser.NavigateToStream(lastStream);
        }
    }
}
