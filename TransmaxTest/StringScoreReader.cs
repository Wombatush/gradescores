namespace TransmaxTest
{
    using System;
    using System.Globalization;

    /// <summary>
    /// String score reader implementation.
    /// </summary>
    internal sealed class StringScoreReader : IStringScoreReader
    {
        /// <summary>
        /// Gets score separator.
        /// </summary>
        public static string ScoreSeparator { get; } = ",";

        /// <summary>
        /// Tries to read <paramref name="score"/> value from the given <paramref name="source"/> string.
        /// </summary>
        /// <param name="source">An input source string.</param>
        /// <param name="score">An output score value, if succeeded; default value otherwise.</param>
        /// <returns>true if succeeded; false otherwise.</returns>
        public bool TryReadScore(string source, out long score)
        {
            if (source == null)
            {
                score = default(long);
                return false;
            }

            var lastIndexOfSeparator = source.LastIndexOf(ScoreSeparator, StringComparison.Ordinal);
            if (lastIndexOfSeparator < 0)
            {
                score = default(long);
                return false;
            }

            var scoreStr = source.Substring(lastIndexOfSeparator + 1);

            return long.TryParse(scoreStr, NumberStyles.Any, CultureInfo.InvariantCulture, out score);
        }
    }
}
