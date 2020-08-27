using Microsoft.Ajax.Utilities;
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

        public Resources()
        {
            this.ZeroResources();
        }

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

        public List<double> BuildList()
        {
            return new List<double>
            {
            Deski ,
            Cegly ,
            Zagle ,
            StaloweBelki ,
            Okna ,
            Ryby ,
            Sznaps ,
            Ubrania ,
            Kiełbasa ,
            Chleb ,
            Mydlo ,
            Piwo ,
            Konserwy ,
            MaszynyDoSzycia ,
            Drewno ,
            Ziemniaki ,
            Welna ,
            Glinianka ,
            Swinie ,
            Zboze ,
            Maka ,
            Zelazo ,
            Wegiel ,
            Stal ,
            Loj ,
            Chmiel ,
            Slod ,
            Piasek ,
            Szklo ,
            Wolowina ,
            Papryka ,
            Gulasz
            };
        }

        public void UnbuildList(List<double> list)
        {
            Deski = list[0];
            Cegly = list[1];
            Zagle = list[2];
            StaloweBelki = list[3];
            Okna = list[4];
            Ryby = list[5];
            Sznaps = list[6];
            Ubrania = list[7];
            Kiełbasa = list[8];
            Chleb = list[9];
            Mydlo = list[10];
            Piwo = list[11];
            Konserwy = list[12];
            MaszynyDoSzycia = list[13];
            Drewno = list[14];
            Ziemniaki = list[15];
            Welna = list[16];
            Glinianka = list[17];
            Swinie = list[18];
            Zboze = list[19];
            Maka = list[20];
            Zelazo = list[21];
            Wegiel = list[22];
            Stal = list[23];
            Loj = list[24];
            Chmiel = list[25];
            Slod = list[26];
            Piasek = list[27];
            Szklo = list[28];
            Wolowina = list[29];
            Papryka = list[30];
            Gulasz = list[31];
        }

        public void AddResources(Resources resources)
        {
            List<double> actualResourcesList = this.BuildList();
            List<double> resourcesList = resources.BuildList();
            int i = 0;
            foreach(double resource in resourcesList)
            {
                actualResourcesList[i] += resource;
                if (actualResourcesList[i] > 200)
                    actualResourcesList[i] = 200;
                i++;
            }
            this.UnbuildList(actualResourcesList);
        }

        public void SubResources(Resources resources)
        {
            List<double> actualResourcesList = this.BuildList();
            List<double> resourcesList = resources.BuildList();
            int i = 0;
            foreach (double resource in resourcesList)
            {
                actualResourcesList[i] -= resource;
                if (actualResourcesList[i] < 0)
                    actualResourcesList[i] = 0;
                i++;
            }
            this.UnbuildList(actualResourcesList);
        }

        public static Resources MultResources(Resources resources, double multiplier)
        {
            List<double> resourcesList = resources.BuildList();
            List<double> newList = new List<double>();
            foreach (double resource in resourcesList)
            {
                newList.Add(resource * multiplier);
            }
            Resources result = new Resources();
            result.UnbuildList(newList);
            return result;
        }

    }
}

