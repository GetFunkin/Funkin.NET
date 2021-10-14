using System;

namespace Funkin.NET.Properties
{
    public static class AssemblyAttributes
    {

        [AttributeUsage(AttributeTargets.Assembly)]
        public sealed class AssemblyConfigurationAttribute : Attribute
        {
            public string Configuration { get; }

            public string Platform { get; }

            public AssemblyConfigurationAttribute(string configuration, string platform)
            {
                Configuration = configuration;
                Platform = platform;
            }
        }
    }
}