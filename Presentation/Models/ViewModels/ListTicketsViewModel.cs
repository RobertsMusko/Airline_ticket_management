using Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels
{
    public class ListTicketsViewModel
    {
        public string Row { get; set; }
        public string Column { get; set; }
        public Guid FlightIdFk { get; set; }
        public Flight Flight { get; set; }
        public int? Passport { get; set; }
        public double PricePaid { get; set; }
        public string? Image { get; set; }
        public Boolean Cancelled { get; set; }
        public Guid Id { get; set; }
        public string? Owner { get; set; }

    }
}
