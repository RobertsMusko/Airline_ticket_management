namespace Presentation.Models.ViewModels
{
    public class ListFlightsAdminViewModel
    {
        public Guid Id { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string CountryFrom { get; set; }
        public string CountryTo { get; set; }
        public double WholesalePrice { get; set; }
        public double CommisionRate { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public double RetailPrice { get; set; }
    }
}
