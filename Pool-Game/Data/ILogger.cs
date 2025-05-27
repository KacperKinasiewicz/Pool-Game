using System;

namespace Data
{
    public interface ILogger : IDisposable
    {
        void LogBallState(IBall ball, DateTime timestamp);
    }
}