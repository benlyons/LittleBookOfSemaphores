using System.Collections.Generic;

namespace BasicSyncPatterns
{
    public class Sec07_ReusableBarrier
    {
        public enum StatementExecuted
        {
            Rendezvous,
            CriticalPoint
        }

        public IList<StatementExecuted> StatementsExecuted = new List<StatementExecuted>();

        public void RunCode(int loopCount)
        {
            for (int i = 0; i < loopCount; i++)
            {
                StatementsExecuted.Add(StatementExecuted.Rendezvous);
                StatementsExecuted.Add(StatementExecuted.CriticalPoint);
            }
        }
    }
}