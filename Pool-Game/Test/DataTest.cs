using Data;
using System;
using System.ComponentModel;
using Xunit;

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
            double mass = 3.0;
            double velocityX = 1.0;
            double velocityY = -1.0;
            var logger = new FileLogger();

            var ball = new Ball(id, x, y, radius, mass, velocityX, velocityY, logger);

            Assert.Equal(id, ball.Id);
            Assert.Equal(x, ball.X);
            Assert.Equal(y, ball.Y);
            Assert.Equal(radius, ball.Radius);
            Assert.Equal(mass, ball.Mass);
            Assert.Equal(velocityX, ball.VelocityX);
            Assert.Equal(velocityY, ball.VelocityY);
            Assert.NotNull(ball.ToString());
        }

        [Fact]
        public void Constructor_InvalidRadius_ThrowsArgumentOutOfRangeException_Test()
        {
            var logger = new FileLogger();
            Assert.Throws<ArgumentOutOfRangeException>("radius", () =>
               new Ball(1, 10.0, 10.0, 0, 2.0, 1.0, 1.0, logger)
           );
            Assert.Throws<ArgumentOutOfRangeException>("radius", () =>
               new Ball(1, 10.0, 10.0, -1.0, 2.0, 1.0, 1.0, logger)
           );
        }

        [Fact]
        public void Constructor_InvalidMass_ThrowsArgumentOutOfRangeException_Test()
        {
            var logger = new FileLogger();
            Assert.Throws<ArgumentOutOfRangeException>("mass", () =>
               new Ball(1, 10.0, 10.0, 5.0, 0, 1.0, 1.0, logger)
           );
            Assert.Throws<ArgumentOutOfRangeException>("mass", () =>
               new Ball(1, 10.0, 10.0, 5.0, -1.0, 1.0, 1.0, logger)
           );
        }

        [Fact]
        public void Constructor_NullLogger_ThrowsArgumentNullException_Test()
        {
            Assert.Throws<ArgumentNullException>("logger", () =>
               new Ball(1, 10.0, 10.0, 5.0, 2.0, 1.0, 1.0, null)
           );
        }

        [Fact]
        public void VelocitySetters_UpdateValuesAndRaisePropertyChanged_Test()
        {
            var logger = new FileLogger();
            var ball = new Ball(1, 10.0, 10.0, 5.0, 2.0, 1.0, 1.0, logger);

            double newVelocityX = 15.0;
            double newVelocityY = -5.0;

            string changedProperty = null;
            ball.PropertyChanged += (sender, args) =>
            {
                changedProperty = args.PropertyName;
            };

            ball.VelocityX = newVelocityX;
            Assert.Equal(newVelocityX, ball.VelocityX);
            Assert.Equal(nameof(Ball.VelocityX), changedProperty);

            changedProperty = null;
            ball.VelocityY = newVelocityY;
            Assert.Equal(newVelocityY, ball.VelocityY);
            Assert.Equal(nameof(Ball.VelocityY), changedProperty);
        }

        [Fact]
        public void VelocitySetters_NoPropertyChangedIfValueIsTheSame_Test()
        {
            var logger = new FileLogger();
            var ball = new Ball(1, 10.0, 10.0, 5.0, 2.0, 1.0, 1.0, logger);
            bool propertyChangedFired = false;

            ball.PropertyChanged += (sender, args) =>
            {
                propertyChangedFired = true;
            };

            ball.VelocityX = ball.VelocityX;
            Assert.False(propertyChangedFired, "PropertyChanged nie powinno być wywołane dla VelocityX, gdy wartość się nie zmienia.");

            propertyChangedFired = false;
            ball.VelocityY = ball.VelocityY;
            Assert.False(propertyChangedFired, "PropertyChanged nie powinno być wywołane dla VelocityY, gdy wartość się nie zmienia.");
        }


        [Fact]
        public void Move_UpdatesPositionCorrectly_Test()
        {
            double initialX = 10.0;
            double initialY = 20.0;
            double velocityX = 2.0;
            double velocityY = -3.0;
            double timeStep = 0.5;
            var logger = new FileLogger();
            var ball = new Ball(1, initialX, initialY, 5.0, 2.0, velocityX, velocityY, logger);

            ball.Move(timeStep);

            Assert.Equal(initialX + velocityX * timeStep, ball.X);
            Assert.Equal(initialY + velocityY * timeStep, ball.Y);
        }

        [Fact]
        public void Move_NoChangeInPositionIfVelocityZero_Test()
        {
            double initialX = 10.0;
            double initialY = 20.0;
            var logger = new FileLogger();
            var ball = new Ball(1, initialX, initialY, 5.0, 2.0, 0.0, 0.0, logger);

            ball.Move(1.0);

            Assert.Equal(initialX, ball.X);
            Assert.Equal(initialY, ball.Y);
        }

        [Fact]
        public void Move_NoChangeInPositionIfElapsedTimeIsZero_Test()
        {
            double initialX = 10.0;
            double initialY = 20.0;
            var logger = new FileLogger();
            var ball = new Ball(1, initialX, initialY, 5.0, 2.0, 1.0, 1.0, logger);

            ball.Move(0.0);

            Assert.Equal(initialX, ball.X);
            Assert.Equal(initialY, ball.Y);
        }

        [Fact]
        public void Move_NoChangeInPositionIfElapsedTimeIsNegative_Test()
        {
            double initialX = 10.0;
            double initialY = 20.0;
            var logger = new FileLogger();
            var ball = new Ball(1, initialX, initialY, 5.0, 2.0, 1.0, 1.0, logger);

            ball.Move(-1.0);

            Assert.Equal(initialX, ball.X);
            Assert.Equal(initialY, ball.Y);
        }
        
        [Fact]
        public void Move_RaisesPropertyChangedForXAndY_WhenPositionChanges_Test()
        {
            var logger = new FileLogger();
            var ball = new Ball(1, 10.0, 10.0, 5.0, 2.0, 1.0, 1.0, logger);

            string lastPropertyChanged = null;
            int xChangedCount = 0;
            int yChangedCount = 0;

            ball.PropertyChanged += (sender, args) =>
            {
                lastPropertyChanged = args.PropertyName;
                if (args.PropertyName == nameof(Ball.X)) xChangedCount++;
                if (args.PropertyName == nameof(Ball.Y)) yChangedCount++;
            };

            ball.Move(0.1);

            Assert.True(xChangedCount >= 1, "PropertyChanged dla X powinno zostać wywołane co najmniej raz.");
            Assert.True(yChangedCount >= 1, "PropertyChanged dla Y powinno zostać wywołane co najmniej raz.");
        }

        [Fact]
        public void Move_DoesNotRaisePropertyChangedForXAndY_WhenPositionDoesNotChange_Test()
        {
            var logger = new FileLogger();
            var ball = new Ball(1, 10.0, 10.0, 5.0, 2.0, 0.0, 0.0, logger);

            bool propertyChangedFired = false;
            ball.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Ball.X) || args.PropertyName == nameof(Ball.Y))
                {
                    propertyChangedFired = true;
                }
            };

            ball.Move(0.1);

            Assert.False(propertyChangedFired, "PropertyChanged dla X lub Y nie powinno być wywołane, gdy pozycja się nie zmienia.");
        }
    }
}