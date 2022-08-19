using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PrinterHelper
{
    public class ArgumentMode
    {
        public void Run(string[] args)
        {
            try
            {
                switch (args[0])
                {
                    case "-q":
                        AddPrinter_Quiet();
                        break;

                    default:
                        break;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Application.Current.Shutdown();
            }
        }

        private void AddPrinter_Quiet()
        {
            try
            {
                PrinterHelp.ErrorEvent += ConsoleOutput;
                PrinterHelp.MsgEvent += ConsoleOutput;

                PrinterHelp.DeleteOldIPPrinters_OtherSubnet();

                if (ConfigInfo.ConfigFileExists(out string config))
                {
                    PrinterHelp.AddNewPrinters(ConfigInfo.GetConfigInfo());
                }
            }
            catch (Exception ex)
            {
                ConsoleOutput(null, ex.GetBaseException().Message);
            }
        }

        private void ConsoleOutput(object sender, string e)
        {
            Console.WriteLine(e);
        }
    }
}
