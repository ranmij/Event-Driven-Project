   M S B A M L   `   `   `  ����J  FInventorySystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:InventorySystemInventorySystem  5   	       InventorySystem.LoginForm   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   S OHandyControl, Version=3.4.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5   	   )localclr-namespace:InventorySystem  5   	   -   5        $��	LoginForm��5   	     � Icon$  /Resources/ic_icon.ico��5	       � 
ResizeMode$ NoResize=�5
   	    � SizeToContent$ Height=�5   	   $	��570��5   	   $	��400��6    � WindowStartupLocation$ CenterScreen=�6"    � WindowStyle$
 None=�5   	    � AllowsTransparency$
 True��62   $	��400��6L   $	��570��6[   ��6   Z� .��5      � -   5         �Name$ GridWrap��5      ��5      �� *Width+ �� ElementName$ 	LoginForm����5      �� *Height+$ 	LoginForm�� � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   ��5   
   �� 5      	 �� ImageSource$	 /Resources/bg_login.png��6   .+�5   
   ]� -   5        $��TopPanel��5      $��Center=�5      $	��Top=�5      $	��200��5      $
��True��5      $��0��5      ��5      �� *Width+$ GridWrap��.+�5      �� 
 �� Orientation$
 
Horizontal=�6   $��Right=�5       $��24��5!      $	��Top=�5"      $��4 4 8 4q�5#      .+�5$      �� -   5         ��Name$ CloseButton��5$      $��0q�5%      $��0q�5&      $��0q�5'      ��5$   .   Z� .��5(      &   HandyControl.Controls.ImageBlock     Source$ /Resources/ic_close.png��6$   $��16��6E   $��16��6Q   5)      5*      � 5+      ��6   �� *Width+$ TopPanel��.+�5,      � $��Center=�6   $��Center=�5-      $	��300��5.      .+�5/      ��    login_form_label#��[  6    $��25(�50       ���#fff$
��Left=�52       �� 53      $��/Resources/ic_user.png��6   $��64��6<   $��64��6H   $��Right=�54      55      56      57      � 58   
   -   5        $ BottomPanel��58      $��Stretch=�59      $��Stretch=�5:      $��1��5;      .+�5<      � -   5        $ BottomWrapperPanel��5<      $	��300��6/   $	��359��5=      .+�5>      �� -   5         ��Name$ InputsPanel��5>      $
 Vertical=�62   $
��Auto��5?      $
��Auto��6+   $��Center=�5@      $��Center=�5A      $��0 0 0 40q�5B      .+�5C      �� -   5        $��
ErrorLabel��5C   >   $
��Null��6    ���#f00$y�Hidden=�5D        �� Padding$ 0 0 0 5q�64   #  HandyControl.Controls.TextBox  5E      -	   5          Name$ UsernameTextBox��5E   !   $��40��6:   $	��250��6F   '  !HandyControl.Controls.InfoElement   	Necessary$
 True��6R   $��0 0 0 5q�5F   !   )  #HandyControl.Controls.BorderElement   CornerRadius$ 0��62      Placeholder$ Username��5G   !   '  !HandyControl.Controls.PasswordBox  5H      -
   5        $��PasswordTextBox��5J   !   $ Password��5H   %   $��40��6K   $	��250��6W   $
 True��6c   $��	0 20 0 10q�5I   !   $ 0��64      ShowEyeButton$
 True��5J   8   5L      �� 5M      -   5        $ ForgotPasswordButton��5M   I     CustomButtonStyle#���  6   ���#FFF$��0q�5N   /   $��Forgot password?��5O      $��Right=�68   $��0 0 0 8q�6T   �� 5P      -   5        $ LoginButton��5P      $��LOGIN��62   $	��250��6B   $��40��6N   ���#8F43EE���#FFF$��0q�6�   $ 0��6�   �� 5Q      -   5        $ SignupButton��5Q   I   #���  6   $��0 20 0 0q�6_   ���#FFF$��0q�5R   /   $��Create an account?��5S      5U      5V      5X      5Y      5Z      