using NewWorld.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewWorld.Models
{
    public class Island
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Place { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Glinianka { get; set; }
        public int Zelazo { get; set; }
        public bool Ziemniaki { get; set; }
        public bool Zboze { get; set; }
        public bool Chmiel { get; set; }
        public bool Papryka { get; set; }

        public virtual UserGameProperty Property { get; set; }
        public virtual Resources Resources { get; set; }
        public virtual Game Game { get; set; }
        public virtual Buildings Buildings { get; set; }




        public static double Distance(int x1, int x2, int y1, int y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }
        public static double Distance(Island islandA, Island islandB)
        {
            return Island.Distance(islandA.X, islandB.X, islandA.Y, islandB.Y);
        }

        public void LeaveIsland()
        {
            Resources.ZeroResources();
            Property = null;
        }

        public void Build(int name, int number)
        {
            BuildingList buildings = new BuildingList(this);
            Building building = buildings.buildings[name];
            long coins = Property.Coins;
            //sprawdzanie ile jest zajętego miejsca
            int sum = 0;
            foreach (Building building1 in buildings.buildings)
                sum += building1.Number;
            bool IsProductionBuilding = building.Build(number, Resources, ref coins, Place - sum) && building is ProductionBuilding;
            buildings.ListToBuildings(this);
            if (IsProductionBuilding)
                Buildings.NeededFarmers += (building as ProductionBuilding).NeededFarmers * number;
            Property.Coins = coins;
        }

        public void Destroy(int name, int number)
        {
            BuildingList buildings = new BuildingList(this);
            Building building = buildings.buildings[name];
            long coins = Property.Coins;
            building.Destroy(number, Resources, ref coins);
            buildings.ListToBuildings(this);
            Property.Coins = coins;
        }

        public bool ChangeName(string newName)
        {
            IslandRepository islandRepository = Context.islandRepository;
            if ( islandRepository.GetAllIslands(Game).Select(b => b.Name).Contains(newName))
                return true;
            Name = newName;
            return false;
        }
    }

}