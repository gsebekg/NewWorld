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
                new ChatkaRybacka(buildings1.ChatkaRybacka) 
            };
        }

        public void ListToBuildings(Island island)
        {
            Buildings buildings1 = island.Buildings;
            buildings1.RezydencjaFarmerow = this.buildings[0].Number;
            buildings1.ChatkaRybacka = this.buildings[1].Number;
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