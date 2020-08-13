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
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            var gameList = db.Games.Where(a => a.Players.Select(c => c.Id).Contains(user.Id)).ToList();

            HomeViewModels viewModels = new HomeViewModels { YourGames = gameList };


            return View(viewModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GameList([Bind(Prefix = "NewGame")] Game game)
        {
            if (ModelState.IsValid)
            {
                game.CreateDate = DateTime.Now;
                game.IsBegan = false;
                ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                game.Players = new List<ApplicationUser>{ user  };
                db.Games.Add(game);
                db.SaveChanges();
            }
            return RedirectToAction("GameList");

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
    }
}