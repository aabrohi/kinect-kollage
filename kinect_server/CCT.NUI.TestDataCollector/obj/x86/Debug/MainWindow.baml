   M S B A M L   `   `   `  ����T  PCCT.NUI.TestDataCollector, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:CCT.NUI.TestDataCollectorCCT.NUI.TestDataCollector  5   	   I ECCT.NUI.Visual, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:CCT.NUI.Visual;assembly=CCT.NUI.VisualCCT.NUI.Visual 5   	   [ WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms 5   	   *    $CCT.NUI.TestDataCollector.MainWindow   5   	   ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   6uc'clr-namespace:CCT.NUI.TestDataCollector     5   	   Ectrl4clr-namespace:CCT.NUI.Visual;assembly=CCT.NUI.Visual     5   	   Owf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     5   	   -   5          � Title$*  $Candescent NUI - Test Data Collector��5   	   $
��1200��66   $	��768��6C   $
��1200��6P   $	��786��6`    � Icon$? 9/CCT.NUI.TestDataCollector;component/Images/organizer.ico��6�    � 	Resources 5      3   -CCT.NUI.TestDataCollector.MultiValueConverter   MultiValueConverter   
ValueCount  
FrameStyle �� 
TargetType$  CCT.NUI.Visual.WpfVideoControl �� Property �� Value �� Triggers �� Binding	 �� Path
 �� Value �� Radius �� 
KernelType��%  &	        &	        5	   
   $ 2��6=   �� 5
   
   "  6#   .�5      ��  � ��$	 100��6&   �� 5       � ��$ 65��6'   �� 5       � ��
 � ���6,   -   5         5      �� 5       6   �� $	 
IsSelected�	
 False6B   .a�5      ��  � ��$ 0q�68   �� 5       �@J�BitmapEffect 5      �� 5      $ 2Q�6/   $	 Box=�6:   5      5      �� 5       6   �� $	 
IsSelected�
 True6B   .a�5      ��  � ��$ 2q�68   5      5      5      .��5      ]� .+�5   
   �� $	��Top=�6   $��1q�6&   
�������._�5       c� -   5        $��toolBar��5    @   $��26��6   $��Stretch=�6"   .@�5!      �� -   5        $��buttonLoadTestFrame��5!      $��Load Test Frames...��6^   ��64   �� $	 LoadFrameCommand�6~   �� 5"      -   5        $��buttonSaveTestFrame��5"      ��64   �� $	 SaveFrameCommand�{�6^   �� $	 IsFrameSelected�.��6�   Save Test Frame...6�   �� 5#      �� 5$      -   5        $��buttonRemoveFrame��5$      ��62   �� $	 RemoveFrameCommand�{�6^   �� $	 IsFrameSelected�.��6�   Remove Selected Frame6�   �� 5%      �� 5&      -   5        $��buttonStartDepthSource��5&      ��6\   �� $,	 &VideoPresenter.StartDepthSourceCommand�.��6�   Start Depth Source6�   �� 5'      �� 5(      -   5        $��buttonCapture��5(      {�.  ��6.   �� $(	 "VideoPresenter.CaptureFrameCommand�.��6|   Capture Frame6�   �� 5)      -	   5        $��buttonCaptureDelayed��5)      {�.  ��65   �� $/	 )VideoPresenter.CaptureFrameDelayedCommand�.��6�   Capture Frame (Delayed)6�   5*      5+      �� 5,   
   $	��Top=�6   $��Hidden=�6,   $
��Auto=�6Q   .��5-      ��  �� Orientation$ 
Horizontal=�6   $��65��62   
�������.+�5.      �� $��10q�6   $
��Bold"�6%   ��5/      �� 50       �� 
StartPoint$
 0, 0&�6.    �� EndPoint$
 0, 1&�6@   .��51      �  � Color$ LightSteelBlue��6+    � Offset$	 0.0Q�6B   � 52      $ 
WhiteSmoke��6+   $	 1.0Q�6>   53       � LayoutTransform 55      �� 56       �� Angle$	 270Q�6*   .��57   -   Frames59      �� 5:      ��6   �� $	 Frames���5;      �� 5<      .R�5=      �� $ 
Horizontal=�6)   5>      ��5@      �� 5A      .R�5B           Stretch$
 Fill=�63   #��[ 6B      Image 6r   �� $	 Image�5C      5E      5F      5G      �� 5H   
   $	��250��6   $��Right=�6!   .@�5I      �� $��Settings��6   .��5J      ]� 
�������.+�5K      �� $
��Bold"�6   $	��Top=�6.   .��6C   Display Options6T   �� 5L      -
   5        $��checkClusterLayer��5L      $��3q�6e   $	��Top=�6p   ��68   �� $	 OptionChangeCommand� d� 	IsChecked 5M      `� 5N       `� 	Converter# [  6+    `� Mode$ OneWayToSource=�6h   .1�5O   "   �� $+	 %LayerViewModel.DisplayClusteringLayer�6*    �� Mode$ OneWayToSource=�6W   �� 5P   "   $:	 4VideoPresenter.LayerViewModel.DisplayClusteringLayer�6*   $ OneWayToSource=�6f   5Q      .��5R   .   Clustering Layer5T      �� 5U      -   5        $��checkHandLayer��5U      $��3q�6b   $	��Top=�6m   ��65   �� $	 OptionChangeCommand� 5V      `� 5W      # [  6+   $ OneWayToSource=�6h   .1�5X   "   �� $%	 LayerViewModel.DisplayHandLayer�6*   $ OneWayToSource=�6Q   �� 5Y   "   $4	 .VideoPresenter.LayerViewModel.DisplayHandLayer�6*   $ OneWayToSource=�6`   5Z      .��5[   .   Hand Data Layer5]      �� 5^      $
��Bold"�6   $	��Top=�6.   .��6C   Data Source Options6X   �� 5_      .@�5`      �� $��
Clustering��6"   .��5a      7  1System.Windows.Forms.Integration.WindowsFormsHost  -   5        $ ��propertyGridClusteringHost��5a   /      Child. 5b   "   '  !System.Windows.Forms.PropertyGrid  "  System.Windows.Forms.Control   Dock$
 Fill=�62   5c      5d      �� 5e      $��Shape��6"   .��5f        -   5        $��propertyGridShapeHost��5f   /   . 5g   "     $
 Fill=�62   5h      5i      �� 5j      $��Hand Detection��6"   .��5k        -   5        $��propertyGridHandHost��5k   /   . 5l   "     $
 Fill=�62   5m      5n      5o      5p      5q      �� 5r      $��Analysis��6   5t      5u      �� 5v   
   -   5        $��tabControlMain��5v   )   
�������.@�5w      �� $��Capture Frames��6   .��5x      .   (CCT.NUI.TestDataCollector.DisplayControl  -   5          Name$ displayControl��5x   $      ImageSource 6<   �� $ 	 VideoPresenter.VideoSource�   Layers 6t   �� $*	 $VideoPresenter.LayerViewModel.Layers�5y      �� 5z      $��Edit Frames��6   .��5{      ]� .+�5|      �� -   5        $��
panelHands��5|   9   
�������$
��Left=�6K   $	��5 5q�6a   .+�5}      �� -   5        $��buttonAddHand��5}   !   ��66   �� $	 AddHandCommand�{�6^   �� $	 IsFrameSelected�.��6�   
Add Hand6�   �� 5~      $
��Auto=�6�   ��6'   ��  �� RelativeSource �� *FindAncestor+  �� AncestorType"  ]�$	 ActualHeight�.��5      �� $	��210��6+   ��6:   �� $	 CurrentFrame.Hands���5�   "   �� 5�   &   .R�5�   *   3   -CCT.NUI.TestDataCollector.HandSettingsControl  3   -CCT.NUI.TestDataCollector.HandSettingsControl!   MarkPalmCenter! handControl_MarkPalmCenter6b   "   MarkFingers" handControl_MarkFingers6�   #   Close# handControl_Close6�   $   RemoveFinger$ panelHands_RemoveFinger6�   $��0,10,2,0q�6A   $	��200��6S   6�   5�   '   5�      5�      5�      �� 5�      $ 
Horizontal=�6!   $��Bottom=�6:   .+�5�      �� -   5        $��	labelMode��5�       �� 5�      .��6    Mouse Location:61   �� 5�      -   5        $��labelMouseLocation��5�       ��6:   ��  �� *FindAncestor+"  �$	 DataContext.DepthLocation�5�        5�      -   5        $ displayControlEdit��5�   (   .	   (CCT.NUI.TestDataCollector.DisplayControl% 	  	MouseDown$% displayControl_MouseDown��6W  &   MouseLocation& 6D   ��  �� *FindAncestor+"  �$	 DataContext.MouseLocation�$ TwoWay=� 6�   ��  �� *FindAncestor+"  �$	 DataContext.ImageSource� 6|  ��  �� *FindAncestor+"  �$'	 !DataContext.LayerViewModel.Layers�' � DataContext' 6  �� $	 CurrentFrame�5�      5�      5�      5�      5�      