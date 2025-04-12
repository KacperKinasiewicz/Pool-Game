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
        
        public void InitializeBalls(int ballCount)
        {
            _table.InitializeBalls(ballCount);
        }

        public void UpdateBalls()
        {
            _table.UpdateBalls();
        }
    }
}