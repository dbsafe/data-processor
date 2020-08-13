using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using DataProcessor.Domain.Contracts;
using DataProcessor.Repositories.DynamoDB.Tests.Setup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

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
            // _dbClient = DynamoDBHelper.CreateAmazonDynamoDBClient("DataProcessor");
            var clientConfig = new AmazonDynamoDBConfig { ServiceURL = TestSetup.DynamoDBUrl };
            _dbClient = new AmazonDynamoDBClient(clientConfig);
        }

        [TestMethod]
        public void InitializeFileAsync()
        {
            var request = new InitializeFileRequest
            {
                DateTime = DateTime.Parse("2020-01-02T03:04Z"),
                SizeBytes = 1024,
                Path = "path-2"
            };

            var target = new DynamoDBDataRepository(_dbClient);

            var actual = target.InitializeFileAsync(request).Result;
            
            TestContext.WriteLine($"FileId: {actual.FileId}");

            var items = ReadAllItems("DP_File");

            Assert.AreEqual(1, items.Count);
            
            var item = items[0];
            Assert.AreEqual("path-2", item["Path"].AsString());
            Assert.AreEqual(FileStatusType.Importing.ToString(), item["Status"].AsString());
            Assert.AreEqual(actual.FileId.ToString(), item["FileId"].AsString());
            Assert.AreEqual(1024, item["SizeBytes"].AsInt());
            Assert.AreEqual("2020-01-02T03:04:00.000Z", item["DateTime"].AsString());
        }

        private List<Document> ReadAllItems(string tableName)
        {
            var table = Table.LoadTable(_dbClient, tableName);
            var scanOperationConfig = new ScanOperationConfig();
            return table.Scan(scanOperationConfig).GetRemainingAsync().Result;
        }
    }
}
