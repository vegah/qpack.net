using System;
using System.Runtime.Serialization;

namespace Fantasista
{
    [Serializable]
    internal class UnknownQpackObjectTypeException : Exception
    {
 
        public UnknownQpackObjectTypeException(Type type) : base($"Can not unpack type {type.Name}")
        {
        }

    }
}