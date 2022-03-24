# NLazyToString
Lazy evaluated ToString() string formatting for use in logging when you don't know in advance if you need to format that string at all.

## Performance when string formatting is not needed
About 10 times faster in simple cases:
```
2022-03-24 22:21:20.0647|INFO|NLazyToStringNLogConsole.Program|Interpolate loop start
2022-03-24 22:21:20.1837|INFO|NLazyToStringNLogConsole.Program|Interpolate loop end, took 00:00:00.1198852
2022-03-24 22:21:20.1837|INFO|NLazyToStringNLogConsole.Program|string.Format loop start
2022-03-24 22:21:20.2881|INFO|NLazyToStringNLogConsole.Program|string.Format loop end, took 00:00:00.1042675
2022-03-24 22:21:20.2881|INFO|NLazyToStringNLogConsole.Program|LazyToString loop start
2022-03-24 22:21:20.3032|INFO|NLazyToStringNLogConsole.Program|LazyToString loop end, took 00:00:00.0147171
```

## Overhead when string formatting is needed, indeed.
```
TODO
```

