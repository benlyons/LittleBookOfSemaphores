using System;
using System.Threading;

namespace BasicSyncPatterns
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new _3_1_Signaling();

            Thread threadA = new Thread(() => test.ThreadA());
            Thread threadB = new Thread(() => test.ThreadB());

            threadA.Start();
            threadB.Start();

            threadA.Join();
            threadB.Join();

            Console.WriteLine("Swapping threads...");

            threadA = new Thread(() => test.ThreadA());
            threadB = new Thread(() => test.ThreadB());

            threadB.Start();
            threadA.Start();

            threadA.Join();
            threadB.Join();
        }
    }
}
