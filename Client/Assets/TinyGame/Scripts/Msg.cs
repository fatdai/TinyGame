using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using LitJson;

public class CodecHelper
{
    public static void WriteUShort(ushort value, byte[] dstBuffer, int offset)
    {
        byte[] buf = BitConverter.GetBytes(value);
        Array.Copy(buf, 0, dstBuffer, offset, buf.Length);
    }

    public static void WriteByteArray(byte[] srcBuffer, byte[] dstBuffer, int dstOffset)
    {
        Array.Copy(srcBuffer, 0, dstBuffer, dstOffset, srcBuffer.Length);
    }

    public static void WriteString(string str, byte[] dstBuffer, int dstOffset)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        if(bytes.Length + dstOffset > dstBuffer.Length)
        {
            throw new Exception("dstBuffer 空间不够!");
        }
        Array.Copy(bytes, 0, dstBuffer, dstOffset,bytes.Length);
    }

    public static byte[] EncodeMsg<T>(ushort cmd,T t)
    {
        string str = JsonMapper.ToJson(t);
        byte[] bytes = Encoding.UTF8.GetBytes(str);

        // 换成缓存
        byte[] result = new byte[bytes.Length + 4];
        WriteUShort(cmd, result, 0);
        WriteUShort((ushort)bytes.Length, result,   2);
        WriteByteArray(bytes, result,  4);
        return result;
    }

    public static T DecodeMsg<T>(byte[] buf) 
    {
        string str = Encoding.UTF8.GetString(buf);
        return JsonMapper.ToObject<T>(str);
    }
}

public static class CS_ID
{
    public const ushort LOGIN = 1;
    public const ushort REGIST = 2;
    public const ushort MATCH = 3;
}

public static class SC_ID
{
    public const ushort RES_LOGIN = 1;
    public const ushort RES_REGIST = 2;
    public const ushort RES_MATCH = 3;
}


public class LoginData
{
    public string UserName { get; set; }

    public string Pwd { get; set; }

    public override string ToString()
    {
        return $"username:{UserName},pwd:{Pwd}";
    }
}

public class Res_LoginData
{
    public int UserId { get; set; }

}
