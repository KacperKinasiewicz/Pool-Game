using Data;
using Logic;
using Model;
using Xunit;

namespace Test
{
    public class GameLogicTests
    {
        [Fact]
        public void BallPosition_ShouldUpdateAfterMovement()
        {
            // Tworzymy instancj� repozytorium danych
            var dataRepository = new DataRepository();

            // Tworzymy instancj� GameLogic, przekazuj�c repozytorium
            var gameLogic = new GameLogic(dataRepository);

            // Tworzymy instancj� GameModel, przekazuj�c GameLogic
            var gameModel = new GameModel(gameLogic);
            gameModel.Update(1.0); // Aktualizacja pi�ek po 1 sekundzie

            var ball = gameModel.Balls[0];
            Assert.Equal(110, ball.X); // Oczekujemy, �e pi�ka przesunie si� o VelocityX * deltaTime
            Assert.Equal(110, ball.Y); // Oczekujemy, �e pi�ka przesunie si� o VelocityY * deltaTime
        }
    }
}