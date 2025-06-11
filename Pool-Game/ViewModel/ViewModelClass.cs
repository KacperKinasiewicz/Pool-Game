using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Model;

namespace ViewModel
{
    public class ViewModelClass : INotifyPropertyChanged
    {
        private int _ballCount = 10;
        private CancellationTokenSource _simulationCts;
        private Task _simulationTask;
        private readonly ModelClass _modelClass;
        public ModelClass ModelClass => _modelClass;
        
        public ViewModelClass(int initialBallCount = 2)
        {
            _modelClass = new ModelClass(800, 600);
            BallCount = initialBallCount; 
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
                    _ = InitializeBalls(_ballCount);
                }
            }
        }
        
        private async Task InitializeBalls(int count)
        {
            await _modelClass.InitializeBalls(count);
        }

        
        public async Task StartSimulation()
        {
            if (_simulationTask != null && !_simulationTask.IsCompleted)
            {
                _simulationCts.Cancel();
                try
                {
                    await _simulationTask;
                }
                catch (OperationCanceledException) { }
                finally
                {
                    _simulationCts?.Dispose();
                }
            }

            await InitializeBalls(BallCount);

            _simulationCts = new CancellationTokenSource();
            CancellationToken token = _simulationCts.Token;

            _simulationTask = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    UpdateSimulation();
                    try
                    {
                        await Task.Delay(16, token); 
                    }
                    catch (TaskCanceledException)
                    {
                        break;
                    }
                }
            }, token);
        }
        
        public void UpdateSimulation()
        {
            _modelClass.UpdateBalls();
        }
        
        public void StopSimulation()
        {
            _simulationCts?.Cancel();
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}