﻿using NewWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewWorld
{
    public class CyclicProduct
    {
        private ApplicationDbContext db;
        public CyclicProduct()
        {
            db = new ApplicationDbContext();
        }

        public void CalculateGame(int id)
        {
            Game game = db.Games.Find(id);
            List<UserGameProperty> properties = game.UserGameProperties.ToList();
            foreach(UserGameProperty property in properties)
            {
                long coins = property.Coins;
                foreach (Island island in property.Islands.ToList())
                {
                    Buildings buildings = island.Buildings;
                    List<ProductionBuilding> productionBuildings = new List<ProductionBuilding>
                    {
                        new ChatkaRybacka(buildings.ChatkaRybacka)
                    };
                    List<Residence> residences = new List<Residence>
                    {
                        new RezydencjaFarmerow(buildings.Farmerzy,buildings.RezydencjaFarmerow)
                    };
                    int neededFarmerzy = 0;
                    foreach (ProductionBuilding productionBuilding in productionBuildings)
                    {
                        neededFarmerzy += productionBuilding.NeededFarmers * productionBuilding.Number;
                    }
                    if (neededFarmerzy > 0)
                    {
                        double farmersProductivity = buildings.Farmerzy / neededFarmerzy;
                        foreach (ProductionBuilding productionBuilding in productionBuildings)
                        {
                            productionBuilding.Product(island.Resources, ref coins, farmersProductivity);
                        }
                    }
                    double actualFarmers = 0 ;
                    foreach (Residence residence in residences)
                    {
                        coins += residence.Consume(island.Resources, out actualFarmers);
                    }
                    Resources satisfaction = buildings.FarmersSatisfaction;
                    satisfaction = residences[0].Satisfaction;
                    buildings.Farmerzy = actualFarmers;
                }
                property.Coins = coins;
            }
            db.SaveChanges();

        }
    }
}