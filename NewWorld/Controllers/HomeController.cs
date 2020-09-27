using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using NewWorld.Models;
using NewWorld.Repositories;
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
        readonly GameRepository gameRepository;
        readonly UserRepository userRepository;
        readonly IslandRepository islandRepository;
        readonly UserGamePropertyRepository userGamePropertyRepository;
        public HomeController()
        {
            gameRepository = Context.gameRepository;
            userRepository = Context.userRepository;
            islandRepository = Context.islandRepository;
            userGamePropertyRepository = Context.userGamePropertyRepository;
        }

        //strona główna
        public ActionResult Index()
        {
            return View();
        }

        //Lista gier, możliwość stworzenia, dołączenia itp
        [Authorize]
        public ActionResult GameList(bool? id)  //jeżeli id==true to masz juz maksymalna liczbę gier
        {
            HomeViewModels viewModel = new HomeViewModels();
            GetHomeViewModel(viewModel);
            viewModel.HaveMaxGames = id.GetValueOrDefault();
            return View(viewModel);
        }

        //dodanie nowej gry
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GameList([Bind(Prefix = "NewGame")] Game game)
        {
            HomeViewModels viewModel = new HomeViewModels();
            ApplicationUser user = userRepository.GetUser(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                //sprawdzanie czy gracz nie ma juz zaczętych maksymalnej liczby gier
                if (userRepository.HaveMaxGames(user))
                    viewModel.HaveMaxGames = true;
                else
                {
                    //sprawdzanie czy dana nazwa juz nie istnieje
                    viewModel.NameUsed = gameRepository.NameUsed(game);
                    if (!viewModel.NameUsed.GetValueOrDefault())
                        gameRepository.CreateGame(game, user);
                }
            }
            GetHomeViewModel(viewModel);
            return View(viewModel);
        }

        //wycofanie się z gry
        [Authorize]
        public ActionResult Delete(int id)
        {
            ApplicationUser user = userRepository.GetUser(User.Identity.GetUserId());
            Game game = gameRepository.GetGame(id);
            if (game.IsBegan)
                return RedirectToAction("GameList");
            gameRepository.RemovePlayer(user, game);
            return RedirectToAction("GameList");
        }
        
        // dołącz do istniejącej gry
        [Authorize]
        public ActionResult Join(int id)
        {
            ApplicationUser user = userRepository.GetUser(User.Identity.GetUserId());
            Game game = gameRepository.GetGame(id);
            //sprawdzanie czy gracz nie ma juz zaczętych maksymalnej liczby gier
            if (userRepository.HaveMaxGames(user))
                return RedirectToAction("GameList", new { id = true });
            //jeżeli gra ma hasło wyswietl strone do wprowadzania hasła
            if (game.HavePassword())
            {
                JoinViewModel viewModel = new JoinViewModel { Id = id, Name = game.Name };
                return View(viewModel);
            }
            if (game.Players.Contains(user) || game.IsBegan)
                return RedirectToAction("GameList");
            gameRepository.AddUserToGame(user, game);
            return RedirectToAction("GameList");
        }

        //dołączenie po sprawdzeniu hasła
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Join([Bind(Include = "Id,Password")] JoinViewModel viewModel)
        {
            ApplicationUser user = userRepository.GetUser(User.Identity.GetUserId());
            Game game = gameRepository.GetGame(viewModel.Id);
            //sprawdzanie czy gracz nie ma juz zaczętych maksymalnej liczby gier
            if (userRepository.HaveMaxGames(user))
                return RedirectToAction("GameList", new { id = true });
            if (ModelState.IsValid)
            {
                if (game.Password != viewModel.Password)
                    return View(new JoinViewModel { Id = viewModel.Id, Name = game.Name, WrongPassword = true });
                if (game.Players.Contains(user) || game.IsBegan)
                    return RedirectToAction("GameList");
                gameRepository.AddUserToGame(user, game);
                return RedirectToAction("GameList");
            }
            return View(new JoinViewModel { Id = viewModel.Id, Name = game.Name, WrongPassword = true });
        }

        //poproś o potwierdzxenie decyzji
        [Authorize]
        public ActionResult GiveUp(int id)
        {
            Game game = gameRepository.GetGame(id);
            return View(game);
        }

        //po potwierdzeniu zwalniamy wyspy i wycofujemy gracza z gry
        [Authorize]
        public ActionResult GiveUpConfirmed(int id)
        {
            ApplicationUser user = userRepository.GetUser(User.Identity.GetUserId());
            Game game = gameRepository.GetGame(id);
            List<Island> islands = islandRepository.GetAllIslands(game, user);
            foreach (Island island in islands)
                island.LeaveIsland();
            userGamePropertyRepository.Deactive(game, user);
            
            return RedirectToAction("GameList");
        }

        //helpers
        private void GetHomeViewModel(HomeViewModels viewModel)
        {
            ApplicationUser user = userRepository.GetUser(User.Identity.GetUserId());
            viewModel.YourGames = gameRepository.GetUserGames(user);
            viewModel.OpenGames = gameRepository.GetOpenGames(viewModel.YourGames);
        }
    }
}