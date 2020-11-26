using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Phoenixnet.Extensions
{
    public static class PlatformUtils
    {
        public static string GetMachineName()
        {
            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            return isWindows ? WindowsIdentity.GetCurrent().Name : string.Empty;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIp()
        {
            var firstUpInterface = NetworkInterface.GetAllNetworkInterfaces()
                .FirstOrDefault(c => c.NetworkInterfaceType != NetworkInterfaceType.Loopback && c.OperationalStatus == OperationalStatus.Up);
            if (firstUpInterface == null) return string.Empty;
            var props = firstUpInterface.GetIPProperties();
            return props.UnicastAddresses
                .Where(c => c.Address.AddressFamily == AddressFamily.InterNetwork)
                .Select(c => c.Address)
                .FirstOrDefault()
                ?.ToString();
        }
    }
}