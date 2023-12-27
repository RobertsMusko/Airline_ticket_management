using Data.Context;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Data.Repositories
{
    public class TicketFileRepository : ITicketRepository
    {
        string FilePath;

        private AirlineDbContext _airlineDbContext;

        public TicketFileRepository(AirlineDbContext airlineDbContext)
        {
            _airlineDbContext = airlineDbContext;
        }

        public TicketFileRepository(string pathToTicketsFile)
        {
            FilePath = pathToTicketsFile;

            if (System.IO.File.Exists(FilePath) == false)
            {
                using (var myFile = System.IO.File.Create(FilePath))
                {
                    myFile.Close();
                }
            }
        }

        public IQueryable<Ticket> GetTickets()
        {
            string allText = System.IO.File.ReadAllText(FilePath);

            if (allText == "")
            {
                return new List<Ticket>().AsQueryable();
            }
            else
            {
                try
                {
                    List<Ticket> tickets = JsonSerializer.Deserialize<List<Ticket>>(allText);
                    return tickets.AsQueryable();
                }
                catch (Exception ex)
                {
                    return new List<Ticket>().AsQueryable();
                }
            }
        }

        public Ticket? GetTicket(Guid Id)
        {
            var tickets = GetTickets();
            return tickets.FirstOrDefault(t => t.Id == Id);
        }



        public bool book(Ticket ticket)
        {
            ticket.Id = Guid.NewGuid();

            try
            {
                List<Ticket> existingTickets;

                string allText = System.IO.File.ReadAllText(FilePath);

                if (string.IsNullOrEmpty(allText))
                {
                    existingTickets = new List<Ticket>();
                }
                else
                {
                    existingTickets = JsonSerializer.Deserialize<List<Ticket>>(allText);
                }

                var cancelledTicket = existingTickets.FirstOrDefault(
                    x => x.Row == ticket.Row && x.Column == ticket.Column && x.Cancelled);

                var alreadyBooked = existingTickets.Any(x => x.Row == ticket.Row && x.Column == ticket.Column);

                if (cancelledTicket != null)
                {

                    existingTickets.Remove(cancelledTicket);
                }
                else
                {
                    if (alreadyBooked)
                    {
                        throw new Exception("Seat already taken.");
                    }
                }

                existingTickets.Add(ticket);

                string jsonString = JsonSerializer.Serialize(existingTickets);
                System.IO.File.WriteAllText(FilePath, jsonString);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        public void cancel(Ticket ticket)
        {
            var existingTickets = GetTickets().ToList();

            var ticketToCancel = existingTickets.FirstOrDefault(x => x.Id == ticket.Id && x.FlightIdFk == ticket.FlightIdFk);

            if (ticketToCancel != null)
            {
                ticketToCancel.Id = ticket.Id;
                ticketToCancel.Cancelled = ticket.Cancelled;
                ticketToCancel.Passport = ticket.Passport;
                ticketToCancel.PricePaid = ticket.PricePaid;
                ticketToCancel.Image = ticket.Image;
                ticketToCancel.Column = ticket.Column;
                ticketToCancel.Row = ticket.Row;
                ticketToCancel.Owner = ticket.Owner;
                ticketToCancel.FlightIdFk = ticket.FlightIdFk;

                string jsonString = JsonSerializer.Serialize(existingTickets);
                System.IO.File.WriteAllText(FilePath, jsonString);
            }
        }


    }
}
