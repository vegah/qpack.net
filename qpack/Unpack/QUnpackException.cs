using System;
using System.Runtime.Serialization;

namespace Fantasista
{
    [Serializable]
    internal class QUnpackException : Exception
    {
        public QUnpackException(byte header) : base($"Uknown type 0x{header.ToString("X")}")
        {
        }

    }
}