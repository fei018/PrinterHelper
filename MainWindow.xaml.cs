using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrinterHelper
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DisplayConfigInfo();

            PrinterHelp.ErrorEvent += PrinterHelp_ErrorEvent;
            PrinterHelp.MsgEvent += PrinterHelp_MsgEvent;

            lv_LocalPrinterList_Refresh();
        }

        private List<ConfigInfo> _configInfos;

        //---- function

        #region private void lv_LocalPrinterList_Refresh()
        private async void lv_LocalPrinterList_Refresh()
        {
            await Task.Delay(1000);
            lv_LocalPrinterList.ItemsSource = PrinterInfo.GetAllPrinterList();
        }
        #endregion

        #region private void TextboxStatus_AppendLine(string text)
        private void TextboxStatus_AppendLine(string text)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                txt_Status.AppendText(text + "\r\n\r\n");
            });
        }
        #endregion

        #region PrinterHelp_MsgEvent(object sender, string e)
        private void PrinterHelp_MsgEvent(object sender, string e)
        {
            TextboxStatus_AppendLine(e);
        }

        private void PrinterHelp_ErrorEvent(object sender, string e)
        {
            TextboxStatus_AppendLine(e);
        }
        #endregion        

        #region DisplayConfigInfo()
        private void DisplayConfigInfo()
        {
            try
            {
                TextboxStatus_AppendLine("Local IP: " + UtilityTools.GetLocalIPv4().ToString());

                _configInfos = ConfigInfo.GetConfigInfo();

                lv_SitePrinterList.ItemsSource = _configInfos;
            }
            catch (Exception ex)
            {
                TextboxStatus_AppendLine(ex.Message);
            }

            TextboxStatus_AppendLine("------------------------------------");
        }

        #endregion

        #region ShowProgressBar(bool show)
        private void ShowProgressBar(bool show)
        {
            if (show)
            {
                progressBar1.Visibility = Visibility.Visible;
                btn_AddSitePrinters.IsEnabled = false;
                btn_DeleteOldPrinters.IsEnabled = false;
                lv_LocalPrinterList.IsEnabled = false;
                lv_SitePrinterList.IsEnabled = false;
            }
            else
            {
                progressBar1.Visibility = Visibility.Hidden;
                btn_AddSitePrinters.IsEnabled = true;
                btn_DeleteOldPrinters.IsEnabled = true;
                lv_LocalPrinterList.IsEnabled = true;
                lv_SitePrinterList.IsEnabled = true;
            }
        }
        #endregion


        //------ control event

        #region btn_AddSitePrinters_Click(object sender, RoutedEventArgs e)
        private async void btn_AddSitePrinters_Click(object sender, RoutedEventArgs e)
        {
            if (_configInfos == null || _configInfos.Count <= 0)
            {
                MessageBox.Show("Printer config is null.");
                return;
            }

            var result = MessageBox.Show(this, "Add site all printers ?", "Add Site Printers",
                                         MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    ShowProgressBar(true);

                    await Task.Run(() =>
                    {
                        PrinterHelp.AddNewPrinters(_configInfos);
                    });

                    TextboxStatus_AppendLine("Install done.");
                }
                catch (Exception ex)
                {
                    TextboxStatus_AppendLine(ex.Message);
                }
                finally
                {
                    ShowProgressBar(false);

                    lv_LocalPrinterList_Refresh();
                }
            }
        }
        #endregion

        #region btn_DeleteOldPrinters_Click(object sender, RoutedEventArgs e)
        private async void btn_DeleteOldPrinters_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Delete old ip printers ?", "Delete Old Printers",
                                         MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    ShowProgressBar(true);

                    await Task.Run(() =>
                    {
                        PrinterHelp.DeleteOldIPPrinters_OtherSubnet();
                    });

                    TextboxStatus_AppendLine("Delete done.");
                }
                catch (Exception ex)
                {
                    TextboxStatus_AppendLine(ex.Message);
                }
                finally
                {
                    ShowProgressBar(false);
                    lv_LocalPrinterList_Refresh();
                }
            }
        }
        #endregion

        #region txt_Status_TextChanged(object sender, TextChangedEventArgs e)
        private void txt_Status_TextChanged(object sender, TextChangedEventArgs e)
        {
            txt_Status.Focus();
            txt_Status.CaretIndex = txt_Status.Text.Length;
            txt_Status.ScrollToEnd();
        }
        #endregion

        #region ListView LocalPrinterList ContextMenu
        private void lv_LocalPrinterList_cm_Preferences_Click(object sender, RoutedEventArgs e)
        {
            var printer = lv_LocalPrinterList.SelectedItem as PrinterInfo;
            printer?.CallPreferencesGUI();
        }

        private void lv_LocalPrinterList_cm_SetDefault_Click(object sender, RoutedEventArgs e)
        {
            var printer = lv_LocalPrinterList.SelectedItem as PrinterInfo;
            printer?.SetDefaultPrinter();
        }

        private void lv_LocalPrinterList_cm_Properties_Click(object sender, RoutedEventArgs e)
        {
            var printer = lv_LocalPrinterList.SelectedItem as PrinterInfo;
            printer?.CallPropertiesGUI();
        }

        private void lv_LocalPrinterList_cm_ShowPrintJob_Click(object sender, RoutedEventArgs e)
        {
            var printer = lv_LocalPrinterList.SelectedItem as PrinterInfo;
            printer?.CallShowPrintJobGUI();
        }

        private void lv_LocalPrinterList_cm_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var printer = lv_LocalPrinterList.SelectedItem as PrinterInfo;
                printer?.Delete();
                printer?.TcpIPPort?.Delete();
            }
            catch (Exception ex)
            {
                TextboxStatus_AppendLine(ex.Message);
            }
            finally
            {               
                lv_LocalPrinterList_Refresh();
            }
        }
        #endregion

        #region ListView SitePrinterList ContextMenu
        private async void lv_SitePrinterList_cm_Add_Click(object sender, RoutedEventArgs e)
        {
            var info = lv_SitePrinterList.SelectedItem as ConfigInfo;

            if (info != null)
            {
                try
                {
                    ShowProgressBar(true);

                    await Task.Run(() =>
                    {
                        PrinterHelp.AddNewPrinter(info);
                    });
                }
                catch (Exception ex)
                {
                    TextboxStatus_AppendLine(ex.Message);
                }
                finally
                {
                    ShowProgressBar(false);

                    lv_LocalPrinterList_Refresh();
                }
            }
        }
        #endregion

    }
}
