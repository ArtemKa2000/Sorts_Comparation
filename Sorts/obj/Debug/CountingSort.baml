   M S B A M L   `   `   `  ����@  <Sorts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:SortsSorts  5   	   ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35tOclr-namespace:System.Windows.Forms.Integration;assembly=WindowsFormsIntegration System.Windows.Forms.Integration 5   	   [ WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms 5   	   m iSystem.Windows.Forms.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35�mclr-namespace:System.Windows.Forms.DataVisualization.Charting;assembly=System.Windows.Forms.DataVisualization/System.Windows.Forms.DataVisualization.Charting 5	   	       Sorts.CountingSort   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:Sorts     5   	   _wfiOclr-namespace:System.Windows.Forms.Integration;assembly=WindowsFormsIntegration     5   	   Owf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     5   	   }dvcmclr-namespace:System.Windows.Forms.DataVisualization.Charting;assembly=System.Windows.Forms.DataVisualization     5	   	   -   5          � Title$J  DЕфективність сортування підрахунком��5      $	��600��68   $
��1200��6E    � WindowState$ 	Maximized=�6R    � Icon$ Images/LIT-.ico��6j   .��5      �  � RowDefinitions 5   
   �� 5      $��80 �6   6*   �� 5      6    � ColumnDefinitions 5   
   �� 5      6!   �� 5      $
��auto �6   6.   .+�5   
   � ���	#FFF3F3F3$��2��6&    5      �� 5      $
��0.5* �6#   62   �� 5      $
��1.4* �6#   62   �� 5      $
��0.6* �6#   62   �� 5      $
��1.5* �6#   62   �� 5      $
��1.3* �6#   62   �� 5      $
��1.3* �6#   62   .+�5      �� -   5        $��button_Back��5   4   $��10,10q�6   $��0��6$    � ToolTip$ Головне меню��6�   ��6I   Z� ��6_   Z� ��6v   Z� .��5      �� $��Images/Back.png��6   $&  До головного меню��61   5       � 5!      $��1��6   $��0q�6$    5"      �� 5#      6)   �� 5$      $
��0.3* �6'   66   .+�5&      �� $��20(�6   $��Times New Roman)�6&   $,��&Кількість елементів:��6C   $��Center=�6b   $��Center=�6�   �� 5'      -   5         ��Name$ tb_elementCount��5'   *   $��1��6   $��25��6C   $��25(�6M   $��35��6[   $��Times New Roman)�6j   
���   �
���   � �� SelectionBrush �	#FFC1B7B7$��Center=�6�   $��Stretch=�6  $��Center=�6&   �� 
CaretBrush
 �   ���6�   Z� 6A  5(      �� 5*      $��2��6   $��20(�6$   $��Times New Roman)�62   $��Залежність:��6O   $��Center=�6e   $��Center=�6�   �� 5+      -   5        $��cb_Type��5+   +   $��3��6   $��Times New Roman)�6:   $��20(�6W   $��Center=�6e   $��Center=�6�   $��40��6�   $��Center=�6�   $	��5,0q�6�   .@�5,      �� 	 �� 
IsSelected	 . .��65   .,Вплив діапазону значень6N   �� 5-      .��5.      �� -   5        $��tb_DifferentValues��5.       
 �� TextAlignment$
 Center=�6:   $
��Wrap=�6Q   ;9Вплив кількості різних значень6�   6�   5/      �� 51      -   5        $��Button_CountingSort��51   P   $��0��6   $��4��6#   $��Times New Roman)�63   $'��!Побудувати графік��6k   $��20(�6�   $
��5,15q�6�   �� 52      -   5        $��Button_Clear��52   3   $��1��6   $��5��6#   $��Очистити��6G   $��20(�6[   $��Times New Roman)�6i   $��
5,15,10,15q�6�   53      7  1System.Windows.Forms.Integration.WindowsFormsHost  54   
   -   5          Name$
 host��54      $��1��6*      Child. 55      ;  5System.Windows.Forms.DataVisualization.Charting.Chart  -	   5        	chart��"  System.Windows.Forms.Control   Dock$
 Fill=�55   '      BackgroundImageLayout$
 None=�63      Legends 56      <  6System.Windows.Forms.DataVisualization.Charting.Legend  57         
ChartAreas 59      ?  9System.Windows.Forms.DataVisualization.Charting.ChartArea  5:         Name$
 Data��6$   5<      5=      5?      5@      