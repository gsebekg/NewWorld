﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NewWorld.Repositories;

namespace NewWorld.Models
{
    // Możesz dodać dane profilu dla użytkownika, dodając więcej właściwości do klasy ApplicationUser. Odwiedź stronę https://go.microsoft.com/fwlink/?LinkID=317594, aby dowiedzieć się więcej.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Element authenticationType musi pasować do elementu zdefiniowanego w elemencie CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Dodaj tutaj niestandardowe oświadczenia użytkownika
            return userIdentity;
        }
        public string Test { get; set; }
        public virtual ICollection<Game> Games { get; set; }
        public virtual ICollection<UserGameProperty> UserGameProperties { get; set; }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<Game> Games { get; set; }
        public DbSet<UserGameProperty> UserGameProperties { get; set; }
        public DbSet<Island> Islands { get; set; }
        public DbSet<Resources> Resources { get; set; }
        public DbSet<Buildings> Buildings { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public static class Context
    {
        public static ApplicationDbContext DbContext;
        public static GameRepository gameRepository;
        public static UserRepository userRepository;
        public static UserGamePropertyRepository userGamePropertyRepository;
        public static IslandRepository islandRepository;
    }
}