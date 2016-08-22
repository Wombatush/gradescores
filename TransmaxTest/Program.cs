namespace TransmaxTest
{
    using System;
    using System.IO;

    class Program
    {
        private const int ErrCodeSuccess = 0;
        private const int ErrCodeInputFileNameMissing = 1;
        private const int ErrCodeInputFileNameInvalidChars = 2;
        private const int ErrCodeInputFileDoesNotExist = 3;
        private const int ErrCodeInputParsingFailed = 4;

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                ExitWithErrorCode("Input file name is missing", ErrCodeInputFileNameMissing);
            }

            var inputFile = args[0];

            if (inputFile.IndexOfAny(Path.GetInvalidPathChars()) != -1)
            {
                ExitWithErrorCode("Input file name contains invalid path characters", ErrCodeInputFileNameInvalidChars);
            }

            var inputPath = Path.GetFullPath(inputFile);
            if (!File.Exists(inputPath))
            {
                ExitWithErrorCode($"Input file does not exist: {inputPath}", ErrCodeInputFileDoesNotExist);
            }

            var outputPath = Path.GetFileNameWithoutExtension(inputPath) + "-graded.txt";
            if (File.Exists(outputPath))
            {
                Console.WriteLine($"Output file exist and will be overwritten: {outputPath}");
            }

            var outputFile = Path.GetFileName(outputPath);

            var scoreReader = new StringScoreReader();
            var orderByScoreService = new OrderByScoreService(scoreReader);
            
            using (var inputStream = File.OpenRead(inputPath))
            using (var outputStream = File.OpenWrite(outputPath))
            using (var inputReader = new StreamReader(inputStream))
            using (var outputWriter = new StreamWriter(outputStream, inputReader.CurrentEncoding))
            using (var writerProxy = new TextWriterProxy(outputWriter, Console.Out))
            {
                if (orderByScoreService.OrderByScore(inputReader, writerProxy))
                {
                    ExitWithErrorCode($"Finished: created {outputFile}", ErrCodeSuccess);
                }
                else
                {
                    ExitWithErrorCode($"Cannot parse {outputFile}", ErrCodeInputParsingFailed);
                }
            }
        }

        static void ExitWithErrorCode(string errorMessage, int errorCode)
        {
            Console.WriteLine(errorMessage);
            Environment.Exit(errorCode);
        }
    }
}
