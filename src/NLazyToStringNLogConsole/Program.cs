using NLog;
using System;
using System.Diagnostics;
using static NLazyToString.LazyToString;

namespace NLazyToStringNLogConsole
{
    internal static class Program
    {
        private const int LoopCounter = 1_000_000;

        private static readonly string LongString = new('9', 20);
        private static readonly ILogger Logger= LogManager.GetCurrentClassLogger();

        static Program()
        {
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            var config = new NLog.Config.LoggingConfiguration();
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);

            LogManager.Configuration = config;
        }

        static void Main(string[] args)
        {
            TestWhenInterpolationIsNotNeeded();
            TestWhenInterpolationIsNeeded();
        }

        private static void TestWhenInterpolationIsNotNeeded()
        {
            Console.WriteLine("Testing savings when interpolation is not needed...");
            Console.WriteLine();

            long ticksInterpolate, ticksFormat, ticksLazyString;
            var logger = Logger;

            using (var chronometer = new Chronometer("Interpolate loop"))
            {
                for (int i = 0; i < LoopCounter; i++)
                    logger.Debug($"Gaidys dbg {i} {LongString}");
                ticksInterpolate = chronometer.ElapsedTicks;
            }

            using (var chronometer = new Chronometer("string.Format loop"))
            {
                for (int i = 0; i < LoopCounter; i++)
                    logger.Debug(string.Format("Gaidys dbg {0} {1}", i, LongString));
                ticksFormat = chronometer.ElapsedTicks;
            }

            using (var chronometer = new Chronometer("LazyToString loop"))
            {
                for (int i = 0; i < LoopCounter; i++)
                    logger.Debug(LazyString(() => $"Gaidys dbg {i} {LongString}"));
                ticksLazyString = chronometer.ElapsedTicks;
            }

            Console.WriteLine();
            Console.WriteLine($"Ticks: interpolate={ticksInterpolate}, format={ticksFormat}, {nameof(LazyString)}={ticksLazyString}");
            Console.WriteLine();
            Console.WriteLine($"{nameof(LazyString)} was faster than interpolate {(double)ticksInterpolate / ticksLazyString} times.");
            Console.WriteLine();
            Console.WriteLine($"{nameof(LazyString)} was faster than format {(double)ticksFormat / ticksLazyString} times.");
            Console.WriteLine();
        }

        private static void TestWhenInterpolationIsNeeded()
        {
            Console.WriteLine("Testing overhead when interpolation is needed...");
            Console.WriteLine();

            long ticksInterpolate, ticksFormat, ticksLazyString;
            var logger = new FakeLogger();

            using (var chronometer = new Chronometer("Interpolate loop"))
            {
                for (int i = 0; i < LoopCounter; i++)
                    logger.Debug($"Gaidys dbg {i} {LongString}");
                ticksInterpolate = chronometer.ElapsedTicks;
            }

            using (var chronometer = new Chronometer("string.Format loop"))
            {
                for (int i = 0; i < LoopCounter; i++)
                    logger.Debug(string.Format("Gaidys dbg {0} {1}", i, LongString));
                ticksFormat = chronometer.ElapsedTicks;
            }

            using (var chronometer = new Chronometer("LazyToString loop"))
            {
                for (int i = 0; i < LoopCounter; i++)
                    logger.Debug(LazyString(() => $"Gaidys dbg {i} {LongString}"));
                ticksLazyString = chronometer.ElapsedTicks;
            }

            Console.WriteLine();
            Console.WriteLine($"Ticks: interpolate={ticksInterpolate}, format={ticksFormat}, {nameof(LazyString)}={ticksLazyString}");
            Console.WriteLine();
            Console.WriteLine($"{nameof(LazyString)} was slower than interpolate {(double)ticksLazyString / ticksInterpolate} times.");
            Console.WriteLine();
            Console.WriteLine($"{nameof(LazyString)} was slower than format {(double)ticksLazyString / ticksFormat } times.");
            Console.WriteLine();
        }

        private sealed class Chronometer : IDisposable
        {
            private readonly string _episode;
            private readonly Stopwatch _stopWatch;

            public Chronometer(string episode)
            {
                _episode = episode;
                _stopWatch = Stopwatch.StartNew();
                Logger.Info($"{_episode} start");
            }

            public long ElapsedTicks => _stopWatch.ElapsedTicks;

            public void Dispose()
            {
                _stopWatch.Stop();
                Logger.Info($"{_episode} end, took {_stopWatch.Elapsed}");
            }
        }

        private sealed class FakeLogger
        {
            internal void Debug(string str) { SimulateLog(str); }

            internal void Debug(object obj) { SimulateLog(obj?.ToString()); }

            private void SimulateLog(string str)
            {
            }
        }
    }
}
