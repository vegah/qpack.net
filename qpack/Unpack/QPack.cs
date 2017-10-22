using System;
using System.Collections.Generic;
using System.Linq;

namespace Fantasista
{
    public  static partial class QPack
    {
        public static object Unpack(IEnumerable<byte> bytes)
        {
            var first = bytes.First();
            if (first<=0x7b || (first>=0xe8 && first<=0xeb))
                return UnpackInt(first, bytes); 
            return null;
        }

        private static object UnpackInt(byte first, IEnumerable<byte> bytes)
        {
            if (first<=0x3f)
                return (sbyte)first;
            else if (first<0x7b)
                return (sbyte)63-first;
            else if (first==0xe8)
                return (byte)bytes.ToArray()[1];
            else if (first==0xe9)
                return (short)BitConverter.ToInt16(bytes.ToArray(),1);
            else if (first==0xea)
                return (int)BitConverter.ToInt32(bytes.ToArray(),1);
            else if (first==0xeb)
                return (long)BitConverter.ToInt64(bytes.ToArray(),1);
            throw new QUnpackException(first);
        }
    }
}