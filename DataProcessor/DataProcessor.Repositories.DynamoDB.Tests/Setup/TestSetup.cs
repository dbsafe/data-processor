using Amazon.DynamoDBv2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;

namespace DataProcessor.Repositories.DynamoDB.Tests.Setup
{
    [TestClass]
    public class TestSetup
    {
        private static readonly DockerManager _dockerManager = new DockerManager();
        private static TestContext _testContext;
        private static string _assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static string DynamoDBUrl { get; private set; } = $"http://localhost:{_dockerManager.DynamoDBHostPort}";

        [AssemblyInitialize]
        public static void Initialize(TestContext testContext)
        {
            _testContext = testContext;
            Print($"DynamoDBUrl: {DynamoDBUrl}");
            Print("Starting container");
            _dockerManager.StartContainer(_dockerManager.DynamoDBHostPort.ToString());
            CreateTables();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            
            Print("Stopping container");
            _dockerManager.StopContainer();
        }

        private static void Print(string message)
        {
            _testContext.WriteLine(message);
        }

        private static void CreateTables()
        {
            Print("Creating tables");
            var clientConfig = new AmazonDynamoDBConfig { ServiceURL = DynamoDBUrl };
            var dbClient = new AmazonDynamoDBClient(clientConfig);
            var dynamoDBManager = new DynamoDBManager(dbClient);

            string tableName;
            string tableFilename;

            tableName = "DP_File";
            tableFilename = "Table_DP_File.json";
            Print($"Creating table '{tableName}'");
            dynamoDBManager.CreateTableFromJsonFile(Path.Combine(_assemblyFolder, "AWS", tableFilename));
            
            var isTableActive = dynamoDBManager.WaitForTableToBeActive(tableName, TimeSpan.FromMilliseconds(1000));
            if (!isTableActive)
            {
                throw new Exception($"Table '{tableName}' is not active");
            }
        }
    }
}
