   M S B A M L   `   `   `  ????	  OMEGA'    !WpfApplication1.DoctorInstruction   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .??5      ?    Bg#???   6   .+?5
   
   ?? -   5        $??btnSave??5
   6   $
??Save??6   $??505,0,10,10q?6!   $??3??6E   $??Bottom=?6R   $??Center=?6m   $	??170??6?    
 BgSave#???  6?   ?? 5   
   -   5          ??Name$  TabControl4??5      $??Center=?6*   $??0,0,0,53q?6N   .@?5      ?? -   5         ??Name$ TabItem4??5   $   $
??ICSI??6   .??5      ? #???   6   .+?5      ? $??5q?6   ??5      ?? 5       ?? EndPoint$ 0.5,1&?62    ?? 
StartPoint$ 0.5,0&?6C   .??5   "   ?  ? Color$ White??6/    ? Offset$ 0Q?6=   ? 5   "   $ 	#FF6DD9FF??6/   $ 1Q?6A   5      .+?5      ?? -   5         ??Name$ lblNotes??5   ?   ???	#FF078200$??1q?68   $??Notes??6L   ???	#FF078200$??28??6s   $??Center=?6   $??
11,104,0,0q?6?    ? UseLayoutRounding . $	??153??6?   $
??Left=?6?   $	??Top=?6	  ?? 5      -   5         ??Name$ Notes??5   T   $??64??6"   $??169,104,0,0q?6.   	 ?? 	MaxLength$
	 1000??6C   $??13??6c   $
??Left=?6q   $	??Top=?6?   $	??461??6?   
 ?? AutoWordSelection
 .  ?? AcceptsReturn .  ?? TextWrapping$
 Wrap=?5   +   " ?? VerticalScrollBarVisibility$ Visible=?6?   .??6f   ??5      ?? 5   "   $ 0.5,1&?66   $ 0.5,0&?6G   .??5   &   ? $ White??63   $ 0Q?6A   ? 5   &   $ 	#37FFFF00??63   $ 1Q?6E   5   #   .??5   2   5      ?? 5      -   5        $ lblAimOfTheDay??5   ?   ???	#FF078200$??1q?68   $??Aim Of The Day??6L   ???	#FF078200$??28??6|   $??Center=?6?   $??	11,36,0,0q?6?    . $	??153??6?   $
??Left=?6?   $	??Top=?6  ?? 5       -   5        $ AimOfTheDay??5    S   $??64??6"   $??
169,36,0,0q?6.   $
	 1000??6B   $??13??6h   $
??Left=?6v   $	??Top=?6?   $	??461??6?   
 .  . $
 Wrap=?5!   +   $ Visible=?6?   .??6f   ??5"      ?? 5#   "   $ 0.5,1&?66   $ 0.5,0&?6G   .??5$   &   ? $ White??63   $ 0Q?6A   ? 5%   &   $ 	#37FFFF00??63   $ 1Q?6E   5&   #   .??5'   2   5(      ?? 5)      -   5        $ 
lblDayDate??5)   ?   ???	#FF078200$??1q?68   $??Day Date??6L   ???	#FF078200$??28??6v   $??Center=?6?   $??11,3,0,0q?6?    . $	??153??6?   $	??Top=?6?   $
??Left=?6	  (  "System.Windows.Controls.DatePicker  5*      -	   5          Name$ DayDate??5*   ^      FirstDayOfWeek$ Saturday=?6%   $??28??6?   $??	169,3,0,0q?6K   $??3??6o   $	??129??6|   $	??Top=?6?   $
??Left=?6?   7  1System.Windows.Forms.Integration.WindowsFormsHost  5+      -
   5          Name$	 WFH??5+   +   $??11,279,0,10q?68   $y?Visible=?6M      
Background 6b   Z? ?? 5,      -   5         ??Name$
 Dose??5,   /   $??28??6#   $??1??6=   $	??Top=?6J   $??169,204,0,0q?6b   $
??Left=?6w   $	??258??6?    ?? 
IsEditable . ??5-      ?? 5.   "   $ 0.5,1&?66   $ 0.5,0&?6G   .??5/   &   ? $ White??63   $ 0Q?6A   ? 50   &   $ 	#37FFFF00??63   $ 1Q?6E   51   #   53      ?? 54      -   5         ??Name$
 Add3??54   ?   
???2?2?$	??Add??68   $
??Bold"?6F   $??30??6X   $??Right=?6d   $??0,238,179,0q?6?     GlossyCloseButton#??[ 6?   $??38??6?   $	??Top=?6?   $	??143??6?   ?? 55      -   5        $
 Drug??55   /   $??28??6#   $??1??6=   $	??Top=?6J   $??169,171,0,0q?6b   $
??Left=?6w   $	??258??6?    . ??56      ?? 57   "   $ 0.5,1&?66   $ 0.5,0&?6G   .??58   &   ? $ White??63   $ 0Q?6A   ? 59   &   $ 	#37FFFF00??63   $ 1Q?6E   5:   #   5<      ?? 5=      -   5        $ Route??5=   /   $??28??6#   $??1??6>   $	??Top=?6K   $??169,239,0,0q?6c   $
??Left=?6x   $	??258??6?    . ??5>      ?? 5?   "   $ 0.5,1&?66   $ 0.5,0&?6G   .??5@   &   ? $ White??63   $ 0Q?6A   ? 5A   &   $ 	#37FFFF00??63   $ 1Q?6E   5B   #   5D      ?? 5E      -   5        $ lblDose??5E   ?   ???	#FF078200$??1q?68   $
??Dose??6L   ???	#FF078200$??28??6r   $??Center=?6~   $??
11,204,0,0q?6?    . $	??153??6?   $
??Left=?6?   $	??Top=?6  ?? 5F      -   5        $ lblDrug??5F   ?   ???	#FF078200$??1q?68   $
??Drug??6L   ???	#FF078200$??28??6r   $??Center=?6~   $??
11,171,0,0q?6?    . $	??153??6?   $
??Left=?6?   $	??Top=?6  ?? 5G      -   5        $ lblRoute??5G   ?   ???	#FF078200$??1q?68   $??Route??6L   ???	#FF078200$??28??6s   $??Center=?6   $??
11,237,0,0q?6?    . $	??153??6?   $
??Left=?6?   $	??Top=?6	  ?? 5H      -   5        $ btnReNew??5H   y   
???2?2?$??Renew??68   $
??Bold"?6H   $??30??6Z   $??	345,1,0,0q?6f   #??[ 6?   $??38??6?   $	??Top=?6?   $
??Left=?6?   $	??213??6?   ?? 5I      -   5        $ 	btnDelete??5I   ?   
???2?2?$??Delete Current Row??68   $
??Bold"?6U   $??30??6g   $??Right=?6s   $??
0,238,21,0q?6?   #??[ 6?   $??38??6?   $	??Top=?6?   $	??143??6  5L      5M      5O      5Q      5R      5S      