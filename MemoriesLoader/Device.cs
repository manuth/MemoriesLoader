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
    /// Represents a UPNP-device.
    /// </summary>
    [XmlType("device")]
    public class Device
    {
        /// <summary>
        /// Gets or sets the type of the device.
        /// </summary>
        [XmlElement("deviceType")]
        public string DeviceType { get; set; }

        /// <summary>
        /// Gets or sets the name of the device.
        /// </summary>
        [XmlElement("friendlyName")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the global unique ID of the device.
        /// </summary>
        [XmlElement("UDN")]
        public string GUID { get; set; }

        /// <summary>
        /// Gets or sets the serial number of the device.
        /// </summary>
        [XmlElement("serialNumber")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the name of the manufacturer of the device.
        /// </summary>
        [XmlElement("manufacturer")]
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the url to the website of the manufacturer of the device.
        /// </summary>
        [XmlElement("manufacturerURL")]
        public string ManufacturerUri { get; set; }

        /// <summary>
        /// Gets or sets the model name of the device.
        /// </summary>
        [XmlElement("modelName")]
        public string ModelName { get; set; }

        /// <summary>
        /// Gets or sets the url to the website of the model of the device.
        /// </summary>
        [XmlElement("modelURL")]
        public string ModelUri { get; set; }

        /// <summary>
        /// Gets or sets the description of the model of the device.
        /// </summary>
        [XmlElement("modelDescription")]
        public string ModelDescription { get; set; }
    }
}
