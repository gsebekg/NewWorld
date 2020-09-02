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
            CyclicProduct.CalculateGame(id,db);
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
            CyclicProduct.CalculateGame(island.Game.Id,db);
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
            viewModel.Buildings = island.Buildings;
            BuildingList buildings = new BuildingList(island);
            int sum = 0;
            if (viewModel.YourIsland)
            {
                viewModel.MaxBuildings = new List<int>();
                foreach (Building building in buildings.buildings)
                    sum += building.Number;
                foreach (Building building in buildings.buildings)
                    viewModel.MaxBuildings.Add(building.HowManyCanYouBuild(island.Resources, viewModel.Coins, island.Place-sum));
                viewModel.ResourceImages = Resources.ResourceImage();
                viewModel.ResourcesList = viewModel.Resources.BuildList();
            }
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Build([Bind(Include = "Id,Number,Name")] BuildDestroyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                Island island = db.Islands.Find(viewModel.Id);
                if (island != null && island.Property.Player.Id == user.Id)
                {
                    CyclicProduct.CalculateGame(island.Game.Id,db);
                    BuildingList buildings = new BuildingList(island);
                    Building building = buildings.buildings[viewModel.Name];
                    long coins = island.Property.Coins;
                    int sum = 0;
                    foreach (Building building1 in buildings.buildings)
                        sum += building1.Number;
                    bool IsProductionBuilding = building.Build(viewModel.Number, island.Resources, ref coins, island.Place - sum) && building is ProductionBuilding;
                    buildings.ListToBuildings(island);
                    if (IsProductionBuilding)
                        island.Buildings.NeededFarmers+= (building as ProductionBuilding).NeededFarmers * viewModel.Number;
                    island.Property.Coins = coins;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Island", new { id = viewModel.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Destroy([Bind(Include = "Id,Number,Name")] BuildDestroyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                Island island = db.Islands.Find(viewModel.Id);
                if (island != null && island.Property.Player.Id == user.Id)
                {
                    CyclicProduct.CalculateGame(island.Game.Id,db);
                    BuildingList buildings = new BuildingList(island);
                    Building building = buildings.buildings[viewModel.Name];
                    long coins = island.Property.Coins;
                    building.Destroy(viewModel.Number, island.Resources, ref coins);
                    buildings.ListToBuildings(island);
                    island.Property.Coins = coins;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Island", new { id = viewModel.Id });
        }
        [Authorize]
        public ActionResult ChangeName(int id)
        {
            Island island = db.Islands.Find(id);
            if (island.Property.Player.Id != User.Identity.GetUserId())
                return RedirectToAction("Map");
            ChangeNameViewModel viewModel = new ChangeNameViewModel { NameUsed = false, Id = id };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult ChangeName([Bind(Include = "Id,NewName")] ChangeNameViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.NewName = viewModel.NewName.Trim();
                ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                Island island = db.Islands.Find(viewModel.Id);
                if (island != null && island.Property.Player.Id == user.Id)
                {
                    Game game = island.Game;
                    if (db.Islands.Where(a => a.Game.Id == game.Id).Select(b => b.Name).Contains(viewModel.NewName))
                    {
                        viewModel.NameUsed = true;
                        return View(viewModel);
                    }
                    island.Name = viewModel.NewName;
                    db.SaveChanges();
                    return RedirectToAction("Island", new { id = viewModel.Id });
                }
            }
            return View(viewModel);
        }
    }
}