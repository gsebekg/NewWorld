using NewWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewWorld
{
    public class ZakladTkaczy : ProductionBuilding
    {
        public ZakladTkaczy(int number):this()
        {

            Number = number;
        }
        public ZakladTkaczy()
        {
            ResourcesCost = new Resources();
            ResourcesCost.Deski = 2;
            CoinsCost = 400;
            NeededResources = new Resources();
            NeededResources.Welna = 0.5;
            ProductResources = new Resources();
            ProductResources.Ubrania = 0.5;
            NeededFarmers = 50;
            WorkCost = 50;
            Number = 0;
        }
    }
}