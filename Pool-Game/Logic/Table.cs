using System;
using System.Collections.ObjectModel;
using Data;

namespace Logic
{
    public class Table : ITable
    {
        public double Width { get; }
        public double Height { get; }
        
        private readonly Random _random = new Random();
        private readonly ObservableCollection<IBall> _balls = new ObservableCollection<IBall>();
        public ObservableCollection<IBall> Balls => _balls;
        public Table(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public void InitializeBalls(int count)
        {
            _balls.Clear();
            const double radius = 10;

            for (int i = 0; i < count; i++)
            {
                double x = _random.NextDouble() * (Width - 2 * radius);
                double y = _random.NextDouble() * (Height - 2 * radius);
                double velocityX = (_random.NextDouble() - 0.5) * 5;
                double velocityY = (_random.NextDouble() - 0.5) * 5;
                
                var ball = new Ball(i, x, y, velocityX, velocityY, radius);
                _balls.Add(ball);
            }
        }

        public void UpdateBalls()
        {
            foreach (var ball in _balls)
            {
                ball.X += ball.VelocityX;
                ball.Y += ball.VelocityY;

                if (ball.X - ball.Radius < 0 || ball.X + 2 * ball.Radius > Width || ball.Y - ball.Radius < 0 || ball.Y + 2 * ball.Radius > Height)
                {
                    ball.VelocityX = 0;
                    ball.VelocityY = 0;
                }

                if (ball.X < ball.Radius)
                    ball.X = ball.Radius;
                else if (ball.X > Width - ball.Radius)
                    ball.X = Width - ball.Radius;

                if (ball.Y < ball.Radius)
                    ball.Y = ball.Radius;
                else if (ball.Y > Height - ball.Radius)
                    ball.Y = Height - ball.Radius;
            }
        }
        
        public void AddBall(IBall ball)
        {
            if (ball == null) throw new ArgumentNullException(nameof(ball));
            _balls.Add(ball);
        }
        
        public void RemoveBall(IBall ball)
        {
            if (ball == null) throw new ArgumentNullException(nameof(ball));
            if (_balls.Contains(ball))
            {
                _balls.Remove(ball);
            }
        }
    }
}
