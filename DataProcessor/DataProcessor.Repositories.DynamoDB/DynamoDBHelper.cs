using Amazon.DynamoDBv2;
using Amazon.Runtime;
using System;

namespace DataProcessor.Repositories.DynamoDB
{
    public static class DynamoDBHelper
    {
        public static AmazonDynamoDBClient CreateAmazonDynamoDBClient(string profileName = null)
        {
            if (string.IsNullOrEmpty(profileName))
            {
                return new AmazonDynamoDBClient();
            }
            else
            {
                var chain = new Amazon.Runtime.CredentialManagement.CredentialProfileStoreChain();
                if (chain.TryGetAWSCredentials(profileName, out AWSCredentials awsCredentials))
                {
                    return new AmazonDynamoDBClient(awsCredentials);
                }

                throw new Exception($"Failed to get credentials form profile '{profileName}'");
            }
        }
    }
}
