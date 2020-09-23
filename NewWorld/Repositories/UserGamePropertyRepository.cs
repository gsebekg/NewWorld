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
        private ApplicationDbContext db;
        public UserGamePropertyRepository()
        {
            db = Context.DbContext;
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
    }
}