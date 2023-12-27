using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.ViewModels
{
    public class ListFlightViewModel
    {
        public Guid Id { get; set; }
        public string DepartureDate { get; set; }
        public string ArrivalDate { get; set; }
        public string CountryFrom { get; set; }
        public string CountryTo { get; set; }
        public double RetailPrice { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public double WholesalePrice { get; set; }
        public double CommisionRate { get; set; }
        public int Columns { get; set; }
        public int Rows { get; set; }
        public string[] SelectedSeats { get; set; }


    }
}
