using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.IModuleServices.DMPHub.Models
{
    [DataContract]
    public class Manager
    {
        [DataMember]
        public int UserID { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string Sex { get; set; }

        [DataMember]
        public string CreateTime { get; set; }

        [DataMember]
        public string UpdateTime { get; set; }
    }
}
