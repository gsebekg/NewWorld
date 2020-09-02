using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using NewWorld.Models;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewWorld.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db;
        public HomeController()
        {
            db = new ApplicationDbContext();
        }

        public HomeController(ApplicationDbContext db)
        {
            this.db = db;
        }
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult GameList(bool? id)  //jeżeli id==true to masz juz maksymalna liczbę gier
        {
            HomeViewModels viewModel = GetHomeViewModel();
            viewModel.HaveMaxGames = id.GetValueOrDefault();
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GameList([Bind(Prefix = "NewGame")] Game game)
        {
            game.Name = game.Name.Trim();
            bool alreadyUsed = false;
            bool haveMaxGames = false;
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                //sprawdzanie czy gracz nie ma juz zaczętych maksymalnej liczby gier
                if (user.UserGameProperties.Where(a => a.Active).Count() < 3)
                {
                    //sprawdzanie czy dana nazwa juz nie istnieje
                    if (db.Games.Where(a => a.Name.Equals(game.Name)).ToList().Count() > 0)
                        alreadyUsed = true;
                    else
                    {
                        game.CreateDate = DateTime.Now;
                        game.Update = DateTime.Now;
                        game.IsBegan = false;
                        UserGameProperty userGameProperty = new UserGameProperty { Active = true, Color = 0, Player = user, Coins=50000 };
                        game.Players = new List<ApplicationUser> { user };
                        game.UserGameProperties = new List<UserGameProperty> { userGameProperty };
                        db.Games.Add(game);
                        db.SaveChanges();
                    }
                }
                else
                    haveMaxGames = true;
            }
            HomeViewModels viewModel = GetHomeViewModel();
            viewModel.NameUsed = alreadyUsed;
            viewModel.HaveMaxGames = haveMaxGames;
            return View(viewModel);

        }
        [Authorize]
        public ActionResult Delete(int id)
        {
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            Game game = db.Games.Find(id);
            UserGameProperty userGameProperty = db.UserGameProperties.Where(a => a.Player.Id == user.Id).Where(b => b.Game.Id == game.Id).FirstOrDefault();
            if (game.IsBegan)
                return RedirectToAction("GameList");
            game.Players.Remove(user);
            db.UserGameProperties.Remove(userGameProperty);
            if (game.Players.Count == 0)
            {
                List<Island> islands = db.Islands.Where(a => a.Game.Id == game.Id).ToList();
                db.Islands.RemoveRange(islands).ToList();
                db.Games.Remove(game);
            }
            db.SaveChanges();
            return RedirectToAction("GameList");
        }
        // dołącz do istniejącej gry
        [Authorize]
        public ActionResult Join(int id)
        {
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            Game game = db.Games.Find(id);
            //sprawdzanie czy gracz nie ma juz zaczętych maksymalnej liczby gier
            if (user.UserGameProperties.Where(a => a.Active).Count() >= 3)
                return RedirectToAction("GameList", new { id = true });
            //jeżeli gra ma hasło wyswietl strone do wprowadzania hasła
            if (game.HavePassword())
            {
                JoinViewModel viewModel = new JoinViewModel { Id = id, Name = game.Name };
                return View(viewModel);
            }
            if (game.Players.Contains(user) || game.IsBegan)
                return RedirectToAction("GameList");
            AddUserToGame(user, game);
            db.SaveChanges();
            return RedirectToAction("GameList");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Join([Bind(Include = "Id,Password")] JoinViewModel viewModel)
        {
            Game game = db.Games.Find(viewModel.Id);
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            //sprawdzanie czy gracz nie ma juz zaczętych maksymalnej liczby gier
            if (user.UserGameProperties.Where(a => a.Active).Count() >= 3)
                return RedirectToAction("GameList", new { id = true });
            if (ModelState.IsValid)
            {
                if (game.Password != viewModel.Password)
                    return View(new JoinViewModel { Id = viewModel.Id, Name = game.Name, WrongPassword = true });
                if (game.Players.Contains(user) || game.IsBegan)
                    return RedirectToAction("GameList");
                AddUserToGame(user, game);
                db.SaveChanges();
                return RedirectToAction("GameList");
            }
            return View(new JoinViewModel { Id = viewModel.Id, Name = game.Name, WrongPassword = true });
        }

        [Authorize]
        public ActionResult GiveUp(int id)
        {
            Game game = db.Games.Find(id);
            return View(game);
        }

        [Authorize]
        public ActionResult GiveUpConfirmed(int id)
        {
            Game game = db.Games.Find(id);
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            UserGameProperty userGameProperty = db.UserGameProperties.Where(a => a.Game.Id == game.Id).Where(b => b.Player.Id == user.Id).FirstOrDefault();
            List<Island> islands = db.Islands.Where(a => a.Property.Id == userGameProperty.Id).ToList();
            foreach (Island island in islands)
            {
                island.Resources.ZeroResources();
                island.Property = null;
            }

            userGameProperty.Active = false;
            db.SaveChanges();
            
            return RedirectToAction("GameList");
        }


        //helpers
        private HomeViewModels GetHomeViewModel()
        {
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            // pobieranie gier do których należy gracz
            var gameList = db.Games.Where(a => a.Players.Select(c => c.Id).Contains(user.Id)).ToList();
            //wybieranie tych w których jest aktywny
            gameList = gameList.Where(a => a.UserGameProperties.Where(b => b.Player.Id == User.Identity.GetUserId()).Where(c => c.Active).ToList().Count() == 1).ToList();
            var gameListIds = gameList.Select(a => a.Id).ToList();
            //pobieranie otwartych gier z wyjątkiem tych do których dołączył gracz
            List<Game> otherGames = db.Games.Where(a => !a.IsBegan).Where(b => !gameListIds.Contains(b.Id)).ToList();
            HomeViewModels viewModel = new HomeViewModels { YourGames = gameList, OpenGames = otherGames };
            return viewModel;
        }
        //dodanie gracza do gry, jeżeli gra jest pełna to wystartuj
        private void AddUserToGame(ApplicationUser user, Game game)
        {
            game.Players.Add(user);
            UserGameProperty userGameProperty = new UserGameProperty { Active = true, Color = (Color)(game.NumberOfPlayers() - 1), Player = user, Coins=10000 };
            game.UserGameProperties.Add(userGameProperty);
            if (game.NumberOfPlayers() == game.MaxPlayers)
            {
                game.IsBegan = true;
                game.Update= DateTime.Now;
                db.SaveChanges();
                Random rand = new Random();
                List<UserGameProperty> properties = db.UserGameProperties.Where(a => a.Game.Id == game.Id).ToList();
                List<Island> islands = new List<Island>();
                int x, y;
                int wielkoscMapy = game.MaxPlayers * 5;
                bool positionOk;
                //losowanie wysp graczy
                for (int i=0;i<game.MaxPlayers;i++)
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
                        Game = game
                    };
                    island.Buildings = new Buildings();
                    islands.Add(island);
                }
                //losowanie pustych wysp
                for(int i=0; i<game.MaxPlayers*4;i++)
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
                        Name = "Wyspa " + (i+1),
                        Place = rand.Next(300,601),
                        X = x,
                        Y = y,
                        Resources = resources,
                        Ziemniaki = rand.Next(1,9)<=3,
                        Chmiel = rand.Next(1, 9) <= 3,
                        Zboze = rand.Next(1, 9) <= 5,
                        Papryka = rand.Next(1, 9) <= 5,
                        Glinianka = glinianka,
                        Zelazo = zelazo,
                        Property = null,
                        Game=game
                    };
                    island.Buildings = new Buildings();
                    islands.Add(island);
                }
                db.Islands.AddRange(islands);
                db.SaveChanges();
            }

        }
    }
}