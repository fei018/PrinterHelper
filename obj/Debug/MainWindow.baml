   M S B A M L   `   `   `  ????H  DPrinterHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:PrinterHelperPrinterHelper  5   	       PrinterHelper.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:PrinterHelper     5   	     ? Title$  Printer Helper 1.1??5   	   $	??600??6$   $	??900??61    ? WindowStartupLocation$ CenterScreen=?5	   	    ? Icon$ printer.ico??6.    ? 
ResizeMode$ 	CanResize=?6A   .??5      ?  ? RowDefinitions 5   
   ?? 5      $??40 ?6   ?? 5      ?? 5      ?? 5      $??30 ?6   .+?5   
   ?? $??0??6   
???????$??1q?61   $??1q?6E   ._?5      ]? .+?5      `? $	??Top=?6   .?5      c? $??40??6   .@?5      ?? -   5        $??btn_AddSitePrinters??5   !   $??Add Site Printers??5      5      ?? 5      ?? 5      -   5        $??btn_DeleteOldPrinters??5   !   $??Delete Old Printers??5      5       5!      5"      5#      5$      ?? 5'   
   $??1??6   
???????$??1q?61   $??1q?6E   ._?5(      ?  ? ColumnDefinitions 5)      ?? 5*      $
??1.2* ?6'   ?? 5+      .+?5.      ? $??0??6   $??1q?6'    5/      ?? 50      $??25 ?6(   ?? 51      .+?54      ?? $??Local Printer List??6    $??16(?6:   $??Center=?6H   $??Center=?6e   6?   ?? 56      -   5        $??lv_LocalPrinterList??56   ,   $??1??6    ?? View 57      ?? 58      .P?59   "   ?? $??Printer Name??61    ?? DisplayMemberBinding 6G   ?? *Name+?? 5:   "   $??	Port Name??61    6D   ?? *
PortName+5;       ? ContextMenu 5=      ?? 5>      .@?5?   "   m? -   5        $(??"lv_LocalPrinterList_cm_Preferences??5?   +   $??Preferences??6U   6j   m? 5@   "   -   5        $'??!lv_LocalPrinterList_cm_SetDefault??5@   +   $??Set default??6T   6i   m? 5A   "   -   5        $'??!lv_LocalPrinterList_cm_Properties??5A   +   $??
Properties??6T   6h   m? 5B   "   -   5        $)??#lv_LocalPrinterList_cm_ShowPrintJob??5B   +   $??Show Print Job??6V   6n   m? 5C   "   -   5        $#??lv_LocalPrinterList_cm_Delete??5C   +   $??Delete??6P   6`   5D      5F      5G      ? 5I      $??1??6   $??1q?6'    5J      ?? 5K      $??25 ?6(   ?? 5L      .+?5O      ?? $??Site Printer List??6    $??16(?69   $??Center=?6G   $??Center=?6d   6?   ?? 5Q      -	   5        $??lv_SitePrinterList??5Q   ,   $??1??6    5R      ?? 5S      .P?5T   "   ?? $??Printer Name??61    6G   ?? *PrinterName+?? 5U   "   $??IP??61    6=   ?? *IP+5V       5X      ?? 5Y      .@?5Z   "   m? -
   5        $??lv_SitePrinterList_cm_Add??5Z   +   $??Add Printer??6L   6a   5[      5]      5^      5_      5`      ?? 5c   
   $??2??6   
???????$??1q?61   $??1q?6E   ._?5d      ?? -   5        $??
txt_Status??5d      	 ?? TextWrapping$
	 Wrap=?6(   
 ?? 
IsReadOnly
 . " ?? VerticalScrollBarVisibility$
 Auto=?5e      .??6b   5f      5g      ?? 5j   
   $??3??6   
???????$??1q?61   $??1q?6E   ._?5k      ? -   5        $??progressBar1??5k       ? IsIndeterminate . $y?Hidden=?6F   5l      5m      5n      5o      