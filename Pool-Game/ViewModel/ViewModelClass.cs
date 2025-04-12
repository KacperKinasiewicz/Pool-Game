using System.ComponentModel;
using System.Timers;
using Model;

namespace ViewModel
{
    public class ViewModelClass : INotifyPropertyChanged
    {
        private int _ballCount = 10;
        private readonly Timer _timer;
        private readonly ModelClass _modelClass;
        public ModelClass ModelClass => _modelClass;
        
        public ViewModelClass(int ballCount)
        {
            _modelClass = new ModelClass(800, 600);
            this.BallCount = ballCount;
            
            _timer = new Timer(16);
            _timer.Elapsed += Timer_Elapsed;
            _timer.AutoReset = true;
        }
        
        public int BallCount
        {
            get => _ballCount;
            set
            {
                if (_ballCount != value && value > 0)
                {
                    _ballCount = value;
                    OnPropertyChanged(nameof(BallCount));
                }
            }
        }
        
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            UpdateSimulation();
        }
        
        public void StartSimulation()
        {
            _modelClass.InitializeBalls(BallCount);
            _timer.Start();
        }
        
        public void UpdateSimulation()
        {
            _modelClass.UpdateBalls();
        }
        
        public void StopSimulation()
        {
            _timer.Stop();
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}