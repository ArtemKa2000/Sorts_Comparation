   M S B A M L   `   `   `  ����@  <Sorts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:SortsSorts  5   	   ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35tOclr-namespace:System.Windows.Forms.Integration;assembly=WindowsFormsIntegration System.Windows.Forms.Integration 5   	   [ WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms 5   	   m iSystem.Windows.Forms.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35�mclr-namespace:System.Windows.Forms.DataVisualization.Charting;assembly=System.Windows.Forms.DataVisualization/System.Windows.Forms.DataVisualization.Charting 5	   	       Sorts.WrongPair   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:Sorts     5   	   _wfiOclr-namespace:System.Windows.Forms.Integration;assembly=WindowsFormsIntegration     5   	   Owf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     5   	   }dvcmclr-namespace:System.Windows.Forms.DataVisualization.Charting;assembly=System.Windows.Forms.DataVisualization     5	   	   -   5          � Title$1  +Впорядкованість масиву��5   	   $	��600��6(   $
��1200��65    � WindowState$ 	Maximized=�6B    � Icon$ Images/LIT-.ico��6Z   .��5      �  � RowDefinitions 5   
   �� 5      $��80 �6   6*   �� 5      6    � ColumnDefinitions 5   
   �� 5      6!   �� 5      $
��auto �6   6.   .+�5   
   � ���	#FFF3F3F3$��2��6&    5      �� 5      $
��0.7* �6#   62   �� 5      $��2* �6#   60   �� 5      $
��0.6* �6#   62   �� 5      $��2* �6#   60   �� 5      $
��0.6* �6#   62   �� 5      $��2* �6#   60   �� 5      $��2* �6#   60   .+�5      �� -   5        $��button_Back��5   A   $��10,10q�6   $��2��6$   $��0��61    � ToolTip$&  До головного меню��6�   ��6U   Z� ��6k   Z� ��6�   Z� .��5       �� $��Images/Back.png��6   5!      �� 5"      $��1��6   $��20(�6$   $��Times New Roman)�62   $,��&Кількість елементів:��6O   $��Center=�6n   $��Center=�6�   �� 5#      -   5         ��Name$ tb_elementCount��5#   4   $��3��6   $��2��6#   $��25��6M   $��25(�6W   $��Times New Roman)�6e   
���   �
���   � �� SelectionBrush �	#FFC1B7B7$��Center=�6�    �� 
CaretBrush
 �   �$��Center=�6  $��5q�6+  ��6�   Z� 6+  �� 5$      $��3��6   $1��+Масивів для розрахунку:��6$   $��20(�6F   $��Times New Roman)�6T   $��Center=�6q   $��Center=�6�   �� 5%      -   5        $��tb_WrongPairAccuracy��5%   ?   $��5q�6   $��4��6"   $��4��6/   $
��0,10q�6\   $��25(�6j   $��Times New Roman)�6x   
���   �
���   � �	#FFC1B7B7$��Center=�6�   $��5��6  
 �   �$��Center=�6+  ��6�   Z� 6+  �� 5&      -   5        $��Button_WrongPair��5&   O   $��0��6   $��5��6"   $��Times New Roman)�62   $'��!Побудувати графік��6g   $��20(�6�   $��
15,15,5,15q�6�   6�   �� 5'      -   5        $��Button_Clear��5'   2   $��1��6   $��6��6"   $��Очистити��6F   $��20(�6Z   $��Times New Roman)�6h   $��10,15q�6�   6�   5(      7  1System.Windows.Forms.Integration.WindowsFormsHost  5)   
   -   5        	  Name$
	 host��5)      $��1��6*   
   Child.
 5*      ;  5System.Windows.Forms.DataVisualization.Charting.Chart  -   5        	chart��"  System.Windows.Forms.Control   Dock$
 Fill=�5*   '      BackgroundImageLayout$
 None=�63      Legends 5+      <  6System.Windows.Forms.DataVisualization.Charting.Legend  5,         
ChartAreas 5.      ?  9System.Windows.Forms.DataVisualization.Charting.ChartArea  5/         Name$
 Data��6$   51      52      53      54      