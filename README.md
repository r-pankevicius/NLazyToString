# NLazyToString
Lazy evaluated ToString() string formatting for use in logging when you don't know in advance if you need to format that string at all.

## It solves: How much debugging log can I add to my program?
 The answer is: **Add as much as you can**. **No performance penalties, almost**.

## How does it do it?
By being lazy and creative. Instead of passing a formatted string to a logger...
Wait! "passing a formatted string to a logger" - so you formatted it before passing to the logger?
So, you did:
```csharp
Logger.Debug($"Detected a new bluetooth device in range: {device.GetInfo()}");
```
Logger in production most likely is configured at Info level, so it will not log that string you passed.
While string formatting is fast in .net 5, but you had called device.GetInfo() that may be slow.
You'd like it to be called only if logging level is set to debug when troubleshooting some problems,
but not in "all is fine Sun is shinning" mode.

So, instead of passing a formatted string you may pass an [magic object](./src/NLazyToString/LazyToString.cs) to the Logger.Debug(...).

So do like this, instead:
```csharp
using static NLazyToString.LazyToString;
// ...
Logger.Debug(LazyString(() => $"Detected a new bluetooth device in range: {device.GetInfo()}"));
```
Not so much different, but ```device.GetInfo()``` will get called only if needed, in debug log mode, 0.0001%.

## Performance when string formatting is not needed (most often)
More than 5 times faster in simple cases:
```
Testing savings when interpolation is not needed...

2022-04-05 22:06:25.7814|INFO|NLazyToStringNLogConsole.Program|Interpolate loop start
2022-04-05 22:06:25.8928|INFO|NLazyToStringNLogConsole.Program|Interpolate loop end, took 00:00:00.1123801
2022-04-05 22:06:25.8928|INFO|NLazyToStringNLogConsole.Program|string.Format loop start
2022-04-05 22:06:25.9892|INFO|NLazyToStringNLogConsole.Program|string.Format loop end, took 00:00:00.0959689
2022-04-05 22:06:25.9892|INFO|NLazyToStringNLogConsole.Program|LazyToString loop start
2022-04-05 22:06:25.9892|INFO|NLazyToStringNLogConsole.Program|LazyToString loop end, took 00:00:00.0109420

Ticks: interpolate=1122287, format=959684, LazyString=109419

LazyString was faster than interpolate 10.2567835567863 times.

LazyString was faster than format 8.77072537676272 times.
```

## Overhead when string formatting is needed, indeed (in rare cases)
Less than 20% (1/5) overhead:
```
Testing overhead when interpolation is needed...

2022-04-05 22:06:25.9892|INFO|NLazyToStringNLogConsole.Program|Interpolate loop start
2022-04-05 22:06:26.1018|INFO|NLazyToStringNLogConsole.Program|Interpolate loop end, took 00:00:00.0983547
2022-04-05 22:06:26.1018|INFO|NLazyToStringNLogConsole.Program|string.Format loop start
2022-04-05 22:06:26.1971|INFO|NLazyToStringNLogConsole.Program|string.Format loop end, took 00:00:00.0951963
2022-04-05 22:06:26.1971|INFO|NLazyToStringNLogConsole.Program|LazyToString loop start
2022-04-05 22:06:26.3051|INFO|NLazyToStringNLogConsole.Program|LazyToString loop end, took 00:00:00.1078384

Ticks: interpolate=983542, format=951960, LazyString=1078376

LazyString was slower than interpolate 1.0964208950914145 times.

LazyString was slower than format 1.1327954956090591 times.
```

## ...Some thoughts about future
Built-in support in loggers for this concept.

Then async support in loggers is easy to implement. (await Logger.DebugAsync()).

await string.FormatAsync("...{device.GetInfoAsync()}...) from MSFT.
