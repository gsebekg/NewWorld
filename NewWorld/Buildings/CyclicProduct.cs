using NewWorld.Models;
using NewWorld.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewWorld
{
    public static class CyclicProduct
    {

        public static void CalculateGame(int id)
        {
            GameRepository gameRepository = Context.gameRepository;
            UserGamePropertyRepository userGamePropertyRepository = Context.userGamePropertyRepository;
            Game game = gameRepository.GetGame(id);
            List<UserGameProperty> properties = userGamePropertyRepository.GetAllUserGameProperties(game);
            //obliczanie ile minut upłyneło od ostatniego update
            int numberOfCycles = game.CalculateNumberOfCycles();
            for (int i = 0; i < numberOfCycles; i++)
            {
                foreach (UserGameProperty property in properties)
                {
                    property.CalculateGameForPlayer();
                }
            }
            gameRepository.Save();

        }
    }
}