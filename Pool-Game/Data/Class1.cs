using System.Collections.Generic;

namespace Data
{
    public interface IDataRepository
    {
        List<Ball> GetBalls();
        Table GetTable();
    }

    public class DataRepository : IDataRepository
    {
        public List<Ball> GetBalls()
        {
            return new List<Ball>
            {
                new Ball { Id = 1, X = 100, Y = 100, VelocityX = 10, VelocityY = 10 },
                new Ball { Id = 2, X = 200, Y = 200, VelocityX = 0, VelocityY = 0 }
            };
        }

        public Table GetTable()
        {
            return new Table();
        }
    }
}