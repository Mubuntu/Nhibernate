using FluentNHibernate.Mapping;
using SC.BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC.DAL.Nhibernate.Mapping
{
    class TicketResponseMap:ClassMap<TicketResponse>
    {
        public TicketResponseMap()
        {
            Id(tr => tr.Id);
            Map(tr => tr.Text).Not.Nullable();
            Map(tr => tr.Date);
            Map(tr => tr.IsClientResponse);
            References(tr => tr.Ticket).Not.Nullable();
        }
    }
}
