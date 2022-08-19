using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.AccessControl;
using System.Linq;

namespace PrinterHelper
{
    public class UtilityTools
    {
        #region Base64Encode
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        #endregion

        #region BytesToSizeSuffix
        private static readonly string[] SizeSuffixes =
                   { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        /// <summary>
        /// decimalPlaces<br/>
        /// 0="bytes", 1="KB", 2="MB", 3="GB", 4="TB", 5="PB", 6="EB", 7="ZB", 8="YB"
        /// </summary>
        public static string BytesToSizeSuffix(Int64 value, int decimalPlaces = 2)
        {
            if (decimalPlaces < 0) { return "0"; }
            if (value < 0) { return "-" + BytesToSizeSuffix(-value, decimalPlaces); }
            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }
        #endregion

        #region + public static void SetDirACL_AuthenticatedUsers_Modify(string dirPath)
        public static void SetDirACL_AuthenticatedUsers_Modify(string dirPath)
        {
            var dirInfo = new DirectoryInfo(dirPath);

            var dirACL = dirInfo.GetAccessControl();

            var rule = new FileSystemAccessRule("Authenticated Users",
                    FileSystemRights.Modify,
                    InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                    PropagationFlags.None,
                    AccessControlType.Allow);

            dirACL.AddAccessRule(rule);
            dirInfo.SetAccessControl(dirACL);
        }
        #endregion

        #region + public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }
        #endregion

        #region + public static IPAddress ComputeNetworkAddress(IPAddress address, IPAddress subnetMask)
        /// <summary>
        /// https://docs.microsoft.com/en-us/archive/blogs/knom/ip-address-calculations-with-c-subnetmasks-networks
        /// </summary>
        /// <param name="address"></param>
        /// <param name="subnetMask"></param>
        /// <returns></returns>
        public static IPAddress ComputeNetworkAddress(IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] & (subnetMaskBytes[i]));
            }
            return new IPAddress(broadcastAddress);
        }
        #endregion

        #region + public static IPAddress GetLocalIPv4Mask()
        public static IPAddress GetLocalIPv4Mask()
        {
            try
            {
                return NetworkInterface.GetAllNetworkInterfaces()
                                .First(n => n.OperationalStatus == OperationalStatus.Up
                                        || n.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                                .GetIPProperties().UnicastAddresses
                                .First(n => n.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                .IPv4Mask;
            }
            catch (Exception)
            {
                return IPAddress.Parse("255.255.255.0");
            }
        }
        #endregion

        #region + public static IPAddress GetLocalIPv4()
        public static IPAddress GetLocalIPv4()
        {
            try
            {
                return NetworkInterface.GetAllNetworkInterfaces()
                                .First(n => n.OperationalStatus == OperationalStatus.Up
                                        || n.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                                .GetIPProperties().UnicastAddresses
                                .First(n => n.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                .Address;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        
    }
}
