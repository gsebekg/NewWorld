using NewWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewWorld
{
    public class Tartak : ProductionBuilding
    {
        public Tartak(int number):this()
        {

            Number = number;
        }
        public Tartak()
        {
            ResourcesCost = new Resources();
            CoinsCost = 100;
            NeededResources = new Resources();
            NeededResources.Drewno = 1;
            ProductResources = new Resources();
            ProductResources.Deski = 1;
            NeededFarmers = 10;
            WorkCost = 10;
            Number = 0;
        }
    }
}