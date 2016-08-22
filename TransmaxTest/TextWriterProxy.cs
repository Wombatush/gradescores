namespace TransmaxTest
{
    using System;
    using System.IO;
    using System.Text;
    using JetBrains.Annotations;

    internal sealed class TextWriterProxy : TextWriter
    {
        private readonly TextWriter baseWriter;
        private readonly TextWriter auxiliaryWriter;

        /// <summary>
        /// Initializes a new instance of <see cref="TextWriterProxy"/>.
        /// </summary>
        /// <param name="baseWriter">Base text writer instance.</param>
        /// <param name="auxiliaryWriter">Auxiliart text writer instance.</param>
        /// <exception cref="ArgumentNullException">Parameter <paramref name="baseWriter"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Parameter <paramref name="auxiliaryWriter"/> is null.</exception>
        public TextWriterProxy([NotNull] TextWriter baseWriter, [NotNull] TextWriter auxiliaryWriter)
        {
            if (baseWriter == null)
            {
                throw new ArgumentNullException(nameof(baseWriter));
            }

            if (auxiliaryWriter == null)
            {
                throw new ArgumentNullException(nameof(auxiliaryWriter));
            }

            this.baseWriter = baseWriter;
            this.auxiliaryWriter = auxiliaryWriter;
        }

        /// <summary>
        /// When overridden in a derived class, returns the character encoding in which the output is written.
        /// </summary>
        /// <returns>The character encoding in which the output is written.</returns>
        public override Encoding Encoding => baseWriter.Encoding;

        /// <summary>
        /// Writes a string followed by a line terminator to the text string or stream.
        /// </summary>
        /// <param name="value">The string to write. If <paramref name="value"/> is null, only the line terminator is written. </param>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter"/> is closed. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override void WriteLine(string value)
        {
            baseWriter.WriteLine(value);
            auxiliaryWriter.WriteLine(value);
            baseWriter.Flush();
        }
    }
}
