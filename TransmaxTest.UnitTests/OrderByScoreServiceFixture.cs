namespace TransmaxTest.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    internal sealed class OrderByScoreServiceFixture
    {
        private Mock<IStringScoreReader> scoreReader;
        private Mock<TextReader> reader;
        private Mock<TextWriter> writer;

        [SetUp]
        public void SetUp()
        {
            scoreReader = new Mock<IStringScoreReader>(MockBehavior.Strict);
            reader = new Mock<TextReader>(MockBehavior.Strict);
            writer = new Mock<TextWriter>(MockBehavior.Strict);
        }

        [Test]
        public void ShouldConstructInstance()
        {
            // Given
            var instance = default(OrderByScoreService);

            // When
            Action action = () => instance = CreateInstance();

            // Then
            action.ShouldNotThrow();
            instance.Should().NotBeNull();
        }

        [Test]
        public void ShouldNotConstructWithNullScoreReader()
        {
            // Given
            var instance = default(OrderByScoreService);

            // When
            Action action = () => instance = new OrderByScoreService(default(IStringScoreReader));

            // Then
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("scoreReader");
            instance.Should().BeNull();
        }

        [Test]
        public void ShouldNotOrderByScoreWhenReaderIsNull()
        {
            // Given
            var instance = CreateInstance();

            // When
            Action action = () => instance.OrderByScore(null, writer.Object);

            // Then
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("reader");
        }

        [Test]
        public void ShouldNotOrderByScoreWhenWriterIsNull()
        {
            // Given
            var instance = CreateInstance();

            // When
            Action action = () => instance.OrderByScore(reader.Object, null);

            // Then
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("writer");
        }

        [Test]
        public void ShouldOrderByScoreEmptyFile([Values(null, "")] string empty)
        {
            // Given
            var instance = CreateInstance();
            reader.Setup(x => x.ReadLine()).Returns(empty);
            
            // When
            var result = instance.OrderByScore(reader.Object, writer.Object);

            // Then
            result.Should().BeTrue();
            reader.Verify(x => x.ReadLine(), Times.Once);
        }

        [Test]
        public void ShouldUseScoreReaderAndItsResult()
        {
            // Given
            var lines = new Queue<string>();
            var results = new Dictionary<string, bool>();
            var instance = CreateInstance();
            lines.Enqueue("A");
            lines.Enqueue("B");
            results.Add("A", true);
            results.Add("B", false);
            reader.Setup(x => x.ReadLine()).Returns(() => lines.Dequeue());
            long score;
            scoreReader.Setup(x => x.TryReadScore(It.IsAny<string>(), out score)).Returns<string, long>((source, _) => results[source]);

            // When
            var result = instance.OrderByScore(reader.Object, writer.Object);

            // Then
            result.Should().BeFalse();
            reader.Verify(x => x.ReadLine(), Times.Exactly(2));
            scoreReader.Verify(x => x.TryReadScore(It.IsAny<string>(), out score), Times.Exactly(2));
            scoreReader.Verify(x => x.TryReadScore("A", out score), Times.Once);
            scoreReader.Verify(x => x.TryReadScore("B", out score), Times.Once);
        }

        [Test]
        public void ShouldSortByScoreDescendingAndByNameAscending()
        {
            // Given
            var scoreForBundyTeressa = 88L;
            var scoreForSmithAllan = 70L;
            var scoreForKingMadison = 88L;
            var scoreForSmithFrancis = 85L;
            var textForBundyTeressa = $"BUNDY, TERESSA, {scoreForBundyTeressa}";
            var textForSmithAllan = $"SMITH, ALLAN, {scoreForSmithAllan}";
            var textForKingMadison =  $"KING, MADISON, {scoreForKingMadison}";
            var textForSmithFrancis = $"SMITH, FRANCIS, {scoreForSmithFrancis}";

            var input = new Queue<string>();
            var output = new List<string>();
            var instance = CreateInstance();
            input.Enqueue(textForBundyTeressa);
            input.Enqueue(textForSmithAllan);
            input.Enqueue(textForKingMadison);
            input.Enqueue(textForSmithFrancis);
            reader.Setup(x => x.ReadLine()).Returns(() => input.Count > 0 ? input.Dequeue() : default(string));
            writer.Setup(x => x.WriteLine(It.IsAny<string>())).Callback<string>(x => output.Add(x));
            scoreReader.Setup(x => x.TryReadScore(textForBundyTeressa, out scoreForBundyTeressa)).Returns(true);
            scoreReader.Setup(x => x.TryReadScore(textForSmithAllan, out scoreForSmithAllan)).Returns(true);
            scoreReader.Setup(x => x.TryReadScore(textForKingMadison, out scoreForKingMadison)).Returns(true);
            scoreReader.Setup(x => x.TryReadScore(textForSmithFrancis, out scoreForSmithFrancis)).Returns(true);

            // When
            var result = instance.OrderByScore(reader.Object, writer.Object);

            // Then
            result.Should().BeTrue();
            reader.Verify(x => x.ReadLine(), Times.Exactly(5));
            writer.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(4));
            writer.Verify(x => x.WriteLine(textForBundyTeressa), Times.Once);
            writer.Verify(x => x.WriteLine(textForSmithAllan), Times.Once);
            writer.Verify(x => x.WriteLine(textForKingMadison), Times.Once);
            writer.Verify(x => x.WriteLine(textForSmithFrancis), Times.Once);
            scoreReader.Verify(x => x.TryReadScore(textForBundyTeressa, out scoreForBundyTeressa), Times.Once);
            scoreReader.Verify(x => x.TryReadScore(textForSmithAllan, out scoreForSmithAllan), Times.Once);
            scoreReader.Verify(x => x.TryReadScore(textForKingMadison, out scoreForKingMadison), Times.Once);
            scoreReader.Verify(x => x.TryReadScore(textForSmithFrancis, out scoreForSmithFrancis), Times.Once);
            output.Should().HaveCount(4);
            output[0].Should().Be(textForBundyTeressa);
            output[1].Should().Be(textForKingMadison);
            output[2].Should().Be(textForSmithFrancis);
            output[3].Should().Be(textForSmithAllan);
        }

        private OrderByScoreService CreateInstance()
        {
            return new OrderByScoreService(scoreReader.Object);
        }
    }
}
