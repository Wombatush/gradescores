namespace TransmaxTest.UnitTests
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    internal sealed class InvertedLongComparerFixture
    {
        [Test]
        public void ShouldConstructInstance()
        {
            // Given
            var instance = default(InvertedLongComparer);

            // When
            Action action = () => instance = CreateInstance();

            // Then
            action.ShouldNotThrow();
            instance.Should().NotBeNull();
        }

        [Test]
        [TestCase(long.MaxValue, long.MaxValue, 0)]
        [TestCase(long.MinValue, long.MinValue, 0)]
        [TestCase(long.MaxValue, long.MinValue, -1)]
        [TestCase(long.MinValue, long.MaxValue, 1)]
        [TestCase(0L, 0L, 0)]
        [TestCase(1L, 0L, -1)]
        [TestCase(0L, 1L, 1)]
        [TestCase(1L, 1L, 0)]
        [TestCase(-1L, -1L, 0)]
        [TestCase(-1L, 1L, 1)]
        [TestCase(1L, -1L, -1)]
        [TestCase(-1L, 0L, 1)]
        public void ShouldCompareProperly(long left, long right, int expected)
        {
            // Given
            var instance = CreateInstance();

            // When
            var result = instance.Compare(left, right);

            // Then
            result.Should().Be(expected);
        }

        private static InvertedLongComparer CreateInstance()
        {
            return new InvertedLongComparer();
        }
    }
}
