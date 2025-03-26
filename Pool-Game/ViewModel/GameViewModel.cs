using Model;
using System.Collections.Generic;

namespace ViewModel
{
    public class GameViewModel
    {
        private readonly GameModel _gameModel;

        public GameViewModel(GameModel gameModel)
        {
            _gameModel = gameModel;
        }

        public List<string> GetBallPositions()
        {
            var positions = new List<string>();

            foreach (var ball in _gameModel.Balls)
            {
                positions.Add($"Ball {ball.Id}: X={ball.X}, Y={ball.Y}");
            }

            return positions;
        }
    }
}