using System.Collections.Generic;
using System.Threading;

namespace BasicSyncPatterns
{
    public class Sec07_ReusableBarrier
    {
        public enum StatementExecuted
        {
            Rendezvous,
            CriticalPoint
        }

        private class TwoPhaseBarrier
        {
            public int threadCount;
            public Semaphore mutex;
            public Semaphore semaphore1;
            public Semaphore semaphore2;
            public int n;
        }

        public Sec07_ReusableBarrier(int threadCount)
        {
            this.barrier = new TwoPhaseBarrier();

            this.barrier.n = 0;
            this.barrier.threadCount = threadCount;

            this.barrier.mutex = new Semaphore(1, 1);
            this.barrier.semaphore1 = new Semaphore(0, 1);
            this.barrier.semaphore2 = new Semaphore(1, 1);
        }

        public IList<StatementExecuted> StatementsExecuted = new List<StatementExecuted>();

        private TwoPhaseBarrier barrier;

        public void RunCode(int loopCount)
        {
            for (int i = 0; i < loopCount; i++)
            {
                StatementsExecuted.Add(StatementExecuted.Rendezvous);

                Phase1();

                StatementsExecuted.Add(StatementExecuted.CriticalPoint);

                Phase2();
            }
        }

        private void Phase1()
        {
            this.barrier.mutex.WaitOne();
            this.barrier.n++;

            if (this.barrier.n == this.barrier.threadCount)
            {
                this.barrier.semaphore2.WaitOne();
                this.barrier.semaphore1.Release();
            }
            this.barrier.mutex.Release();

            this.barrier.semaphore1.WaitOne();
            this.barrier.semaphore1.Release();
        }

        private void Phase2()
        {
            this.barrier.mutex.WaitOne();
            this.barrier.n--;

            if (this.barrier.n == 0)
            {
                this.barrier.semaphore1.WaitOne();
                this.barrier.semaphore2.Release();
            }
            this.barrier.mutex.Release();

            this.barrier.semaphore2.WaitOne();
            this.barrier.semaphore2.Release();
        }
    }
}