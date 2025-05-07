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
        private readonly object _lock = new object();

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id => _id;

        public double X
        {
            get { lock (_lock) { return _x; } }
            set
            {
                bool changed = false;
                lock (_lock)
                {
                    if (_x != value)
                    {
                        _x = value;
                        changed = true;
                    }
                }
                if (changed) { OnPropertyChanged(); }
            }
        }

        public double Y
        {
            get { lock (_lock) { return _y; } }
            set
            {
                bool changed = false;
                lock (_lock)
                {
                    if (_y != value)
                    {
                        _y = value;
                        changed = true;
                    }
                }
                if (changed) { OnPropertyChanged(); }
            }
        }

        public double Radius => _radius;
        public double Mass => _mass;

        public double VelocityX
        {
            get { lock (_lock) { return _velocityX; } }
            set
            {
                bool changed = false;
                lock (_lock)
                {
                    if (_velocityX != value)
                    {
                        _velocityX = value;
                        changed = true;
                    }
                }
                if (changed) { OnPropertyChanged(); }
            }
        }

        public double VelocityY
        {
            get { lock (_lock) { return _velocityY; } }
            set
            {
                bool changed = false;
                lock (_lock)
                {
                    if (_velocityY != value)
                    {
                        _velocityY = value;
                        changed = true;
                    }
                }
                if (changed) { OnPropertyChanged(); }
            }
        }

        public Ball(int id, double x, double y, double radius, double mass, double velocityX, double velocityY)
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
        }

        public void Move(double timeStep)
        {
            double newX;
            double newY;

            lock (_lock)
            {
                newX = _x + _velocityX * timeStep;
                newY = _y + _velocityY * timeStep;
            }
            
            X = newX;
            Y = newY;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}