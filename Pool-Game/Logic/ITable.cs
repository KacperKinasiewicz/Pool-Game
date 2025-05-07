using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Data;

namespace Logic
{
    public interface ITable
    {
        double Width { get; }
        double Height { get; }
        ObservableCollection<IBall> Balls { get; }
        
        Task CreateBalls(int count, double defaultRadius = 10, double defaultMass = 10);
        void UpdateSimulationStep();
    }
}