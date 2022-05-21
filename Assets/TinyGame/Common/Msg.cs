using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
//using ProtoBuf;

namespace MyTinyGame
{

    public static class CodecHelper
    {
        public static byte[] Serialze(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, obj);
            byte[] newArray = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(newArray, 0, (int)stream.Length);
            stream.Close();
            return newArray;
        }

        public static T Deserialize<T>(byte[] array)
        {
            MemoryStream stream = new MemoryStream(array);
            BinaryFormatter bf = new BinaryFormatter();
            T obj = (T)bf.Deserialize(stream);
            stream.Close();
            return obj;
        }
    }

    public class BufferUtils
    {
        public static void WriteUShort(ushort value, byte[] dstBuffer, int offset)
        {
            byte[] buf = BitConverter.GetBytes(value);
            Array.Copy(buf, 0, dstBuffer, offset, buf.Length);
        }

        public static void WriteByteArray(byte[] srcBuffer,byte[] dstBuffer,int dstOffset)
        {
            Array.Copy(srcBuffer, 0, dstBuffer, dstOffset, srcBuffer.Length);
        }
        
        public static void WriteString(string str,byte[] dstBuffer,int dstOffset)
        {
            
        }

    }

    public static class CS_ID
    {
        public const ushort LOGIN = 1;
        public const ushort REGIST = 2;
    }

    public static class SC_ID
    {
        public const ushort RES_LOGIN = 1;
        public const ushort RES_REGIST = 2;
    }

    //[ProtoContract]
    [Serializable]
    public class LoginData
    {
        //[ProtoMember(1)]
        public string UserName { get; set; }

        //[ProtoMember(2)]
        public string Pwd { get; set; }

        public override string ToString()
        {
            return $"username:{UserName},pwd:{Pwd}";
        }
    }
}
