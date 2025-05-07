using Logic;

namespace Test
{
    public class LogicTest
    {
        [Fact]
        public void ConstructorTest()
        {
            double width = 100.0;
            double height = 200.0;
            
            var table = new Table(width, height);
            
            Assert.Equal(width, table.Width);
            Assert.Equal(height, table.Height);
            Assert.NotNull(table.Balls);
            Assert.Empty(table.Balls);
        }

        [Fact]
        public async Task CreateBalls_CreatesCorrectNumberOfBalls_Test()
        {
            var table = new Table(100, 100);
            int ballCount = 5;

            await table.CreateBalls(ballCount);

            Assert.Equal(ballCount, table.Balls.Count);
            foreach (object ball in table.Balls) 
            {
                Assert.NotNull(ball);
            }
        }

        [Fact]
        public async Task CreateBalls_ClearsPreviousBalls_Test()
        {
            var table = new Table(100, 100);
            await table.CreateBalls(3);
            Assert.Equal(3, table.Balls.Count);

            await table.CreateBalls(2);
            Assert.Equal(2, table.Balls.Count);
        }
        
        [Fact]
        public async Task CreateBalls_InvalidCount_ThrowsArgumentOutOfRangeException_Test()
        {
            var table = new Table(100, 100);
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>("count", async () => 
                await table.CreateBalls(-1)
            );
        }

        [Fact]
        public async Task CreateBalls_InvalidDefaultRadius_ThrowsArgumentOutOfRangeException_Test()
        {
            var table = new Table(100, 100);
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>("defaultRadius", async () => 
                await table.CreateBalls(1, defaultRadius: 0)
            );
        }

        [Fact]
        public async Task CreateBalls_InvalidDefaultMass_ThrowsArgumentOutOfRangeException_Test()
        {
            var table = new Table(100, 100);
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>("defaultMass", async () => 
                await table.CreateBalls(1, defaultMass: -5)
            );
        }

        [Fact]
        public void UpdateSimulationStep_RunsWithoutError_WhenBallsEmpty_Test()
        {
            var table = new Table(100, 100);
            var exception = Record.Exception(() => table.UpdateSimulationStep());
            Assert.Null(exception);
        }

        [Fact]
        public async Task UpdateSimulationStep_RunsWithoutError_WhenBallsExist_Test()
        {
            var table = new Table(100, 100);
            await table.CreateBalls(3);
            
            var exception = Record.Exception(() => table.UpdateSimulationStep());
            Assert.Null(exception);
        }
    }
}