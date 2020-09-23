using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using NewWorld.Models;
using NewWorld.Repositories;
using Owin;

[assembly: OwinStartupAttribute(typeof(NewWorld.Startup))]
namespace NewWorld
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            Context.DbContext = new ApplicationDbContext();
            Context.userRepository = new UserRepository();
            Context.gameRopository = new GameRopository();
        }
    }
}
