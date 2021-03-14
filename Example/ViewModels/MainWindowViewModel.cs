using System;
using System.Text;

namespace Example.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting
        {
            get
            {
                StringBuilder @string = new("Welcome to Avalonia!");
                @string.AppendLine(Environment.NewLine);
                @string.AppendFormat("OSVersion: {0}", Environment.OSVersion.Version);
                @string.AppendLine();
#if NET_WINDOWS_10
                if (Environment.OSVersion.Version.Major == 10 && Environment.OSVersion.Version.Build >= 10240)
                {
#pragma warning disable CA1416 // 验证平台兼容性
                    Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation deviceInfo = new();
                    @string.AppendFormat("FriendlyName: {0}", deviceInfo.FriendlyName);
                    @string.AppendLine();
                    @string.AppendFormat("OperatingSystem: {0}", deviceInfo.OperatingSystem);
                    @string.AppendLine();
                    @string.AppendFormat("SystemFirmwareVersion: {0}", deviceInfo.SystemFirmwareVersion);
                    @string.AppendLine();
                    @string.AppendFormat("SystemHardwareVersion: {0}", deviceInfo.SystemHardwareVersion);
                    @string.AppendLine();
                    @string.AppendFormat("SystemManufacturer: {0}", deviceInfo.SystemManufacturer);
                    @string.AppendLine();
                    @string.AppendFormat("SystemProductName: {0}", deviceInfo.SystemProductName);
                    @string.AppendLine();
                    @string.AppendFormat("SystemSku: {0}", deviceInfo.SystemSku);
                    @string.AppendLine();
#pragma warning restore CA1416 // 验证平台兼容性
                }
#endif
                return @string.ToString();
            }
        }
    }
}