﻿#pragma checksum "..\..\MainWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "12B95EE3ECC61A35A1328F34977ADBC8878CFB161A5BD4C55D5D8EC54EB15D5A"
//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.42000
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

using PrinterHelper;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace PrinterHelper {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_AddSitePrinters;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_DeleteOldPrinters;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView lv_LocalPrinterList;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem lv_LocalPrinterList_cm_Preferences;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem lv_LocalPrinterList_cm_SetDefault;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem lv_LocalPrinterList_cm_Properties;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem lv_LocalPrinterList_cm_ShowPrintJob;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem lv_LocalPrinterList_cm_Delete;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView lv_SitePrinterList;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem lv_SitePrinterList_cm_Add;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txt_Status;
        
        #line default
        #line hidden
        
        
        #line 107 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar progressBar1;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/PrinterHelper;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.btn_AddSitePrinters = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\MainWindow.xaml"
            this.btn_AddSitePrinters.Click += new System.Windows.RoutedEventHandler(this.btn_AddSitePrinters_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btn_DeleteOldPrinters = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\MainWindow.xaml"
            this.btn_DeleteOldPrinters.Click += new System.Windows.RoutedEventHandler(this.btn_DeleteOldPrinters_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.lv_LocalPrinterList = ((System.Windows.Controls.ListView)(target));
            return;
            case 4:
            this.lv_LocalPrinterList_cm_Preferences = ((System.Windows.Controls.MenuItem)(target));
            
            #line 63 "..\..\MainWindow.xaml"
            this.lv_LocalPrinterList_cm_Preferences.Click += new System.Windows.RoutedEventHandler(this.lv_LocalPrinterList_cm_Preferences_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.lv_LocalPrinterList_cm_SetDefault = ((System.Windows.Controls.MenuItem)(target));
            
            #line 64 "..\..\MainWindow.xaml"
            this.lv_LocalPrinterList_cm_SetDefault.Click += new System.Windows.RoutedEventHandler(this.lv_LocalPrinterList_cm_SetDefault_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.lv_LocalPrinterList_cm_Properties = ((System.Windows.Controls.MenuItem)(target));
            
            #line 65 "..\..\MainWindow.xaml"
            this.lv_LocalPrinterList_cm_Properties.Click += new System.Windows.RoutedEventHandler(this.lv_LocalPrinterList_cm_Properties_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.lv_LocalPrinterList_cm_ShowPrintJob = ((System.Windows.Controls.MenuItem)(target));
            
            #line 66 "..\..\MainWindow.xaml"
            this.lv_LocalPrinterList_cm_ShowPrintJob.Click += new System.Windows.RoutedEventHandler(this.lv_LocalPrinterList_cm_ShowPrintJob_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.lv_LocalPrinterList_cm_Delete = ((System.Windows.Controls.MenuItem)(target));
            
            #line 67 "..\..\MainWindow.xaml"
            this.lv_LocalPrinterList_cm_Delete.Click += new System.Windows.RoutedEventHandler(this.lv_LocalPrinterList_cm_Delete_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.lv_SitePrinterList = ((System.Windows.Controls.ListView)(target));
            return;
            case 10:
            this.lv_SitePrinterList_cm_Add = ((System.Windows.Controls.MenuItem)(target));
            
            #line 90 "..\..\MainWindow.xaml"
            this.lv_SitePrinterList_cm_Add.Click += new System.Windows.RoutedEventHandler(this.lv_SitePrinterList_cm_Add_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.txt_Status = ((System.Windows.Controls.TextBox)(target));
            
            #line 101 "..\..\MainWindow.xaml"
            this.txt_Status.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txt_Status_TextChanged);
            
            #line default
            #line hidden
            return;
            case 12:
            this.progressBar1 = ((System.Windows.Controls.ProgressBar)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

