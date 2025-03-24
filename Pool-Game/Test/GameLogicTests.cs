using System.Collections.Generic;
using Data;
using Logic;
using Xunit;

namespace Test {
    public class GameLogicTests {
        [Fact]
        public void BallMovement_ShouldUpdatePosition() {
            var dataRepository = new DataRepository();
            var balls = dataRepository.GetBalls();
            var logic = new GameLogic();

            logic.Update(balls, 1);

            Assert.Equal(110, balls[0].X);
            Assert.Equal(110, balls[0].Y);
        }
    }
}