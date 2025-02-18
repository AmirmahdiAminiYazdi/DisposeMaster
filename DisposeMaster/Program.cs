using DisposeMaster;
using System.Diagnostics;

class Program
{
    private static WeakReference<LogManager> _weakLogger;

    // Schedules periodic logging using an async Task-based approach
    static async Task ScheduleLoggingAsync()
    {
        while (true)
        {
            if (_weakLogger.TryGetTarget(out LogManager logger))
            {
                logger.WriteLog("Scheduled log entry");
            }
            else
            {
                Console.WriteLine("LogManager object has been garbage collected.");
                break; // Stop loop if object is garbage collected
            }
            await Task.Delay(5000); // Wait for 5 seconds before logging again
        }
    }

    static async Task Main()
    {
        Console.WriteLine("Garbage Collection & IDisposable Demo");

        long initialMemory = GC.GetTotalMemory(true);
        Console.WriteLine($"Initial Memory Usage: {initialMemory} bytes");

        // Measure execution time of Dispose
        Stopwatch stopwatch = Stopwatch.StartNew();

        // Proper usage of IDisposable to manage resources
        var logger = new LogManager("log.txt");
        _weakLogger = new WeakReference<LogManager>(logger);
        logger.WriteLog("Application Started");
        await ScheduleLoggingAsync();

        // Forcing Garbage Collection to test Dispose behavior
        Console.WriteLine("Forcing GC Collection...");
        GC.Collect();
        GC.WaitForPendingFinalizers();

        long finalMemory = GC.GetTotalMemory(true);
        Console.WriteLine($"Final Memory Usage: {finalMemory} bytes");

        stopwatch.Stop();
        Console.WriteLine($"Dispose Execution Time: {stopwatch.ElapsedMilliseconds} ms");

        // Uncomment to simulate a memory leak scenario
        // CauseMemoryLeak();

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();

        logger.Dispose(); // Ensure proper cleanup before exiting
    }
}

