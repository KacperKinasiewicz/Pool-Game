using System.ComponentModel;

namespace Data
{
    public interface IBall : INotifyPropertyChanged
    {
        int Id { get; }
        double X { get; set; }
        double Y { get; set; }
        double VelocityX { get; set; }
        double VelocityY { get; set; }
        double Radius { get; }

        event PropertyChangedEventHandler PropertyChanged;
    }
}