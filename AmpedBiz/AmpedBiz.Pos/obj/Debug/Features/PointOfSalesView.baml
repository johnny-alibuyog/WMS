   M S B A M L   `   `   `  ����G  CAmpedBiz.Pos, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:AmpedBiz.Pos.FeaturesAmpedBiz.Pos.Features  5      _4clr-namespace:AmpedBiz.Pos.Common.AttachedProperties&AmpedBiz.Pos.Common.AttachedProperties  5	      O,clr-namespace:AmpedBiz.Pos.Common.ExtentionsAmpedBiz.Pos.Common.Extentions  5
      +    %AmpedBiz.Pos.Features.PointOfSaleView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      J FReactiveUI.WPF, Version=9.15.0.0, Culture=neutral, PublicKeyToken=null rxuihttp://reactiveui.net  5      V RMaterialDesignThemes.Wpf, Version=2.5.1.1345, Culture=neutral, PublicKeyToken=null:md1http://materialdesigninxaml.net/winfx/xaml/themes  5      /local#clr-namespace:AmpedBiz.Pos.Features  5      =ap4clr-namespace:AmpedBiz.Pos.Common.AttachedProperties  5	      5ex,clr-namespace:AmpedBiz.Pos.Common.Extentions  5
        � 	Resources  5         LabelControl �� 
TargetType   MaterialDesignBody2TextBlock �� BasedOn �� Property �� Value  InputControl  NumericHeader=  7System.Windows.Controls.Primitives.DataGridColumnHeader & "MaterialDesignDataGridColumnHeader �� TextAlignment  NumericElement  NumericEditingElement 0 ,MaterialDesignDataGridTextColumnEditingStyle��%  &	        7  &	 �     &	 /    7  &	 -    &	 x    7  �� 5   
   " ��6%   8   .�5      ��  � ��$ Bottom=�62   �� 5       �@��TextAlignment$ Right=�6.   �� 5       �@��Padding$ 0 0 20 6q�6(   5      �� 5   
   " ��6%   .�5      ��  � ��$ Center=�62   �� 5       � ��$ 	20 0 50 0q�6'   5      �� 5   
   "  6'   8   .�5      ��  � ��$ Right=�64   �� 5       � ��$
 Auto��6&   �� 5       � �� 5      �� 5      .R�5      �� $
��Wrap=�6$   $ Right=�6I   ��68   �� 6I   5      5       5!      �� 5#   
   " ��6'   .�5$      ��  � ��$ Right=�64   5%      �� 5'   
   " ��6.   8   .�5(      ��  � ��$ Right=�64   5)      5*      .��5,      � $��25q�6   $	��Top=�6    � RowDefinitions 5-   
   �� 5.      $��* �6   6)   �� 5/      $
��Auto �6   6,   �� 50      $��* �6   6)    � ColumnDefinitions 53   
   �� 54      $��* �6   6+   .+�58   
   � $��0��6    59      �� 5:      $
��Auto �6    60   �� 5;      $
��Auto �6    60    5>      �� 5?      $��* �6#   6/   �� 5@      $��* �6#   6/   �� 5A      $��75 �6#   60   .+�5D      �� $��0��6    # MaterialDesignHeadlineTextBlock#��[ 6%   $	��5 0q�6^   Sales Details6z   �� 5F      $��1��6   $��0��6&    �� Orientation$ Vertical=�66   	 � IsSharedSizeScope	 . .+�5G      �  5H      �� 5I      $
��Auto �6+   
 z� SharedSizeGroup$
 TextBock��68   �� 5J      $��* �6+   .+�5L      �� $��0��5M      $��	Customer:��5N      #��[  5O      5P      �� 5X      -   5        $��CustomerComboBox��5[      $��1��5Y       �� 
IsEditable . )  #MaterialDesignThemes.Wpf.HintAssist   HintOpacity$	 .26Q�5\      ��5]      8   2AmpedBiz.Pos.Common.Extentions.MultiStyleExtension  *1/MaterialDesignFloatingHintComboBox InputControl+��5^      �� 5_      .R�5`   "   �� ��6,   �� 5a      5c      5d      � 5e       5f      �� 5g      $
��Auto �6+   $
 TextBock��68   �� 5h      $��* �6+   .+�5j      �� $��0��5k      $��Invoice Number:��5l      #��[  5m      5n      �� 5o      -   5        $��InvoiceNumberTextBox��5t      $��1��5p      {�.  $	 .26Q�5r      ��5s        *$"MaterialDesignTextBox InputControl+.��5t   5   5u      5v      5w      �� 5y      $��1��6   $��1��6&   $ Vertical=�66   	 . .+�5z      �  5{      �� 5|      $
��Auto �6+   $
 Label��68   �� 5}      $��* �6+   .+�5      �� $��0��5�      $��	Sales On:��5�      #��[  5�      5�      (  "System.Windows.Controls.DatePicker  5�      -   5        $��SalesOnDatePicker��5�      $��1��5�      {�.  $	 .26Q�5�      ��5�        *31MaterialDesignFloatingHintDatePicker InputControl+5�      5�      � 5�       5�      �� 5�      $
��Auto �6+   $
 Label��68   �� 5�      $��* �6+   .+�5�      �� $��0��5�      $��	Sales By:��5�      #��[  5�      5�      �� 5�      -   5        $��SalesByTextBox��5�      $��1��5�      {�.  $	 .26Q�5�      ��5�        *$"MaterialDesignTextBox InputControl+.��5�   /   5�      5�      5�      �� 5�      -   5        $��CreateItemButton��5�      $��2q�5�      $��1��5�      $��2��5�       � ToolTip$
 Icon��5�      $��Center=�5�      $��Bottom=�5�      ��5�      0  *MaterialDesignThemes.Wpf.PackIconExtension  *Plus+5�      5�      �� 5�   
   $��1��6   $
��Auto��6"   $��0 10 0 0q�60   .+�5�      &   System.Windows.Controls.DataGrid  -   5        $��ItemsDataGridView��5�         CanUserSortColumns .     CanUserAddRows .     AutoGenerateColumns .     SelectionUnit$ FullRow=�5�      ��. $
��Auto=�5�      $
��Auto=�5�      $ �� IsSynchronizedWithCurrentItem$
 TrueY�5�      -  'MaterialDesignThemes.Wpf.DataGridAssist   CellPadding$ 4 2 2 2q�5�         ColumnHeaderPadding$ 4 2 2 2q�5�         EnableEditBoxAssist .  $	��294��5�      I   CAmpedBiz.Pos.Common.AttachedProperties.ScrollToViewAttachedProperty   SelectedItem 5�      �� *	ViewModel+ �� Mode$ TwoWay=�   Columns 5�      4	  .System.Windows.Controls.DataGridTemplateColumn	  5�      -   5        ActionColumn��,
  &System.Windows.Controls.DataGridColumn 
  Header$ Action��5�       	  CellTemplate 5�      �� 5�       �� DataType$ md:PackIconKind��6+   .R�5�   "   �� $ 
Horizontal=�6-    � Cursor$
 Hand��6F   .+�5�   &   �� $
��Left=�5�   )   $, &MaterialDesignFloatingActionMiniButton��5�   )    *	 &MaterialDesignFloatingActionMiniButton#��[	 5�   )   $��20��5�   )   $��20��65   ��5�   )   ��  �� Path$! ViewModel.DeleteItemCommand� �� RelativeSource �� *FindAncestor+ �� AncestorType" >���5�   )   �� .��5�   *   '  !MaterialDesignThemes.Wpf.PackIcon  �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.5.1.1345, Culture=neutral, PublicKeyToken=null]]    Kind$  Delete=�66   $��12��6D   $��12��6P   5�   '   5�   #   5�      5�      0  *System.Windows.Controls.DataGridTextColumn  5�      ! 
  Width5  /System.Windows.Controls.DataGridLengthConverter$	! 310 5�      -   5        ProductColumn��$ Product��5�      " 
  
IsReadOnly" . 1  +System.Windows.Controls.DataGridBoundColumn#   Binding# 5�      �� *	Product+5�        5�      $
! Auto 5�      -	   5        BarcodeColumn��$ Barcode��5�      " . # 5�      �� *	Barcode+5�        5�      $! 75 5�      -
   5        
UnitColumn��$
 Unit��5�      " . $ Right=�5�      # 5�      �� *Unit+5�      9  3MaterialDesignThemes.Wpf.MaterialDataGridTextColumn  5�      $	! 100 5�      -   5        QuantityColumn��$	 Qty��5�      $ 
  HeaderStyle#$ [ 5�      %   ElementStyle#% [ 5�      &   EditingElementStyle#& [ 5�      # 5�      �� *
Quantity+5�        5�      $	! 100 5�      $ Price��5      -   5        PriceColumn��#$ [ 5     #% [ 5     #& [ 5     # 5     �� *Price+5       5     $	! 100 5	     $ Total��5
     " . -   5        TotalColumn��#$ [ 5     #% [ 5     #& [ 5     # 5     �� *Total+5     5     �� 5     $
��Left=�6   $ 
Horizontal=�64   $	��0 7q�6M   .+�5     � ��6   �� ' �� ElementName$' ActionColumn��$ ActualWidth�.+�5     �� $��5,0,0,0q�6    63   5     � 5     ��6   �� $' ProductColumn��$ ActualWidth�.+�5     �� $��5,0,0,0q�6    63   5     � 5     ��6   �� $' BarcodeColumn��$ ActualWidth�.+�5     �� $��5,0,0,0q�6    63   5     � 5      ��6   �� $' QuantityColumn��$ ActualWidth�.+�5!     �� $��5,0,0,0q�6    63   5"     � 5#     ��6   �� $' 
UnitColumn��$ ActualWidth�.+�5$     �� $��5,0,0,0q�6    63   5%     � 5&     ��6   �� $' PriceColumn��$ ActualWidth�.+�5'     �� ( �� Padding$( 0 0 5 0q�6    $ Right=�62   #��[  6H   $��Total:��6n   6~   5(     � 5)     ��6   �� $' TotalColumn��$ ActualWidth�.+�5*     �� -   5        $��GrandTotalTextBlock��5*  2   $( 0 0 5 0q�6    $ Right=�6M   ��6c   �� *PointOfSale.GrandTotal+5+     5,     5-     �� 5/  
   $��2��6   59     5;     5<     