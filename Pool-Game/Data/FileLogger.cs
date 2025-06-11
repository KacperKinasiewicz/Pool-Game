using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Data
{
    public class FileLogger : ILogger
    {
        private class CollisionLogEntry
        {
            public int Ball1Id { get; }
            public int? Ball2Id { get; }
            public double X1 { get; }
            public double Y1 { get; }
            public double? X2 { get; }
            public double? Y2 { get; }
            public double VelocityX1 { get; }
            public double VelocityY1 { get; }
            public double? VelocityX2 { get; }
            public double? VelocityY2 { get; }
            public DateTime Timestamp { get; }
            public string EventType { get; }

            public CollisionLogEntry(IBall ball, DateTime timestamp)
            {
                Ball1Id = ball.Id;
                Ball2Id = null;
                X1 = ball.X;
                Y1 = ball.Y;
                X2 = null;
                Y2 = null;
                VelocityX1 = ball.VelocityX;
                VelocityY1 = ball.VelocityY;
                VelocityX2 = null;
                VelocityY2 = null;
                Timestamp = timestamp;
                EventType = "Kolizja ze ścianą";
            }

            public CollisionLogEntry(IBall ball1, IBall ball2, DateTime timestamp)
            {
                Ball1Id = ball1.Id;
                Ball2Id = ball2.Id;
                X1 = ball1.X;
                Y1 = ball1.Y;
                X2 = ball2.X;
                Y2 = ball2.Y;
                VelocityX1 = ball1.VelocityX;
                VelocityY1 = ball1.VelocityY;
                VelocityX2 = ball2.VelocityX;
                VelocityY2 = ball2.VelocityY;
                Timestamp = timestamp;
                EventType = "Kolizja między kulami";
            }

            public string ToJsonString()
            {
                return string.Format(CultureInfo.InvariantCulture,
                    "{{\"Ball1Id\":{0},\"Ball2Id\":{1},\"X1\":{2:F2},\"Y1\":{3:F2},\"X2\":{4:F2},\"Y2\":{5:F2},\"VelocityX1\":{6:F2},\"VelocityY1\":{7:F2},\"VelocityX2\":{8:F2},\"VelocityY2\":{9:F2},\"Timestamp\":\"{10:yyyy-MM-ddTHH:mm:ss.fffZ}\",\"EventType\":\"{11}\"}}",
                    Ball1Id, Ball2Id, X1, Y1, X2, Y2, VelocityX1, VelocityY1, VelocityX2, VelocityY2, Timestamp.ToUniversalTime(), EventType);
            }
        }

        private readonly BlockingCollection<CollisionLogEntry> _logQueue;
        private readonly string _logFilePath;
        private readonly Task _loggingTask;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private const int InternalCacheSize = 100;
        private static readonly string DefaultLogDirectory = "../../../../Logs";

        public FileLogger()
        {
            string logFileName = $"log_{DateTime.UtcNow:yyyyMMdd_HHmmss}.json";
            _logFilePath = Path.Combine(DefaultLogDirectory, logFileName);

            if (!Directory.Exists(DefaultLogDirectory))
            {
                Directory.CreateDirectory(DefaultLogDirectory);
            }

            _logQueue = new BlockingCollection<CollisionLogEntry>(new ConcurrentQueue<CollisionLogEntry>(), InternalCacheSize);
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                using (StreamWriter writer = new StreamWriter(_logFilePath, false, Encoding.UTF8)) { }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Nie można zainicjalizować pliku logu: {_logFilePath}. Błąd: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Brak uprawnień do utworzenia/zapisu pliku logu: {_logFilePath}. Błąd: {ex.Message}");
            }

            _loggingTask = Task.Run(() => ProcessQueue(_cancellationTokenSource.Token));
        }

        public void LogCollision(IBall ball, DateTime timestamp)
        {
            var logEntry = new CollisionLogEntry(ball, timestamp);
            _logQueue.TryAdd(logEntry);
        }

        public void LogCollision(IBall ball1, IBall ball2, DateTime timestamp)
        {
            var logEntry = new CollisionLogEntry(ball1, ball2, timestamp);
            _logQueue.TryAdd(logEntry);
        }

        private void ProcessQueue(CancellationToken token)
        {
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(_logFilePath, true, Encoding.UTF8))
                {
                    while (!_logQueue.IsCompleted || token.IsCancellationRequested)
                    {
                        if (token.IsCancellationRequested) break;

                        CollisionLogEntry entryToLog;
                        try
                        {
                            entryToLog = _logQueue.Take(token);
                        }
                        catch (OperationCanceledException)
                        {
                            break;
                        }
                        catch (InvalidOperationException)
                        {
                            break;
                        }

                        if (entryToLog != null)
                        {
                            string jsonString = entryToLog.ToJsonString();
                            streamWriter.WriteLine(jsonString);
                            streamWriter.Flush();
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Błąd zapisu do pliku logu w ProcessQueue: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Brak uprawnień do zapisu do pliku logu w ProcessQueue: {_logFilePath}. Błąd: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Niespodziewany błąd w ProcessQueue: {ex.Message}");
            }
        }

        public void Dispose()
        {
            if (!_logQueue.IsAddingCompleted)
            {
                _logQueue.CompleteAdding();
            }

            if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
            }

            try
            {
                _loggingTask?.Wait(TimeSpan.FromSeconds(1));
            }
            catch (OperationCanceledException)
            {
            }
            catch (AggregateException ae)
            {
                ae.Handle(ex => ex is OperationCanceledException || ex is TaskCanceledException);
            }
            finally
            {
                _cancellationTokenSource?.Dispose();
                _logQueue?.Dispose();
            }
        }
    }
}