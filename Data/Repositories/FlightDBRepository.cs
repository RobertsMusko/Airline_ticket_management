using Data.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class FlightDBRepository
    {

        private  AirlineDbContext _airlineDbContext;

        public FlightDBRepository(AirlineDbContext airlineDbContext)
        {
            _airlineDbContext = airlineDbContext;
        }

      

        public IQueryable<Flight> GetFlights()
        {
            return _airlineDbContext.Flights;
        }

        public Flight? GetFlight(Guid Id)
        {
            return GetFlights().SingleOrDefault(x => x.Id == Id);

        }
    }
}
