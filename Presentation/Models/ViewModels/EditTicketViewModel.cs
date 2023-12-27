using Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels
{
    public class EditTicketViewModel
    {
        public string Row { get; set; }
        public string Column { get; set; }
        public int Passport { get; set; }
        public bool? Cancelled { get; set; }
        public string? Image { get; set; }
        public Guid FlightIdFk { get; set; }
        public Guid Id { get; set; }
        public double PricePaid { get; set; }
        public string? Owner { get; set; }



        public int Columns {  get; set; }
        public int Rows {  get; set; }

    }
}
