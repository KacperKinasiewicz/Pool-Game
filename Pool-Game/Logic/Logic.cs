using System;
using Model;
using System.Collections.Generic;

namespace Logic
{
    public interface IGameLogic
    {
        void Update(List<Ball> balls, double deltaTime);
    }

    public class GameLogic : IGameLogic
    {
        public void Update(List<Ball> balls, double deltaTime)
        {
            foreach (var ball in balls)
            {
                ball.X += ball.VelocityX * deltaTime;
                ball.Y += ball.VelocityY * deltaTime;
            }
        }
    }
}
