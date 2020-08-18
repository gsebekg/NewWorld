using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewWorld.Models
{
    public class HomeViewModels
    {
        public List<Game> YourGames { get; set; }
        public List<Game> OpenGames { get; set; }
        public Game NewGame { get; set; }
        public bool? NameUsed { get; set; }
        public bool? HaveMaxGames { get; set; }

    }

    public class JoinViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool? WrongPassword { get; set; }
    }
}