   M S B A M L   `   `   `  ����J  FInventorySystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:InventorySystemInventorySystem  5      !    InventorySystem.ProductCard   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:InventorySystem     5      S OHandyControl, Version=3.4.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5        � 	Resources  5	         Button.MouseOver.Background �� Color  Button.MouseOver.Border  Button.Pressed.Background  Button.Pressed.Border  Button.Disabled.Background  Button.Disabled.Border  Button.Disabled.Foreground  ProductCardTemplate �� 
TargetType ��Name J� SnapsToDevicePixels ��Name �� Triggers P� Property P� Value	 �� 
TargetName
 �� Value �� Property��%�  &	        &	 %     &	 J     &	 o     &	 �     &	 �     &	 �     &	     7   7  7  7  7  7  7  �� 5
   
   $ Transparent��6>   �� 5   
   $ Transparent��6:   �� 5   
   $ Transparent��6<   �� 5   
   $ Transparent��68   �� 5   
   $ Transparent��6=   �� 5   
   $ Transparent��69   �� 5   
   $ Transparent��6=   �� 5   
   " ��66   .R�5      �� $ border��6   #��z��6%   #��z��6O   #��z��6{   $
 true��6�   ._�5      �� $ contentPresenter��6#   $|�False��6=   #��z��6O   #��z��6�   $
��True��6�   # z 6�   #��z��6  5       5      P� 5       �@��IsDefaulted$
 true��68   . �5      �� $	 border��64   #
 � ��6H    � ��5      P� 5       �@J�IsMouseOver$
 true��61   . �5      �� $	 border��63   8
    � ��6G   �� 5      $	 border��64   8
   � ��6H   5      P� 5       � ��$
 true��6/   . �5      �� $	 border��63   8
   � ��6G   �� 5      $	 border��64   8
   � ��6H   5       P� 5!       � {�$ false��6/   . �5"      �� $	 border��63   8
   � ��6G   �� 5#      $	 border��64   8
   � ��6H   �� 5$      $	 contentPresenter��6?   8
   � ��6]   5%      5'      5(      .��5)      �� -   5        $��ProductCardButton��5)   ?   #���  6   $��0q�6i   $��0q�6u   
������ $��0q�6�   ��6X   Z� .��5*   
      HandyControl.Controls.Card  $	��300��6   $	��175��6!   $��0q�6-     EffectShadow2 J� Effect# �  6A   $
��10 8q�6j   .��5,      ��  �� CornerRadius$ 4,4,0,0��6   $	��160��6,   $	��160��68   ._�5-      &   HandyControl.Controls.ImageBlock  $��80��6?   $	��120��6K      Source 6    �� *
IMAGE_PATH+6K   5.         Footer 50      �� 51      $��10q�6   $	��160��6)   .+�53      �� -   5        $��ProductName��53       $��NoWrap=�63    	 TextBlockLargeBold#��� 	 6J   $��CharacterEllipsis=�6w   $
��Left=�55   (   $��12(�6C   ��54   (   �� *
CARD_TITLE+55   C   �� 57      -   5        $��ProductPrice��57       $��NoWrap=�64    
 TextBlockDefault#��� 
 6J   $��CharacterEllipsis=�6u   $��0,6,0,0q�58   C   $
��Left=�59   (   ��58   (   �� *	CARD_DESC+59   (   5:      5<      5=      5>      