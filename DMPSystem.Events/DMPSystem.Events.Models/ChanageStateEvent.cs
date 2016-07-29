using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMPSystem.Core.EventBus.Rabbit;

namespace DMPSystem.Events.Models
{
   public class ChanageStateEvent:Event
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Sex { get; set; }

        public new string CreateTime { get; set; }

        public string UpdateTime { get; set; }
    }
}
