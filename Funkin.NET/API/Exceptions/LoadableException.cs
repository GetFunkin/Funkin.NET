using System;
using System.Reflection;

namespace Funkin.NET.API.Exceptions
{
    public class LoadableException : Exception
    {
        public LoadableException(MemberInfo type) : base(
            $"Attempted to load loadable type \"{type.Name}\", but the activated type failed to cast!"
        )
        {

        }
    }
}