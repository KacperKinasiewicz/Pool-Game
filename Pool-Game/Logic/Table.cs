using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Data;

namespace Logic
{
    public class Table : ITable
    {
        public double Width { get; }
        public double Height { get; }

        private readonly ObservableCollection<IBall> _balls = new ObservableCollection<IBall>();
        public ObservableCollection<IBall> Balls => _balls;

        private readonly Random _random = new Random();
        private readonly object _ballsLock = new object();

        private const double TimeStep = 0.1;

        public Table(double width, double height)
        {
            if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width), "Szerokość musi być dodatnia.");
            if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height), "Wysokość musi być dodatnia.");
            Width = width;
            Height = height;
        }
        
        public async Task CreateBalls(int count, double defaultRadius = 10, double defaultMass = 10)
        {
            if (count < 0) { throw new ArgumentOutOfRangeException(nameof(count), "Liczba kul nie może być ujemna."); }
            if (defaultRadius <= 0) { throw new ArgumentOutOfRangeException(nameof(defaultRadius), "Promień musi być dodatni."); }
            if (defaultMass <= 0) { throw new ArgumentOutOfRangeException(nameof(defaultMass), "Masa musi być dodatnia."); }

            List<IBall> tempGeneratedBalls = await Task.Run(() =>
            {
                var localBallsList = new List<IBall>();
                for (int i = 0; i < count; i++)
                {
                    bool placedSuccessfully = false;
                    IBall newBall = null;
                    int placementAttempts = 0;
                    const int maxPlacementAttempts = 100;

                    while (!placedSuccessfully && placementAttempts < maxPlacementAttempts)
                    {
                        double x = _random.NextDouble() * (Width - 2 * defaultRadius) + defaultRadius;
                        double y = _random.NextDouble() * (Height - 2 * defaultRadius) + defaultRadius;
                        double velocityX = (_random.NextDouble() * 100) - 50;
                        double velocityY = (_random.NextDouble() * 100) - 50;

                        newBall = new Ball(i, x, y, defaultRadius, defaultMass, velocityX, velocityY);

                        bool overlapsWithExisting = false;
                        foreach (var existingBall in localBallsList)
                        {
                            double dxCheck = existingBall.X - newBall.X;
                            double dyCheck = existingBall.Y - newBall.Y;
                            double distanceSquaredCheck = dxCheck * dxCheck + dyCheck * dyCheck;
                            double sumRadii = existingBall.Radius + newBall.Radius;
                            if (distanceSquaredCheck < sumRadii * sumRadii)
                            {
                                overlapsWithExisting = true;
                                break;
                            }
                        }

                        if (!overlapsWithExisting)
                        {
                            localBallsList.Add(newBall);
                            placedSuccessfully = true;
                        }
                        placementAttempts++;
                    }
                }
                return localBallsList;
            });

            lock (_ballsLock)
            {
                _balls.Clear();
                foreach (var ball in tempGeneratedBalls)
                {
                    _balls.Add(ball);
                }
            }
        }

        public void UpdateSimulationStep()
        {
            var currentBallsSnapshot = new List<IBall>();
            lock(_ballsLock)
            {
                currentBallsSnapshot = _balls.ToList();
            }

            foreach (var ball in currentBallsSnapshot)
            {
                ball.Move(TimeStep);
            }
            
            foreach (var ball in currentBallsSnapshot)
            {
                HandleWallCollision(ball);
            }

            for (int i = 0; i < currentBallsSnapshot.Count; i++)
            {
                for (int j = i + 1; j < currentBallsSnapshot.Count; j++)
                {
                    HandleBallPairCollision(currentBallsSnapshot[i], currentBallsSnapshot[j]);
                }
            }
        }

        private void HandleWallCollision(IBall ball)
        {
            if (ball.X - ball.Radius < 0)
            {
                ball.VelocityX = Math.Abs(ball.VelocityX);
            }
            else if (ball.X + (2 * ball.Radius) > Width)
            {
                ball.VelocityX = -Math.Abs(ball.VelocityX);
            }

            if (ball.Y - ball.Radius < 0)
            {
                ball.VelocityY = Math.Abs(ball.VelocityY);
            }
            else if (ball.Y + (2 * ball.Radius) > Height)
            {
                ball.VelocityY = -Math.Abs(ball.VelocityY);
            }
        }

        private void HandleBallPairCollision(IBall ball1, IBall ball2)
        {
            double dx = ball2.X - ball1.X;
            double dy = ball2.Y - ball1.Y;
            double distanceSquared = dx * dx + dy * dy;
            double sumRadii = ball1.Radius + ball2.Radius;

            if (distanceSquared <= sumRadii * sumRadii && distanceSquared > 0)
            {
                double distance = Math.Sqrt(distanceSquared);

                double nx = dx / distance;
                double ny = dy / distance;

                double tx = -ny;
                double ty = nx;

                double dpTan1 = ball1.VelocityX * tx + ball1.VelocityY * ty;
                double dpTan2 = ball2.VelocityX * tx + ball2.VelocityY * ty;

                double dpNorm1 = ball1.VelocityX * nx + ball1.VelocityY * ny;
                double dpNorm2 = ball2.VelocityX * nx + ball2.VelocityY * ny;

                double m1 = ball1.Mass;
                double m2 = ball2.Mass;

                double newDpNorm1 = (dpNorm1 * (m1 - m2) + 2 * m2 * dpNorm2) / (m1 + m2);
                double newDpNorm2 = (dpNorm2 * (m2 - m1) + 2 * m1 * dpNorm1) / (m1 + m2);

                ball1.VelocityX = tx * dpTan1 + nx * newDpNorm1;
                ball1.VelocityY = ty * dpTan1 + ny * newDpNorm1;
                ball2.VelocityX = tx * dpTan2 + nx * newDpNorm2;
                ball2.VelocityY = ty * dpTan2 + ny * newDpNorm2;
            }
        }
    }
}