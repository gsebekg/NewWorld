using NewWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewWorld
{
    public class RezydencjaFarmerow : Residence
    {
        public RezydencjaFarmerow(double farmers, int number)
        {
            NeededResources = new Resources();
            NeededResources.Ryby = 0.00625;
            NeededResources.Ubrania = 0.0077;
            NeededResources.Sznaps = 0.00833333;
            CoinsFromResources = new Resources();
            CoinsFromResources.Ryby = 1;
            CoinsFromResources.Ubrania = 3;
            CoinsFromResources.Sznaps = 5;
            Satisfaction = new Resources();
            FarmersFromResources = new Resources();
            FarmersFromResources.Ryby = 3;
            FarmersFromResources.Ubrania = 2;
            ResourcesCost = new Resources();
            ResourcesCost.Deski = 2;
            ActualFarmers = farmers;
            Number = number;
        }
    }
}