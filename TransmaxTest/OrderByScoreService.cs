namespace TransmaxTest
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using JetBrains.Annotations;

    /// <summary>
    /// Order by score service implementation.
    /// </summary>
    internal sealed class OrderByScoreService : IOrderByScoreService
    {
        private static readonly IComparer<long> ScoreComparer = new InvertedLongComparer();

        private readonly IStringScoreReader scoreReader;

        /// <summary>
        /// Initializes a new instance of <see cref="OrderByScoreService"/>.
        /// </summary>
        /// <param name="scoreReader">Score reader instance.</param>
        /// <exception cref="ArgumentNullException">Parameter <paramref name="scoreReader"/> is null.</exception>
        public OrderByScoreService([NotNull] IStringScoreReader scoreReader)
        {
            if (scoreReader == null)
            {
                throw new ArgumentNullException(nameof(scoreReader));
            }

            this.scoreReader = scoreReader;
        }

        /// <summary>
        /// Orders entries by their scores in descending order from the given <paramref name="reader"/> into given <paramref name="writer"/>.
        /// </summary>
        /// <param name="reader">Text reader used to read data.</param>
        /// <param name="writer">Text writer used to write data.</param>
        /// <returns>true if succeeded; false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Parameter <paramref name="reader"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Parameter <paramref name="writer"/> is null.</exception>
        public bool OrderByScore(TextReader reader, TextWriter writer)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            // Step 1. Reading the entire given reader.

            // Sorted dictionary is used to group and order entries by score. 
            // Custom ScoreComparer of type InvertedLongComparer is used to provide descending sorting.
            var entries = new SortedDictionary<long, List<string>>(ScoreComparer);
            while (true)
            {
                var entry = reader.ReadLine();
                if (string.IsNullOrEmpty(entry))
                {
                    // End of stream.
                    break;
                }

                long score;
                if (!scoreReader.TryReadScore(entry, out score))
                {
                    // Data error.
                    return false;
                }

                List<string> values;
                if (!entries.TryGetValue(score, out values))
                {
                    // It is the first entry with the given score.
                    // Adding entry as a new list.
                    values = new List<string> { entry };
                    entries.Add(score, values);
                }
                else
                {
                    // There are values with the same score. 
                    // Use binary search to maintain sorting in the list.
                    var matchIndex = values.BinarySearch(entry, StringComparer.Ordinal);

                    // Transforming bitwise complement, see https://msdn.microsoft.com/en-us/library/ftfdbfx6(v=vs.110).aspx 
                    var indexForInsert = matchIndex < 0 ? ~matchIndex : matchIndex;

                    // Adding entry maintaining the sort.
                    values.Insert(indexForInsert, entry);
                }
            }

            // Step 2. Writing sorted data.

            foreach (var entry in entries.SelectMany(x => x.Value))
            {
                writer.WriteLine(entry);
            }

            return true;
        }
    }
}
