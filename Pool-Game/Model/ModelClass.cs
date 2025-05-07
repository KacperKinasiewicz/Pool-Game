using System.Threading.Tasks;
using Logic;

namespace Model
{
    public class ModelClass
    {
        private readonly ITable _table;
        public ITable Table => _table;

        public ModelClass(double width, double height)
        {
            _table = new Table(width, height);
        }
        
        public async Task InitializeBalls(int ballCount)
        {
            await _table.CreateBalls(ballCount);
        }

        public void UpdateBalls()
        {
            _table.UpdateSimulationStep();
        }
    }
}