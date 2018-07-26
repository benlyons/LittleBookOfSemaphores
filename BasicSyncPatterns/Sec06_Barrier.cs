using System.Collections.Generic;
using System.Threading;

namespace BasicSyncPatterns
{
    public class Sec06_Barrier
    {
        public enum StatementExecuted
        {
            Rendezvous,
            CriticalPoint
        }

        public Sec06_Barrier(int maxThreadCount)
        {
            this.n = 0;
            this.threadCount = maxThreadCount;

            this.mutex = new Semaphore(1, 1);
            this.barrier = new Semaphore(0, 1);
        }

        public static IList<StatementExecuted> StatementsExecuted = new List<StatementExecuted>();
        private int n;
        private readonly int threadCount;
        private readonly Semaphore mutex;
        private readonly Semaphore barrier;

        public void RunCode()
        {
            StatementsExecuted.Add(StatementExecuted.Rendezvous);

            this.mutex.WaitOne();
            this.n++;
            this.mutex.Release();

            if(this.threadCount == this.n)
            {
                this.barrier.Release();
            }

            this.barrier.WaitOne();
            this.barrier.Release();

            StatementsExecuted.Add(StatementExecuted.CriticalPoint);
        }
    }
}