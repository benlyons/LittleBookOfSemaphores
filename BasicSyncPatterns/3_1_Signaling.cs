using System;
using System.Threading;

namespace BasicSyncPatterns
{
    public class _3_1_Signaling
    {
        private Semaphore semaphore;
        
        public _3_1_Signaling()
        {
            this.semaphore = new Semaphore(0, 1);
        }

        public void ThreadA()
        {
            Console.WriteLine("A1");
            this.semaphore.Release();
        }

        public void ThreadB()
        {
            this.semaphore.WaitOne();
            Console.WriteLine("B1");
        }
    }
}
