using NewWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewWorld
{
    public class FarmaOwiec : ProductionBuilding
    {
        public FarmaOwiec(int number):this()
        {

            Number = number;
        }
        public FarmaOwiec()
        {
            ResourcesCost = new Resources();
            ResourcesCost.Deski = 2;
            CoinsCost = 100;
            NeededResources = new Resources();
            ProductResources = new Resources();
            ProductResources.Welna = 0.5;
            NeededFarmers = 10;
            WorkCost = 20;
            Number = 0;
        }
    }
}