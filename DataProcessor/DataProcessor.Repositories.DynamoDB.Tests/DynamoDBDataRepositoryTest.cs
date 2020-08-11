using Amazon.DynamoDBv2;
using DataProcessor.Domain.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DataProcessor.Repositories.DynamoDB.Tests
{
    [TestClass]
    public class DynamoDBDataRepositoryTest
    {
        private AmazonDynamoDBClient _dbClient;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            _dbClient = DynamoDBHelper.CreateAmazonDynamoDBClient("DataProcessor");
        }

        [TestMethod]
        public void InitializeFileAsync()
        {
            var request = new InitializeFileRequest
            {
                DateTime = DateTime.Parse("2020-01-02T03:04"),
                SizeBytes = 1024,
                Status = FileStatusType.Importing,
                Path = "path-1"
            };

            var target = new DynamoDBDataRepository(_dbClient);
            
            var actual = target.InitializeFileAsync(request).Result;

            TestContext.WriteLine($"FileId: {actual.FileId}");
        }
    }
}
