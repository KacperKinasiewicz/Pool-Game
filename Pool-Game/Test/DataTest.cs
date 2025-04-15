using Data;

namespace Test
{
    public class DataTest
    {
        [Fact]
        public void ConstructorTest()
        {
            int id = 1;
            double x = 10.0;
            double y = 10.0;
            double velocityX = 10.0;
            double velocityY = 10.0;
            double radius = 10.0;
            
            var ball = new Ball(id, x, y, velocityX, velocityY, radius);
            
            Assert.Equal(id, ball.Id);
            Assert.Equal(x, ball.X);
            Assert.Equal(y, ball.Y);
            Assert.Equal(velocityX, ball.VelocityX);
            Assert.Equal(velocityY, ball.VelocityY);
            Assert.Equal(radius, ball.Radius);
        }
        
        [Fact]
        public void SetterTest()
        {
            int id1 = 1;
            double x1 = 10.0;
            double y1 = 10.0;
            double velocityX1 = 10.0;
            double velocityY1 = 10.0;
            double radius1 = 10.0;
            
            var ball = new Ball(id1, x1, y1, velocityX1, velocityY1, radius1);
            
            double x2 = 20.0;
            double y2 = 20.0;
            double velocityX2 = 20.0;
            double velocityY2 = 20.0;
            
            ball.X = x2;
            ball.Y = y2;
            ball.VelocityX = velocityX2;
            ball.VelocityY = velocityY2;
            
            Assert.Equal(x2, ball.X);
            Assert.Equal(y2, ball.Y);
            Assert.Equal(velocityX2, ball.VelocityX);
            Assert.Equal(velocityY2, ball.VelocityY);
        }
    }
}