using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC.BL.Domain; 

namespace SC.DAL.Nhibernate.Mapping
{
    class HardWareTicketMap:SubclassMap<HardwareTicket>
    {
        public HardWareTicketMap()
        {
            Map(hwt => hwt.DeviceName);
        }
    }
}
