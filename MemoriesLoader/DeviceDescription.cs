using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MemoriesLoader
{
    /// <summary>
    /// Represents a description of a device.
    /// </summary>
    [XmlRoot("root", Namespace = "urn:schemas-upnp-org:device-1-0")]
    public class DeviceDescription
    {
        /// <summary>
        /// Gets or sets the description of the device.
        /// </summary>
        [XmlElement(ElementName = "device")]
        public Device Device { get; set; }
    }
}
