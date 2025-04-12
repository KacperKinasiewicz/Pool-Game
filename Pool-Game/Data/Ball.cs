using System;
using System.ComponentModel;

namespace Data
{
    public class Ball : IBall, INotifyPropertyChanged
    {
        private int id;
        private double x;
        private double y;
        private double velocityX;
        private double velocityY;
        private double radius;

        public int Id => id;
        public double Radius => radius;

        public Ball(int id, double x, double y, double velocityX, double velocityY, double radius)
        {
            if (radius <= 0)
                throw new ArgumentException("Promień musi być większy od zera!", nameof(radius));
            
            this.id = id;
            this.x = x;
            this.y = y;
            this.velocityX = velocityX;
            this.velocityY = velocityY;
            this.radius = radius;
        }
        
        public double X
        {
            get => x;
            set
            {
                if (x != value)
                {
                    x = value;
                    OnPropertyChanged(nameof(X));
                }
            }
        }

        public double Y
        {
            get => y;
            set
            {
                if (y != value)
                {
                    y = value;
                    OnPropertyChanged(nameof(Y));
                }
            }
        }

        public double VelocityX
        {
            get => velocityX;
            set
            {
                if (velocityX != value)
                {
                    velocityX = value;
                    OnPropertyChanged(nameof(VelocityX));
                }
            }
        }

        public double VelocityY
        {
            get => velocityY;
            set
            {
                if (velocityY != value)
                {
                    velocityY = value;
                    OnPropertyChanged(nameof(VelocityY));
                }
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}