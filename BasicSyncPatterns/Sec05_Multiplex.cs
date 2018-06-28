using System;
using System.Threading;

namespace BasicSyncPatterns
{
    public class Sec05_Multiplex
    {
        private readonly Semaphore multiplex;

        private readonly Semaphore mutex;

        private int threadsInCriticalSection;

        public Sec05_Multiplex(int maxThreadsInCriticalSection)
        {
            this.PeakThreadsInCriticalSection = 0;
            this.multiplex = new Semaphore(5, maxThreadsInCriticalSection);

            // This mutex isn't really part of this demonstration, but is necessary in order to 
            // make this code testable. I want to keep track of how many threads are in the critical
            // section, and in order to do that in a thread-safe way I need to make sure that only
            // one of the threads in the critical section is accessing that count!
            this.mutex = new Semaphore(1, 1);
        }

        public int PeakThreadsInCriticalSection { get; private set; }

        public void RunCode()
        {
            this.RunCriticalSection();
        }

        private void RunCriticalSection()
        {
            this.multiplex.WaitOne();

            this.mutex.WaitOne();
            this.threadsInCriticalSection++;
            this.mutex.Release();

            this.PeakThreadsInCriticalSection = Math.Max(this.PeakThreadsInCriticalSection, this.threadsInCriticalSection);

            // Simulate doing some stuff
            Thread.Sleep(5);

            this.mutex.WaitOne();
            this.threadsInCriticalSection--;
            this.mutex.Release();

            this.multiplex.Release();
        }
    }
}
