using Data.Context;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class TicketDBRepository : ITicketRepository
    {

        private  AirlineDbContext _airlineDbContext;

        public TicketDBRepository(AirlineDbContext airlineDbContext)
        {
            _airlineDbContext = airlineDbContext;
        }

        public IQueryable<Ticket> GetTickets()
        {
            return _airlineDbContext.Tickets;

        }

        public Ticket? GetTicket(Guid Id)
        {
            return GetTickets().SingleOrDefault(x => x.Id == Id);
        }


        public bool book(Ticket ticket)
        {
            var alreadyBookedFlights = _airlineDbContext.Tickets
                .Where(x => x.FlightIdFk == ticket.FlightIdFk && x.Row == ticket.Row && x.Column == ticket.Column)
                .ToList();

            if (alreadyBookedFlights.Any())
            {
                var cancelledBooking = alreadyBookedFlights.FirstOrDefault(x => x.Cancelled == true);

                if (cancelledBooking != null)
                {
                    _airlineDbContext.Tickets.Add(ticket);
                    _airlineDbContext.SaveChanges();
                    return true;
                }
                else
                {
                    throw new Exception("Seat already taken.");
                }
            }
            else
            {
                _airlineDbContext.Tickets.Add(ticket);
                _airlineDbContext.SaveChanges();
                return true;
            }
        }


        public void cancel(Ticket ticket)
        {
            var cancell = _airlineDbContext.Tickets.FirstOrDefault(x => x.Id == ticket.Id && x.FlightIdFk == ticket.FlightIdFk);
            if (cancell != null)
            {
                cancell.Cancelled = ticket.Cancelled;
                cancell.Passport = ticket.Passport;
                cancell.PricePaid = ticket.PricePaid;
                cancell.Image = ticket.Image;
                cancell.Column = ticket.Column;
                cancell.Row = ticket.Row;
                cancell.Owner = ticket.Owner;
                cancell.FlightIdFk = ticket.FlightIdFk;

                _airlineDbContext.SaveChanges();
            }
        }





    }
}
