using NewWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewWorld
{
    public static class CyclicProduct
    {

        public static void CalculateGame(int id, ApplicationDbContext db)
        {
            Game game = db.Games.Find(id);
            List<UserGameProperty> properties = game.UserGameProperties.ToList();
            //obliczanie ile minut upłyneło od ostatniego update
            TimeSpan timeSinceLastUpdate = DateTime.Now.Subtract(game.Update);
            int NumberOfCycles = (int)timeSinceLastUpdate.TotalMinutes;
            game.Update = game.Update.AddMinutes(NumberOfCycles);
            db.SaveChanges();
            for (int i = 0; i < NumberOfCycles; i++)
            {
                foreach (UserGameProperty property in properties)
                {
                    long coins = property.Coins;
                    foreach (Island island in property.Islands.ToList())
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
                    property.Coins = coins;
                }
            }
            db.SaveChanges();

        }
    }
}