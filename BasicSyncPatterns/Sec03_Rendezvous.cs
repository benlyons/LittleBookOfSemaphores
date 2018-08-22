using System.Collections.Generic;
using System.Threading;

namespace CypressTree.BasicSyncPatterns
{
    public class Sec03_Rendezvous
    {
        public enum StatementExecuted
        {
            A1,
            A2,
            B1,
            B2
        }

        public IList<StatementExecuted> StatementsExecuted = new List<StatementExecuted>();

        private readonly Semaphore aArrived;

        private readonly Semaphore bArrived;

        public Sec03_Rendezvous()
        {
            this.aArrived = new Semaphore(0, 1);
            this.bArrived = new Semaphore(0, 1);
        }

        public void ThreadA()
        {
            this.StatementsExecuted.Add(StatementExecuted.A1);

            this.aArrived.Release();
            this.bArrived.WaitOne();

            this.StatementsExecuted.Add(StatementExecuted.A2);
        }

        public void ThreadB()
        {
            this.StatementsExecuted.Add(StatementExecuted.B1);

            this.bArrived.Release();
            this.aArrived.WaitOne();

            this.StatementsExecuted.Add(StatementExecuted.B2);
        }
    }
}