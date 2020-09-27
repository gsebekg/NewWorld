using Antlr.Runtime;
using Microsoft.AspNet.Identity;
using NewWorld.Models;
using NewWorld.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewWorld.Controllers
{
    public class GameController : Controller
    {
        readonly GameRepository gameRepository;
        readonly UserRepository userRepository;
        readonly IslandRepository islandRepository;
        readonly UserGamePropertyRepository userGamePropertyRepository;
        public GameController()
        {
            gameRepository = Context.gameRepository;
            userRepository = Context.userRepository;
            islandRepository = Context.islandRepository;
            userGamePropertyRepository = Context.userGamePropertyRepository;
        }

        //wyśiwetlanie mapy gry
        [Authorize]
        public ActionResult Map(int id)
        {
            ApplicationUser user = userRepository.GetUser(User.Identity.GetUserId());
            Game game = gameRepository.GetGame(id);
            if ((!game.IsBegan || game == null))
                return RedirectToAction("GameList", "Home");
            UserGameProperty userGameProperty = userGamePropertyRepository.GetUserGameProperty(user, game);
            if (!userGameProperty.Active || userGameProperty == null)
                return RedirectToAction("GameList", "Home");
            CyclicProduct.CalculateGame(id);
            MapViewModel viewModel = new MapViewModel
            {
                Game = game,
                Property = userGameProperty,
                Islands = islandRepository.GetIslandsToMap(game),
                AllPlayers = userGamePropertyRepository.GetAllUserGameProperties(game)
            };
            return View(viewModel);
        }

        //pobieranie informacji o wyspie
        [Authorize]
        public ActionResult Island(int id)
        {
            ApplicationUser user = userRepository.GetUser(User.Identity.GetUserId());
            Island island = islandRepository.GetIsland(id);
            if (!island.Game.PlayerIsActive(user))
                return RedirectToAction("GameList", "Home");
            CyclicProduct.CalculateGame(island.Game.Id);
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
            viewModel.Coins = userGamePropertyRepository.GetUserGameProperty(user,island.Game).Coins;
            viewModel.Buildings = island.Buildings;
            BuildingList buildings = new BuildingList(island);
            if (viewModel.YourIsland)
            {
                viewModel.CalculateMaxBuildings(island, buildings.buildings);
                viewModel.ResourceImages = Resources.ResourceImage();
                viewModel.ResourcesList = viewModel.Resources.BuildList();
            }
            return View(viewModel);
        }

        //Budowanie budynków - każdy budynek ma inny Name przypisany w widoku
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Build([Bind(Include = "Id,Number,Name")] BuildDestroyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = userRepository.GetUser(User.Identity.GetUserId());
                Island island = islandRepository.GetIsland(viewModel.Id);
                if (island != null && island.Property.Player.Id == user.Id)
                {
                    CyclicProduct.CalculateGame(island.Game.Id);  // ponowne odświerzanie danych by sprawdzić czy dalej stać użytkownika
                    island.Build(viewModel.Name, viewModel.Number);
                    gameRepository.Save();
                }
            }
            return RedirectToAction("Island", new { id = viewModel.Id });
        }

        //wyburzanie budynków -zwrot połowy surowców
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Destroy([Bind(Include = "Id,Number,Name")] BuildDestroyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = userRepository.GetUser(User.Identity.GetUserId());
                Island island = islandRepository.GetIsland(viewModel.Id);
                if (island != null && island.Property.Player.Id == user.Id)
                {
                    CyclicProduct.CalculateGame(island.Game.Id);
                    island.Destroy(viewModel.Name, viewModel.Number);
                    gameRepository.Save();
                }
            }
            return RedirectToAction("Island", new { id = viewModel.Id });
        }

        //zmiana nazwy wyspy
        [Authorize]
        public ActionResult ChangeName(int id)
        {
            Island island = islandRepository.GetIsland(id);
            if (island.Property.Player.Id != User.Identity.GetUserId())
                return RedirectToAction("Map");
            ChangeNameViewModel viewModel = new ChangeNameViewModel { NameUsed = false, Id = id };
            return View(viewModel);
        }

        //zmiana nazwy wyspy
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult ChangeName([Bind(Include = "Id,NewName")] ChangeNameViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.NewName = viewModel.NewName.Trim();
                ApplicationUser user = userRepository.GetUser(User.Identity.GetUserId());
                Island island = islandRepository.GetIsland(viewModel.Id);
                if (island != null && island.Property.Player.Id == user.Id)
                {
                    if (island.ChangeName(viewModel.NewName))
                    {
                        viewModel.NameUsed = true;
                        return View(viewModel);
                    }
                    gameRepository.Save();
                    return RedirectToAction("Island", new { id = viewModel.Id });
                }
            }
            return View(viewModel);
        }
    }
}