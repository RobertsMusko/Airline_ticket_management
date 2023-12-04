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
        public int Id { get; set; }
        public string Row {  get; set; }    
        public string Column { get; set; }

        [ForeignKey("Flight")]
        public int FlightIdFK { get; set; }
        public Flight Flight { get; set; }


        public int Passaport {  get; set; }
        public double PricePaid {  get; set; }
        public Boolean Cancelled {  get; set; } 

    }
}
