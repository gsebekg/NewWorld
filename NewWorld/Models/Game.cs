using Microsoft.Ajax.Utilities;
using NewWorld.Repositories;
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

        public List<Island> InitializeGame(List<UserGameProperty> properties)
        {
            IsBegan = true;
            Update = DateTime.Now;
            Random rand = new Random();
            List<Island> islands = new List<Island>();
            int x, y;
            int wielkoscMapy = MaxPlayers * 5;
            bool positionOk;
            //losowanie wysp graczy
            for (int i = 0; i < MaxPlayers; i++)
            {
                //losujemy współrzędne tak by wyspa nie znajdowała sie za blisko innej
                do
                {
                    positionOk = true;
                    x = rand.Next(0, wielkoscMapy);
                    y = rand.Next(0, wielkoscMapy);
                    foreach (Island item in islands)
                    {
                        if (Island.Distance(x, item.X, y, item.Y) < 2)
                        {
                            positionOk = false;
                            break;
                        }
                    }
                }
                while (!positionOk);
                Resources resources = new Resources();
                resources.InitialResources();
                Island island = new Island
                {
                    Name = "Wyspa " + properties[i].Player.UserName,
                    Place = 500,
                    X = x,
                    Y = y,
                    Resources = resources,
                    Ziemniaki = true,
                    Chmiel = true,
                    Zboze = false,
                    Papryka = false,
                    Glinianka = 2,
                    Zelazo = 2,
                    Property = properties[i],
                    Game = this
                };
                island.Buildings = new Buildings();
                island.Buildings.FarmersSatisfaction = new Resources();
                islands.Add(island);
            }
            //losowanie pustych wysp
            for (int i = 0; i < MaxPlayers * 4; i++)
            {
                do
                {
                    positionOk = true;
                    x = rand.Next(0, wielkoscMapy);
                    y = rand.Next(0, wielkoscMapy);
                    foreach (Island item in islands)
                    {
                        if (Island.Distance(x, item.X, y, item.Y) < 2)
                        {
                            positionOk = false;
                            break;
                        }
                    }
                }
                while (!positionOk);
                Resources resources = new Resources();
                resources.ZeroResources();
                int glinianka = rand.Next(-1, 4);
                glinianka = (glinianka == -1) ? 0 : glinianka;
                int zelazo = rand.Next(-1, 4);
                zelazo = (zelazo == -1) ? 0 : zelazo;
                Island island = new Island
                {
                    Name = "Wyspa " + (i + 1),
                    Place = rand.Next(300, 601),
                    X = x,
                    Y = y,
                    Resources = resources,
                    Ziemniaki = rand.Next(1, 9) <= 3,
                    Chmiel = rand.Next(1, 9) <= 3,
                    Zboze = rand.Next(1, 9) <= 5,
                    Papryka = rand.Next(1, 9) <= 5,
                    Glinianka = glinianka,
                    Zelazo = zelazo,
                    Property = null,
                    Game = this
                };
                island.Buildings = new Buildings();
                islands.Add(island);
            }
            return islands;
        }

        //obliczanie ile minut upłyneło od ostatniego update
        public int CalculateNumberOfCycles()
        {
            GameRepository gameRepository = Context.gameRepository;
            TimeSpan timeSinceLastUpdate = DateTime.Now.Subtract(Update);
            int numberOfCycles = (int)timeSinceLastUpdate.TotalMinutes;
            Update = Update.AddMinutes(numberOfCycles);
            gameRepository.Save();
            return numberOfCycles;
        }

        public bool PlayerIsActive(ApplicationUser user)
        {
            var ids = Players.Select(a => a.Id).ToList();
            //sprawdzanie czy gracz należy do tej gry i czy się nie wycofał
            return (ids.Contains(user.Id) && UserGameProperties.Where(a => a.Player.Id == user.Id).SingleOrDefault().Active == true);
        }
    }
}