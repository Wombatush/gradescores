namespace TransmaxTest
{
    using System;
    using System.IO;
    using JetBrains.Annotations;

    /// <summary>
    /// Order by score service interface.
    /// </summary>
    internal interface IOrderByScoreService
    {
        /// <summary>
        /// Orders entries by their scores in descending order from the given <paramref name="reader"/> into given <paramref name="writer"/>.
        /// </summary>
        /// <param name="reader">Text reader used to read data.</param>
        /// <param name="writer">Text writer used to write data.</param>
        /// <returns>true if succeeded; false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Parameter <paramref name="reader"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Parameter <paramref name="writer"/> is null.</exception>
        [ContractAnnotation("reader:null=>halt; writer:null=>halt;")]
        bool OrderByScore([NotNull] TextReader reader, [NotNull] TextWriter writer);
    }
}