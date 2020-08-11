using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using DataProcessor.Domain.Contracts;
using System;
using System.Threading.Tasks;

namespace DataProcessor.Repositories.DynamoDB
{
    public class DynamoDBDataRepositoryConfig
    {
        public string ProfileName { get; set; }
    }

    public class DynamoDBDataRepository : IDataRepository
    {
        private const string TABLE_FILE = "DP_File";
        private const string FIELD_FILE_ID = "FileId";
        private const string FIELD_FILE_PATH = "Path";
        private const string FIELD_FILE_STATUS = "Status";
        private const string FIELD_FILE_DATETIME = "DateTime";
        private const string FIELD_FILE_SIZE = "SizeBytes";

        private readonly AmazonDynamoDBClient _dbClient;

        public DynamoDBDataRepository(AmazonDynamoDBClient dbClient)
        {
            _dbClient = dbClient;
        }

        public async Task<InitializeFileResult> InitializeFileAsync(InitializeFileRequest request)
        {
            var result = new InitializeFileResult { FileId = Guid.NewGuid() };

            var table = Table.LoadTable(_dbClient, TABLE_FILE);
            var item = new Document
            {
                [FIELD_FILE_ID] = result.FileId.ToString(),
                [FIELD_FILE_PATH] = request.Path,
                [FIELD_FILE_DATETIME] = request.DateTime,
                [FIELD_FILE_STATUS] = request.Status.ToString(),
                [FIELD_FILE_SIZE] = request.SizeBytes
            };

            await table.PutItemAsync(item);

            return result;
        }

        public void InsertData(InsertRowRequest request)
        {
            throw new NotImplementedException();
        }

        public void InsertHeader(InsertRowRequest request)
        {
            throw new NotImplementedException();
        }

        public void InsertTrailer(InsertRowRequest request)
        {
            throw new NotImplementedException();
        }

        public void SetFileStatusWithFileLoadedCompleted(SetFileStatusWithFileLoadedCompletedRequest request)
        {
            throw new NotImplementedException();
        }

        public void SetFileStatusWithWileLoadError(SetFileStatusWithFileLoadErrorRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
