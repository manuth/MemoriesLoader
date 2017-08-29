using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MemoriesLoader
{
    [XmlType("device")]
    public class Device
    {
        [XmlElement("deviceType")]
        public string DeviceType { get; set; }

        [XmlElement("friendlyName")]
        public string Name { get; set; }

        [XmlElement("UDN")]
        public string GUID { get; set; }

        [XmlElement("serialNumber")]
        public string SerialNumber { get; set; }

        [XmlElement("manufacturer")]
        public string Manufacturer { get; set; }

        [XmlElement("manufacturerURL")]
        public string ManufacturerUri { get; set; }

        [XmlElement("modelName")]
        public string ModelName { get; set; }

        [XmlElement("modelURL")]
        public string ModelUri { get; set; }

        [XmlElement("modelDescription")]
        public string ModelDescription { get; set; }
    }
}
