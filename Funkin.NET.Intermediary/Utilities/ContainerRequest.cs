namespace Funkin.NET.Intermediary.Utilities
{
    public readonly struct ContainerRequest
    {
        public string ContainerName { get; }

        public ContainerRequest(string containerName)
        {
            ContainerName = containerName;
        }
    }
}