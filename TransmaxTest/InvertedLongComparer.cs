namespace TransmaxTest
{
    using System.Collections.Generic;

    /// <summary>
    /// Inverted long comparer, which can be used to sort values of types <see cref="long"/> in descending order.
    /// </summary>
    internal sealed class InvertedLongComparer : IComparer<long>
    {
        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>.
        /// Less than zero - <paramref name="x"/> is greater than <paramref name="y"/>.
        /// Zero - <paramref name="x"/> equals <paramref name="y"/>.
        /// Greater than zero - <paramref name="x"/> is less than <paramref name="y"/>.
        /// </returns>
        public int Compare(long x, long y) => -x.CompareTo(y);
    }
}