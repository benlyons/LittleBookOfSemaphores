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

        // These methods are somewhat artificial. I found it almost impossible to get the code to go wrong just with
        // this.Count++; even without a mutex. Therefore, I've broken the increment down into steps and introduced a delay.
        // Now, it always goes wrong without the mutex. This isn't what I was aiming for either - I was hoping for something
        // unpredictable - but at least it makes the mutex necessary.
        public void ThreadA()
        {
            this.Mutex.WaitOne();

            int currentCount = this.Count;

            int incrementedCount = currentCount + 1;

            Thread.Sleep(1);

            this.Count = incrementedCount;

            this.Mutex.Release();
        }

        public void ThreadB()
        {
            this.Mutex.WaitOne();

            int currentCount = this.Count;

            int incrementedCount = currentCount + 1;

            Thread.Sleep(1);

            this.Count = incrementedCount;

            this.Mutex.Release();
        }
    }
}