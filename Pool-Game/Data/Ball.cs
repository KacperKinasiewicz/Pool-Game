using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Data
{
    public class Ball : IBall
    {
        private readonly int _id;
        private double _x;
        private double _y;
        private double _velocityX;
        private double _velocityY;
        private readonly double _radius;
        private readonly double _mass;
        private readonly ILogger _logger;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id => _id;

        public double X
        {
            get { return _x; }
            private set
            {
                if (_x != value)
                {
                    _x = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Y
        {
            get { return _y; }
            private set
            {
                if (_y != value)
                {
                    _y = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Radius => _radius;
        public double Mass => _mass;

        public double VelocityX
        {
            get { return _velocityX; }
            set
            {
                if (_velocityX != value)
                {
                    _velocityX = value;
                    OnPropertyChanged();
                }
            }
        }

        public double VelocityY
        {
            get { return _velocityY; }
            set
            {
                if (_velocityY != value)
                {
                    _velocityY = value;
                    OnPropertyChanged();
                }
            }
        }

        public Ball(int id, double x, double y, double radius, double mass, double velocityX, double velocityY, ILogger logger)
        {
            if (radius <= 0) { throw new ArgumentOutOfRangeException(nameof(radius), "Promień musi być większy od zera!"); }
            if (mass <= 0) { throw new ArgumentOutOfRangeException(nameof(mass), "Masa musi być większa od zera!"); }

            _id = id;
            _x = x;
            _y = y;
            _radius = radius;
            _mass = mass;
            _velocityX = velocityX;
            _velocityY = velocityY;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger nie może być null.");
        }

        public void Move(double elapsedTime)
        {
            if (elapsedTime <= 0) return;

            double newX;
            double newY;

            newX = _x + _velocityX * elapsedTime;
            newY = _y + _velocityY * elapsedTime;
            
            X = newX;
            Y = newY;

            _logger.LogBallState(this, DateTime.UtcNow);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}