using System;
using System.Collections.Generic;
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
        public long Coins { get; set; }
    }
}