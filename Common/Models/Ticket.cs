using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public Guid Id { get; set; }
        public string Row {  get; set; }    
        public string Column { get; set; }

        [ForeignKey("Flight")]
        public Guid FlightIdFk { get; set; }
        public Flight Flight { get; set; }
        public int Passport {  get; set; }
        public double PricePaid {  get; set; }
        public Boolean Cancelled {  get; set; }
        public string? Image { get; set; }
        public string? Owner { get; set; }

        public string SeatId { get { return Row + "," + Column; } }

    }
}
