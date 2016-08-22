namespace TransmaxTest.UnitTests
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    internal sealed class StringScoreReaderFixture
    {
        [Test]
        public void ShouldHaveExpectedSeparator()
        {
            // Then
            StringScoreReader.ScoreSeparator.Should().Be(",");
        }

        [Test]
        public void ShouldConstructInstance()
        {
            // Given
            var instance = default(StringScoreReader);

            // When
            Action action = () => instance = CreateInstance();

            // Then
            action.ShouldNotThrow();
            instance.Should().NotBeNull();
        }

        [Test]
        [TestCase("-1")]
        [TestCase("0")]
        [TestCase("1")]
        [TestCase("SPECIFIC, TO, CULTURE, 9 223 372 036 854 775 807")]
        [TestCase("WRONG, MINUS, - 9223372036854775808")]
        [TestCase("LONG, OVER MAXIMUM, 9223372036854775808")]
        [TestCase("LONG, UNDER MINIMUM, -9223372036854775809")]
        [TestCase("FLOATING POINT, RATIONAL, 0.1")]
        [TestCase("NUMBER AS TEXT, TEN")]
        [TestCase("NOTHING,")]
        public void ShouldTryReadScoreFromInvalidSource(string source)
        {
            // Given
            var instance = CreateInstance();

            // When
            long score;
            bool result = instance.TryReadScore(source, out score);

            // Then
            result.Should().BeFalse();
            score.Should().Be(default(long));
        }

        [Test]
        [TestCase("LONG, ZERO, 000", 0L)]
        [TestCase("LONG, TEN, 010", 10L)]
        [TestCase("LONG, MAXIMUM, 9223372036854775807", long.MaxValue)]
        [TestCase("LONG, MINIMUM, -9223372036854775808", long.MinValue)]
        [TestCase("9,223,372,036,854,775,807", 807)]
        [TestCase("WHITE SPACE, BEFORE AND AFTER,\t123456789 ", 123456789L)]
        [TestCase("FLOATING POINT, INTEGER, 1.0", 1L)]
        public void ShouldTryReadScoreFromValidSource(string source, long expected)
        {
            // Given
            var instance = CreateInstance();

            // When
            long score;
            bool result = instance.TryReadScore(source, out score);

            // Then
            result.Should().BeTrue();
            score.Should().Be(expected);
        }

        private StringScoreReader CreateInstance()
        {
            return new StringScoreReader();
        }
    }
}
