   M S B A M L   `   `   `  ����I  Eyp_02_trushkin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:yp_02_trushkin.Accountyp_02_trushkin.Account  5      $    yp_02_trushkin.Account.Profile   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6local$clr-namespace:yp_02_trushkin.Account     5        N� Title$  Profile��5	      .��5      � 
������� � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   � $��0��6   $
��Left=�6   $��Center=�67   $
��0 10q�6R   .+�5      ��  �� Orientation$ 
Horizontal=�6   $��Center=�62   $��Center=�6O   .+�5      �� $��	/logo.png��6   $��40��6+   $��40��66   $
��10,0q�6B   �� 5      $��Профиль��6   $��18(�6*   $
��Bold"�68   $��Center=�6J   $��Center=�6e    
  Labels#��[  6�   5      5      �� 5      $ 
Horizontal=�6   $��Center=�62   $��	0,10,0,10q�6O   .+�5       �� -   5         ��Name$ BtnActiveAds��5       $+��%Активные объявления��6/   $
��10,5q�5"      $��16(�6    ���#2E7D32
�������$	��200��5$      $��5q�6    � Cursor$
 Hand��6(   �� 5&      -   5        $ BtnCompletedAds��5&      $��Завершённые��62   $
��10,5q�5(      $��16(�6    
�������
�������$	��200��5*      $��5q�6   $
 Hand��6(   5+      �� 5.   
   -   5         ��Name$ AdsListView��5.      $��Stretch=�6(   $
��Auto=�5/      $
��1240��50      $��1��6   $��
17,0,17,10q�6)   ��51      �� 52      .R�53      A�  A� Columns$ 3��6"   $��Stretch=�6.   54      ��56      �� 57      .R�58      �� $��10q�6    �� Padding$ 10q�6)   $��1q�59      
������� �� CornerRadius$ 10��5:      
�������$	��300��5;      $	��250��6&   	 J� Effect	 5<      3  -System.Windows.Media.Effects.DropShadowEffect  5=      
   
BlurRadius$
 8Q�6/      ShadowDepth$ 2Q�6>      Opacity$	 0.2Q�6N   ._�5@      �� $��Center=�6%   .+�5A      �� $	��150��6$   $	��150��60   $��UniformToFill=�6=   $��0 5 0 10q�5B       $��Center=�62   ��5C   "   �� 5D   &    �� Path$ ad_image�6.    �� TargetNullValue 5E   *   ��5F   .   	/logo.png��6E   5H   '   5J      �� 5L      $��16(�5M   $   $
��Bold"�62    �� TextAlignment$ Center=�5N   $   $
��Wrap=�6;   $��5 2 5 0q�5O   $   ��5L   (   �� *title+5O   $   �� 5Q      $��16(�5R   $   $
��Bold"�62   ���#2E7D32$ Center=�5S   9   $��5 2 5 0q�5T   $   ��5Q   (   �� *
ad_price+ �� StringFormat$ {0} ₽��5T   $   �� 5V      $��14(�5W   $   
�������$ Center=�5X   $   $	��5 0q�6;   ��5V   (   �� *
CityName+5X   ;   �� 5Z      $��14(�5[   $   
�������$ Center=�5\   $   ��5Z   (   �� *CategoryName+5\   $   �� 5^      $��14(�5_   $   
�������$ Center=�5`   $   ��5^   (   �� *
TypeName+5`   $   5a      5b      5c      5e      �� 5h   
   $��3��6   $ 
Horizontal=�6"   $��Stretch=�6;   $��10q�6Y   .+�5i      �� $ 
Horizontal=�6   $
��Left=�62   .+�5j      �� -   5        $ BtnAdd��5j      $��Добавить��6)   $	��120��6<   $��35��6H     Buttons#��[ 6i   $��5q�6�   �� 5k      -   5        $ 	BtnDelete��5k      $��Удалить��6,   $	��120��6>   $��35��6J   #��[ 6n   $��5q�6�   5l      �� 5n      $ 
Horizontal=�6   $��Right=�62   .+�5p      �� -   5        $��
Назад��5p      $	��120��6)   $��35��65   #��[ 6Z   $��	10,0,10,0q�6{   �� 5q      -   5        $��TotalProfitTextBlock��5q        
TextBlocks#��[ 68   $��16(�6\   $
��Bold"�6j   $��Center=�6|   5r      5s       � 5t   
   -   5        $��ProfileFrame��5t      $��3��6$     � NavigationUIVisibility$ Hidden=�65   5u      5v      