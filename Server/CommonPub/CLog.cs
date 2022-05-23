using System;
using System.Threading;

namespace CommonPub
{
    public class CLog
    {
        public static void Info(string msg)
        {
            Console.WriteLine($"<{Thread.CurrentThread.ManagedThreadId}> {msg}");
        }
    }
}
