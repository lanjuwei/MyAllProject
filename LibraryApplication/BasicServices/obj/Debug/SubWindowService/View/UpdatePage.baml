   M S B A M L   `   `   `  ����H  DBasicServices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullY1clr-namespace:BasicServices.SubWindowService.View#BasicServices.SubWindowService.View  5      4    .BasicServices.SubWindowService.View.UpdatePage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Clocal1clr-namespace:BasicServices.SubWindowService.View     5        N� Title$  
UpdatePage��5	       � 	Resources 5         myProgressBar �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius ��Name �� 
StartPoint	 �� EndPoint
 � Offset � Color��%W  &	        �� 5   
   " �5      .�5      ��  � �� 5      �� 5      " �6&   .R�5      � $��36��6   .+�5      �� $��
PART_Track��5      $ 20��5      
�������$��1q�5      ���#ECECEC5      �� 5      $ PART_Indicator��5      $ 16��5      $��0q�5      $
��Left=�5      ��5   "   �� 5   &   $ 0,0.5&�6:   $		 1,1&�6M   .��5    *   � $
 0Q�67   $ #23bFFF��6B   � 5!   *   $	
 0.5Q�67   $ #23adff��6D   � 5"   *   $
 1Q�67   $ #239AFF��6B   5#   '   5%      5&      5'      5)      5*      5+      .��5,      � $	��600��6   $	��464��6   $��Center=�6$   $��Center=�6A   .+�5-   
   �� $1��+/BasicServices;component/Images/upgrade.png��6   6I   �� 5.   
   -   5         ��Name$ MyStackPanel��5.      $��Bottom=�6+   $��0,0,0,40q�6G   .+�5/      �� -   5         ��Name$ 
UpgradeTip��5/      $��初始化中...��6,   $��32(�6;   ���#000000 J� Opacity$	 0.8Q�50      $��Center=�6&   $��0 0 0 0 q�6C   � 51      -   5         �Name$ MyProgressBar��51      $	��464��61   #��[  6=    � Value$	 100Q�52      $��0 20 0 0q�6*    � Maximum$	 100Q�6<   �� 53      -   5        $ MyProgressVlaue��53      $��28(�62   ���#239AFF$	 0.8Q�54      $��Center=�6&   $��0 4 0 0q�6C   �� 55      �6   ��  �� ElementName$ MyProgressBar�� �� Path$ Value�6V    �� 56      .�6   %6!   57      58      59      5:      