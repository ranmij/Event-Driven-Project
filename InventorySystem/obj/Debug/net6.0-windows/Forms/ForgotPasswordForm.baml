   M S B A M L   `   `   `  ����J  FInventorySystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:InventorySystemInventorySystem  5   	   (    "InventorySystem.ForgotPasswordForm   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   S OHandyControl, Version=3.4.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5   	   )localclr-namespace:InventorySystem  5   	   -   5        $��ForgotPasswordForm��5   	     � Icon$  /Resources/ic_icon.ico��5	       � 
ResizeMode$ NoResize=�5
   	    � SizeToContent$ Height=�5   	   $	��570��5   	   $	��400��6    � WindowStartupLocation$ CenterScreen=�6"    � WindowStyle$
 None=�5   	    � AllowsTransparency$
 True��62   $	��400��6L   $	��570��6[   ��6   Z� .��5      � ���#FFF.+�5   
   �� -   5         ��Name$ CloseButton��5      $��0q�5      $��0q�5      $��4q�6%   $��16��60   $��16��6<   $	��Top=�6G   $��Right=�6_   ��5   &   Z� .��5      &   HandyControl.Controls.ImageBlock     Source$7 1pack://application:,,,/Resources/ic_closedark.png��6   $��16��6X   $��16��6d   5      ,  &HandyControl.Controls.SimpleStackPanel  5   
   -   5        $��FindContainer��5      $y�Visible=�63   $��0��6H   $	��Top=�6U   $��	0 100 0 0q�6m   $��Center=�6�   $	��300��6�   $	��200��6�   .+�5      �� $��16(�6   $��Find your account:��6&   $
��Wrap=�6@    �� Padding$	 0 8q�6T   #  HandyControl.Controls.TextBox  5      -   5        	  Name$	 UserIDTextBox��5      '  !HandyControl.Controls.InfoElement
   Placeholder$"
 Enter your email or username��60   )  #HandyControl.Controls.BorderElement   CornerRadius$ 0��6j   $��10q�6�   �� 5      -   5        $ 
FindButton��5      $��Stretch=�6)   $
��Find��6G   $
��0 10q�6V   $��30��6d   ���#8F43EE���#FFF$��0q�6�   $ 0��6�   5        5   
   -   5        $��ConfirmContainer��5      $y�	Collapsed=�66   $��0��6M   $	��Top=�6Z   $��	0 100 0 0q�6r   $��Center=�6�   $	��300��6�   $	��200��6�   .+�5      �� $��16(�6   $��Enter Code:��6&   $
��Wrap=�69   $	 0 8q�6M     5      -   5        $	 CodeTextBox��5      $ 0��6.   $��10q�6P   $
 
Enter Code��6]   �� 5      -   5        $ ConfirmCodeButton��5      $��Stretch=�60   $��Continue��6N   $
��0 10q�6a   $��30��6o   ���#8F43EE���#FFF$��0q�6�   $ 0��6�   5        5   
   -	   5        $��ChangeContainer��5      $y�	Collapsed=�65   $��0��6L   $	��Top=�6Y   $��	0 100 0 0q�6q   $��Center=�6�   $	��300��6�   $	��200��6�   .+�5        -
   5        $	 NewPasswordTextBox��5      $
 New password��65   $ 0��6`   $��10q�6�   $��	0 10 0 15q�6�     5       -   5        $	 ConfirmPasswordTextBox��5       $
 Confirm password��69   $ 0��6h   $��10q�6�   �� 5!      -   5        $ ChangePasswordButton��5!      $��Stretch=�63   $
��Done��6Q   $
��0 10q�6`   $��30��6n   ���#8F43EE���#FFF$��0q�6�   $ 0��6�   5"      5#      5$      