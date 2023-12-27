using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITicketRepository
    {
        IQueryable<Ticket> GetTickets();

        Ticket? GetTicket(Guid Id);

        bool book(Ticket ticket);

        void cancel(Ticket ticket);
        

    }
}
