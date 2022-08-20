using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Management;
using System.Management.Automation;
using System.Net;
using System.Printing;

namespace PrinterHelper
{
    public class PrinterHelp
    {
        private static object _Locker_DeletePrinter = new object();

        public static event EventHandler<string> ErrorEvent;

        public static event EventHandler<string> MsgEvent;

        //// delete printer method

        #region + public static void DeleteOldIPPrinters_OtherSubnet()
        public static void DeleteOldIPPrinters_OtherSubnet()
        {
            try
            {
                var tcpPrinters = PrinterInfo.GetIPPrinterList();
                if (tcpPrinters.Count <= 0)
                {
                    return;
                }

                foreach (var printer in tcpPrinters)
                {
                    try
                    {
                        var printerSubnet = UtilityTools.ComputeNetworkAddress(IPAddress.Parse(printer.GetIP()),
                                                                               UtilityTools.GetLocalIPv4Mask())
                                                                               .ToString();

                        var localSubnet = UtilityTools.ComputeNetworkAddress(UtilityTools.GetLocalIPv4(),
                                                                             UtilityTools.GetLocalIPv4Mask())
                                                                             .ToString();

                        if (localSubnet != printerSubnet)
                        {
                            printer.Delete();
                            printer.TcpIPPort.Delete();
                        }

                        MsgEvent.Invoke(null, $"Delete Printer: {printer.Name}");
                    }
                    catch (Exception ex)
                    {
                        ErrorEvent?.Invoke(null, ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorEvent?.Invoke(null, ex.Message);
            }
        }
        #endregion


        //// add printer method

        #region + public static void AddNewPrinters(List<ConfigInfo> configInfos)
        public static void AddNewPrinters(List<ConfigInfo> configInfos)
        {
            foreach (var info in configInfos)
            {
                try
                {
                    if (PrinterExist(info.PrinterName))
                    {
                        throw new Exception($"Printer exists: {info.PrinterName}");
                    }

                    // add port
                    AddOrUpdatePrinterTcpIPPort(info.IP);

                    // install driver
                    if (!PrinterDriverExist(info.PrinterModel))
                    {
                        InstallPrinterDriver_WMI(info.PrinterModel, info.DriverInfPath);
                    }

                    // install printer
                    InstallPrinter_PS(info.PrinterName, info.PrinterModel, info.IP);

                    // set config
                    SetPrinterConfig_WMI(info.PrinterName);
                    SetPrinterConfig_Printing(info.PrinterName);                   

                    MsgEvent?.Invoke(null, $"Add Priner: {info.PrinterName}");
                }
                catch (Exception ex)
                {
                    ErrorEvent?.Invoke(null, ex.Message);
                }
            }
        }
        #endregion

        #region + public static void AddNewPrinter(ConfigInfo info)
        public static void AddNewPrinter(ConfigInfo info)
        {
            try
            {
                if (PrinterExist(info.PrinterName))
                {
                    throw new Exception($"Printer exists: {info.PrinterName}");
                }

                // add port
                AddOrUpdatePrinterTcpIPPort(info.IP);

                // install driver
                if (!PrinterDriverExist(info.PrinterModel))
                {
                    InstallPrinterDriver_WMI(info.PrinterModel, info.DriverInfPath);
                }

                // install printer
                InstallPrinter_WMI(info.PrinterName, info.PrinterModel, info.IP);

                // set config
                SetPrinterConfig_WMI(info.PrinterName);
                SetPrinterConfig_Printing(info.PrinterName);

                MsgEvent?.Invoke(null, $"Add Priner: {info.PrinterName}");
            }
            catch (Exception ex)
            {
                ErrorEvent?.Invoke(null, ex.Message);
            }
        }
        #endregion
        //// private

        // printer

        #region + private static bool PrinterExist(string printerName)
        private static bool PrinterExist(string printerName)
        {
            try
            {
                ManagementScope mgmtscope = new ManagementScope(@"\root\StandardCimv2");
                var query = new ObjectQuery($"Select * from MSFT_Printer Where Name = '{printerName}'");

                using (var objsearcher = new ManagementObjectSearcher(mgmtscope, query))
                using (var printers = objsearcher.Get())
                {
                    var success = printers.Count >= 1;

                    printers.Dispose();
                    return success;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + private static void InstallPrinter_Printing(string printerName, string driverName, string portName)
        private static void InstallPrinter_Printing(string printerName, string driverName, string portName)
        {
            PrintServer printServer = null;
            PrintQueue printQ = null;

            try
            {
                printServer = new PrintServer();
                printQ = printServer.InstallPrintQueue(printerName,
                                                   driverName,
                                                   new string[] { portName },
                                                   "winprint",
                                                   PrintQueueAttributes.ScheduleCompletedJobsFirst);

                if (printQ == null)
                {
                    throw new Exception("Install Printer Fail: " + printerName + "\r\n");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                printQ?.Dispose();
                printServer?.Dispose();
            }
        }
        #endregion

        #region + private static void InstallPrinter_WMI(string printerName, string driverName, string portName)
        private static void InstallPrinter_WMI(string printerName, string driverName, string portName)
        {
            try
            {
                using (ManagementClass printClass = new ManagementClass(@"\root\cimv2:Win32_Printer"))
                using (ManagementObject printObject = printClass.CreateInstance())
                {
                    printObject["Name"] = printerName;
                    printObject["DriverName"] = driverName;
                    printObject["PortName"] = portName;
                    printObject["DoCompleteFirst"] = true;

                    PutOptions options = new PutOptions(null, TimeSpan.FromSeconds(30), false, PutType.CreateOnly);
                    printObject.Put(options);

                    printObject.Dispose();
                }

                if (!PrinterExist(printerName))
                {
                    throw new Exception("Install Printer Fail: " + printerName + "\r\n");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + private static void InstallPrinter_WMI2(string printerName, string driverName, string portName)
        private static void InstallPrinter_WMI2(string printerName, string driverName, string portName)
        {
            try
            {
                using (ManagementClass mc = new ManagementClass(@"\root\StandardCimv2:MSFT_Printer"))
                using (ManagementObject printerObj = mc.CreateInstance())
                {
                    string methodName = "AddByExistingPort";

                    var inParam = mc.GetMethodParameters(methodName);
                    inParam["Name"] = printerName;
                    inParam["DriverName"] = driverName;
                    inParam["PortName"] = portName;

                    //var invokeOption = new InvokeMethodOptions(null, TimeSpan.FromMinutes(5));
                    mc.InvokeMethod(methodName, inParam, null);
                    mc.Dispose();
                }

                // check add succeed

                if (!PrinterExist(printerName))
                {
                    throw new Exception("Install Printer Fail: " + printerName + "\r\n");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + private static void InstallPrinter_PS(string printerName, string driverName, string portName)
        private static void InstallPrinter_PS(string printerName, string driverName, string portName)
        {
            try
            {
                using (var ps = PowerShell.Create())
                {
                    ps.AddCommand("Add-Printer")
                         .AddParameter("Name", printerName)
                         .AddParameter("DriverName", driverName)
                         .AddParameter("PortName", portName);

                    var result = ps.Invoke();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Install Printer {printerName}, {ex.Message}");
            }
        }
        #endregion

        // port

        #region + private static bool PrinterIPPortExist(string ipAddress)
        private static bool PrinterIPPortExist(string ipAddress)
        {
            try
            {
                ManagementScope mgmtscope = new ManagementScope(@"\root\cimv2");
                var query = new ObjectQuery($"Select * from Win32_TCPIPPrinterPort Where Name = '{ipAddress}'");

                using (ManagementObjectSearcher objsearcher = new ManagementObjectSearcher(mgmtscope, query))
                using (var tcps = objsearcher.Get())
                {
                    var success = tcps.Count >= 1;
                    tcps.Dispose();
                    return success;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + private static void AddOrUpdatePrinterTcpIPPort(string ipAddress)
        private static void AddOrUpdatePrinterTcpIPPort(string ipAddress)
        {
            try
            {
                using (ManagementClass portClass = new ManagementClass(@"\root\cimv2:Win32_TCPIPPrinterPort"))
                using (ManagementObject portObject = portClass.CreateInstance())
                {
                    portObject["Name"] = ipAddress;
                    portObject["HostAddress"] = ipAddress;
                    portObject["PortNumber"] = 9100;
                    portObject["Protocol"] = 1;
                    portObject["SNMPCommunity"] = "public";
                    portObject["SNMPEnabled"] = false;
                    portObject["SNMPDevIndex"] = 1;

                    PutOptions options = new PutOptions(null, TimeSpan.FromSeconds(30), false, PutType.UpdateOrCreate);
                    //options.Type = PutType.UpdateOrCreate;
                    //put a newly created object to WMI objects set             
                    portObject.Put(options);
                }

                // check whether add succeed

                if (!PrinterIPPortExist(ipAddress))
                {
                    throw new Exception("Create Or Update Printer TCPIP Port Fail: " + ipAddress + "\r\n");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        // driver

        #region + private static bool PrinterDriverExist(string driverName)
        private static bool PrinterDriverExist(string driverName)
        {
            try
            {
                ManagementScope mgmtscope = new ManagementScope(@"\ROOT\StandardCimv2");
                var query = new ObjectQuery($"Select * from MSFT_PrinterDriver Where Name = '{driverName}'");

                using (ManagementObjectSearcher objsearcher = new ManagementObjectSearcher(mgmtscope, query))
                using (ManagementObjectCollection drivers = objsearcher.Get())
                {
                    int count = drivers.Count;
                    drivers.Dispose();
                    objsearcher.Dispose();

                    if (count >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + private static void InstallPrinterDriver_WMI(string driverName, string infPath)
        private static void InstallPrinterDriver_WMI(string driverName, string infPath)
        {
            try
            {
                var mScope = CreateManagementScope_Cimv2();

                using (ManagementClass mc = new ManagementClass(mScope, new ManagementPath("Win32_PrinterDriver"), new ObjectGetOptions()))
                using (var driverInfo = mc.CreateInstance())
                {
                    driverInfo.SetPropertyValue("Name", driverName);
                    driverInfo.SetPropertyValue("InfName", infPath);
                    //dirver["FilePath"] = infDir;

                    using (var methodParam = mc.GetMethodParameters("AddPrinterDriver"))
                    {
                        methodParam["DriverInfo"] = driverInfo;

                        //var invokeOption = new InvokeMethodOptions(null, TimeSpan.FromMinutes(5));
                        var result = mc.InvokeMethod("AddPrinterDriver", methodParam, null);
                        var code = (uint)result.Properties["ReturnValue"].Value;

                        result.Dispose();

                        switch (code)
                        {
                            case 0:
                                return;

                            case 5:
                                throw new Exception("Access denied.");

                            case 87:
                                throw new Exception("The parameter is incorrect.");

                            case 1797:
                                throw new Exception("The printer driver is unknown.");

                            default:
                                throw new Exception("ReturnValue:" + code);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Install printer driver fail: {ex.Message}");
            }
        }
        #endregion

        // config

        #region + private static void SetPrinterConfig_Printing(string printerName)
        private static void SetPrinterConfig_Printing(string printerName)
        {
            try
            {
                using (var pServer = new PrintServer())
                using (var printQ = pServer.GetPrintQueue(printerName))
                {
                    try
                    {
                        printQ.DefaultPrintTicket.Duplexing = Duplexing.OneSided;
                        printQ.DefaultPrintTicket.OutputColor = OutputColor.Grayscale;
                        printQ.Commit();
                    }
                    catch (Exception) { }

                    try
                    {
                        printQ.UserPrintTicket.Duplexing = Duplexing.OneSided;
                        printQ.UserPrintTicket.OutputColor = OutputColor.Grayscale;
                        printQ.Commit();
                    }
                    catch (Exception) { }

                    printQ.Dispose();
                    pServer.Dispose();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + private static void SetPrinterConfig_WMI(string printerName)
        private static void SetPrinterConfig_WMI(string printerName)
        {
            /**
             * DuplexingMode:
             *      OneSided = 0
             *      TwoSidedLongEdge = 1
             *      TwoSidedShortEdge = 2
             * 
             * Color:
             *      black = false
             *      color = true
            **/

            try
            {
                var mScope = CreateManagementScope_StandardCimv2();
                using (ManagementClass mc = new ManagementClass(mScope, new ManagementPath("MSFT_PrinterConfiguration"), new ObjectGetOptions()))
                using (var methodParams = mc.GetMethodParameters("SetByPrinterName"))
                {
                    methodParams.SetPropertyValue("PrinterName", printerName);
                    methodParams.SetPropertyValue("Color", false);
                    methodParams.SetPropertyValue("DuplexingMode", 0);                   

                    //var invokeOption = new InvokeMethodOptions(null, TimeSpan.FromSeconds(30));
                    mc.InvokeMethod("SetByPrinterName", methodParams, null);

                    methodParams.Dispose();
                    mc.Dispose();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        //// CreateManagementScope

        #region + private static ManagementScope CreateManagementScope_Cimv2()
        private static ManagementScope CreateManagementScope_Cimv2()
        {
            var wmiConnectionOptions = new ConnectionOptions();
            wmiConnectionOptions.Impersonation = ImpersonationLevel.Impersonate;
            wmiConnectionOptions.Authentication = AuthenticationLevel.Default;
            wmiConnectionOptions.EnablePrivileges = true; // required to load/install the driver.
                                                          // Supposed equivalent to VBScript objWMIService.Security_.Privileges.AddAsString "SeLoadDriverPrivilege", True 

            var managementScope = new ManagementScope("\\root\\cimv2", wmiConnectionOptions);
            managementScope.Connect();
            return managementScope;
        }
        #endregion

        #region + private static ManagementScope CreateManagementScope_StandardCimv2()
        private static ManagementScope CreateManagementScope_StandardCimv2()
        {
            var wmiConnectionOptions = new ConnectionOptions();
            wmiConnectionOptions.Impersonation = ImpersonationLevel.Impersonate;
            wmiConnectionOptions.Authentication = AuthenticationLevel.Default;
            wmiConnectionOptions.EnablePrivileges = true; // required to load/install the driver.
                                                          // Supposed equivalent to VBScript objWMIService.Security_.Privileges.AddAsString "SeLoadDriverPrivilege", True 

            var managementScope = new ManagementScope("\\root\\StandardCimv2", wmiConnectionOptions);
            managementScope.Connect();
            return managementScope;
        }
        #endregion
    }
}
