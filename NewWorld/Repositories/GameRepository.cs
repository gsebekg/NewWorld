using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using NewWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewWorld.Repositories
{
    public class GameRepository
    {
        private ApplicationDbContext db;
        private UserGamePropertyRepository userGamePropertyRepository;
        private IslandRepository islandRepository;
        public GameRepository()
        {
            db = Context.DbContext;
            userGamePropertyRepository = Context.userGamePropertyRepository;
            islandRepository = Context.islandRepository;
        }

        public Game GetGame(int id)
        {
            return db.Games.Find(id);
        }

        public bool NameUsed(Game game)
        {
            game.Name = game.Name.Trim();
            return db.Games.Where(a => a.Name.Equals(game.Name)).ToList().Count() > 0;
        }

        public void CreateGame(Game game, ApplicationUser user)
        {
            UserGamePropertyRepository userGamePropertyRepository = new UserGamePropertyRepository();
            game.CreateDate = DateTime.Now;
            game.Update = DateTime.Now;
            game.IsBegan = false;
            game.Players = new List<ApplicationUser> { user };
            game.UserGameProperties = new List<UserGameProperty> { userGamePropertyRepository.CreateUserGameProperty(user,0) };
            db.Games.Add(game);
            db.SaveChanges();
        }

        public List<Game> GetUserGames(ApplicationUser user)
        {
            // pobieranie gier do których należy gracz
            var gameList = db.Games.Where(a => a.Players.Select(c => c.Id).Contains(user.Id)).ToList();
            //wybieranie tych w których jest aktywny
            return gameList.Where(a => a.UserGameProperties.Where(b => b.Player.Id == user.Id).Where(c => c.Active).ToList().Count() == 1).ToList();
        }

        public List<Game> GetOpenGames(List<Game> userGames)
        {
            var gameListIds = userGames.Select(a => a.Id).ToList();
            return db.Games.Where(a => !a.IsBegan).Where(b => !gameListIds.Contains(b.Id)).ToList();
        }

        public void RemovePlayer(ApplicationUser user,Game game)
        {
            game.Players.Remove(user);
            userGamePropertyRepository.RemoveUserGameProperty(user, game);
            //jeżeli nie ma więcej graczy to usuń grę
            if (game.Players.Count == 0)
            {
                islandRepository.RemoveAllIslands(game);
                db.Games.Remove(game);
            }
            db.SaveChanges();
        }
        //dodanie gracza do gry, jeżeli gra jest pełna to wystartuj
        public void AddUserToGame(ApplicationUser user, Game game)
        {
            game.Players.Add(user);
            UserGameProperty userGameProperty =  userGamePropertyRepository.CreateUserGameProperty(user, game.NumberOfPlayers() - 1);
            game.UserGameProperties.Add(userGameProperty);
            if (game.NumberOfPlayers() == game.MaxPlayers)
            {
                List<UserGameProperty> properties = userGamePropertyRepository.GetAllUserGameProperties(game);
                List<Island> islands = game.InitializeGame(properties);
                islandRepository.AddIslands(islands);
                
            }
            db.SaveChanges();
        }
    }
}