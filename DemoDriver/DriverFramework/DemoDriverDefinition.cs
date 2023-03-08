using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoOS.Platform.DriverFramework;
using System.Security;
using VideoOS.Platform.DriverFramework.Data.Settings;
using VideoOS.Platform.DriverFramework.Definitions;
using VideoOS.Platform.DriverFramework.DeviceDiscovery;

namespace DemoDriver
{
    public class DemoDriverDefinition : DriverDefinition
    {
        /// <summary>
        /// Create session to device, or throw exceptions if not successful
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        protected override Container CreateContainer(Uri uri, string userName, SecureString password, ICollection<HardwareSetting> hardwareSettings)
        {
            return new DemoContainer(this);
        }

        protected override DriverInfo CreateDriverInfo()
        {
            DriverInfo driverInfo = new DriverInfo(Constants.HardwareId, "DemoDriver", "Demo", "1.0", new[] { Constants.Product1 });
            
            //MaxHardwarePerProcess has been added in release 2020 R3 - if driver is used with older versions, MaxHardwarePerProcess won't work and should be removed. 
            driverInfo.MaxHardwarePerProcess = 10;
            driverInfo.SupportedSchemes.Add(new ONVIFScheme() { NameRegex = "Demo", HardwareRegex = "DemoDriverDevice" });
            return driverInfo;
        }
    }
}
