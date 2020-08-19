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
        public List<Island> Islands { get; set; }
    }
}