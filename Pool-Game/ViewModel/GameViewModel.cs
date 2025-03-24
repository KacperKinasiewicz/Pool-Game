using System;
using Data;
using Logic;
using Model;
using System.Collections.Generic;

namespace ViewModel
{
    public class GameViewModel
    {
        public List<Ball> Balls { get; set; }
        public Table Table { get; set; }
        private readonly IGameLogic _logic;

        public GameViewModel(IGameLogic logic, IDataRepository data)
        {
            _logic = logic;
            Balls = data.GetBalls();
            Table = data.GetTable();
        }

        public void Update(double deltaTime) => _logic.Update(Balls, deltaTime);
    }
}
