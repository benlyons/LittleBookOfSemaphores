using System;
using System.Threading;

namespace CypressTree.BasicSyncPatterns
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new Sec01_Signaling();

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
