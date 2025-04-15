using Logic;
using Data;

namespace Test
{
    public class LogicTest
    {
        [Fact]
        public void ConstructorTest()
        {
            double width = 100.0;
            double height = 100.0;
            
            var table = new Table(width, height);
            
            Assert.Equal(width, table.Width);
            Assert.Equal(height, table.Height);
        }
        
        [Fact]
        public void UpdateBallTest()
        {
            double width = 100.0;
            double height = 100.0;
            
            int id = 1;
            double x = 10.0;
            double y = 10.0;
            double velocityX = 10.0;
            double velocityY = 10.0;
            double radius = 10.0;
            
            var ball = new Ball(id, x, y, velocityX, velocityY, radius);
            var table = new Table(width, height);
            
            Assert.Empty(table.Balls);
            table.AddBall(ball);
            Assert.Single(table.Balls);
            table.UpdateBalls();
            Assert.Equal(20.0, ball.X);
            table.RemoveBall(ball);
            Assert.Empty(table.Balls);
        }
    }
}