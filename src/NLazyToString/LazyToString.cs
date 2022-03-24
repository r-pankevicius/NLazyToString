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
        /// <exception cref="ArgumentNullException">If <paramref name="toStringFunc"/>is null.</exception>
        public LazyToString(Func<string> toStringFunc)
        {
            _toStringFunc = toStringFunc ?? throw new ArgumentNullException(nameof(toStringFunc));
        }

        /// ///<inheritdoc/>
        public override string ToString() => _toStringFunc();

        /// <summary>
        /// Convienence method.
        /// </summary>
        /// <param name="toStringFunc">Function that returns a formatted string.</param>
        /// <returns>Formatted string.</returns>
        public static LazyToString LazyString(Func<string> toStringFunc) => new(toStringFunc);
    }
}
