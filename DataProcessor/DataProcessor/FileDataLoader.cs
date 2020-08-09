using DataProcessor.DataSource.File;
using DataProcessor.Domain.Contracts;
using DataProcessor.Domain.Models;
using DataProcessor.InputDefinitionFile;
using DataProcessor.ProcessorDefinition;
using DataProcessor.ProcessorDefinition.Models;
using System;

namespace DataProcessor
{
    public class FileDataLoaderConfig
    {
        public string Delimiter { get; set; }
        public bool HasFieldsEnclosedInQuotes { get; set; }
    }

    public class FileDataLoader
    {
        private readonly IDataRepository _dataRepository;
        private readonly FileDataLoaderConfig _config;

        public FileDataLoader(FileDataLoaderConfig config, IDataRepository dataRepository)
        {
            _config = config;
            _dataRepository = dataRepository;
        }

        public bool Load(string dataFilePath, string fileSpecPath)
        {
            var fileId = InitializeFile(dataFilePath);

            ParsedData parsedData;
            try
            {
                parsedData = ParseFileData(dataFilePath, fileSpecPath);
            }
            catch (Exception ex)
            {
                var setFileStatusWithFileLoadErrorRequest = new SetFileStatusWithFileLoadErrorRequest
                {
                    FileId = fileId,
                    Error = $"Error Loading and Parsing file. {ex}"
                };

                _dataRepository.SetFileStatusWithWileLoadError(setFileStatusWithFileLoadErrorRequest);
                return false;
            }

            InsertHeader(parsedData, fileId);
            InsertData(parsedData, fileId);
            InsertTrailer(parsedData, fileId);

            var setFileStatusWithFileLoadedCompletedRequest = new SetFileStatusWithFileLoadedCompletedRequest
            {
                FileId = fileId,
                Errors = parsedData.Errors,
                ValidationResult = parsedData.ValidationResult
            };

            _dataRepository.SetFileStatusWithFileLoadedCompleted(setFileStatusWithFileLoadedCompletedRequest);
            return true;
        }

        private void InsertTrailer(ParsedData parsedData, Guid fileId)
        {
            if (parsedData.Trailer != null)
            {
                _dataRepository.InsertTrailer(CreateInsertRowRequest(parsedData.Trailer, fileId));
            }
        }

        private void InsertHeader(ParsedData parsedData, Guid fileId)
        {
            if (parsedData.Header != null)
            {
                _dataRepository.InsertHeader(CreateInsertRowRequest(parsedData.Header, fileId));
            }
        }

        private InsertRowRequest CreateInsertRowRequest(Row row, Guid fileId)
        {
            return new InsertRowRequest
            {
                FileId = fileId,
                ValidationResult = row.ValidationResult,
                Raw = row.Raw,
                Decoded = row.Json,
                Errors = row.Errors
            };
        }

        private void InsertData(ParsedData parsedData, Guid fileId)
        {
            foreach(var dataRow in parsedData.DataRows)
            {
                _dataRepository.InsertData(CreateInsertRowRequest(dataRow, fileId));
            }
        }

        private Guid InitializeFile(string dataFilePath)
        {
            var initializeFileRequest = new InitializeFileRequest
            {
                Path = dataFilePath
            };

            var initializeFileResult = _dataRepository.InitializeFile(initializeFileRequest);
            return initializeFileResult.FileId;
        }

        private ParsedData ParseFileData(string dataFilePath, string fileSpecPath)
        {
            var fileDataSource = CreateDataSource(dataFilePath);
            var fileProcessorDefinition = CreateFileProcessorDefinition(fileSpecPath);
            var parsedDataProcessor = new ParsedDataProcessor(fileDataSource, fileProcessorDefinition);

            var parsedData = parsedDataProcessor.Process();
            return parsedData;
        }

        private IDataSource CreateDataSource(string dataFilePath)
        {
            var fileDataSourceConfig = new FileDataSourceConfig
            {
                Delimiter = _config.Delimiter,
                HasFieldsEnclosedInQuotes = _config.HasFieldsEnclosedInQuotes,
                Path = dataFilePath
            };

            return new FileDataSource(fileDataSourceConfig);
        }

        private FileProcessorDefinition CreateFileProcessorDefinition(string fileSpecPath)
        {
            var inputDefinitionFile = FileLoader.Load<InputDefinitionFile_10>(fileSpecPath);
            return FileProcessorDefinitionBuilder.CreateFileProcessorDefinition(inputDefinitionFile);
        }
    }
}
