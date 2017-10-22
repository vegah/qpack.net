using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Fantasista
{
    public class QPack
    {
        public static byte[] Pack(object o)
        {
            if (o==null) return new byte[]{0xfb};
            var type = o.GetType();
            if (type==typeof(int)||type==typeof(long)||type==typeof(short)||type==typeof(byte))
                return PackInt(Convert.ToInt64(o));
            else if (type==typeof(float)||type==typeof(double))
                return PackFloat((double)o);
            else if (type==typeof(bool))
            {
                var val = (bool)o?0xf9:0xfa;
            }
            else if (type==typeof(string))
            {
                return PackString((string)o);
            }
            else if (typeof(System.Collections.IDictionary).IsAssignableFrom(type))
            {
                return PackMap((System.Collections.IDictionary)o);
            }
            else if (typeof(System.Collections.IEnumerable).IsAssignableFrom(type))
            {
                return PackArray((System.Collections.IEnumerable)o);
            }
            else if (type.GetProperties().Length>0)
            {
                return PackObject(o);
            }
            throw new UnknownQpackObjectTypeException(type);
        }

        private static  byte[] PackObject(object o)
        {
            var properties = o.GetType().GetProperties();
            var dictionarizedObject = new Dictionary<string, object>();
            foreach (var property in properties)
            {
                dictionarizedObject.Add(property.Name,property.GetValue(o));
            }
            return PackMap(dictionarizedObject);
        }

        private static  byte[] PackMap(IDictionary o)
        {
            var length = o.Keys.Count;
            var bytes = new List<byte>();
            if (length>=6) bytes.Add(0xfd);
            else bytes.Add((byte)(0xf3+length));
            foreach (var element in o.Keys)
            {
                Console.WriteLine("Hei");
                bytes.AddRange(Pack(element));
                bytes.AddRange(Pack(o[element]));
            }
            if (length>=6) bytes.Add(0xff);
            return bytes.ToArray();
        }

        private static  byte[] PackArray(IEnumerable o)
        {
            var bytes = new List<byte>();
            var counter = 0;
            foreach (var element in o)
            {
                counter++;
                bytes.AddRange(Pack(element));
            }
            var retval = new List<byte>();
            if (counter>=6) retval.Add(0xfc);
            else retval.Add((byte)(0xed+counter));
            retval.AddRange(bytes);
            if (counter>=6) retval.Add(0xfe);
            return retval.ToArray();
        }

        private static  byte[] PackString(string s)
        {
            long l;
            double d;
            if (long.TryParse(s,out l))
                return PackInt(l);
            else if (double.TryParse(s,out d))
                return Pack(d);
            return PackActualString(s);                
        }

        private static  byte[] PackActualString(string s)
        {
            var list = new List<byte>();
            var bytes = (new UTF8Encoding()).GetBytes(s);
            var length = bytes.Length;
            if (length<0x64) list.Add((byte)(0x80+length));
            else if (length<0x100) list.AddRange(new byte[]{0xe4,(byte)length});
            else if (length<0x10000) {
                list.AddRange(new byte[]{0xe5});
                list.AddRange(BitConverter.GetBytes((short)length));
            }
            else  {
                list.AddRange(new byte[]{0xe6});
                list.AddRange(BitConverter.GetBytes(length));
            }
            list.AddRange(bytes);
            return list.ToArray();
        }

        private static  byte[] PackFloat(double o)
        {
            if (o==0) return new byte[] {0x7e};
            else if (o==-1.0) return new byte[] {0x7d};
            else if (o==1.0) return new byte[] {0x7f};
            var list = new List<byte>(new byte[]{0xec});
            list.AddRange(BitConverter.GetBytes(o));
            return list.ToArray();
            
        }

        private static  byte[] PackInt(long i)
        {
            if (i>=0 && i<64)
                return new byte[] {(byte)i};
            else if (i>=-60 && i<0)
                return new byte[] {(byte)(63-i)};
            else if ((sbyte)i==i)
                return new byte[] {0xe8,(byte)i};
            else if ((short)i==i)
            {
                var list = new List<byte>(new byte[]{0xe9});
                list.AddRange(BitConverter.GetBytes((short)i));
                return list.ToArray();
            }                
            else if ((int)i==i)
            {
                var list = new List<byte>(new byte[]{0xea});
                list.AddRange(BitConverter.GetBytes((int)i));
                return list.ToArray();
            }                
            else
            {
                var list = new List<byte>(new byte[]{0xeb});
                list.AddRange(BitConverter.GetBytes(i));
                return list.ToArray();
            }                
        }
    }
}
