using System;
using System.Collections.Generic;
using System.Linq;

namespace Fantasista
{
    public  static partial class QPack
    {
        public static object Unpack(byte[] bytes) 
        {
            var info = Unpack(bytes,new UnpackInformation());
            return info.Values.Last();
        }
        private static UnpackInformation Unpack(byte[] bytes, UnpackInformation info)
        {
            var first = bytes[info.Position];

            info.Position++;
            if (first<=0x7b || (first>=0xe8 && first<=0xeb))
                UnpackInt(first, bytes,info); 
            else if (first==0xec)
                UnpackFloat(first,bytes,info);
            else if (first==0x7c || first==0xfb)
                UnpackSpecial(first,bytes,info);
            else if (first>=0x80 && first<=0xe3)
                UnpackString(first,bytes,info);
            else if (first>=0xf3 && first<=0xf8)
                UnpackMap(first,bytes,info);
            else if (first>=0xed && first<=0xfc)
                UnpackArray(first,bytes,info);
            else if (first == 0xfe || first==0xff)
                UnpackSpecialValue(first,bytes,info);
            return info;
        }

        private static void UnpackSpecialValue(byte first,byte[] bytes,UnpackInformation info)
        {
            info.Values.Add(new SpecialValue(first));
        }
        private static void UnpackArray(byte first,byte[] bytes,UnpackInformation info)
        {
            var length = first-0xed;
            var elementsRead = 0;
            var array = new List<string>();
            do
            {
                Unpack(bytes,info);
                array.Add(info.Values.Last().ToString());
                elementsRead++;
            }
            while ((length!=15 && elementsRead<length) || (length==15 && !(info.Values.Last() is SpecialValue && ((SpecialValue)info.Values.Last()).Value==0xfe)));
            info.Values.Add(array.GetRange(0,length!=15?length:array.Count-1));
        }

        private static void UnpackMap(byte first,byte[] bytes,UnpackInformation info)
        {
            var map = new Dictionary<string,string>();
            var length = first-243;
            for (var i=0;i<length;i++)
            {
                UnpackMapElement(bytes,info,map);
            }
            info.Values.Add(map);
        }

        private static void UnpackMapElement(byte[] bytes,UnpackInformation info,IDictionary<string,string> map)
        {
            var key = Unpack(bytes,info).Values.Last();
            var value = Unpack(bytes,info).Values.Last();
            map.Add(key.ToString(),value.ToString());
        }

        private static void UnpackString(byte first,byte[] bytes,UnpackInformation info)
        {
            var length = first-128;
            info.Values.Add(System.Text.Encoding.Default.GetString(bytes,info.Position,length));
            info.Position+=length;
        }
        private static void UnpackSpecial(byte first,byte[] bytes,UnpackInformation info)
        {
            info.Values.Add(null);

        }

        private static void UnpackFloat(byte first,byte[] bytes, UnpackInformation info)
        {
            info.Values.Add(BitConverter.ToDouble(bytes.ToArray(),info.Position));
            info.Position+=8;
        }

        private static void UnpackInt(byte first, byte[] bytes, UnpackInformation info)
        {
            if (first<=0x3f)
                info.Values.Add((sbyte)first);
            else if (first<0x7b)
                info.Values.Add((sbyte)63-first);
            else if (first==0xe8)
                info.Values.Add((byte)bytes.ToArray()[1]);
            else if (first==0xe9)
            {
                info.Values.Add((short)BitConverter.ToInt16(bytes.ToArray(),1));
                info.Position+=2;
            }
            else if (first==0xea)
            {
                info.Values.Add((int)BitConverter.ToInt32(bytes.ToArray(),1));
                info.Position+=4;
            }
            else if (first==0xeb)
            {
                info.Values.Add((long)BitConverter.ToInt64(bytes.ToArray(),1));
                info.Position+=8;
            }
            else
                throw new QUnpackException(first);
        }
    }
}