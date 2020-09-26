using NewWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewWorld.Repositories
{
    public class IslandRepository
    {
        private ApplicationDbContext db;
        private UserGamePropertyRepository userGamePropertyRepository;
        public IslandRepository()
        {
            db = Context.DbContext;
            userGamePropertyRepository = Context.userGamePropertyRepository;
        }

        public void RemoveAllIslands(Game game)
        {
            List<Island> islands = db.Islands.Where(a => a.Game.Id == game.Id).ToList();
            db.Islands.RemoveRange(islands).ToList();
        }

        public List<Island> GetIslands(Game game, ApplicationUser user)
        {
            UserGameProperty userGameProperty = userGamePropertyRepository.GetUserGameProperty(user, game);
            return db.Islands.Where(a => a.Property.Id == userGameProperty.Id).ToList();
        }

        public void AddIslands(List<Island> islands)
        {
            db.Islands.AddRange(islands);
        }
    }
}