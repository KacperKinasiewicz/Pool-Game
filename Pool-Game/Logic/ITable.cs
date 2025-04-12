using System.Collections.Generic;
using Data;

namespace Logic
{
    public interface ITable
    {
        double Width { get; }
        double Height { get; }
        IReadOnlyList<IBall> Balls { get; }

        void InitializeBalls(int count);
        void UpdateBalls();
        void AddBall(IBall ball);
        void RemoveBall(IBall ball);
    }
}