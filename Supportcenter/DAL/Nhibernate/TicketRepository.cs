using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC.BL.Domain;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

namespace SC.DAL.Nhibernate
{
    public class TicketRepository : ITicketRepository
    {
        private List<Ticket> tickets;
        private List<TicketResponse> responses;
        private ISessionFactory _sessionFactory;
        public TicketRepository()
        {
            _sessionFactory = CreateSessionFactory();
            Seed();
        }
        private void Seed()
        {
            tickets = new List<Ticket>();
            responses = new List<TicketResponse>();

            // Aanmaken eerste ticket met drie responses
            Ticket t1 = new Ticket()
            {
                TicketNumber = tickets.Count + 1,
                AccountId = 1,
                Text = "Ik kan mij niet aanmelden op de webmail",
                DateOpened = new DateTime(2012, 9, 9, 13, 5, 59),
                State = TicketState.Closed,
                Responses = new List<TicketResponse>()
            };

            tickets.Add(t1);

            TicketResponse t1r1 = new TicketResponse()
            {
                Id = responses.Count + 1,
                Ticket = t1,
                Text = "Account is geblokkeerd",
                Date = new DateTime(2012, 9, 9, 13, 24, 48),
                IsClientResponse = false
            };
            t1.Responses.Add(t1r1);
            responses.Add(t1r1);

            TicketResponse t1r2 = new TicketResponse()
            {
                Id = responses.Count + 1,
                Ticket = t1,
                Text = "Account terug in orde en nieuw paswoord ingesteld",
                Date = new DateTime(2012, 9, 9, 13, 29, 11),
                IsClientResponse = false
            };
            t1.Responses.Add(t1r2);
            responses.Add(t1r2);

            TicketResponse t1r3 = new TicketResponse()
            {
                Id = responses.Count + 1,
                Ticket = t1,
                Text = "Aanmelden gelukt en paswoord gewijzigd",
                Date = new DateTime(2012, 9, 10, 7, 22, 36),
                IsClientResponse = true
            };
            t1.Responses.Add(t1r3);
            responses.Add(t1r3);

            t1.State = TicketState.Closed;


            //Aanmaken tweede ticket met één response
            Ticket t2 = new Ticket()
            {
                TicketNumber = tickets.Count + 1,
                AccountId = 1,
                Text = "Geen internetverbinding",
                DateOpened = new DateTime(2012, 11, 5, 9, 45, 13),
                State = TicketState.Open,
                Responses = new List<TicketResponse>()
            };

            tickets.Add(t2);

            TicketResponse t2r1 = new TicketResponse()
            {
                Id = responses.Count + 1,
                Ticket = t2,
                Text = "Controleer of de kabel goed is aangesloten",
                Date = new DateTime(2012, 11, 5, 11, 25, 42),
                IsClientResponse = false
            };
            t2.Responses.Add(t2r1);
            responses.Add(t2r1);

            t2.State = TicketState.Answered;

            //Aanmaken eerste HardwareTicket
            HardwareTicket ht1 = new HardwareTicket()
            {
                TicketNumber = tickets.Count + 1,
                AccountId = 2,
                Text = "Blue screen!",
                DateOpened = new DateTime(2012, 12, 14, 19, 15, 32),
                State = TicketState.Open,
                //Responses = new List<TicketResponse>(),
                DeviceName = "PC-123456"
            };

            tickets.Add(ht1);
            this.CreateTickets(tickets);

        }

        private static ISessionFactory CreateSessionFactory()
        {
            var sessionFactory = Fluently.Configure()
                               .Database(MsSqlConfiguration.MsSql2012
                               .ConnectionString(c => c
                               .FromConnectionStringWithKey("SupportCenterDB_EFCodeFirst"))
                               .ShowSql())
                               .Mappings(m => m
                               .FluentMappings.AddFromAssemblyOf<Ticket>().ExportTo(@"..\Nhibernate\Mapping"))
                               .BuildSessionFactory();
            /*sessionFactory geeft een nullwaarde trg oplossen**/
            return sessionFactory;
        }
        public void CreateTickets(List<Ticket> ticketsToCreate)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(ticketsToCreate);
                    transaction.Commit();
                }
            }
        }

        public void CreateTickResponses(List<TicketResponse> TicketResponsesToCreate)
        {
            foreach (var ticketResponse in TicketResponsesToCreate)
            {
                CreateTicketResponse(ticketResponse);
            }
        }
        public Ticket CreateTicket(Ticket ticket)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(ticket);
                    transaction.Commit();
                }
            }
            return ticket;
        }

        public TicketResponse CreateTicketResponse(TicketResponse response)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(response);
                    transaction.Commit();
                }
            }
            return response;
        }

        public void DeleteTicket(int ticketNumber)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    Ticket ticket = session.Get<Ticket>(ticketNumber);
                    session.Delete(ticket);
                    transaction.Commit();
                    session.Close();
                }
            }
        }

        public Ticket ReadTicket(int ticketNumber)
        {
            Ticket ticket;
            using (var session = _sessionFactory.OpenSession())
            {
                ticket = session.Get<Ticket>(ticketNumber);
                session.Close();
            }
            return ticket;
        }

        public IEnumerable<TicketResponse> ReadTicketResponsesOfTicket(int ticketNumber)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var responses = session.QueryOver<Ticket>()
                                           .Fetch(t => t.Responses)
                                           .Lazy
                                           .Where(t => t.TicketNumber == ticketNumber);
                    transaction.Commit();
                    session.Close();
                }
            }
            return responses;
        }

        public IEnumerable<Ticket> ReadTickets()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var tickets = session.CreateCriteria<Ticket>();
            }
            return tickets;
        }

        public void UpdateTicket(Ticket ticket)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Update(ticket);
                    transaction.Commit();
                    session.Close();
                }
            }
        }

        public void UpdateTicketStateToClosed(int ticketNumber)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    IQuery query = session.GetNamedQuery("sp_CloseTicket");
                    query.SetInt32("ticketNumber", ticketNumber);
                    query.ExecuteUpdate();
                    session.Close();
                }
            }
        }
    }
}
