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
        private class BallToLogEntry
        {
            public int BallId { get; }
            public double X { get; }
            public double Y { get; }
            public double VelocityX { get; }
            public double VelocityY { get; }
            public DateTime Timestamp { get; }

            public BallToLogEntry(int ballId, double x, double y, double velocityX, double velocityY, DateTime timestamp)
            {
                BallId = ballId;
                X = x;
                Y = y;
                VelocityX = velocityX;
                VelocityY = velocityY;
                Timestamp = timestamp;
            }

            public string ToJsonString()
            {
                return string.Format(CultureInfo.InvariantCulture,
                    "{{\"BallId\":{0},\"X\":{1:F2},\"Y\":{2:F2},\"VelocityX\":{3:F2},\"VelocityY\":{4:F2},\"Timestamp\":\"{5:yyyy-MM-ddTHH:mm:ss.fffZ}\"}}",
                    BallId, X, Y, VelocityX, VelocityY, Timestamp.ToUniversalTime());
            }
        }

        private readonly BlockingCollection<BallToLogEntry> _logQueue;
        private readonly string _logFilePath;
        private readonly Task _loggingTask;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        
        private bool _isOverCacheSize = false;
        private readonly object _cacheLock = new object();

        private const int DefaultCacheSize = 200;

        public FileLogger(string filePath = "./log.json", int cacheSize = DefaultCacheSize)
        {
            if (!Path.IsPathRooted(filePath))
            {
                _logFilePath = Path.Combine(Directory.GetCurrentDirectory(), filePath);
            }
            else
            {
                _logFilePath = filePath;
            }
            
            _logQueue = new BlockingCollection<BallToLogEntry>(new ConcurrentQueue<BallToLogEntry>(), cacheSize > 0 ? cacheSize : DefaultCacheSize);

            try
            {
                using (new StreamWriter(_logFilePath, false, Encoding.UTF8)) { }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Nie można zainicjalizować pliku logu: {_logFilePath}. Błąd: {ex.Message}");
            }
            
            _loggingTask = Task.Run(ProcessQueue, _cancellationTokenSource.Token);
        }

        public void LogBallState(IBall ball, DateTime timestamp)
        {
            var logEntry = new BallToLogEntry(ball.Id, ball.X, ball.Y, ball.VelocityX, ball.VelocityY, timestamp);
            bool addedSuccessfully = _logQueue.TryAdd(logEntry);

            if (!addedSuccessfully)
            {
                lock (_cacheLock)
                {
                    _isOverCacheSize = true;
                }
            }
        }

        private async Task ProcessQueue()
        {
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(_logFilePath, true, Encoding.UTF8))
                {
                    while (!_logQueue.IsCompleted)
                    {
                        bool overflowOccurred = false;
                        lock (_cacheLock)
                        {
                            if (_isOverCacheSize)
                            {
                                overflowOccurred = true;
                                _isOverCacheSize = false;
                            }
                        }

                        if (overflowOccurred)
                        {
                            await streamWriter.WriteLineAsync("Cache is over size. Some logs might have been lost.");
                        }

                        BallToLogEntry entryToLog = null;
                        try
                        {
                            entryToLog = _logQueue.Take(_cancellationTokenSource.Token);
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
                            await streamWriter.WriteLineAsync(jsonString);
                            await streamWriter.FlushAsync();
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Błąd zapisu do pliku logu w ProcessQueue: {ex.Message}");
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

            _cancellationTokenSource.Cancel();

            try
            {
                _loggingTask?.Wait(TimeSpan.FromSeconds(5));
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
                _cancellationTokenSource.Dispose();
                _logQueue.Dispose();
            }
        }
    }
}