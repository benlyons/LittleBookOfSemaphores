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

            public void Phase1()
            {
                this.mutex.WaitOne();
                this.n++;

                if (this.n == this.threadCount)
                {
                    this.semaphore2.WaitOne();
                    this.semaphore1.Release();
                }
                this.mutex.Release();

                this.semaphore1.WaitOne();
                this.semaphore1.Release();
            }

            public void Phase2()
            {
                this.mutex.WaitOne();
                this.n--;

                if (this.n == 0)
                {
                    this.semaphore1.WaitOne();
                    this.semaphore2.Release();
                }
                this.mutex.Release();

                this.semaphore2.WaitOne();
                this.semaphore2.Release();
            }
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

                this.barrier.Phase1();

                StatementsExecuted.Add(StatementExecuted.CriticalPoint);

                this.barrier.Phase2();
            }
        }
    }
}