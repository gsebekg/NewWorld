using NewWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewWorld
{
    public abstract class Building
    {
        public Resources ResourcesCost { get; set; }
        public long CoinsCost { get; set; }
        public int Number { get; set; }
        public int HowManyCanYouBuild(Resources yourResources, long yourCoins, int freePlace) 
        {
            int max=freePlace;
            int maxTmp;
            List<double> YourResourcesList = yourResources.BuildList();
            List<double> ResourcesCostList = ResourcesCost.BuildList();
            int i = 0;
            foreach(double resource in YourResourcesList)
            {
                if(ResourcesCostList[i]>0)
                {
                    maxTmp = (int)Math.Floor(resource / ResourcesCostList[i]);
                    if (maxTmp < max)
                        max = maxTmp;
                }
                i++;
            }
            maxTmp = (int)(yourCoins / CoinsCost);
            if (maxTmp < max)
                max = maxTmp;
            return max;
        }

        public bool Build(int howMany, Resources yourResources, ref long yourCoins, int freePlace)
        {
            if(HowManyCanYouBuild(yourResources,yourCoins,freePlace)>=howMany)
            {
                yourResources.SubResources(Resources.MultResources(ResourcesCost, howMany));
                yourCoins -= CoinsCost * howMany;
                Number += howMany;
                return true;
            }
            return false;
        }

        public bool Destroy(int howMany, Resources yourResources, ref long yourCoins)
        {
            if(Number>=howMany)
            {
                yourResources.AddResources(Resources.MultResources(ResourcesCost, 0.5 * howMany);
                yourCoins += (long)(CoinsCost * 0.5 * howMany);
                Number -= howMany;
                return true;
            }
            return false;
        }

    }
}