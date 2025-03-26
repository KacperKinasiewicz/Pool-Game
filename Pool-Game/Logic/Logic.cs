using Data;
using System.Collections.Generic;

namespace Logic
{
    public interface IGameLogic
    {
        void UpdateBallPositions(List<Ball> balls, double deltaTime);
    }
}

// Logic/GameLogic.cs
namespace Logic
{
    public class GameLogic : IGameLogic
    {
        private readonly IDataRepository _dataRepository;

        // Konstruktor, który wstrzykuje repozytorium danych
        public GameLogic(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public void UpdateBallPositions(List<Ball> balls, double deltaTime)
        {
            foreach (var ball in balls)
            {
                ball.X += ball.VelocityX * deltaTime;
                ball.Y += ball.VelocityY * deltaTime;
            }
        }
    }
}