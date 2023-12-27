using Data.Context;
using Data.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels;
using System.Data;
using System.IO;

namespace Presentation.Controllers
{
    public class TicketsController : Controller
    {

        private ITicketRepository _ticketDBRepository;
        private FlightDBRepository _flightDBRepository;
        private  UserManager<CustomUser> _userManager;

        public TicketsController(ITicketRepository ticketDBRepository, FlightDBRepository flightDBRepository, UserManager<CustomUser> userManager)
        {
            _ticketDBRepository = ticketDBRepository;
            _flightDBRepository = flightDBRepository;
            _userManager = userManager;
        }



        //4.UZD a

        public IActionResult ListFlights()
        {
            try
            {


                DateTime DateNow = DateTime.Today;

                IQueryable<Flight> list = _flightDBRepository.GetFlights().Where(x => x.DepartureDate > DateNow);

                var flights = list.ToList(); 

                var output = flights.Select(p =>
                {
                    var flightViewModel = new ListFlightViewModel
                    {
                        Id = p.Id,
                        DepartureDate = p.DepartureDate.ToString(),
                        ArrivalDate = p.ArrivalDate.ToString(),
                        CountryFrom = p.CountryFrom,
                        CountryTo = p.CountryTo,
                        RetailPrice = p.WholesalePrice * p.CommisionRate + p.WholesalePrice,
                        TotalSeats = p.Rows * p.Columns,
                        AvailableSeats = (p.Rows * p.Columns) - _ticketDBRepository.GetTickets().Count(t => t.FlightIdFk == p.Id) + _ticketDBRepository.GetTickets().Count(t => t.Cancelled == true),

                    };


                    return flightViewModel;
                });

                return View(output);
            }
            catch (Exception ex)
            {
                TempData["error"] = "There was an Error, please try again.";
                return View();
            }
        }








        //4.uzd b


        [HttpGet]
        public async Task<IActionResult> bookFlight(Guid Id)
        {

            var flight = _flightDBRepository.GetFlight(Id);

            var user = await _userManager.GetUserAsync(User);
            var passportNo = user?.PassportNo;

            if (user == null && flight != null)
            {
                var flightViewModel = new ListFlightViewModel
                {
                    Id = flight.Id,
                    DepartureDate = flight.DepartureDate.ToString(),
                    ArrivalDate = flight.ArrivalDate.ToString(),
                    CountryFrom = flight.CountryFrom,
                    CountryTo = flight.CountryTo,
                    RetailPrice = flight.WholesalePrice * flight.CommisionRate + flight.WholesalePrice,
                    TotalSeats = flight.Rows * flight.Columns,
                    AvailableSeats = flight.AvailableSeats,
                    WholesalePrice = flight.WholesalePrice,
                    CommisionRate = flight.CommisionRate,
                    Columns = flight.Columns,
                    Rows = flight.Rows,
                };

                BookFlightViewModel viewModel = new BookFlightViewModel();
                    viewModel.FlightIdFk = Id;
                    viewModel.PricePaid = flightViewModel.RetailPrice;
                    viewModel.PassportNo = passportNo;
                    viewModel.Columns = flightViewModel.Columns;
                    viewModel.Rows = flightViewModel.Rows;


                

                return View(viewModel);
            }else if (user != null && flight != null)
            {
                var flightViewModel = new ListFlightViewModel
                {
                    Id = flight.Id,
                    DepartureDate = flight.DepartureDate.ToString(),
                    ArrivalDate = flight.ArrivalDate.ToString(),
                    CountryFrom = flight.CountryFrom,
                    CountryTo = flight.CountryTo,
                    RetailPrice = flight.WholesalePrice * flight.CommisionRate + flight.WholesalePrice,
                    TotalSeats = flight.Rows * flight.Columns,
                    AvailableSeats = flight.AvailableSeats,
                    WholesalePrice = flight.WholesalePrice,
                    CommisionRate = flight.CommisionRate,
                    Columns = flight.Columns,
                    Rows = flight.Rows,
                };

                BookFlightViewModel viewModel = new BookFlightViewModel();
                viewModel.FlightIdFk = Id;
                viewModel.PricePaid = flightViewModel.RetailPrice;
                viewModel.PassportNo = passportNo;
                viewModel.Columns = flightViewModel.Columns;
                viewModel.Rows = flightViewModel.Rows;

                return View(viewModel);
            }
            else
            {

                TempData["error"] = "There was an Error, please try again.";
                return View ();
            }
        }



           
                
                
        





        [HttpPost]
        public IActionResult bookFlight(BookFlightViewModel p,[FromServices]IWebHostEnvironment host)
        {
            try
            {
                string RelativePath = "";

                if (p.PassportImage != null)
                {
                    string NewFileName = Guid.NewGuid().ToString()
                        + Path.GetExtension(p.PassportImage.FileName);

                    RelativePath = "/images/" + NewFileName;

                    string AbsolutePath = host.WebRootPath + "\\images\\" + NewFileName;

                    using (FileStream fs = new FileStream(AbsolutePath, FileMode.CreateNew))
                    {
                        p.PassportImage.CopyTo(fs);
                        fs.Flush();
                    } 
                }
              
                foreach (var seat in p.SelectedSeats)
                {
                    string[] seatInfo = seat.Split(',');
                    int row = int.Parse(seatInfo[0]);
                    int column = int.Parse(seatInfo[1]);

                    var existingTicket = _ticketDBRepository.GetTickets().FirstOrDefault(x =>x.FlightIdFk == p.FlightIdFk && x.Row == row.ToString() &&x.Column == column.ToString() &&!x.Cancelled);

                    if (existingTicket != null)
                    {
                        TempData["error"] = "Seat is already booked.";
                        return View(p);
                    }

                    _ticketDBRepository.book(new Ticket
                    {
                        Row = row.ToString(),
                        Column = column.ToString(),
                        Passport = p.Passport,
                        PricePaid = p.PricePaid,
                        FlightIdFk = p.FlightIdFk,
                        Image = RelativePath,
                        Owner = User.Identity.Name,
                    });
                }

                

                TempData["message"] = "Ticket added successfully!";
                return RedirectToAction("ListFlights");

            }catch (Exception ex)
            {
                TempData["error"] = "There was an Error, please try again.";
                return View(p);
            }
        }





        //4.uzd c





        public async Task<IActionResult> ListTickets(Guid Id)
        {
            try
            {

                IQueryable<Ticket> list = _ticketDBRepository.GetTickets().Where(x => x.FlightIdFk == Id && User.Identity.Name == x.Owner);

                var user = await _userManager.GetUserAsync(User);
                var passportNo = user?.PassportNo;

                if (user != null)
                {
                    var output = from p in list
                                 select new ListTicketsViewModel()
                                 {
                                     Row = p.Row,
                                     Column = p.Column,
                                     Passport = passportNo,
                                     PricePaid = p.PricePaid,
                                     FlightIdFk = p.FlightIdFk,
                                     Image = p.Image,
                                     Cancelled = p.Cancelled,
                                     Id = p.Id,


                                 };


                    return View(output);
                }
                else
                {
                    var output = from p in list
                                 select new ListTicketsViewModel()
                                 {
                                     Row = p.Row,
                                     Column = p.Column,
                                     Passport = p.Passport,
                                     PricePaid = p.PricePaid,
                                     FlightIdFk = p.FlightIdFk,
                                     Image = p.Image,
                                     Cancelled = p.Cancelled,
                                     Id = p.Id,
                                 };
                    return View(output);
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "There was an Error, please try again.";
                return View();
            }
        }



        [HttpGet]
        public IActionResult cancel(Guid Id)
        {
            try
            {
                EditTicketViewModel myModel = new EditTicketViewModel();
                var cancelTicket = _ticketDBRepository.GetTicket(Id);

                myModel.Cancelled = cancelTicket.Cancelled;
                myModel.Column = cancelTicket.Column;
                myModel.Row = cancelTicket.Row;
                myModel.Passport = cancelTicket.Passport;
                myModel.FlightIdFk = cancelTicket.FlightIdFk;
                myModel.Image = cancelTicket.Image;
                myModel.Id = cancelTicket.Id;
                myModel.PricePaid = cancelTicket.PricePaid;
                myModel.Owner = cancelTicket.Owner;
                

                return View(myModel);
            }
            catch (Exception ex) 
            {
                TempData["error"] = "There was an Error, please try again.";
                return View(); 
            }
            
        }


        [HttpPost]
        public IActionResult cancel(Guid Id, BookFlightViewModel p, [FromServices] IWebHostEnvironment host)
        {
            try
            {
                string RelativePath = "";

                if (p.PassportImage != null)
                {
                    string NewFileName = Guid.NewGuid().ToString()
                        + Path.GetExtension(p.PassportImage.FileName);

                    RelativePath = "/images/" + NewFileName;

                    string AbsolutePath = host.WebRootPath + "\\images\\" + NewFileName;

                    using (FileStream fs = new FileStream(AbsolutePath, FileMode.CreateNew))
                    {
                        p.PassportImage.CopyTo(fs);
                        fs.Flush();
                    }

                    string oldAbsolutePath = host.WebRootPath + "\\images\\" + System.IO.Path.GetFileName(RelativePath);
                    System.IO.File.Delete(oldAbsolutePath);
                }
                else
                {

                    RelativePath = _ticketDBRepository.GetTicket(p.Id).Image;
                }




                    _ticketDBRepository.cancel(
                  new Ticket()
                  {
                      Row = p.Row,
                      Column = p.Column,
                      Cancelled = p.Cancelled,
                      Passport = p.Passport,
                      PricePaid = p.PricePaid,
                      FlightIdFk = p.FlightIdFk,
                      Image = RelativePath,
                      Id = p.Id,
                      Owner = User.Identity.Name,
                  }
                    );

                TempData["message"] = "Ticket has been Successfuly changed!";
                return RedirectToAction("ListFlights");

            }
            catch (Exception ex)
            {
                TempData["error"] = "There was an Error, please try again.";
                return View(p);
            }

        }



    }
}
