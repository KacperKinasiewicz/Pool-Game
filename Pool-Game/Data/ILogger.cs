using System;

namespace Data
{
    public interface ILogger
    {
        void LogCollision(IBall ball, DateTime time);
        void LogCollision(IBall ball1, IBall ball2, DateTime time);
    }
}