using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterHelper
{
    public class ConfigInfo
    {
        public string IP { get; set; }

        public string PrinterName { get; set; }

        public string PrinterModel { get; set; }

        public string DriverInfPath { get; set; }

        // function

        #region public static bool ConfigFileExists(out string configPath)
        public static bool ConfigFileExists(out string configPath)
        {
            var ip = UtilityTools.ComputeNetworkAddress(UtilityTools.GetLocalIPv4(), UtilityTools.GetLocalIPv4Mask());

            string configName = ip.ToString() + ".txt";

            // local config path
            configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configName);

            if (File.Exists(configPath))
            {
                return true;
            }

            // hhsites server path
            configPath = Path.Combine(@"\\hhsites.hiphing.hk\netlogon\SoftwareDeployment\PrinterHelper", configName);

            return File.Exists(configPath);
        }
        #endregion

        #region + public static List<ConfigInfo> GetConfigInfo()
        public static List<ConfigInfo> GetConfigInfo()
        {
            if (!ConfigFileExists(out string config))
            {
                throw new Exception("Error: " + Path.GetFileName(config) + " not exists.");
            }

            string[] lines = File.ReadAllLines(config);

            var list = new List<ConfigInfo>();
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (line.TrimStart().StartsWith("#"))
                {
                    continue;
                }

                var sub = line.Split(',');
                if (sub.Length == 4)
                {
                    var info = new ConfigInfo()
                    {
                        IP = sub[0].Trim(),
                        PrinterName = sub[1].Trim(),
                        PrinterModel = sub[2].Trim(),
                        DriverInfPath = sub[3].Trim()
                    };

                    list.Add(info);
                }
            }

            return list;
        }
        #endregion
    }
}
