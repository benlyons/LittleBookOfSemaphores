using System;
using System.Threading;

namespace BasicSyncPatterns
{
    public class Sec01_Signaling
    {
        private Semaphore semaphore;
        
        public Sec01_Signaling()
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
