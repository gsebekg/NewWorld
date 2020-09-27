using NewWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewWorld.Repositories
{
    public class UserRepository
    {
        private readonly ApplicationDbContext db;
        public UserRepository()
        {
            db = Context.DbContext;
        }

        public ApplicationUser GetUser(string id)
        {
            return db.Users.Find(id);
        }

        public bool HaveMaxGames(ApplicationUser user)
        {
            return user.UserGameProperties.Where(a => a.Active).Count() >= 3;
        }
    }
}