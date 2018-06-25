using CypressTree.BasicSyncPatterns;
using NUnit.Framework;
using System.Threading;

namespace CypressTree.BasicSyncPatternsTest
{
    public class TestSignaling
    {
        private Sec01_Signaling signaling;

        private Thread threadA;

        private Thread threadB;

        [SetUp]
        public void SetUp()
        {
            this.signaling = new Sec01_Signaling();

            this.threadA = new Thread(() => this.signaling.ThreadA());
            this.threadB = new Thread(() => this.signaling.ThreadB());
        }

        [Test]
        public void Test_Statement_A_Runs_Before_Statement_B_When_Thread_A_Starts_First()
        {
            this.threadA.Start();
            this.threadB.Start();

            this.WaitForThreadsToFinish();

            this.Check_Statement_A_Runs_Before_Statement_B();
        }

        [Test]
        public void Test_Statement_A_Runs_Before_Statement_B_When_Thread_B_Starts_First()
        {
            this.threadB.Start();
            this.threadA.Start();

            this.WaitForThreadsToFinish();

            this.Check_Statement_A_Runs_Before_Statement_B();
        }

        private void Check_Statement_A_Runs_Before_Statement_B()
        {
            Assert.That(this.signaling.StatementsExecuted[0], Is.EqualTo(Sec01_Signaling.StatementExecuted.OnThreadA));
            Assert.That(this.signaling.StatementsExecuted[1], Is.EqualTo(Sec01_Signaling.StatementExecuted.OnThreadB));
        }

        private void WaitForThreadsToFinish()
        {
            this.threadA.Join();
            this.threadB.Join();
        }
    }
}