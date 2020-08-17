using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewWorld.Models
{
    public class HomeViewModels
    {
        public List<Game> YourGames { get; set; }
        public Game NewGame { get; set; }
        public bool? NameUsed { get; set; }

    }
}