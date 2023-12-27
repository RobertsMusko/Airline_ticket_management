using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels
{
    public class ListTicketsAdminViewModel
    {
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
