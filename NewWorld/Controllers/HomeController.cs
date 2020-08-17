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
        public ActionResult GameList()  //jeżeli wartość id==true to nazwa jest zajęta
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

        public ActionResult Delete(int id)
        {
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            Game game = db.Games.Find(id);
            game.Players.Remove(user);
            if (game.Players.Count == 0)
                db.Games.Remove(game);
            db.SaveChanges();
            return RedirectToAction("GameList");
        }

        //helpers
        private HomeViewModels GetHomeViewModel()
        {
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            var gameList = db.Games.Where(a => a.Players.Select(c => c.Id).Contains(user.Id)).ToList();
            HomeViewModels viewModel = new HomeViewModels { YourGames = gameList };
            return viewModel;
        }
    }
}