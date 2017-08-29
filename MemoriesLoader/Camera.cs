using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesLoader
{
    [DataContract]
    public class Camera
    {
        public IPAddress IPAddress { get; set; }

        [DataMember(Name = "directory")]
        public string Directory { get; set; }

        [DataMember(Name = "ipAddress")]
        private string ipAddressString
        {
            get
            {
                return IPAddress.ToString();
            }

            set
            {
                IPAddress = IPAddress.Parse(value);
            }
        }
    }
}
