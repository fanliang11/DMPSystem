using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.Common.FormatModel
{
   public class Page<T>
    {
        [DataMember]
        public int PageIndex { get; set; }

        [DataMember]
        public int PageTotal { get; set; }

        [DataMember]
        public int PageSize { get; set; }

        public List<T> Items
        {
            get;
            set;
        }
    }
}
