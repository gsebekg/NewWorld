using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewWorld.Models
{
    public class MapViewModel
    {
        public Game Game { get; set; }
        public UserGameProperty Property { get; set; }
        public List<UserGameProperty> AllPlayers { get; set; }
        public List<Island> Islands { get; set; }
    }

    public class IslandViewModel
    {
        public bool EmptyIsland { get; set; }
        public bool YourIsland { get; set; }
        public UserGameProperty Property { get; set; }
        public Resources Resources { get; set; }
        public Island Island { get; set; }
        public Buildings Buildings { get; set; }
        public List<int> MaxBuildings { get; set; }
        public long Coins { get; set; }
        public List<string> ResourceImages { get; set; }
        public List<double> ResourcesList { get; set; }


        public void CalculateMaxBuildings(Island island, List<Building> buildings)
        {
            MaxBuildings = new List<int>();
            int sum = 0;
            foreach (Building building in buildings)
                sum += building.Number;
            foreach (Building building in buildings)
                MaxBuildings.Add(building.HowManyCanYouBuild(island.Resources, Coins, island.Place - sum));
        }
    }

    public class BuildDestroyViewModel
    {
        public int Name { get; set; }
        public int Id { get; set; }
        public int Number { get; set; }
    }

    public class BuildingList
    {
        public List<Building> buildings;
        public BuildingList(Island island)
        {
            Buildings buildings1 = island.Buildings;
            buildings = new List<Building> {
                new RezydencjaFarmerow(buildings1.Farmerzy,buildings1.RezydencjaFarmerow),
                new ChatkaRybacka(buildings1.ChatkaRybacka),
                new ChatkaDrwala(buildings1.ChatkaDrwala),
                new Tartak(buildings1.Tartak),
                new FarmaOwiec(buildings1.FarmaOwiec),
                new ZakladTkaczy(buildings1.ZakladTkaczy),
                new FarmaZiemniakow(buildings1.FarmaZiemniakow),
                new DestylarniaSznapsu(buildings1.DestylarniaSznapsu)
            };
        }

        public void ListToBuildings(Island island)
        {
            Buildings buildings1 = island.Buildings;
            buildings1.RezydencjaFarmerow = buildings[0].Number;
            buildings1.ChatkaRybacka = buildings[1].Number;
            buildings1.ChatkaDrwala = buildings[2].Number;
            buildings1.Tartak = buildings[3].Number;
            buildings1.FarmaOwiec = buildings[4].Number;
            buildings1.ZakladTkaczy = buildings[5].Number;
            buildings1.FarmaZiemniakow = buildings[6].Number;
            buildings1.DestylarniaSznapsu = buildings[7].Number;
        }
    }

    public class ChangeNameViewModel
    {
        [Required(ErrorMessage = "Wprowadź nową nazwę wyspy")]
        [Display(Name = "Nowa nazwa wyspy")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Nazwa musi mieć długość z zakresu od {2} do {1}")]
        public string NewName { get; set; }
        public int Id { get; set; }
        public bool? NameUsed { get; set; }
    }
}