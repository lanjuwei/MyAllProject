   M S B A M L   `   `   `  ����M  ICommonUserControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:CommonUserControlsCommonUserControls  5      I ECommonControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:CommonControls;assembly=CommonControlsCommonControls 5      4    .CommonUserControls.VolumeControllerUserControl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      2local clr-namespace:CommonUserControls     5      Ocommoncontrols4clr-namespace:CommonControls;assembly=CommonControls     5        � 	Resources  5
         RepeatButtonTransparent1 �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius ! SliderThumb.Static.Foreground �� Color $  SliderThumb.MouseOver.Background   SliderThumb.MouseOver.Border " SliderThumb.Pressed.Background  SliderThumb.Pressed.Border # SliderThumb.Disabled.Background  SliderThumb.Disabled.Border ! SliderThumb.Static.Background 	 SliderThumb.Static.Border 
 SliderThumbHorizontalTop � UseLayoutRounding F�Name	 �� Stretch
 J� SnapsToDevicePixels �� Triggers P� Property P� Value �� 
TargetName  SliderThumbHorizontalBottom  SliderThumb.Track.Border   SliderThumb.Track.Background   SliderThumbHorizontalDefault  SliderHorizontal ��Name � RowDefinitions m�Name m� Fill m� 	Placement ��Name ]�Name ]� DecreaseRepeatButton �� DecreaseLarge  RepeatButtonTransparent ]� IncreaseRepeatButton �� IncreaseLarge ]� Thumb n�Name � OverridesDefaultStyle  SliderThumbVerticalLeft  SliderThumbVerticalRight  SliderThumbVerticalDefault A�Name  SliderVertical � ColumnDefinitions  SliderStyle1 �� Triggers��%�&  &	        &	 [    &	 ~    &	 �    &	 �    &	 �    &	 
    &	 -    &	 P    &		 s    &	
 �    7  7 	 7  7  7  7  7  7  &	 �    7  7 	 7  7  7  7  7  7  &	 �    &	 �    &	 	    7  7 	 7  7  7  7  7  7  &	 �    7  7  7  7  7  7 
 7  &	 �    7  7 	 7  7  7  7  7  7  &	 �    7  7 	 7  7  7  7  7  7  &	 �    7  7  &	 �    7  7   7  7  7  7  &	 �#    7  7  7  �� 5   
   " ��61   .�5      ��  �@�OverridesDefaultStyle . 66   �� 5       � ��
 ���� 6+   �� 5       � |� .  6*   �� 5       � �� .  6*   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   #��z��6L   #��z��6n   $ 0,0,4,4��6�   5      5      5      �� 5   
   $ 	#FFE5E5E5��6@   �� 5   
   $ 	#FFDCECFC��6C   �� 5   
   $ 	#FF7Eb4EA��6?   �� 5   
   $ 	#FFDAECFC��6A   �� 5   
   $ 	#FF569DE5��6=   �� 5   
   $ 	#FFF0F0F0��6B   �� 5   
   $ 	#FFD9D9D9��6>   �� 5   
   $ 	#FFF0F0F0��6@   �� 5    
   $ 	#FFACACAC��6<   �� 5!   
   " n�6;   .R�5"      � $��Center=�6    . $��Center=�6I   .+�5#      F� $
 grip��6   �����    ��[ �    ��[ `�S `�S �`�S     �ا ��[ �ا ��[ ��ا ��[ �ا ���ا ����ا �������    ����[ ��[ 	8��  $
	 Fill=�6�   
 . 8�� $��1��6   . $��Center=�6D  5$       5%      P� 5&       �@J�IsMouseOver . . �5'      �� $
 grip��6-   8   � ��6?   �� 5(      $
 grip��6/   8   � ��6A   5)      P� 5*       �@n�
IsDragging . . �5+      �� $
 grip��6-   8   � ��6?   �� 5,      $
 grip��6/   8   � ��6A   5-      P� 5.       � {� .  . �5/      �� $
 grip��6-   8   � ��6?   �� 50      $
 grip��6/   8   � ��6A   51      53      �� 54   
   " n�6>   .R�55      � $��Center=�6    . $��Center=�6I   .+�56      F� $
 grip��6   �����     � �     � `�S ��`�S ���`�S ���ا  � �ا  � ��ا  � �ا �ا ��ا     �         �  � 	8��  $
	 Fill=�6�   
 . 8�� $��1��6   . $��Center=�6H  57       58      P� 59       �@J�IsMouseOver . . �5:      �� $
 grip��6-   8   � ��6?   �� 5;      $
 grip��6/   8   � ��6A   5<      P� 5=       �@n�
IsDragging . . �5>      �� $
 grip��6-   8   � ��6?   �� 5?      $
 grip��6/   8   � ��6A   5@      P� 5A       � {� .  . �5B      �� $
 grip��6-   8   � ��6?   �� 5C      $
 grip��6/   8   � ��6A   5D      5F      �� 5G   
   $ 	#FFD6D6D6��6;   �� 5H   
   $ 	#FFE7EAEA��6?   �� 5I   
   " n�6?   .R�5J      � $��Center=�6    . $��Center=�6I   .+�5K      F� $
 grip��6   d����        �        �ا �ا ��ا     �ا ���ا ����ا �������    ��	8��  $
	 Fill=�6�   
 . 8�� $��1��6   . $��Center=�62  5L       5M      P� 5N       �@J�IsMouseOver . . �5O      �� $
 grip��6-   8   � ��6?   �� 5P      $
 grip��6/   8   � ��6A   5Q      P� 5R       �@n�
IsDragging . . �5S      �� $
 grip��6-   8   � ��6?   �� 5T      $
 grip��6/   8   � ��6A   5U      P� 5V       � {� .  . �5W      �� $
 grip��6-   8   � ��6?   �� 5X      $
 grip��6/   8   � ��6A   5Y      5[      �� 5\   
   " ��63   .R�5]      �� $ border��6   #��z��6%   #��z��6Q   #��z��6�   
 . ._�5^      �  5_      �� 5`      $
��Auto �6(   �� 5a      $
��Auto �6(   #��z��66   �� 5b      $
��Auto �6(   .+�5d      m� $ TopTick��6   # z��6/   $��4��6S   $��0,0,0,2q�6^   $	 Top=�6o   $��0��6   $y�	Collapsed=�6�   m� 5e      $ 
BottomTick��6   # z��62   $��4��6V   $��0,2,0,0q�6a   $ Bottom=�6r   $��2��6�   $y�	Collapsed=�6�   �� 5f      $ TrackBackground��6   8��  $��1q�6n   8�� $	��4.0��6�   $	��5,0q�6�   $��1��6�   $��center=�6�   ._�5g      �� $��-6,-1q�6!   .+�5h      �� $ PART_SelectionRange��6(   #��� ��6E   $	��4.0��6�   $y�Hidden=�6�   5i      5j      ]� 5k      $ 
PART_Track��6   $��1��60    5l      �� 5m      #��Z 6+   8�� 6U    5o      �� 5p      #��Z 6+   8�� 6U    5r      n� 5s      $ Thumb��6$   |�.  $��18��6E    . 8�� $��Center=�6�   $��11��6�   5u      5v      5w       5x      P� 5y       �@��TickPlacement$ TopLeft=�63   . �5z      �� $ TopTick��63    � y�$ Visible=�6H   �� 5{      $ Thumb��61   8   � ��6D   �� 5|      $ TrackBackground��6/    � ��$ 5,2,5,0q�6L   5}      P� 5~       �@��TickPlacement$ BottomRight=�63   . �5      �� $ 
BottomTick��63    � y�$ Visible=�6K   �� 5�      $ Thumb��61   8   � ��6D   �� 5�      $ TrackBackground��6/    � ��$ 5,0,5,2q�6L   5�      P� 5�       �@��TickPlacement$
 Both=�63   . �5�      �� $ TopTick��63    � y�$ Visible=�6H   �� 5�      $ 
BottomTick��63    � y�$ Visible=�6K   5�      P� 5�       �@��IsSelectionRangeEnabled . . �5�      �� $ PART_SelectionRange��63    � y�$ Visible=�6T   5�      P� 5�       �@J�IsKeyboardFocused . . �5�      �� $ Thumb��63    � ��
 ��  �6F   5�      5�      �� 5�   
   " n�6:   .R�5�      � $��Center=�6    . $��Center=�6I   .+�5�      F� $
 grip��6   �������[ �ا Ӏ�[ �ا `�S `�S �    `�S ��[ ��[ Ӏ�[     ����Ӏ�    ���ا ���ا Ӏ��ا ��[ �ا ��[ �ا 	8��  $
	 Fill=�6�   8�� 6�   5�       5�      P� 5�       �@J�IsMouseOver . . �5�      �� $
 grip��6-   8   � ��6?   �� 5�      $
 grip��6/   8   � ��6A   5�      P� 5�       �@n�
IsDragging . . �5�      �� $
 grip��6-   8   � ��6?   �� 5�      $
 grip��6/   8   � ��6A   5�      P� 5�       � {� .  . �5�      �� $
 grip��6-   8   � ��6?   �� 5�      $
 grip��6/   8   � ��6A   5�      5�      �� 5�   
   " n�6;   .R�5�      � $��Center=�6    . $��Center=�6I   .+�5�      F� $
 grip��6   ����� � �ا � � �ا ��`�S ��`�S Ӏ�`�S  �  � � �     �        �ا �ا �    �ا  � �ا  � �ا 	8��  $
	 Fill=�6�   8�� 6�   5�       5�      P� 5�       �@J�IsMouseOver . . �5�      �� $
 grip��6-   8   � ��6?   �� 5�      $
 grip��6/   8   � ��6A   5�      P� 5�       �@n�
IsDragging . . �5�      �� $
 grip��6-   8   � ��6?   �� 5�      $
 grip��6/   8   � ��6A   5�      P� 5�       � {� .  . �5�      �� $
 grip��6-   8   � ��6?   �� 5�      $
 grip��6/   8   � ��6A   5�      5�      �� 5�   
   " n�6=   .R�5�      � $��Center=�6    . $��Center=�6I   .+�5�      A� $
 grip��6   $��20��6(   $��20��63   ���#21affc$
	 Fill=�6N   ���#21affc6]   A� 5�      $��6��6   $��6��6$   
�������$��Center=�6<   $��Center=�6Y   6v   5�       5�      P� 5�       �@J�IsMouseOver . 5�      P� 5�       �@n�
IsDragging . 5�      P� 5�       � {� .  . �5�      �� $
 grip��6-   8    � ��6?   �� 5�      $
 grip��6/   8   � ��6A   5�      5�      �� 5�   
   " ��61   .R�5�      �� $ border��6   #��z��6%   #��z��6Q   #��z��6�   
 . ._�5�      �  5�      �� 5�      $
��Auto �6+   �� 5�      #��z��6+   $
��Auto �6Q   �� 5�      $
��Auto �6+   .+�5�      m� $ TopTick��6   $��0��6/   # z��6?   $��0,0,2,0q�6c   $
 Left=�6t   $y�	Collapsed=�6�   $��4��6�   m� 5�      $ 
BottomTick��6   $��2��62   # z��6B   $��2,0,0,0q�6f   $ Right=�6w   $y�	Collapsed=�6�   $��4��6�   �� 5�      $ TrackBackground��6   8��  $��1q�6n   
�������$��1��6�   $��center=�6�   $	��0,2q�6�   $��8��6�   $ 4��6�   ._�5�      �� $��-1,-6q�6!   .+�5�      �� $ PART_SelectionRange��6(   #��� ��6E   $y�Hidden=�6�   $	��4.0��6�   5�      5�      ]� 5�      $ 
PART_Track��6   $��1��60    5�      �� 5�      #��Z 6+   $��0,-1,0,0q�6U   ���#21affc8�� $��8��6�    5�      �� 5�      #��Z 6+   8�� 6W    5�      n� 5�      $ Thumb��6$   |�.   . 8�� $	��Top=�6�   5�      5�      5�       5�      P� 5�       �@��TickPlacement$ TopLeft=�63   . �5�      �� $ TopTick��63    � y�$ Visible=�6H   �� 5�      $ Thumb��61   8   � ��6D   �� 5�      $ TrackBackground��6/    � ��$ 2,5,0,5q�6L   5�      P� 5�       �@��TickPlacement$ BottomRight=�63   . �5�      �� $ 
BottomTick��63    � y�$ Visible=�6K   �� 5�      $ Thumb��61   8   � ��6D   �� 5�      $ TrackBackground��6/    � ��$ 0,5,2,5q�6L   5�      P� 5�       �@��TickPlacement$
 Both=�63   . �5�      �� $ TopTick��63    � y�$ Visible=�6H   �� 5�      $ 
BottomTick��63    � y�$ Visible=�6K   5�      P� 5�       �@��IsSelectionRangeEnabled . . �5�      �� $ PART_SelectionRange��63    � y�$ Visible=�6T   5�      P� 5�       �@J�IsKeyboardFocused . . �5�      �� $ Thumb��63    � ��
 ��  �6F   5�      5�      �� 5�   
   " ��6%   .�5�      ��  �@��IsPressAndHoldEnabled .  6=   �� 5�       � ��
 ���� 6+   �� 5�       � ��
 ���� 6,   �� 5       � ��8   6+   �� 5      � ��8  6)    5     P� 5      �@��Orientation$ Vertical=�61   . �5     ��  � ��8  61   5     5     5	     .��5
     � $	��194��6   $��60��6   .+�5  
   �� 
���   �  J� Opacity$	  0.4Q�6$   $ 30��62   6G   �� 5  
   $	��5,0q�6   .+�5     �� -   5        ! ��Name$	! tb1��5  +   
�������$��Center=�68   $��12(�6U   $��0,15,0,5q�6d   1006{   �� 5     -   5        " ��Name$" slider��5     # �� TickFrequency$# 1Q�6I   #���  5     $��Center=�6<   $ �� Orientation$$ Vertical=�5     % � Maximum$	% 100Q�6,   & � Minimum$& 0Q�6:   $	��124��6G   $��Center=�6T   6q   #  CommonControls.CommonCheckBox  5     -   5        '  Name$' 	CheckBox1��5  ,   $��20��6?   $��20��6J   (   NormalImageSource$:( 4/CommonUserControls;component/Images/yinliangkai.png��5  ,   )   CheckImageSource$=) 7/CommonUserControls;component/Images/yinliangguanbi.png��5  ,   $	��0,5q�6w   6�   5     5     5     