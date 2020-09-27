using NewWorld.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NewWorld.Repositories
{
    public class UserGamePropertyRepository
    {
        private readonly ApplicationDbContext db;
        public UserGamePropertyRepository()
        {
            db = Context.DbContext;
        }

        public UserGameProperty GetUserGameProperty(ApplicationUser user, Game game)
        {
            return db.UserGameProperties.Where(a => a.Game.Id == game.Id).Where(b => b.Player.Id == user.Id).FirstOrDefault();
        }

        public UserGameProperty CreateUserGameProperty(ApplicationUser user, int color)
        {
            return new UserGameProperty
            {
                Coins = 50000,
                Player = user,
                Color = (Color)color,
                Active = true
            };
        }

        public void RemoveUserGameProperty(ApplicationUser user, Game game)
        {
            UserGameProperty userGameProperty = db.UserGameProperties.Where(a => a.Player.Id == user.Id).Where(b => b.Game.Id == game.Id).FirstOrDefault();
            db.UserGameProperties.Remove(userGameProperty);
        }

        public List<UserGameProperty> GetAllUserGameProperties(Game game)
        {
            return db.UserGameProperties.Where(a => a.Game.Id == game.Id).ToList();
        }

        public void Deactive(Game game, ApplicationUser user)
        {
            UserGameProperty userGameProperty = GetUserGameProperty(user, game);
            userGameProperty.Active = false;
            db.SaveChanges();
        }

    }
}