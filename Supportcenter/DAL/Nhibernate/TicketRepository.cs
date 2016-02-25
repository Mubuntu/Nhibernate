using NHibernate;
using SC.BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC.DAL.Nhibernate
{
    public class TicketRepository:ITicketRepository
    {
        public BL.Domain.Ticket CreateTicket(BL.Domain.Ticket ticket)
        {
            //eerst sessie openen
            using (ISession session = NhibernateHelper.OpenSession())
            {
                //dan transactie starten
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(ticket);
                    transaction.Commit();
                    //transaction locks gaan open en savepointscleared
                }
            }
            return ticket;
        }

        public IEnumerable<BL.Domain.Ticket> ReadTickets()
        {
            IEnumerable<Ticket> tickets;
            using (ISession session = NhibernateHelper.OpenSession())
            {
                IQuery query = session.CreateQuery("FROM Ticket");
                tickets = query.Enumerable<Ticket>();
                #region andere manieren
                //OF STATEMENT VIA HQL MET DISJOINCT PARAMETER
                //tickets = session.CreateCriteria<Ticket>()
                //.Add(Restrictions.Disjunction()
                //.Add(Restrictions.Eq("TicketNumber", "1"))
                //.Add(Restrictions.Eq("Text", "bluescreen")))
                //.List<Blog>();
                //IQuery query = session.CreateQuery("FROM Ticket");
                //tickets = (IEnumerable<Ticket>)query.List();
                //OF
                //tickets = (IEnumerable<Ticket>)session.CreateCriteria<Ticket>().List();
                //OF OR STATEMENT
                //tickets = session.CreateQuery("from Ticket t where t.TicketNumber = :ticketNumber or t.Text = :text")
                //.SetParameter("tickNumber", ticketNumber)
                //.SetParameter("text", "blue screen")
                //.List<Ticket>();
                //OF and statement
                //tickets = session.CreateQuery("from Ticket t where t.TicketNumber = :ticketNumber and t.Text = :text")
                //.SetParameter("tickNumber", ticketNumber)
                //.SetParameter("text", "blue screen")
                //.List<Ticket>();
                #endregion
            }

            return tickets;
        }

        public BL.Domain.Ticket ReadTicket(int ticketNumber)
        {
            Ticket ticket;
            using (ISession session = NhibernateHelper.OpenSession())
            {
                IQuery query = session.CreateQuery("FROM Ticket WHERE Ticket.TicketNumber = " + ticketNumber);
                //query.SetParameter("ticketNumber", ticketNumber);
                ticket = (Ticket)query.List();
                #region andere manieren
                //ticket = (Ticket)session.CreateCriteria<Ticket>().Add(Restrictions.Eq("TicketNumber", ticketNumber));
                //OF
                //IQuery query = session.CreateQuery("FROM Ticket WHERE Ticket.TicketNumber = " + ticketNumber);
                //query.SetParameter("ticketNumber", ticketNumber);
                //ticket = (Ticket)query.List();
                //ticket = session.Get<Ticket>(ticketNumber);
                //OF
                //ticket = session.Load<Ticket>(ticketNumber);
                #endregion
            }


            return ticket;
        }

        public void UpdateTicket(BL.Domain.Ticket ticket)
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {

                    session.Update(ticket);
                    transaction.Commit();
                    //session.flush() is hier niet nodig omdat het transaction object 
                    //met de  transaction.commit() de tabels vrij maakt en alle bestande synchroniseert met de database 
                }
            }
        }

        public void DeleteTicket(int ticketNumber)
        {
            Ticket ticket;
            using (ISession session = NhibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    IQuery query = session.CreateQuery("FROM Ticket WHERE Ticket.TicketNumber = :ticketNumber");
                    query.SetParameter("ticketNumber", ticketNumber);
                    ticket = query.List<Ticket>()[0];
                    session.Delete(ticket);
                    transaction.Commit();
                    #region andere manieren
                    //ticket = session.Get<Ticket>(ticketNumber);
                    //OF
                    //ticket = session.Load<Ticket>(ticketNumber);
                    //OF
                    //IQuery query = session.CreateQuery("FROM Ticket WHERE Ticket.TicketNumber = :ticketNumber");
                    //query.SetParameter("ticketNumber", ticketNumber);
                    //ticket = query.List<Ticket>()[0];
                    //session.Delete(ticketResponses) is niet nodig want we hebben een cascade dus alle responses worden respectievelijk verwijderd
                    #endregion
                }
            }
        }

        public IEnumerable<BL.Domain.TicketResponse> ReadTicketResponsesOfTicket(int ticketNumber)
        {
            Ticket ticket;
            using (ISession session = NhibernateHelper.OpenSession())
            {
                IQuery query = session.CreateQuery("FROM Ticket WHERE Ticket.TicketNumber = :ticketNumber");
                query.SetParameter("ticketNumber", ticketNumber);
                ticket = query.List<Ticket>()[0];
                session.Close();
                #region andere manieren
                //ticket = session.Get<Ticket>(ticketNumber);
                //OF
                //ticket = session.Load<Ticket>(ticketNumber);
                #endregion
            }
            return ticket.Responses;
        }

        public BL.Domain.TicketResponse CreateTicketResponse(BL.Domain.TicketResponse response)
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    if (response.Ticket != null)
                    {
                        Ticket ticket = session.Load<Ticket>(response.Ticket.TicketNumber);
                        ticket.Responses.Add(response);
                        session.Save(ticket);
                        transaction.Commit();
                        #region andere manieren
                        //OF
                        //Ticket ticket = session.Get<Ticket>(response.Ticket.TicketNumber);
                        //OF
                        //  IQuery query = session.CreateQuery("FROM Ticket WHERE Ticket.TicketNumber = :ticketNumber");
                        //query.SetParameter("ticketNumber", response.Ticket.TicketNumber);
                        //ticket = query.List<Ticket>()[0];
                        #endregion
                    }
                    else
                    {
                        throw new ArgumentException("The ticketresponse has no ticket attached to it");
                    }
                }
            }
            return response;
        }

        public void UpdateTicketStateToClosed(int ticketNumber)
        {
            throw new NotImplementedException();
        }
    }
}
