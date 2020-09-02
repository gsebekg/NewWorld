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
        public int NeededFarmers { get; set; }
        public virtual Resources FarmersSatisfaction { get; set; }
        public int ChatkaRybacka { get; set; }
        public int ChatkaDrwala { get; set; }
        public int Tartak { get; set; }
        public int FarmaOwiec { get; set; }
        public int ZakladTkaczy { get; set; }
        public int FarmaZiemniakow { get; set; }
        public int DestylarniaSznapsu { get; set; }
    }
}