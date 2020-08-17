using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewWorld.Models
{
    public class UserGameProperty
    {
        [Key]
        public int Id { get; set; }
        public Color Color { get; set; }
        public long Coins { get; set; }
        public bool Active { get; set; }
        public virtual Game Game { get; set; }
        public virtual ApplicationUser Player { get; set; }
    }

    public enum Color
    {
        red,
        green,
        yellow,
        purple
    }
}