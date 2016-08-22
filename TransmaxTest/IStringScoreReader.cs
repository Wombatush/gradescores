namespace TransmaxTest
{
    using JetBrains.Annotations;

    /// <summary>
    /// String score reader interface.
    /// </summary>
    internal interface IStringScoreReader
    {
        /// <summary>
        /// Tries to read <paramref name="score"/> value from the given <paramref name="source"/> string.
        /// </summary>
        /// <param name="source">An input source string.</param>
        /// <param name="score">An output score value, if succeeded; default value otherwise.</param>
        /// <returns>true if succeeded; false otherwise.</returns>
        [ContractAnnotation("source:null=>false;")]
        bool TryReadScore([CanBeNull] string source, out long score);
    }
}