using NewWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewWorld
{
    public class DestylarniaSznapsu : ProductionBuilding
    {
        public DestylarniaSznapsu(int number):this()
        {

            Number = number;
        }
        public DestylarniaSznapsu()
        {
            ResourcesCost = new Resources();
            ResourcesCost.Deski = 2;
            CoinsCost = 100;
            NeededResources = new Resources();
            NeededResources.Ziemniaki = 0.5;
            ProductResources = new Resources();
            ProductResources.Sznaps = 0.5;
            NeededFarmers = 50;
            WorkCost = 40;
            Number = 0;
        }
    }
}