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
            if ((!game.IsBegan || game == null))
                return RedirectToAction("GameList", "Home");
            UserGameProperty userGameProperty = db.UserGameProperties.Where(a => a.Game.Id == game.Id).Where(b => b.Player.Id == user.Id).SingleOrDefault();
            if (!userGameProperty.Active || userGameProperty == null)
                return RedirectToAction("GameList", "Home");
            //USUNĄĆ!!!
            CyclicProduct cyclic = new CyclicProduct();
            cyclic.CalculateGame(id);
            MapViewModel viewModel = new MapViewModel
            {
                Game = game,
                Property = userGameProperty,
                Islands = db.Islands.Where(a => a.Game.Id == game.Id).OrderBy(b => b.Y).ThenBy(c => c.X).ToList(),
                AllPlayers = db.UserGameProperties.Where(a => a.Game.Id == game.Id).ToList()
            };
            return View(viewModel);
        }

        [Authorize]
        public ActionResult Island(int id)
        {
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            Island island = db.Islands.Find(id);
            var ids = island.Game.Players.Select(a => a.Id).ToList();
            //sprawdzanie czy gracz należy do tej gry i czy się nie wycofał
            if (!ids.Contains(user.Id) || island.Game.UserGameProperties.Where(a => a.Player.Id == user.Id).SingleOrDefault().Active == false)
                return RedirectToAction("GameList", "Home");
            IslandViewModel viewModel = new IslandViewModel();
            viewModel.EmptyIsland = island.Property == null;
            if (!viewModel.EmptyIsland)
            {
                viewModel.YourIsland = island.Property.Player.Id == user.Id;
                viewModel.Property = island.Property;
            }
            viewModel.Island = island;
            if (viewModel.YourIsland)
                viewModel.Resources = island.Resources;
            viewModel.Coins = db.UserGameProperties.Where(a => a.Player.Id == user.Id).Where(b => b.Game.Id == island.Game.Id).SingleOrDefault().Coins;
            return View(viewModel);
        }
    }
}