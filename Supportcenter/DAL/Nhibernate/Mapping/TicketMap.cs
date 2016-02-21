using FluentNHibernate.Mapping;
using SC.BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC.DAL.Nhibernate.Mapping
{
    public class TicketMap : ClassMap<Ticket>
    {
        /**
        ***omdat onze hbm.xml mappings ingaan tegen 
        ***ons seperation of code principe, niet mee
        ***worden gecompiled (aanpassing in klasse
        ***wordt niet in .xml mapping aangepast,
        ***maken we daarom gebruik van in-code 
        ***gegenereerde mappings
         **/
        public TicketMap()
        {
            Id(t => t.TicketNumber);
            Map(t => t.AccountId);
            Map(t => t.Text);
            Map(t => t.DateOpened);
            Map(t => t.State);
            //een lijst met ticketresponses waar de veel-zijde(*) op updates opslaat
            HasMany<TicketResponse>(t => t.Responses).Inverse();
            Map(x => x.State);
        }
    }
}
