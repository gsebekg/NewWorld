using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewWorld.Models
{
    public class UserGameProperty
    {
        [Key]
        public int Id { get; set; }
        public Color Color { get; set; }
        public long Coins { get; set; }
        public bool Active { get; set; }
        public virtual Game Game { get; set; }
        public virtual ApplicationUser Player { get; set; }
        public virtual ICollection<Island> Islands { get; set; }

        public void CalculateGameForPlayer()
        {
            long coins = Coins;
            foreach (Island island in Islands.ToList())
            {
                Buildings buildings = island.Buildings;
                List<ProductionBuilding> productionBuildings = new List<ProductionBuilding>
                    {
                        new ChatkaRybacka(buildings.ChatkaRybacka),
                        new ChatkaDrwala(buildings.ChatkaDrwala),
                        new Tartak(buildings.Tartak),
                        new FarmaOwiec(buildings.FarmaOwiec),
                        new ZakladTkaczy(buildings.ZakladTkaczy),
                        new FarmaZiemniakow(buildings.FarmaZiemniakow),
                        new DestylarniaSznapsu(buildings.DestylarniaSznapsu)
                    };
                List<Residence> residences = new List<Residence>
                    {
                        new RezydencjaFarmerow(buildings.Farmerzy,buildings.RezydencjaFarmerow)
                    };
                int neededFarmerzy = 0;
                //obliczanie potrzebnej ilości mieszkańców  w sumie
                foreach (ProductionBuilding productionBuilding in productionBuildings)
                {
                    neededFarmerzy += productionBuilding.NeededFarmers * productionBuilding.Number;
                }
                buildings.NeededFarmers = neededFarmerzy;
                if (neededFarmerzy > 0)
                {
                    double farmersProductivity = buildings.Farmerzy / neededFarmerzy;
                    //produkcja dla każdego rodzaju budynku produkcyjnego
                    foreach (ProductionBuilding productionBuilding in productionBuildings)
                    {
                        productionBuilding.Product(island.Resources, ref coins, farmersProductivity);
                    }
                }
                double actualFarmers = 0;
                if (buildings.FarmersSatisfaction == null)
                    buildings.FarmersSatisfaction = new Resources();
                residences[0].Satisfaction = buildings.FarmersSatisfaction;
                // wyżywnie ludzi z każdego typu budynku mieszkalnego
                foreach (Residence residence in residences)
                {
                    coins += residence.Consume(island.Resources, out actualFarmers);
                }
                buildings.FarmersSatisfaction = residences[0].Satisfaction;
                buildings.Farmerzy = actualFarmers;
            }
            Coins = coins;
        }
    }


    public enum Color
    {
        red,
        green,
        yellow,
        purple
    }

    
}