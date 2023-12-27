using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Presentation.Models.ViewModels
{
    public class TestViewModel
    {

        public IEnumerable<ListTicketsAdminViewModel> Tickets { get; set; }
        public UserManager<CustomUser> UserManager { get; set; }

        public Guid Id { get; set; }
        public string Row { get; set; }
        public string Column { get; set; }
        public Guid FlightIdFk { get; set; }
        public int? Passport { get; set; }
        public double PricePaid { get; set; }
        public Boolean Cancelled { get; set; }
        public string? Image { get; set; }
        public string? Owner { get; set; }
        public int? PassportNo { get; set; }
    }
}
