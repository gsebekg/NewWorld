using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewWorld.Models
{
    public class Resources
    {
        [Key]
        public int Id { get; set; }
        public double Deski { get; set; }
        public double Cegly { get; set; }
        public double Zagle { get; set; }
        public double StaloweBelki { get; set; }
        public double Okna { get; set; }
        public double Ryby { get; set; }
        public double Sznaps { get; set; }
        public double Ubrania { get; set; }
        public double Kiełbasa { get; set; }
        public double Chleb { get; set; }
        public double Mydlo { get; set; }
        public double Piwo { get; set; }
        public double Konserwy { get; set; }
        public double MaszynyDoSzycia { get; set; }
        public double Drewno { get; set; }
        public double Ziemniaki { get; set; }
        public double Welna { get; set; }
        public double Glinianka { get; set; }
        public double Swinie { get; set; }
        public double Zboze { get; set; }
        public double Maka { get; set; }
        public double Zelazo { get; set; }
        public double Wegiel { get; set; }
        public double Stal { get; set; }
        public double Loj { get; set; }
        public double Chmiel { get; set; }
        public double Slod { get; set; }
        public double Piasek { get; set; }
        public double Szklo { get; set; }
        public double Wolowina { get; set; }
        public double Papryka { get; set; }
        public double Gulasz { get; set; }



        public void ZeroResources()
        {
            Deski = 0;
            Cegly = 0;
            Zagle = 0;
            StaloweBelki = 0;
            Okna = 0;
            Ryby = 0;
            Sznaps = 0;
            Ubrania = 0;
            Kiełbasa = 0;
            Chleb = 0;
            Mydlo = 0;
            Piwo = 0;
            Konserwy = 0;
            MaszynyDoSzycia = 0;
            Drewno = 0;
            Ziemniaki = 0;
            Welna = 0;
            Glinianka = 0;
            Swinie = 0;
            Zboze = 0;
            Maka = 0;
            Zelazo = 0;
            Wegiel = 0;
            Stal = 0;
            Loj = 0;
            Chmiel = 0;
            Slod = 0;
            Piasek = 0;
            Szklo = 0;
            Wolowina = 0;
            Papryka = 0;
            Gulasz = 0;

        }
        public void InitialResources()
        {
            this.ZeroResources();
            Deski = 50;
            Ryby = 10;
        }
    }

}