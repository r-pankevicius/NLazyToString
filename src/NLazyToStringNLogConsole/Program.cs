using NLog;
using System;
using System.Diagnostics;

namespace NLazyToStringNLogConsole
{
    internal struct LazyToString
    {
        private readonly Func<string> _toStringFunc;

        public LazyToString(Func<string> toStringFunc)
        {
            _toStringFunc = toStringFunc ?? throw new ArgumentNullException(nameof(toStringFunc));
        }

        public override string ToString() => _toStringFunc();

        public static LazyToString Wrap(Func<string> toStringFunc) => new(toStringFunc);
    }

    internal static class Program
    {
        private const int LoopCounter = 1_000_000;

        private static string LongString = new('9', 80);
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
            using (var chronometer = new Chronometer("Interpolate loop"))
            {
                for (int i = 0; i < LoopCounter; i++)
                    Logger.Debug($"Gaidys dbg {i} {LongString}");
            }

            using (var chronometer = new Chronometer("string.Format loop"))
            {
                for (int i = 0; i < LoopCounter; i++)
                    Logger.Debug(string.Format("Gaidys dbg {0} {1}", i, LongString));
            }

            using (var chronometer = new Chronometer("LazyToString loop"))
            {
                for (int i = 0; i < LoopCounter; i++)
                    Logger.Debug(LazyToString.Wrap(() => $"Gaidys dbg {i} {LongString}"));
            }
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

            public void Dispose()
            {
                _stopWatch.Stop();
                Logger.Info($"{_episode} end, took {_stopWatch.Elapsed}");
            }
        }
    }
}
