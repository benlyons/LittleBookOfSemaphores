using BasicSyncPatterns;
using NUnit.Framework;
using System.Linq;
using System.Threading;
using static BasicSyncPatterns.Sec06_Barrier;

namespace BasicSyncPatternsTest
{
    public class Test06_Barrier
    {
        [Test]
        public void TestSingleThreaded()
        {
            var test = new Sec06_Barrier(1);

            test.RunCode();

            Assert.That(Sec06_Barrier.StatementsExecuted[0], Is.EqualTo(StatementExecuted.Rendezvous));
            Assert.That(Sec06_Barrier.StatementsExecuted[1], Is.EqualTo(StatementExecuted.CriticalPoint));
        }

        [Test]
        public void TestMultiThreaded()
        {
            int threadCount = 25;

            var test = new Sec06_Barrier(threadCount);

            for (int i = 0; i < threadCount; i++)
            {
                Thread thread = new Thread(() => test.RunCode());
                thread.Start();
            }

            var expectedStatementsExecuted =
                Enumerable.Repeat(StatementExecuted.Rendezvous, threadCount).Concat(
                Enumerable.Repeat(StatementExecuted.CriticalPoint, threadCount));

            CollectionAssert.AreEqual(expectedStatementsExecuted, Sec06_Barrier.StatementsExecuted);
        }
    }
}