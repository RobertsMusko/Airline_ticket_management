using Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels
{
    public class BookFlightViewModel
    {
        //TE IR SEDVIETAS NUMURI

        public string Row { get; set; }
        public string Column { get; set; }
        public int Passport { get; set; }
        public double PricePaid { get; set; }
        public Boolean Cancelled { get; set; }
        public Guid FlightIdFk { get; set; }
        public IFormFile PassportImage { get; set; }
        public Guid Id { get; set; }
        public int? PassportNo { get; set; }
        public int Columns { get; set; }
        public int Rows { get; set; }

        public string[] SelectedSeats { get; set; }





    }
}
