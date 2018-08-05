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
            private readonly int threadCount;
            private readonly Semaphore mutex;
            private readonly Semaphore semaphore1;
            private readonly Semaphore semaphore2;

            private int n;

            public TwoPhaseBarrier(int threadCount)
            {
                this.threadCount = threadCount;
                this.n = 0;
                this.mutex = new Semaphore(1, 1);
                this.semaphore1 = new Semaphore(0, 1);
                this.semaphore2 = new Semaphore(1, 1);
            }

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
            this.barrier = new TwoPhaseBarrier(threadCount);
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