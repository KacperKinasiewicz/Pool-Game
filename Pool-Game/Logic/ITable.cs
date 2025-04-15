using System.Collections.ObjectModel;
using Data;

namespace Logic
{
    public interface ITable
    {
        double Width { get; }
        double Height { get; }
        ObservableCollection<IBall> Balls { get; }

        void InitializeBalls(int count);
        void UpdateBalls();
        void AddBall(IBall ball);
        void RemoveBall(IBall ball);
    }
}