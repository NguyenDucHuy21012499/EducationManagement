   M S B A M L   `   `   `  ����A  =COURSE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:COURSECOURSE  5   	       COURSE.SignWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:COURSE     5   	   $	��500��5   	   $	��300��6     � WindowStartupLocation$  CenterScreen=�5   	    � WindowStyle$
 None=�6.    � AllowsTransparency . 
������ .��5	      �� 
������ �� Padding$ 5,5,5,5q�6#    �� CornerRadius$ 10��65   ._�5
   
   �� -   5        
�������$ 10��5
   $   ._�5      � .+�5      �� -   5        $��X��5      $��250,5,5,450q�6?      
exitButton#��[  6T   �� 5      $��Center=�6   .+�5      ��  �� Orientation$ 
Horizontal=�6!   $
��0 20q�6:   $��Center=�6H   .+�5      �� $��Images/sign.png��6    $	��100��69   $	��100��6E   5      �� 5      $��1q�6   
������$
��35 7q�6H   $ 5��6V   ._�5      � $	��7 9q�6    � ColumnDefinitions 5      �� 5   "   $
��Auto �63   �� 5   "   $��* �63   .+�5      �� $��Images/email.png��6$   $��20��6>   $��20��6I   �� 5      -   5         ��Name$ 
EmailBlock��5   (   $��Email��6\     textHint#��[ 6i   �� 5      -   5         ��Name$ 	EmailText��5   &     textBox#��[ 6]   5      5      �� 5      $��1q�6   
������$
��35 7q�6H   $ 5��6V   ._�5      � $	��7 9q�6    5       �� 5!   "   $
��Auto �63   �� 5"   "   $��* �63   .+�5%      �� $��Images/user.png��6$   $��20��6=   $��20��6H   �� 5&      -   5        $ UsernameBlock��5&   (   $��Username��6b   #��[ 6r   �� 5'      -   5        $ UsernameText��5'   &   #��[ 6c   5(      5)      �� 5+      $��1q�6   
������$
��35 7q�6H   $ 5��6V   ._�5,      � $	��7 9q�6    5-      �� 5.   "   $
��Auto �63   �� 5/   "   $��* �63   .+�52      �� $��Images/pass.png��6$   $��20��6=   $��20��6H   �� 53      -   5        $ 	PassBlock��53   (   $��Password��6Z   #��[ 6j   G� 54      -   5        	 G�Name$	 PassText��54   *     passBox#��[ 6c   55      56      �� 57      -	   5        $��Create��57        signinButton#��[ 6J   58      59      5:      5;      5<      