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
            double y = 15.0;
            double radius = 5.0;
            double mass = 2.0;
            double velocityX = 1.0;
            double velocityY = -1.0;
            
            var ball = new Ball(id, x, y, radius, mass, velocityX, velocityY);
            
            Assert.Equal(id, ball.Id);
            Assert.Equal(x, ball.X);
            Assert.Equal(y, ball.Y);
            Assert.Equal(radius, ball.Radius);
            Assert.Equal(mass, ball.Mass);
            Assert.Equal(velocityX, ball.VelocityX);
            Assert.Equal(velocityY, ball.VelocityY);
        }

        [Fact]
        public void Constructor_InvalidRadius_ThrowsException_Test()
        {
             Assert.Throws<ArgumentOutOfRangeException>("radius", () => 
                new Ball(1, 10.0, 10.0, 0, 2.0, 1.0, 1.0)
            );
        }

        [Fact]
        public void Constructor_InvalidMass_ThrowsException_Test()
        {
            Assert.Throws<ArgumentOutOfRangeException>("mass", () => 
                new Ball(1, 10.0, 10.0, 5.0, -1.0, 1.0, 1.0)
            );
        }
        
        [Fact]
        public void SetterTest()
        {
            var ball = new Ball(1, 10.0, 10.0, 5.0, 2.0, 1.0, 1.0);
            
            double newX = 20.0;
            double newY = 25.0;
            double newVelocityX = 15.0;
            double newVelocityY = -5.0;
            
            ball.X = newX;
            ball.Y = newY;
            ball.VelocityX = newVelocityX;
            ball.VelocityY = newVelocityY;
            
            Assert.Equal(newX, ball.X);
            Assert.Equal(newY, ball.Y);
            Assert.Equal(newVelocityX, ball.VelocityX);
            Assert.Equal(newVelocityY, ball.VelocityY);
        }

        [Fact]
        public void PropertyChanged_Event_FiresOnXChange_Test()
        {
            var ball = new Ball(1, 10.0, 10.0, 5.0, 2.0, 0.0, 0.0);
            string changedProperty = null;
            ball.PropertyChanged += (sender, args) => 
            {
                changedProperty = args.PropertyName;
            };

            ball.X = 100.0;

            Assert.Equal(nameof(Ball.X), changedProperty);
        }


        [Fact]
        public void Move_UpdatesPosition_Test()
        {
            double initialX = 10.0;
            double initialY = 20.0;
            double velocityX = 2.0;
            double velocityY = -3.0;
            double timeStep = 0.5;
            var ball = new Ball(1, initialX, initialY, 5.0, 2.0, velocityX, velocityY);

            ball.Move(timeStep);

            Assert.Equal(initialX + velocityX * timeStep, ball.X);
            Assert.Equal(initialY + velocityY * timeStep, ball.Y);
        }

        [Fact]
        public void Move_NoChangeIfVelocityZero_Test()
        {
            double initialX = 10.0;
            double initialY = 20.0;
            var ball = new Ball(1, initialX, initialY, 5.0, 2.0, 0.0, 0.0);

            ball.Move(1.0);

            Assert.Equal(initialX, ball.X);
            Assert.Equal(initialY, ball.Y);
        }
    }
}