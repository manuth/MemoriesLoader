using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MemoriesLoader
{
    [XmlRoot("root", Namespace = "urn:schemas-upnp-org:device-1-0")]
    public class DeviceDescription
    {
        [XmlElement(ElementName = "device")]
        public Device Device { get; set; }
    }
}
