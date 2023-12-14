﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Flight
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } 
        public int Rows {  get; set; }
        public int Columns { get; set; }    
        public string DepartureDate {  get; set; }
        public string ArrivalDate { get; set; } 
        public string CountryFrom {  get; set; }    
        public string CountryTo { get; set; }
        public double WholesalePrice {  get; set; } 
        public double CommisionRate {  get; set; }  




    }
}
