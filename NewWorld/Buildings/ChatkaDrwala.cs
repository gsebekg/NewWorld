using NewWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewWorld
{
    public class ChatkaDrwala : ProductionBuilding
    {
        public ChatkaDrwala(int number):this()
        {

            Number = number;
        }
        public ChatkaDrwala()
        {
            ResourcesCost = new Resources();
            CoinsCost = 100;
            NeededResources = new Resources();
            ProductResources = new Resources();
            ProductResources.Drewno = 1;
            NeededFarmers = 5;
            WorkCost = 10;
            Number = 0;
        }
    }
}