using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewWorld.Models
{
    public class Buildings
    {
        [Key]
        public int Id { get; set; }
        public int RezydencjaFarmerow { get; set; }
        public double Farmerzy { get; set; }
        public virtual Resources FarmersSatisfaction { get; set; }
        public int ChatkaRybacka { get; set; }
        //public virtual Island Island { get; set; }
    }
}