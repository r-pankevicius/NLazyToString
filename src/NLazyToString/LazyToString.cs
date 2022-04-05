// Used under MIT License, see https://github.com/r-pankevicius/NLazyToString

using System;

namespace NLazyToString
{
    /// <summary>
    /// Encapsulates a function pointer that yields string when asked.
    /// </summary>
    public struct LazyToString
    {
        private readonly Func<string> _toStringFunc;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="toStringFunc">Function that returns a formatted string.</param>
        public LazyToString(Func<string> toStringFunc)
        {
            _toStringFunc = toStringFunc;
        }

        /// ///<inheritdoc/>
        public override string ToString()
        {
            if (_toStringFunc is null)
                return $"PROGRAMMER'S ERROR: passed null function to {nameof(LazyToString)} constructor.";

            string functionResult = _toStringFunc();

            // null arg is formatted as the empty string
            return functionResult ?? string.Empty;
        }

        /// <summary>
        /// Convienence method.
        /// </summary>
        /// <param name="toStringFunc">Function that returns a formatted string.</param>
        /// <returns><see cref="LazyToString"/> that will call <paramref name="toStringFunc"/>
        /// to return string when asked.</returns>
        public static LazyToString LazyString(Func<string> toStringFunc) => new(toStringFunc);
    }
}
