using System.Threading;

namespace BasicSyncPatterns
{
    public class Sec04_Mutex
    {
        private readonly Semaphore Mutex;

        public Sec04_Mutex()
        {
            this.Mutex = new Semaphore(1, 1);
        }

        public int Count { get; private set; } = 0;

        public void ThreadA()
        {
            this.Mutex.WaitOne();

            this.Count = this.Count + 1;

            this.Mutex.Release();
        }

        public void ThreadB()
        {
            this.Mutex.WaitOne();

            this.Count = this.Count + 1;

            this.Mutex.Release();
        }
    }
}