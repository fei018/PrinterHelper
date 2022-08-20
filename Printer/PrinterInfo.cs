using System;
using System.Collections.Generic;
using System.Management;
using System.Management.Automation;
using System.Linq;
using System.Diagnostics;
using System.IO;

namespace PrinterHelper
{
    public class PrinterInfo
    {
        public string Name { get; set; }

        public bool Local { get; set; }

        public bool Network { get; set; }

        public string PortName { get; set; }

        public string ServerName { get; set; }

        public PrinterTCPIPPort TcpIPPort { get; set; }

        public string GetIP()
        {
            return TcpIPPort?.HostAddress;
        }

        //

        #region + public void Delete2()
        public void Delete()
        {
            try
            {
                ManagementScope mgmtscope = new ManagementScope(@"\root\cimv2");
                var query = new ObjectQuery($"Select * from Win32_Printer Where Name='{Name}'");

                using (var objsearcher = new ManagementObjectSearcher(mgmtscope, query))
                using (var printers = objsearcher.Get())
                {
                    foreach (ManagementObject printer in printers)
                    {
                        printer.InvokeMethod("CancelAllJobs", null);
                        printer.Delete();
                        printer.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Delete Printer {Name}\r\n{ex.GetBaseException().Message}");
            }
        }
        #endregion

        #region + public void Delete()
        public void Delete_PS()
        {
            try
            {
                using (var ps = PowerShell.Create())
                {
                    ps.AddCommand("Remove-Printer")
                         .AddParameter("ComputerName", Environment.MachineName)
                         .AddParameter("Name", Name);

                    var result = ps.Invoke();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Delete Printer {Name}, {ex.Message}");
            }
        }
        #endregion

        private string _printui = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "printui.exe");

        #region public void CallPreferencesGUI()
        public void CallPreferencesGUI()
        {
            Process.Start(_printui, $"/n \"{Name}\" /e");
        }
        #endregion

        #region public void SetDefaultPrinter()
        public void SetDefaultPrinter()
        {
            Process.Start(_printui, $"/n \"{Name}\" /y");
        }
        #endregion

        #region public void CallPropertiesGUI()
        public void CallPropertiesGUI()
        {
            Process.Start(_printui, $"/n \"{Name}\" /p");
        }
        #endregion

        #region public void CallShowPrintJobGUI()
        public void CallShowPrintJobGUI()
        {
            Process.Start(_printui, $"/n \"{Name}\" /o");
        }
        #endregion

        // static

        #region + public static List<PrinterInfo> GetAllPrinterList()
        public static List<PrinterInfo> GetAllPrinterList()
        {
            var printerList = new List<PrinterInfo>();

            var tcpPorts = PrinterTCPIPPort.GetList();

            ManagementScope mgmtscope = new ManagementScope(@"\root\cimv2");
            var query = new ObjectQuery("Select * from Win32_Printer");

            using (var objsearcher = new ManagementObjectSearcher(mgmtscope, query))
            using (var printers = objsearcher.Get())
            {
                foreach (ManagementObject printer in printers)
                {
                    try
                    {
                        var p = new PrinterInfo()
                        {
                            Name = printer["Name"]?.ToString(),
                            Local = (bool)printer["Local"],
                            Network = (bool)printer["Network"],
                            PortName = printer["PortName"]?.ToString(),
                            ServerName = printer["ServerName"]?.ToString()
                        };

                        if (p.PortName != null)
                        {
                            p.TcpIPPort = tcpPorts.FirstOrDefault(t => t.Name == p.PortName);
                        }

                        printerList.Add(p);
                    }
                    catch (Exception)
                    {
                    }
                    finally
                    {
                        printer.Dispose();
                    }
                }
            }

            return printerList;
        }
        #endregion

        #region + public static List<PrinterInfo> GetIPPrinterList()
        public static List<PrinterInfo> GetIPPrinterList()
        {
            var ipPrinters = new List<PrinterInfo>();

            var printers = GetAllPrinterList();
            if (printers.Count <= 0)
            {
                return ipPrinters;
            }

            foreach (var p in printers)
            {
                if (p.TcpIPPort != null)
                {
                    ipPrinters.Add(p);
                }
            }

            return ipPrinters;
        }
        #endregion
    }
}
