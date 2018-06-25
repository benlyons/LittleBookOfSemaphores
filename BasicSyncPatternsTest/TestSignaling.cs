using CypressTree.BasicSyncPatterns;
using NUnit.Framework;
using System.Threading;

namespace CypressTree.BasicSyncPatternsTest
{
    public class TestSignaling
    {
        [Test]
        public void Test_Statement_A_Runs_Before_Statement_B_When_Thread_A_Starts_First()
        {
            var signaling = new Sec01_Signaling();

            Thread threadA = new Thread(() => signaling.ThreadA());
            Thread threadB = new Thread(() => signaling.ThreadB());

            threadA.Start();
            threadB.Start();

            WaitForThreadsToFinish(threadA, threadB);

            Check_Statement_A_Runs_Before_Statement_B(signaling);
        }

        [Test]
        public void Test_Statement_A_Runs_Before_Statement_B_When_Thread_B_Starts_First()
        {
            var signaling = new Sec01_Signaling();

            Thread threadA = new Thread(() => signaling.ThreadA());
            Thread threadB = new Thread(() => signaling.ThreadB());

            threadB.Start();
            threadA.Start();

            WaitForThreadsToFinish(threadA, threadB);

            Check_Statement_A_Runs_Before_Statement_B(signaling);
        }

        private static void Check_Statement_A_Runs_Before_Statement_B(Sec01_Signaling signaling)
        {
            Assert.That(signaling.StatementsExecuted[0], Is.EqualTo(Sec01_Signaling.StatementExecuted.OnThreadA));
            Assert.That(signaling.StatementsExecuted[1], Is.EqualTo(Sec01_Signaling.StatementExecuted.OnThreadB));
        }

        private static void WaitForThreadsToFinish(Thread threadA, Thread threadB)
        {
            threadA.Join();
            threadB.Join();
        }
    }
}