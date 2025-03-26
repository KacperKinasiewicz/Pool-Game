using Logic;
using System.Collections.Generic;

namespace Model
{
    public class GameModel
    {
        private readonly IGameLogic _gameLogic;

        public List<Data.Ball> Balls { get; set; }

        public GameModel(IGameLogic gameLogic)
        {
            _gameLogic = gameLogic;
            Balls = new List<Data.Ball>
            {
                new Data.Ball { Id = 1, X = 100, Y = 100, VelocityX = 10, VelocityY = 10 }
            };
        }

        public void Update(double deltaTime)
        {
            _gameLogic.UpdateBallPositions(Balls, deltaTime);
        }
    }
}