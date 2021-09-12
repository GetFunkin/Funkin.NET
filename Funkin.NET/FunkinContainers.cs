using Funkin.NET.Intermediary.Utilities;

namespace Funkin.NET
{
    public static class FunkinContainers
    {
        public static readonly ContainerRequest Settings = new("Settings");
        public static readonly ContainerRequest Cursor = new("Cursor");
        public static readonly ContainerRequest Content = new("Content");
        public static readonly ContainerRequest Screen = new("Screen");
        public static readonly ContainerRequest ScalingContainer = new("ScalingContainer");
    }
}