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
        public ActionResult GameList()  
        {
            return View(GetHomeViewModel());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GameList([Bind(Prefix = "NewGame")] Game game)
        {
            game.Name = game.Name.Trim();
            bool alreadyUsed = false;
            if (ModelState.IsValid)
            {
                //sprawdzanie czy dana nazwa juz nie istnieje
                if (db.Games.Where(a => a.Name.Equals(game.Name)).ToList().Count() > 0)
                    alreadyUsed = true;
                else
                {
                    game.CreateDate = DateTime.Now;
                    game.IsBegan = false;
                    ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                    UserGameProperty userGameProperty = new UserGameProperty { Active = true, Color = 0, Player = user };
                    game.Players = new List<ApplicationUser> { user };
                    game.UserGameProperties = new List<UserGameProperty> { userGameProperty };
                    db.Games.Add(game);
                    db.SaveChanges();
                }
            }
            HomeViewModels viewModel = GetHomeViewModel();
            viewModel.NameUsed = alreadyUsed;
            return View(viewModel);

        }
        [Authorize]
        public ActionResult Delete(int id)
        {
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            Game game = db.Games.Find(id);
            UserGameProperty userGameProperty = db.UserGameProperties.Where(a => a.Player.Id == user.Id).Where(b => b.Game.Id == game.Id).FirstOrDefault();
            if(game.IsBegan)
                return RedirectToAction("GameList");
            game.Players.Remove(user);
            db.UserGameProperties.Remove(userGameProperty);
            if (game.Players.Count == 0)
                db.Games.Remove(game);
            db.SaveChanges();
            return RedirectToAction("GameList");
        }
        // dołącz do istniejącej gry
        [Authorize]
        public ActionResult Join(int id)
        {
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            Game game = db.Games.Find(id);
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
            if (ModelState.IsValid)
            {
                ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                if (game.Password != viewModel.Password)
                    return View(new JoinViewModel { Id = viewModel.Id, Name = game.Name, WrongPassword = true });
                if (game.Players.Contains(user) || game.IsBegan)
                    return RedirectToAction("GameList");
                AddUserToGame(user, game);
                db.SaveChanges();
                return RedirectToAction("GameList");
            }
            return View(new JoinViewModel { Id = viewModel.Id, Name = game.Name, WrongPassword=true });




        }
            //helpers
            private HomeViewModels GetHomeViewModel()
        {
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            // pobieranie gier do których należy gracz
            var gameList = db.Games.Where(a => a.Players.Select(c => c.Id).Contains(user.Id)).ToList();
            //wybieranie tych w których jest aktywny
            gameList.Where(a => a.UserGameProperties.Where(b => b.Player.Id == User.Identity.GetUserId()).Where(c => c.Active).ToList().Count() == 1).ToList();
            var gameListIds = gameList.Select(a => a.Id).ToList();
            //pobieranie otwartych gier z wyjątkiem tych do których dołączył gracz
            List<Game> otherGames = db.Games.Where(a => !a.IsBegan).Where(b=>!gameListIds.Contains(b.Id)).ToList();
            HomeViewModels viewModel = new HomeViewModels { YourGames = gameList, OpenGames=otherGames };
            return viewModel;
        }
        //dodanie gracza do gry
        private void AddUserToGame(ApplicationUser user, Game game)
        {
            game.Players.Add(user);
            UserGameProperty userGameProperty = new UserGameProperty { Active = true, Color = (Color)(game.NumberOfPlayers() - 1), Player = user };
            game.UserGameProperties.Add(userGameProperty);
            if (game.NumberOfPlayers() == game.MaxPlayers)
                game.IsBegan = true;
        }
    }
}