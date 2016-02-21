using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC.BL.Domain;

namespace SC.DAL.Nhibernate
{
    public class TicketRepository : ITicketRepository
    {
        public Ticket CreateTicket(Ticket ticket)
        {
            throw new NotImplementedException();
        }

        public TicketResponse CreateTicketResponse(TicketResponse response)
        {
            throw new NotImplementedException();
        }

        public void DeleteTicket(int ticketNumber)
        {
            throw new NotImplementedException();
        }

        public Ticket ReadTicket(int ticketNumber)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TicketResponse> ReadTicketResponsesOfTicket(int ticketNumber)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Ticket> ReadTickets()
        {
            throw new NotImplementedException();
        }

        public void UpdateTicket(Ticket ticket)
        {
            throw new NotImplementedException();
        }

        public void UpdateTicketStateToClosed(int ticketNumber)
        {
            throw new NotImplementedException();
        }
    }
}
