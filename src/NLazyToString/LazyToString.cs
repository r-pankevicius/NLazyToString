// Used under MIT License, see https://github.com/r-pankevicius/NLazyToString

using System;

namespace NLazyToString
{
    /// <summary>
    /// Encapsulates a function pointer that yields a string when <see cref="ToString()"/> will be called (and only then, not before).
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

        /// <summary>
        /// Framework metod, you sould know it.
        /// </summary>
        /// <returns>Formatted string, whatever the funcion you passed to the constructor returns.</returns>
        public override string ToString()
        {
            if (_toStringFunc is null)
                return $"PROGRAMMER'S ERROR: passed null function to {nameof(LazyToString)} constructor.";

            string functionResult = _toStringFunc();

            // Convenience: null is formatted as the empty string in string.Format() and interpolation
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
