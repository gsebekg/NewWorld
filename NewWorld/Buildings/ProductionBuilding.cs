using NewWorld.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;

namespace NewWorld
{
    public abstract class ProductionBuilding : Building
    {
        public Resources NeededResources { get; set; }
        public Resources ProductResources { get; set; }
        public int NeededFarmers { get; set; }
        public int WorkCost { get; set; }
        public void Product(Resources yourResources, ref long coins, double farmersProductivity)
        {
            if (Number > 0)
            {
                List<double> yourResourcesList = yourResources.BuildList();
                List<double> productResourcesList = ProductResources.BuildList();
                List<double> neededResourcesList = NeededResources.BuildList();
                double productivity = farmersProductivity * Number * neededResourcesList.Max();
                double productivityTmp;
                int i = 0;
                // obliczamy na jaką produktywnośc starczy nam surowców i na ile mamy miejsca
                foreach (double resource in neededResourcesList)
                {
                    if (resource > 0)
                    {
                        productivityTmp = yourResourcesList[i];
                        if (productivityTmp < productivity)
                            productivity = productivityTmp;
                    }
                    if (productResourcesList[i] > 0)
                    {
                        productivityTmp = 200 - yourResourcesList[i];
                        if (productivityTmp < productivity)
                            productivity = productivityTmp;
                    }
                    i++;
                }
                //mnozymy produkowane zasoby i dodajemy do aktualnych
                Resources resultResources = Resources.MultResources(ProductResources, productivity);
                yourResources.AddResources(resultResources);
                //mnozymy potrzebne zasoby i odejmujemy od aktualnych
                resultResources = Resources.MultResources(NeededResources, productivity);
                yourResources.SubResources(resultResources);
                coins -= WorkCost * Number;
            }
        }
    }
}