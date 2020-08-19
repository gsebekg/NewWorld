using Antlr.Runtime;
using Microsoft.AspNet.Identity;
using NewWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewWorld.Controllers
{
    public class GameController : Controller
    {
        private ApplicationDbContext db;

        public GameController()
        {
            db = new ApplicationDbContext();
        }

        [Authorize]
        public ActionResult Map(int id)
        {
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            Game game = db.Games.Find(id);
            if ((!game.IsBegan || game==null))
                return RedirectToAction("GameList", "Home");
            UserGameProperty userGameProperty = db.UserGameProperties.Where(a => a.Game.Id == game.Id).Where(b => b.Player.Id == user.Id).SingleOrDefault();
            if(!userGameProperty.Active || userGameProperty==null)
                return RedirectToAction("GameList", "Home");
            MapViewModel viewModel = new MapViewModel { Game = game, Property = userGameProperty, Islands = db.Islands.Where(a => a.Property.Game.Id == game.Id).ToList() };
            return View(viewModel);
        }
    }
}