using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewWorld.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Wprowadź nazwę gry")]
        [Display(Name = "Nazwa gry")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Nazwa musi mieć długość z zakresu od {2} do {1}")]
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsBegan { get; set; }
        [Required(ErrorMessage = "Podaj maksymalna liczbę graczy")]
        [Display(Name = "Maksymalna liczba graczy")]
        [Range(2, 4, ErrorMessage = "Podaj wartość pomiędzy {1}, a {2}")]
        public int MaxPlayers { get; set; }
        [Display(Name = "Hasło (opcjonalne)")]
        [StringLength(25, ErrorMessage = "Maksymalna długośc hasła to {1}")]
        public string Password { get; set; }
        public DateTime Update { get; set; }
        public virtual ICollection<ApplicationUser> Players { get; set; }
        public virtual ICollection<UserGameProperty> UserGameProperties { get; set; }

        public int NumberOfPlayers()
        {
            return Players.Count;
        }
        public bool HavePassword()
        {
            return !Password.IsNullOrWhiteSpace();
        }
    }
}