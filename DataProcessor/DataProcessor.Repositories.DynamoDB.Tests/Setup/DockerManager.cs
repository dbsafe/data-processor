using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataProcessor.Repositories.DynamoDB.Tests.Setup
{
    public class DockerManager
    {
        private const string CONTAINER_IMAGE_URI = "amazon/dynamodb-local";
        private const string DYNAMODB_PORT = "8000";

        private readonly DockerClient _dockerClient;
        private string _containerId;

        public int DynamoDBHostPort { get; }

        public DockerManager()
        {
            _dockerClient = CreateDockerClient();
            PullImage();
            DynamoDBHostPort = TcpHelper.GetFreeTcpPort();
        }

        public void StopContainer()
        {
            if (_containerId != null)
            {
                var task = Task.Run(async () =>
                {
                    await _dockerClient.Containers.KillContainerAsync(_containerId, new ContainerKillParameters());
                });

                task.Wait();
            }
        }

        public void StartContainer(string dynamoDBHostPort)
        {
            var parameters = new CreateContainerParameters
            {
                Image = CONTAINER_IMAGE_URI,
                ExposedPorts = new Dictionary<string, EmptyStruct>
                {
                    { DYNAMODB_PORT, default }
                },
                HostConfig = new HostConfig
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>
                    {
                        { DYNAMODB_PORT, new List<PortBinding>{ new PortBinding { HostPort = dynamoDBHostPort } } }
                    },
                    PublishAllPorts = false
                }
            };

            var task = Task.Run(async () =>
            {
                var response = await _dockerClient.Containers.CreateContainerAsync(parameters);
                _containerId = response.ID;
                await _dockerClient.Containers.StartContainerAsync(_containerId, null);
            });

            task.Wait();
        }

        private void PullImage()
        {
            var task = Task.Run(async () =>
            {
                var parameters = new ImagesCreateParameters
                {
                    FromImage = CONTAINER_IMAGE_URI,
                    Tag = "latest"
                };

                await _dockerClient.Images.CreateImageAsync(parameters, new AuthConfig(), new Progress<JSONMessage>());
            });

            task.Wait();
        }

        private static DockerClient CreateDockerClient()
        {
            var endpoint = new Uri("npipe://./pipe/docker_engine");
            var dockerClientConfiguration = new DockerClientConfiguration(endpoint);
            return dockerClientConfiguration.CreateClient();
        }
    }
}
