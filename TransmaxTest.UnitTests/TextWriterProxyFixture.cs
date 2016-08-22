namespace TransmaxTest.UnitTests
{
    using System;
    using System.IO;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    internal sealed class TextWriterProxyFixture
    {
        private Mock<TextWriter> baseWriter;
        private Mock<TextWriter> auxiliaryWriter;
        [SetUp]
        public void SetUp()
        {
            baseWriter = new Mock<TextWriter>(MockBehavior.Strict);
            auxiliaryWriter = new Mock<TextWriter>(MockBehavior.Strict);
        }

        [Test]
        public void ShouldConstructInstance()
        {
            // Given
            var instance = default(TextWriterProxy);

            // When
            Action action = () => instance = CreateInstance();

            // Then
            action.ShouldNotThrow();
            instance.Should().NotBeNull();
        }

        [Test]
        public void ShouldNotConstructWithNullBaseWriter()
        {
            // Given
            var instance = default(TextWriterProxy);

            // When
            Action action = () => instance = new TextWriterProxy(null, auxiliaryWriter.Object);

            // Then
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("baseWriter");
            instance.Should().BeNull();
        }

        [Test]
        public void ShouldNotConstructWithNullAuxiliaryWriter()
        {
            // Given
            var instance = default(TextWriterProxy);

            // When
            Action action = () => instance = new TextWriterProxy(baseWriter.Object, null);

            // Then
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("auxiliaryWriter");
            instance.Should().BeNull();
        }

        [Test]
        public void ShouldWriteLineIntoBoth([Values(null, "", " ", "who wants pizza?")] string text)
        {
            // Given
            var instance = CreateInstance();
            baseWriter.Setup(x => x.Flush());
            baseWriter.Setup(x => x.WriteLine(text));
            auxiliaryWriter.Setup(x => x.WriteLine(text));

            // When
            instance.WriteLine(text);

            // Then
            baseWriter.Verify(x => x.Flush(), Times.Once);
            baseWriter.Verify(x => x.WriteLine(text), Times.Once);
            auxiliaryWriter.Verify(x => x.WriteLine(text), Times.Once);
        }
        
        private TextWriterProxy CreateInstance()
        {
            return new TextWriterProxy(baseWriter.Object, auxiliaryWriter.Object);
        }
    }
}
