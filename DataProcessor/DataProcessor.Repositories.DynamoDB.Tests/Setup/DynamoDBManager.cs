using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DataProcessor.Repositories.DynamoDB.Tests.Setup
{
    public class DynamoDBManager
    {
        private readonly AmazonDynamoDBClient _dbClient;

        public DynamoDBManager(AmazonDynamoDBClient dbClient)
        {
            _dbClient = dbClient;
        }

        public void CreateTableFromJsonFile(string path)
        {
            var json = File.ReadAllText(path);
            var createTableRequest = JsonConvert.DeserializeObject<CreateTableRequest>(json);

            var task = Task.Run(async () =>
            {
                await _dbClient.CreateTableAsync(createTableRequest);
            });

            task.Wait();
        }

        public bool WaitForTableToBeActive(string tableName, TimeSpan timeout)
        {
            var tokenSource = new CancellationTokenSource(timeout);
            var token = tokenSource.Token;
            var task = Task.Run(async () =>
            {
                try
                {
                    await WaitForTableToExistsAsync(tableName, token);
                    await WaitForTableToBeActiveAsync(tableName, token);
                    return true;
                }
                catch (OperationCanceledException)
                {
                    return false;
                }
            });

            return task.Result;
        }

        private async Task WaitForTableToBeActiveAsync(string tableName, CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }

                var describeTableResponse = await _dbClient.DescribeTableAsync(tableName);
                if (describeTableResponse.Table.TableStatus == TableStatus.ACTIVE)
                {
                    return;
                }
                else
                {
                    await Task.Delay(1000, cancellationToken);
                }
            }
        }

        private async Task WaitForTableToExistsAsync(string tableName, CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }

                var listTablesResponse = await _dbClient.ListTablesAsync();
                if (listTablesResponse.TableNames.Contains(tableName))
                {
                    return;
                }
                else
                {
                    await Task.Delay(1000, cancellationToken);
                }
            }
        }
    }
}
