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
using System.Windows.Media.Animation;
using System.IO;
using System.Windows.Threading;
using System.Threading;
namespace Sorts
{
    /// <summary>
    /// Логика взаимодействия для Visualization.xaml
    /// </summary>
    public partial class Visualization : Window
    {
        Menu parent;
        public Visualization(Menu p)
        {
            InitializeComponent();
            parent = p;
        }

        //КЕРУЮЧА ЧАСТИНА
        int[] data;
        int totalTime;
        Storyboard storyboard;
        string fontName = "Times New Roman";
        static Color[] colors = new Color[] { Colors.Black, Colors.Blue, Colors.Red, Colors.MediumSeaGreen,Colors.Gold, Colors.Chartreuse,Colors.Maroon };
        frmLegend legend = new frmLegend(colors);
        private void button_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            parent.Show();
        }

        private void button_DataUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int elements = int.Parse(tb_elementCount.Text);
                if (elements < 2)
                    throw new Exception();
                data = Service.CreateMass(elements, int.Parse(tb_Diapazon.Text));
            }
            catch (Exception ex)
            {
                MessageBoxResult result = MessageBox.Show("Невірно заповнені поля.Зчитати дані з файлу?", ex.ToString(), MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    StreamReader sr = new StreamReader("mas.txt");
                    string[] mass = sr.ReadLine().Split(' ');
                    data = new int[mass.Length];

                    for (int i = 0; i < mass.Length; i++)
                    {
                        data[i] = int.Parse(mass[i]);
                    }
                    sr.Close();
                }
                else
                {
                    MessageBox.Show("Будуть сгенеровані випадкові дані.");
                    data = new int[10];
                    Random r = new Random();
                    for (int i = 0; i < 10; i++)
                        data[i] = r.Next(10);
                }
            }
        }

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            if (data == null || Service.pairCount(data, data.Length) == 0)
                button_DataUpdate_Click(button_DataUpdate, new RoutedEventArgs());

            if (timer != null || storyboard != null)
                Button_Stop_Click(new object(), new RoutedEventArgs());

            switch (cb_CompareType.SelectedIndex)
            {
                case 0: StartInsert(); break;
                case 1: StartQSort(); break;
                case 2: StartCounting(); break;
            }
        }

        private void Button_Stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                storyboard.Stop(this);
                timer.Stop();
            }
            catch (Exception)
            {

            }
        }

        private void Button_Legend_Click(object sender, RoutedEventArgs e)
        {
            legend.ShowDialog();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }





        //СОРТУВАННЯ ВСТАВКАМИ
        int width;
        int labelHeight;
        Element[] masE;
        Label counterlb;
        Label inversionlb;
        List<int> operation;
        public void StartInsert()//ініціалізація об'єкту
        {
            width = (int)((can_Graphics.ActualWidth - 10) / data.Length);//вираховуємо ширину елементів
            Button_Stop_Click(this, new RoutedEventArgs());//якщо була стара анімація
            storyboard = new Storyboard();//Групуємо анімацію
            storyboard.Children = new TimelineCollection();
            operation = new List<int>();

            can_Graphics.Children.Clear();//видаляємо старі єлемненти
            masE = new Element[data.Length];

            int max = data[0];
            for (int i = 1; i < masE.Length; i++)//пошук максимального значення
                if (data[i] > max)
                    max = data[i];

            NameScope.SetNameScope(this, new NameScope());
            double scale = (can_Graphics.ActualHeight / 2) / max;//визначення масштабу


            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Height = can_Graphics.ActualHeight / 10 - 10;
            can_Graphics.Children.Add(panel);
            Canvas.SetTop(panel, 5);
            Canvas.SetLeft(panel, 5);

            counterlb = new Label();
            counterlb.Content = "Порівнянь виконано: 0";
            counterlb.FontFamily = new FontFamily(fontName);
            counterlb.FontSize = can_Graphics.ActualHeight / 20;
            counterlb.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            counterlb.VerticalContentAlignment = VerticalAlignment.Center;
            counterlb.VerticalAlignment = VerticalAlignment.Stretch;
            panel.Children.Add(counterlb);

            inversionlb = new Label();
            inversionlb.Content = "Обмінів виконано: 0";
            inversionlb.FontFamily = new FontFamily(fontName);
            inversionlb.FontSize = can_Graphics.ActualHeight / 20;
            inversionlb.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            inversionlb.VerticalContentAlignment = VerticalAlignment.Center;
            inversionlb.VerticalAlignment = VerticalAlignment.Stretch;
            panel.Children.Add(inversionlb);


            labelHeight = (int)(width / 2 * 1.328);
            for (int i = 0; i < masE.Length; i++)
            {
                masE[i] = new Element();
                masE[i].value = data[i];
                masE[i].container = new Grid();
                masE[i].container.RowDefinitions.Add(new RowDefinition());
                masE[i].container.RowDefinitions.Add(new RowDefinition());
                masE[i].rectangle = new Rectangle();
                masE[i].rectangle.Height = data[i] * scale;
                masE[i].rectangle.Width = width;
                SolidColorBrush tempBrush = new SolidColorBrush(colors[1]);
                masE[i].rectangle.Fill = tempBrush;
                masE[i].container.Children.Add(masE[i].rectangle);

                Label lab = new Label();
                lab.Content = data[i];
                lab.FontFamily = new FontFamily(fontName);
                lab.FontSize = width / 2;
                lab.HorizontalContentAlignment = HorizontalAlignment.Center;
                masE[i].container.Children.Add(lab);
                Grid.SetRow(lab, 1);
                Grid.SetRow(masE[i].rectangle, 0);
                masE[i].brush = "Brush" + i;
                RegisterName(masE[i].brush, tempBrush);
                can_Graphics.Children.Add(masE[i].container);
                Canvas.SetLeft(masE[i].container, width * i + 5);
                Canvas.SetBottom(masE[i].container, can_Graphics.ActualHeight / 4 - labelHeight);
            }
            InsertSort();
        }
        private void InsertSort()//Сортування включеннями
        {
            totalTime += 500;
            for (int i = 1; i < masE.Length; i++)//знаходимо місце для включення кожного елементу
            {
                TrueCompare(masE[i]);
                operation.Add(0);
                operation.Add(0);

                if (masE[i].value < masE[i - 1].value)
                {
                    Element temp = masE[i];//поточне значення для включення
                    masE[i] = masE[i - 1];

                    Compare(masE[i - 1]);
                    operation.Add(1);
                    operation.Add(0);
                    operation.Add(0);
                    operation.Add(0);


                    MoveUP(temp);//висуваємо елемент вгору
                    operation.Add(2);
                    operation.Add(0);

                    MoveRight(masE[i-1], i * width + 5);//пересуваємо елемент
                    operation.Add(2);

                    int j = i - 2;//індекс елемента в масиві з яким буде вестися порівняння

                    while (j >= 0 && masE[j].value > temp.value)//"проштовхуємо" елемент до початку масиву, доки він не буде на своїй позиції
                    {
                        masE[j + 1] = masE[j];//зсуваємо поточне значення на 1 позицію праворуч

                        //Візуализуємо
                        Compare(masE[j]);//помічаємо візуально елемент, з яким було порівняння(той самий об'єкт знаходиться і у rectangles[j+1])
                        operation.Add(1);
                        operation.Add(0);
                        operation.Add(0);
                        operation.Add(0);
                        MoveRight(masE[j], (j + 1) * width + 5);//пересуваємо елемент
                        operation.Add(2);
                        j--;
                    }
                    if (j >= 0)//якщо існує попередній елемент - j
                    {
                        FalseCompare(masE[j]);
                        operation.Add(1);
                        operation.Add(0);

                    }

                    masE[j + 1] = temp;//вставляємо наш елемент на знайдену позицію

                    //Візуалізуємо
                    MoveBack(temp, (j + 1) * width + 5);//вставляємо елемент на знайдену позицію 
                    operation.Add(2);
                    operation.Add(0);

                    if(j>=0)
                    {
                        BackColor(masE[j]);//повертаємо основний колір, якщо такий елемент існує
                        operation.Add(0);
                        operation.Add(0);
                    }
                }
                else
                {
                    FalseCompare(masE[i - 1]);
                    operation.Add(1);
                    operation.Add(0);
                    BackColor(masE[i - 1]);
                    operation.Add(0);
                    operation.Add(0);
                    totalTime -= 1000;
                    BackColor(masE[i]);
                    operation.Add(0);
                    operation.Add(0);
                }
            }
            totalTime = 0;
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(TimerTickInsert);
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Start();
            storyboard.Begin(this, true);
        }

        private void Compare(Element r)
        {
            ColorAnimation animation = new ColorAnimation();
            animation.To = colors[0];
            animation.Duration = TimeSpan.FromSeconds(1);
            Storyboard.SetTargetName(animation, r.brush);
            Storyboard.SetTargetProperty(animation, new PropertyPath(SolidColorBrush.ColorProperty));
            animation.BeginTime = TimeSpan.FromMilliseconds(totalTime);
            storyboard.Children.Add(animation);
            totalTime += 1000;
            ColorAnimation animation1 = new ColorAnimation();
            animation1.To = colors[1];
            animation1.Duration = TimeSpan.FromSeconds(1);
            animation1.BeginTime = TimeSpan.FromMilliseconds(totalTime);
            Storyboard.SetTargetName(animation1, r.brush);
            Storyboard.SetTargetProperty(animation1, new PropertyPath(SolidColorBrush.ColorProperty));
            storyboard.Children.Add(animation1);
            totalTime += 1000;

        }
        private void FalseCompare(Element r)
        {
            ColorAnimation animation = new ColorAnimation();
            animation.To = colors[2];
            animation.Duration = TimeSpan.FromSeconds(1);
            Storyboard.SetTargetName(animation, r.brush);
            Storyboard.SetTargetProperty(animation, new PropertyPath(SolidColorBrush.ColorProperty));
            animation.BeginTime = TimeSpan.FromMilliseconds(totalTime);
            storyboard.Children.Add(animation);
            totalTime += 1000;
        }
        private void BackColor(Element r)
        {
            ColorAnimation animation = new ColorAnimation();
            animation.To = colors[1];
            animation.Duration = TimeSpan.FromSeconds(1);
            Storyboard.SetTargetName(animation, r.brush);
            Storyboard.SetTargetProperty(animation, new PropertyPath(SolidColorBrush.ColorProperty));
            animation.BeginTime = TimeSpan.FromMilliseconds(totalTime);
            storyboard.Children.Add(animation);
            totalTime += 1000;
        }
        private void MoveUP(Element r)
        {
            //ColorAnimation animation = new ColorAnimation();
            //animation.To = colors[3];
            //animation.Duration = TimeSpan.FromSeconds(1);
            //Storyboard.SetTargetName(animation, r.brush);
            //Storyboard.SetTargetProperty(animation, new PropertyPath(SolidColorBrush.ColorProperty));
            //animation.BeginTime = TimeSpan.FromMilliseconds(totalTime);
            //storyboard.Children.Add(animation);

            DoubleAnimation animation2 = new DoubleAnimation();//створення об'єкту анімації
            animation2.To = can_Graphics.ActualHeight - r.rectangle.Height - labelHeight;//кінцева точка
            animation2.Duration = TimeSpan.FromMilliseconds(1000);//дія виконуватиметься за 1 секунду
            animation2.BeginTime = TimeSpan.FromMilliseconds(totalTime);
            Storyboard.SetTarget(animation2, r.container);
            Storyboard.SetTargetProperty(animation2, new PropertyPath(Canvas.BottomProperty));
            storyboard.Children.Add(animation2);
            totalTime += 1000;
        }
        private void MoveBack(Element r, int endX)
        {
            DoubleAnimation animation1 = new DoubleAnimation();//анімація переміщення вліво
            animation1.To = endX;//кінцева координата
            animation1.Duration = TimeSpan.FromMilliseconds(1000);//час виконання - 1 секунда
            animation1.BeginTime = TimeSpan.FromMilliseconds(totalTime);
            Storyboard.SetTarget(animation1, r.container);//об'єкт, до якого буде застосовано анімацію
            Storyboard.SetTargetProperty(animation1, new PropertyPath((Canvas.LeftProperty)));//поле, до якого буде застосовано анімацію

            DoubleAnimation animation2 = new DoubleAnimation();//анімація переміщення вниз
            animation2.To = can_Graphics.ActualHeight / 4 - labelHeight;//кінцева позиція
            animation2.Duration = TimeSpan.FromMilliseconds(1000);//час виконання - 1 секунда
            animation2.BeginTime = TimeSpan.FromMilliseconds(totalTime);
            Storyboard.SetTarget(animation2, r.container);//об'єкт, до якого буде застосовано анімацію
            Storyboard.SetTargetProperty(animation2, new PropertyPath((Canvas.BottomProperty)));//поле, до якого буде застосовано 

            ColorAnimation animation = new ColorAnimation();
            animation.To = colors[1];
            animation.Duration = TimeSpan.FromSeconds(1);
            Storyboard.SetTargetName(animation, r.brush);
            Storyboard.SetTargetProperty(animation, new PropertyPath(SolidColorBrush.ColorProperty));
            animation.BeginTime = TimeSpan.FromMilliseconds(totalTime);

            storyboard.Children.Add(animation1);
            storyboard.Children.Add(animation2);
            storyboard.Children.Add(animation);
            totalTime += 1000;
        }
        private void MoveRight(Element r, double endX)
        {
            DoubleAnimation animation = new DoubleAnimation();//створяємо анімацію
            animation.To = endX;//задаємо кінцеву координату
            animation.Duration = TimeSpan.FromMilliseconds(500);//час виконання - 0.5 секунди
            animation.BeginTime = TimeSpan.FromMilliseconds(totalTime);
            totalTime += 500;
            Storyboard.SetTarget(animation, r.container);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.LeftProperty));
            storyboard.Children.Add(animation);
        }
        private void TimerTickInsert(object sender, EventArgs e)
        {
            if(operation[0]==1)
            {
                string end = counterlb.Content.ToString().Substring(counterlb.Content.ToString().LastIndexOf(' ') + 1);
                counterlb.Content = "Порівнянь виконано: " + (int.Parse(end) + 1).ToString();
            }
            if(operation[0]==2)
            {
                string end = inversionlb.Content.ToString().Substring(inversionlb.Content.ToString().LastIndexOf(' ') + 1);
                inversionlb.Content = "Обмінів виконано: " + (int.Parse(end) + 1).ToString();
            }
            operation.RemoveAt(0);
            if (operation.Count ==0)
                timer.Stop();
        }


        //Сортування підрахунком
        Label[] lCount;
        int width2;
        Line pointer;
        DispatcherTimer timer;
        List<KeyValuePair<int, Label>> order;//індекс елементу у тимчасовому масиві, елемент у тимчасовому масиві
        bool trend;//true- етап підрахунку, false-етап розстановки
        Border marker;
        private void StartCounting()
        {
            width = (int)((can_Graphics.ActualWidth - 10) / data.Length);//вираховуємо ширину елементів
            storyboard = new Storyboard();//Групуємо анімацію
            storyboard.Children = new TimelineCollection();

            can_Graphics.Children.Clear();//видаляємо старі єлемненти
            Label[] lValues = new Label[data.Length];


            counterlb = new Label();
            counterlb.Content = "Операцій виконано: 0";
            counterlb.FontFamily = new FontFamily(fontName);
            counterlb.FontSize = can_Graphics.ActualHeight / 20;
            counterlb.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            counterlb.VerticalContentAlignment = VerticalAlignment.Center;
            counterlb.Height = can_Graphics.ActualHeight / 10-10;
            can_Graphics.Children.Add(counterlb);
            Canvas.SetLeft(counterlb, 5);
            Canvas.SetTop(counterlb, 5);

            Grid Data = new Grid();
            Data.Height = can_Graphics.ActualHeight / 4;
            Data.Width = can_Graphics.ActualWidth - 10;
            can_Graphics.Children.Add(Data);
            Canvas.SetLeft(Data, 5);
            Canvas.SetTop(Data, can_Graphics.ActualHeight / 10);

            int min = data[0];
            int max = data[0];
            //10- поля; 5 - перший край
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] > max)
                    max = data[i];
                if (data[i] < min)
                    min = data[i];
                Data.ColumnDefinitions.Add(new ColumnDefinition());

                Border border = new Border();
                border.BorderThickness = new Thickness(5);
                border.BorderBrush = new SolidColorBrush(colors[0]);
                border.VerticalAlignment = VerticalAlignment.Stretch;
                border.HorizontalAlignment = HorizontalAlignment.Stretch;
                Data.Children.Add(border);
                Grid.SetColumn(border, i);

                Label label = new Label();
                label.Content = data[i];
                label.FontFamily = new FontFamily(fontName);
                label.FontSize = width / 2;
                if (label.FontSize * 1.2 > can_Graphics.ActualHeight / 4)
                    label.FontSize = can_Graphics.ActualHeight / (4 * 1.2);
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
                label.VerticalContentAlignment = VerticalAlignment.Center;
                label.Width = width - 8;
                label.Height = can_Graphics.ActualHeight / 4 - 10;
                can_Graphics.Children.Add(label);
                Canvas.SetLeft(label, i * width + 5);
                Canvas.SetTop(label, can_Graphics.ActualHeight / 10 + 5);
                lValues[i] = label;
            }

            Grid temp = new Grid();
            temp.RowDefinitions.Add(new RowDefinition());
            temp.RowDefinitions.Add(new RowDefinition());
            temp.Width = can_Graphics.ActualWidth - 10;
            can_Graphics.Children.Add(temp);
            Canvas.SetLeft(temp, 5);
            Canvas.SetTop(temp, (can_Graphics.ActualHeight * 6) / 10);
            width2 = (int)((can_Graphics.ActualWidth - 10) / (max - min + 1));
            lCount = new Label[max - min + 1];

            for (int i = 0; i < max - min + 1; i++)
            {
                temp.ColumnDefinitions.Add(new ColumnDefinition());
                Border border = new Border();
                border.BorderThickness = new Thickness(5);
                border.BorderBrush = new SolidColorBrush(colors[0]);
                border.HorizontalAlignment = HorizontalAlignment.Stretch;
                border.Height = can_Graphics.ActualHeight / 4;
                temp.Children.Add(border);
                Grid.SetColumn(border, i);

                Label label = new Label();
                label.Content = min + i;
                label.FontFamily = new FontFamily(fontName);
                label.FontSize = can_Graphics.ActualWidth / (max - min + 1) / 4;
                if (label.FontSize * 1.2 > can_Graphics.ActualHeight / 4)
                    label.FontSize = can_Graphics.ActualHeight / (4 * 1.2);
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
                label.VerticalContentAlignment = VerticalAlignment.Center;
                temp.Children.Add(label);
                Grid.SetColumn(label, i);
                Grid.SetRow(label, 1);

                label = new Label();
                label.Content = "";
                label.FontFamily = new FontFamily(fontName);
                label.FontSize = can_Graphics.ActualWidth/(max-min+1) / 2;
                if (label.FontSize * 1.2 > can_Graphics.ActualHeight / 4)
                    label.FontSize = can_Graphics.ActualHeight / (4 * 1.2);
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
                label.VerticalContentAlignment = VerticalAlignment.Center;
                border.Child = label;
                Grid.SetColumn(label, i);
                lCount[i] = label;
            }

            pointer = new Line();
            pointer.Stroke = new SolidColorBrush(colors[2]);
            pointer.StrokeThickness = 5;
            pointer.Y1 = (can_Graphics.ActualHeight * 7) / 20;
            pointer.Y2 = (can_Graphics.ActualHeight * 6) / 10;
            can_Graphics.Children.Add(pointer);

            marker = new Border();
            marker.BorderThickness = new Thickness(10);
            marker.BorderBrush = new SolidColorBrush(colors[3]);
            marker.Height = can_Graphics.ActualHeight / 4 + 10;
            marker.Width = width + 5;
            marker.Visibility = Visibility.Hidden;
            can_Graphics.Children.Add(marker);
            Canvas.SetLeft(marker, 0);
            Canvas.SetTop(marker, can_Graphics.ActualHeight / 10 - 5);
            CountingSort(lValues, max, min);
        }
        private void CountingSort(Label[] lValues, int max, int min)
        {
            int[] tempVal = new int[lCount.Length];
            List<Label>[] tempLab = new List<Label>[lCount.Length];
            order = new List<KeyValuePair<int, Label>>();
            for (int i = 0; i < lCount.Length; i++)
                tempLab[i] = new List<Label>();

            for (int i = 0; i < data.Length; i++)
            {
                tempVal[data[i] - min]++;
                tempLab[data[i] - min].Add(lValues[i]);
                order.Add(new KeyValuePair<int, Label>(data[i] - min, lValues[i]));
                ChangePoiner(width * i + 10  + width / 2, width2 * (data[i] - min) + 10  + width2 / 2);
                MoveToTemp(width2 * (data[i] - min) + 10 + width2 / 4, (can_Graphics.ActualHeight * 6) / 10 + 5, data[i] - min);
            }
            totalTime += 1000;
            order.Add(new KeyValuePair<int, Label>(-1, null));//макрер зміни напряму
            int pos = 0;
            for (int i = 0; i < tempVal.Length; i++)
            {
                if(tempVal[i]==0)
                {
                    order.Add(new KeyValuePair<int, Label>(i,null));
                    ChangePoiner(width * pos + 10 + width / 2, width2 * i + 10 + width2 / 2);
                    totalTime += 1000;//пуста операція переміщення
                }
                for (int j = 0; j < tempVal[i]; j++)
                {
                    data[pos] = i + min;
                    order.Add(new KeyValuePair<int, Label>(i, tempLab[i][0]));
                    ChangePoiner(width * pos + 10 + width / 2, width2 * i + 10  + width2 / 2);
                    MoveToDat(width * pos + 10, can_Graphics.ActualHeight / 10 + 5);
                    tempLab[i].RemoveAt(0);
                    pos++;
                }
            }
            totalTime = 0;
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(TimerTickCounting);
            timer.Interval = TimeSpan.FromMilliseconds(1500);
            trend = true;//почаниємо візуалізувати етап підрахунку
            timer.Start();
            storyboard.Begin(this, true);
        }

        int memory=-1;
        private void MoveToTemp(double endX, double endY, int tempIndex)
        {
            DoubleAnimation animation1 = new DoubleAnimation();
            animation1.To = endX;
            animation1.Duration = TimeSpan.FromSeconds(1);
            animation1.BeginTime = TimeSpan.FromMilliseconds(totalTime);
            Storyboard.SetTarget(animation1, order.Last().Value);
            Storyboard.SetTargetProperty(animation1, new PropertyPath(Canvas.LeftProperty));
            storyboard.Children.Add(animation1);

            DoubleAnimation animation2 = new DoubleAnimation();
            animation2.To = endY;
            animation2.Duration = TimeSpan.FromSeconds(1);
            animation2.BeginTime = TimeSpan.FromMilliseconds(totalTime);
            Storyboard.SetTarget(animation2, order.Last().Value);
            Storyboard.SetTargetProperty(animation2, new PropertyPath(Canvas.TopProperty));
            storyboard.Children.Add(animation2);
            totalTime += 1000;
        }
        private void MoveToDat(double endX, double endY)
        {
            DoubleAnimation animation1 = new DoubleAnimation();
            animation1.To = endX;
            animation1.Duration = TimeSpan.FromSeconds(1);
            animation1.BeginTime = TimeSpan.FromMilliseconds(totalTime);
            Storyboard.SetTarget(animation1, order.Last().Value);
            Storyboard.SetTargetProperty(animation1, new PropertyPath(Canvas.LeftProperty));
            storyboard.Children.Add(animation1);

            DoubleAnimation animation2 = new DoubleAnimation();
            animation2.To = endY;
            animation2.Duration = TimeSpan.FromSeconds(1);
            animation2.BeginTime = TimeSpan.FromMilliseconds(totalTime);
            Storyboard.SetTarget(animation2, order.Last().Value);
            Storyboard.SetTargetProperty(animation2, new PropertyPath(Canvas.TopProperty));
            storyboard.Children.Add(animation2);

            DoubleAnimation animation3 = new DoubleAnimation();
            animation3.To = endX-5;
            animation3.Duration = TimeSpan.FromMilliseconds(500);
            animation3.BeginTime = TimeSpan.FromMilliseconds(totalTime);
            Storyboard.SetTarget(animation3, marker);
            Storyboard.SetTargetProperty(animation3, new PropertyPath(Canvas.LeftProperty));
            storyboard.Children.Add(animation3);
            totalTime += 1000;
        }
        private void ChangePoiner(double x1, double x2)
        {
            DoubleAnimation animationX1 = new DoubleAnimation();
            animationX1.To = x1;
            animationX1.Duration = TimeSpan.FromMilliseconds(500);
            animationX1.BeginTime = TimeSpan.FromMilliseconds(totalTime);
            Storyboard.SetTarget(animationX1, pointer);
            Storyboard.SetTargetProperty(animationX1, new PropertyPath(Line.X1Property));
            storyboard.Children.Add(animationX1);

            DoubleAnimation animationX2 = new DoubleAnimation();
            animationX2.To = x2;
            animationX2.Duration = TimeSpan.FromMilliseconds(500);
            animationX2.BeginTime = TimeSpan.FromMilliseconds(totalTime);
            Storyboard.SetTarget(animationX2, pointer);
            Storyboard.SetTargetProperty(animationX2, new PropertyPath(Line.X2Property));
            storyboard.Children.Add(animationX2);
            totalTime += 500;
        }
        private void TimerTickCounting(object sender, EventArgs e)
        {
            if (trend)
            {
                if (lCount[order[0].Key].Content.ToString().CompareTo("") == 0)
                    lCount[order[0].Key].Content = "1";
                else
                    lCount[order[0].Key].Content = int.Parse(lCount[order[0].Key].Content.ToString()) + 1;
                order[0].Value.Visibility = Visibility.Hidden;
                order.RemoveAt(0);
                if (order[0].Key == -1)//зміна напряму
                {
                    trend = false;
                    order.RemoveAt(0);
                    marker.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (order[0].Value != null)
                {
                    if (lCount[order[0].Key].Content.ToString().CompareTo("1") == 0)
                        lCount[order[0].Key].Content = "";
                    else
                        lCount[order[0].Key].Content = int.Parse(lCount[order[0].Key].Content.ToString()) - 1;
                    order[0].Value.Visibility = Visibility.Visible;
                    if(memory!=order[0].Key)
                    {
                        string v = counterlb.Content.ToString().Substring(counterlb.Content.ToString().LastIndexOf(' ') + 1);
                        counterlb.Content = "Операцій виконано: " + (int.Parse(v) + 1).ToString();
                        memory = order[0].Key;
                    }
                    
                    order.RemoveAt(0);
                    if (order.Count == 0)
                    {
                        timer.Stop();
                    }

                }
                else
                    order.RemoveAt(0);
            }
            string val = counterlb.Content.ToString().Substring(counterlb.Content.ToString().LastIndexOf(' ') + 1);
            counterlb.Content = "Операцій виконано: " + (int.Parse(val) + 1).ToString();
        }


        //QuickSort
        Element rectangleRed;//поточна зона роботи QSort
        Element rectangleChartreus;//Права частина
        Element rectangleBlack;//Ліва частина
        double scale;
        private void StartQSort()
        {
            width = (int)((can_Graphics.ActualWidth - 10) / data.Length);//вираховуємо ширину елементів
            storyboard = new Storyboard();//Групуємо анімацію
            storyboard.Children = new TimelineCollection();
            operation = new List<int>();

            can_Graphics.Children.Clear();//видаляємо старі єлемненти
            masE = new Element[data.Length];

            int max = data[0];
            for (int i = 1; i < masE.Length; i++)//пошук максимального значення
                if (data[i] > max)
                    max = data[i];

            NameScope.SetNameScope(this, new NameScope());
            scale = (can_Graphics.ActualHeight / 2) / max;//визначення масштабу


            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Height = can_Graphics.ActualHeight / 10 - 10;
            can_Graphics.Children.Add(panel);
            Canvas.SetTop(panel, 5);
            Canvas.SetLeft(panel, 5);

            counterlb = new Label();
            counterlb.Content = "Порівнянь виконано: 0";
            counterlb.FontFamily = new FontFamily(fontName);
            counterlb.FontSize = can_Graphics.ActualHeight / 20;
            counterlb.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            counterlb.VerticalContentAlignment = VerticalAlignment.Center;
            counterlb.VerticalAlignment = VerticalAlignment.Stretch;
            panel.Children.Add(counterlb);

            inversionlb = new Label();
            inversionlb.Content = "Обмінів виконано: 0";
            inversionlb.FontFamily = new FontFamily(fontName);
            inversionlb.FontSize = can_Graphics.ActualHeight / 20;
            inversionlb.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            inversionlb.VerticalContentAlignment = VerticalAlignment.Center;
            inversionlb.VerticalAlignment = VerticalAlignment.Stretch;
            panel.Children.Add(inversionlb);

            rectangleRed = new Element();
            rectangleRed.rectangle = new Rectangle();
            SolidColorBrush temp = new SolidColorBrush(colors[4]);
            temp.Opacity = 0;
            rectangleRed.rectangle.Fill = temp;
            rectangleRed.rectangle.Height = 0;
            rectangleRed.rectangle.Width = 0;
            rectangleRed.brush = "red";
            RegisterName(rectangleRed.brush, temp);
            can_Graphics.Children.Add(rectangleRed.rectangle);
            Canvas.SetBottom(rectangleRed.rectangle, can_Graphics.ActualHeight / 4 - 10);
            Canvas.SetLeft(rectangleRed.rectangle, 0);

            rectangleChartreus = new Element();
            rectangleChartreus.rectangle = new Rectangle();
            temp = new SolidColorBrush(colors[5]);
            temp.Opacity = 0;
            rectangleChartreus.rectangle.Fill = temp;
            rectangleChartreus.rectangle.Height = 0;
            rectangleChartreus.rectangle.Width = 0;
            rectangleChartreus.brush = "green";
            RegisterName(rectangleChartreus.brush, temp);
            can_Graphics.Children.Add(rectangleChartreus.rectangle);
            Canvas.SetBottom(rectangleChartreus.rectangle, can_Graphics.ActualHeight / 4 - 10);
            Canvas.SetLeft(rectangleChartreus.rectangle, 0);

            rectangleBlack = new Element();
            rectangleBlack.rectangle = new Rectangle();
            temp = new SolidColorBrush(colors[0]);
            temp.Opacity = 0;
            rectangleBlack.rectangle.Fill = temp;
            rectangleBlack.rectangle.Height = 0;
            rectangleBlack.rectangle.Width = 0;
            rectangleBlack.brush = "yellow";
            RegisterName(rectangleBlack.brush, temp);
            can_Graphics.Children.Add(rectangleBlack.rectangle);
            Canvas.SetBottom(rectangleBlack.rectangle, can_Graphics.ActualHeight / 4 - 10);
            Canvas.SetLeft(rectangleBlack.rectangle, 0);

            labelHeight = (int)(width / 2 * 1.328);//приблизне визнаяення висоти label
            for (int i = 0; i < masE.Length; i++)
            {
                masE[i] = new Element();
                masE[i].value = data[i];
                masE[i].container = new Grid();
                masE[i].container.RowDefinitions.Add(new RowDefinition());
                masE[i].container.RowDefinitions.Add(new RowDefinition());
                masE[i].rectangle = new Rectangle();
                masE[i].rectangle.Height = data[i] * scale;
                masE[i].rectangle.Width = width;
                SolidColorBrush tempBrush = new SolidColorBrush(colors[1]);
                masE[i].rectangle.Fill = tempBrush;
                masE[i].container.Children.Add(masE[i].rectangle);

                Label lab = new Label();
                lab.Content = data[i];
                lab.FontFamily = new FontFamily(fontName);
                lab.FontSize = width / 2;
                lab.HorizontalContentAlignment = HorizontalAlignment.Center;
                masE[i].container.Children.Add(lab);
                Grid.SetRow(lab, 1);
                Grid.SetRow(masE[i].rectangle, 0);

                masE[i].brush = "Brush" + i;
                RegisterName(masE[i].brush, tempBrush);
                can_Graphics.Children.Add(masE[i].container);
                Canvas.SetLeft(masE[i].container, width * i + 5);
                Canvas.SetBottom(masE[i].container, can_Graphics.ActualHeight / 4 - labelHeight);
            }
            totalTime += 500;
            QSort(0, data.Length - 1);
            totalTime = 0;
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(TimerTickQSort);
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            trend = true;//почаниємо візуалізувати етап підрахунку
            timer.Start();
            storyboard.Begin(this, true);
        }
        private void QSort(int l, int r)//метод-прокладка для виклику quickSort(l,r,m).Шукає середнє значення для запобігання надмірної рекурсії
        {
            Hide(rectangleBlack);
            Hide(rectangleChartreus);
            int min = masE[l].value;
            int max = masE[r].value;

            for (int i = l; i <= r; i++)//пошук максимального і мінімального значень
            {
                if (min > masE[i].value)
                    min = masE[i].value;
                if (max < masE[i].value)
                    max = masE[i].value;
            }
            SetRect(rectangleRed, (int)(max * scale + 20), width * (r - l + 1), width * l+5);//задаємо робочу область
            quickSort(l, r, (min + max) / 2, max);//виклик справжнього QSort();
        }
        private void quickSort(int left, int right, double average,int max)
        {
            //робимо копії переданих меж, для операцій з ними
            int i = left;
            int j = right;

            while (i <= j)
            {
                while (masE[i].value < average)
                {
                    TrueCompare(masE[i]);
                    operation.Add(1);
                    i++;
                }//рухаємо ліву межу праворуч, доки не знайдемо елемент >= average
                FalseCompare(masE[i]);
                operation.Add(1);
                while (masE[j].value > average)
                {
                    TrueCompare(masE[j]);
                    operation.Add(1);
                    j--;
                }//рухаємо праву межу ліворуч, доки не знайдемо елемент <= за average
                SecondFalseCompare(masE[j]);
                operation.Add(1);

                if (i <= j)//Якщо межі не розійшлись, то міняємо елементі місцями та зсуваємо межі у відповідних напрямках
                {
                    Exchange(i,j);//візуалізація подальших дій
                    operation.Add(2);
                    TrueCompare(masE[i]);
                    totalTime-=1000;//для одночасної зміни кольорів
                    TrueCompare(masE[j]);
                    operation.Add(0);


                    Element temp = masE[i];
                    masE[i] = masE[j];
                    masE[j] = temp;
                    i++;
                    j--;
                }
            }
            for(int k=left;k<=right;k++)
            {
                BackColor(masE[k]);
                totalTime-=1000;//одночасна зміна кольорів
            }

            totalTime+=1000;//відділення від наступної анімації
            operation.Add(0);
            Hide(rectangleRed);
            SetRect(rectangleChartreus, (int)(max*scale+10), (right - i + 1) * width, width * i+5);
            SetRect(rectangleBlack, (int)(max*scale+10), (j - left + 1) * width, width * left+5);
            totalTime+=1000;
            operation.Add(0);

            if (i < right)//Якщо права частина складається не менш як з 2 елементів, сортуємо її
                QSort(i, right);

            if (left < j)//Якщо ліва частина складається не менш як з 2 елементів, сортуємо її
                QSort(left, j);
        }

        private void TrueCompare(Element e)
        {
            ColorAnimation animation = new ColorAnimation();
            animation.To = colors[3];
            animation.Duration = TimeSpan.FromSeconds(1);
            Storyboard.SetTargetName(animation, e.brush);
            Storyboard.SetTargetProperty(animation, new PropertyPath(SolidColorBrush.ColorProperty));
            animation.BeginTime = TimeSpan.FromMilliseconds(totalTime);
            storyboard.Children.Add(animation);
            totalTime += 1000;
        }
        private void SecondFalseCompare(Element e)
        {
            ColorAnimation animation = new ColorAnimation();
            animation.To = colors[6];
            animation.Duration = TimeSpan.FromSeconds(1);
            Storyboard.SetTargetName(animation, e.brush);
            Storyboard.SetTargetProperty(animation, new PropertyPath(SolidColorBrush.ColorProperty));
            animation.BeginTime = TimeSpan.FromMilliseconds(totalTime);
            storyboard.Children.Add(animation);
            totalTime += 1000;
        }
        private void Exchange(int i,int j)
        {
            DoubleAnimation animation1 = new DoubleAnimation();
            animation1.To = i*width+5;
            animation1.Duration = TimeSpan.FromMilliseconds(1000);
            animation1.BeginTime = TimeSpan.FromMilliseconds(totalTime);
            Storyboard.SetTarget(animation1, masE[j].container);
            Storyboard.SetTargetProperty(animation1, new PropertyPath(Canvas.LeftProperty));
            storyboard.Children.Add(animation1);

            DoubleAnimation animation2 = new DoubleAnimation();
            animation2.To = j*width+5;
            animation2.Duration = TimeSpan.FromMilliseconds(1000);
            animation2.BeginTime = TimeSpan.FromMilliseconds(totalTime);
            Storyboard.SetTarget(animation2, masE[i].container);
            Storyboard.SetTargetProperty(animation2, new PropertyPath(Canvas.LeftProperty));
            storyboard.Children.Add(animation2);

            totalTime += 1000;
        }
        private void Hide(Element e)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.To = 0;
            animation.Duration = TimeSpan.FromMilliseconds(0);
            animation.BeginTime = TimeSpan.FromMilliseconds(totalTime);
            Storyboard.SetTargetName(animation, e.brush);
            Storyboard.SetTargetProperty(animation, new PropertyPath(SolidColorBrush.OpacityProperty));
            storyboard.Children.Add(animation);
        }
        private void SetRect(Element r, int height, int width, int x)//наступна анімація проходить миттєво
        {
            DoubleAnimation animation1 = new DoubleAnimation();
            animation1.To = height;
            animation1.Duration = TimeSpan.FromMilliseconds(0);
            animation1.BeginTime = TimeSpan.FromMilliseconds(totalTime);
            Storyboard.SetTarget(animation1, r.rectangle);
            Storyboard.SetTargetProperty(animation1, new PropertyPath(HeightProperty));
            storyboard.Children.Add(animation1);

            DoubleAnimation animation2 = new DoubleAnimation();
            animation2.To = width;
            animation2.Duration = TimeSpan.FromMilliseconds(0);
            animation2.BeginTime = TimeSpan.FromMilliseconds(totalTime);
            Storyboard.SetTarget(animation2, r.rectangle);
            Storyboard.SetTargetProperty(animation2, new PropertyPath(WidthProperty));
            storyboard.Children.Add(animation2);

            DoubleAnimation animation3 = new DoubleAnimation();
            animation3.To = x;
            animation3.Duration = TimeSpan.FromMilliseconds(0);
            animation3.BeginTime = TimeSpan.FromMilliseconds(totalTime);
            Storyboard.SetTarget(animation3, r.rectangle);
            Storyboard.SetTargetProperty(animation3, new PropertyPath(Canvas.LeftProperty));
            storyboard.Children.Add(animation3);

            DoubleAnimation animation4 = new DoubleAnimation();
            animation4.To = 0.5;
            animation4.Duration = TimeSpan.FromMilliseconds(0);
            animation4.BeginTime = TimeSpan.FromMilliseconds(totalTime);
            Storyboard.SetTargetName(animation4, r.brush);
            Storyboard.SetTargetProperty(animation4, new PropertyPath(SolidColorBrush.OpacityProperty));
            storyboard.Children.Add(animation4);
        }
        private void TimerTickQSort(object sender, EventArgs e)
        {
            if (operation[0] == 1)
            {
                string end = counterlb.Content.ToString().Substring(counterlb.Content.ToString().LastIndexOf(' ') + 1);
                counterlb.Content = "Порівнянь виконано: " + (int.Parse(end) + 1).ToString();
            }
            if (operation[0] == 2)
            {
                string end = inversionlb.Content.ToString().Substring(inversionlb.Content.ToString().LastIndexOf(' ') + 1);
                inversionlb.Content = "Обмінів виконано: " + (int.Parse(end) + 3).ToString();
            }
            operation.RemoveAt(0);
            if (operation.Count == 0)
                timer.Stop();
        }
    }
}