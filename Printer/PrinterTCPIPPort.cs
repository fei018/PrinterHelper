using System;
using System.Collections.Generic;
using System.Management;
using System.Management.Automation;

namespace PrinterHelper
{
    public class PrinterTCPIPPort
    {
        public string Name { get; set; }

        public string HostAddress { get; set; }

        public string PortNumber { get; set; }

        #region + public void Delete()
        public void Delete()
        {
            try
            {
                ManagementScope mgmtscope = new ManagementScope("\\root\\StandardCimv2");
                var query = new ObjectQuery($"Select * from MSFT_PrinterPort Where Name='{Name}'");

                using (var objsearcher = new ManagementObjectSearcher(mgmtscope, query))
                using (var tcps = objsearcher.Get())
                {
                    foreach (ManagementObject tcp in tcps)
                    {
                        tcp.SetPropertyValue("ComputerName", Environment.MachineName);
                        tcp.Delete();
                        tcp.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Delete Port {Name}, {ex.Message}");
            }
        }
        #endregion

        #region MyRegion
        public void Delete_PS()
        {
            try
            {
                using (var ps = PowerShell.Create())
                {
                    ps.AddCommand("Remove-PrinterPort")
                         .AddParameter("ComputerName", Environment.MachineName)
                         .AddParameter("Name", Name);

                    var result = ps.Invoke();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Delete Port {Name}, {ex.Message}");
            }
        }
        #endregion

        #region + public static List<TCPIPPrinterPort> GetList()
        public static List<PrinterTCPIPPort> GetList()
        {
            var tcpIPProtList = new List<PrinterTCPIPPort>();

            ManagementScope mgmtscope = new ManagementScope(@"\root\cimv2");
            var query = new ObjectQuery("Select * from Win32_TCPIPPrinterPort");

            using (ManagementObjectSearcher objsearcher = new ManagementObjectSearcher(mgmtscope, query))
            using (var tcps = objsearcher.Get())
            {
                foreach (ManagementObject tcp in tcps)
                {
                    try
                    {
                        var t = new PrinterTCPIPPort()
                        {
                            Name = tcp["Name"]?.ToString(),
                            HostAddress = tcp["HostAddress"]?.ToString(),
                            PortNumber = tcp["PortNumber"]?.ToString()
                        };

                        tcpIPProtList.Add(t);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return tcpIPProtList;
        }
        #endregion
    }
}
