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

## Performance when string formatting is not needed
About 5 times faster in simple cases:
```
2022-03-25 22:38:00.2946|INFO|NLazyToStringNLogConsole.Program|Interpolate loop start
2022-03-25 22:38:00.4360|INFO|NLazyToStringNLogConsole.Program|Interpolate loop end, took 00:00:00.1423859
2022-03-25 22:38:00.4360|INFO|NLazyToStringNLogConsole.Program|string.Format loop start
2022-03-25 22:38:00.5648|INFO|NLazyToStringNLogConsole.Program|string.Format loop end, took 00:00:00.1286627
2022-03-25 22:38:00.5648|INFO|NLazyToStringNLogConsole.Program|LazyToString loop start
2022-03-25 22:38:00.5898|INFO|NLazyToStringNLogConsole.Program|LazyToString loop end, took 00:00:00.0247922
```

## Overhead when string formatting is needed, indeed.
```
TODO, expect no more that direct function call vs function pointer call (C), virtual method call in C++.
```

## ...Some thoughts about future
Built-in support in loggers for this concept.
Then async support in loggers is easy to implement. (await Logger.DebugAsync()).
await string.FormatAsync("...{await device.GetInfo()}...) from MSFT.
