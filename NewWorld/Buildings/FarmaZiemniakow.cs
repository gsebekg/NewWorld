using NewWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewWorld
{
    public class FarmaZiemniakow : ProductionBuilding
    {
        public FarmaZiemniakow(int number):this()
        {

            Number = number;
        }
        public FarmaZiemniakow()
        {
            ResourcesCost = new Resources();
            ResourcesCost.Deski = 2;
            CoinsCost = 100;
            NeededResources = new Resources();
            ProductResources = new Resources();
            ProductResources.Ziemniaki = 0.5;
            NeededFarmers = 20;
            WorkCost = 20;
            Number = 0;
        }
    }
}