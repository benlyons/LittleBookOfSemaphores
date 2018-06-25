using System.Collections.Generic;
using System.Threading;

namespace CypressTree.BasicSyncPatterns
{
    public class Sec01_Signaling
    {
        public enum StatementExecuted
        {
            OnThreadA,
            OnThreadB
        }

        public IList<StatementExecuted> StatementsExecuted = new List<StatementExecuted>();

        private Semaphore semaphore;
        
        public Sec01_Signaling()
        {
            this.semaphore = new Semaphore(0, 1);
        }

        public void ThreadA()
        {
            this.StatementsExecuted.Add(StatementExecuted.OnThreadA);
            this.semaphore.Release();
        }

        public void ThreadB()
        {
            this.semaphore.WaitOne();
            this.StatementsExecuted.Add(StatementExecuted.OnThreadB);
        }
    }
}