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
            // Tworzymy instancjê repozytorium danych
            var dataRepository = new DataRepository();

            // Tworzymy instancjê GameLogic, przekazuj¹c repozytorium
            var gameLogic = new GameLogic(dataRepository);

            // Tworzymy instancjê GameModel, przekazuj¹c GameLogic
            var gameModel = new GameModel(gameLogic);
            gameModel.Update(1.0); // Aktualizacja pi³ek po 1 sekundzie

            var ball = gameModel.Balls[0];
            Assert.Equal(110, ball.X); // Oczekujemy, ¿e pi³ka przesunie siê o VelocityX * deltaTime
            Assert.Equal(110, ball.Y); // Oczekujemy, ¿e pi³ka przesunie siê o VelocityY * deltaTime
        }
    }
}