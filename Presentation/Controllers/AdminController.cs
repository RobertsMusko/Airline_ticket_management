using Data.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels;

namespace Presentation.Controllers
{
    public class AdminController : Controller
    {
        private ITicketRepository _ticketDBRepository;
        private FlightDBRepository _flightDBRepository;
        private UserManager<CustomUser> _userManager;


        public AdminController(ITicketRepository ticketDBRepository, FlightDBRepository flightDBRepository, UserManager<CustomUser> userManager)
        {
            _ticketDBRepository = ticketDBRepository;
            _flightDBRepository = flightDBRepository;
            _userManager = userManager;

        }


        public IActionResult ListFlights()
        {
            try
            {
                IQueryable<Flight> list = _flightDBRepository.GetFlights();

                var output = list.Select(p => new ListFlightsAdminViewModel
                {
                    Id = p.Id,
                    DepartureDate = p.DepartureDate,
                    ArrivalDate = p.ArrivalDate,
                    CountryFrom = p.CountryFrom,
                    CountryTo = p.CountryTo,
                    RetailPrice = p.WholesalePrice * p.CommisionRate + p.WholesalePrice,
                    TotalSeats = p.Rows * p.Columns,
                    AvailableSeats = (p.Rows * p.Columns) - _ticketDBRepository.GetTickets().Count(t => t.FlightIdFk == p.Id) + _ticketDBRepository.GetTickets().Count(t => t.Cancelled == true),
                    Rows = p.Rows,
                    Columns = p.Columns,
                    WholesalePrice = p.WholesalePrice,
                    CommisionRate = p.CommisionRate,
                });



                return View(output);
            }
            catch (Exception ex)
            {
                TempData["error"] = "There was an Error, please try again.";
                return View();
            }
        }


        public async Task<IActionResult> ListTickets(Guid Id)
        {
            try
            {

                var user = await _userManager.GetUserAsync(User);
                

                IQueryable<Ticket> list = _ticketDBRepository.GetTickets().Where(x => x.FlightIdFk == Id);

                var tickets = new List<ListTicketsAdminViewModel>();

                foreach (var ticket in list)
                {
                    var owner = ticket.Owner;
                    var userByOwnerName = await _userManager.FindByNameAsync(owner);

                    var passport = userByOwnerName?.PassportNo;


                    var ticketViewModel = new ListTicketsAdminViewModel
                    {
                        Row = ticket.Row,
                        Column = ticket.Column,
                        Passport = passport,
                        PricePaid = ticket.PricePaid,
                        FlightIdFk = ticket.FlightIdFk,
                        Image = ticket.Image,
                        Cancelled = ticket.Cancelled,
                        Id = ticket.Id,
                        Owner = owner,
                    };

                    tickets.Add(ticketViewModel);
                }

                return View(tickets);
            }
            catch (Exception ex)
            {
                TempData["error"] = "There was an Error, please try again.";
                return View();
            }
        }




    }
}
