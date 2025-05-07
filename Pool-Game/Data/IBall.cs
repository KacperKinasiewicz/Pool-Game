using System.ComponentModel;

namespace Data
{
    public interface IBall : INotifyPropertyChanged
    {
        int Id { get; }
        double X { get; }
        double Y { get; }
        double Radius { get; }
        double Mass { get; }

        double VelocityX { get; set; }
        double VelocityY { get; set; }

        void Move(double timeStep);
    }
}