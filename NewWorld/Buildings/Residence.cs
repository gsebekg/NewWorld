﻿using NewWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewWorld
{
    public abstract class Residence: Building
    {
        public Resources NeededResources { get; set; }
        public Resources CoinsFromResources { get; set; }
        public Resources Satisfaction { get; set; }
        public Resources FarmersFromResources { get; set; }
        public double ActualFarmers { get; set; }
        protected double goalFarmers;
        protected int MaxFarmers;
        public int Consume(Resources yourResources,out double actualFarmers)
        {
            if (Number > 0)
            {
                List<double> yourResourcesList = yourResources.BuildList();
                List<double> satisfactionList = Satisfaction.BuildList();
                List<double> neededResourcesList = NeededResources.BuildList();
                List<double> CoinsFromResourcesList = CoinsFromResources.BuildList();
                List<double> FarmersFromResourcesList = FarmersFromResources.BuildList();
                int i = 0;
                double coins = 0;
                goalFarmers = 5*Number;
                //konsumpcja i obliczanie satysfakcji dla każdego surowca
                foreach (double neededResource in neededResourcesList)
                {
                    if (neededResource > 0)
                    {
                        satisfactionList[i] = yourResourcesList[i] / (neededResource * Number);
                        if (satisfactionList[i] > 1)
                            satisfactionList[i] = 1;
                        yourResourcesList[i] -= neededResource * Number;
                        if (yourResourcesList[i] < 0)
                            yourResourcesList[i] = 0;
                        coins += CoinsFromResourcesList[i] * satisfactionList[i] * Number;
                        goalFarmers += FarmersFromResourcesList[i] * satisfactionList[i] * Number;
                        satisfactionList[i] *= 100;
                    }
                    i++;
                }
                // zmiana ilości mieszkańców
                if (goalFarmers > ActualFarmers)
                {
                    ActualFarmers += Number;
                    if (goalFarmers < ActualFarmers)
                        ActualFarmers = goalFarmers;
                }
                if (goalFarmers < ActualFarmers)
                {
                    ActualFarmers -= Number;
                    if (goalFarmers > ActualFarmers)
                        ActualFarmers = goalFarmers;
                }
                yourResources.UnbuildList(yourResourcesList);
                Satisfaction.UnbuildList(satisfactionList);
                actualFarmers = ActualFarmers;
                return (int)Math.Floor(coins);
            }
            actualFarmers = ActualFarmers;
            return 0;
        }
    }
}