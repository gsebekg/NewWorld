using NewWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewWorld
{
    public class ChatkaRybacka : ProductionBuilding
    {
        public ChatkaRybacka(int number)
        {
            ResourcesCost = new Resources();
            ResourcesCost.Deski = 2;
            CoinsCost = 100;
            NeededResources = new Resources();
            ProductResources = new Resources();
            ProductResources.Ryby = 0.5;
            NeededFarmers = 25;
            WorkCost = 10;
            Number = number;
        }
    }
}