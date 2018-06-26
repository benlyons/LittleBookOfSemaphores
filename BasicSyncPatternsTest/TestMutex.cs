using BasicSyncPatterns;
using NUnit.Framework;
using System.Threading;

namespace BasicSyncPatternsTest
{
    public class TestMutex
    {
        [Test]
        public void Test_TwoThreadsIncrementCount_CountIsIncrementedCorrectly()
        {
            // I've tried running this this test against code without a mutex 100,000 times in a loop.
            // It almost never goes wrong.
            // Strangely, the only times it did go wrong, it went wrong on the first iteration of the loop.
            // Overall, it's extremely hard to repeat.
            var test = new Sec04_Mutex();

            Thread threadA = new Thread(() => test.ThreadA());
            Thread threadB = new Thread(() => test.ThreadB());

            threadA.Start();
            threadB.Start();

            threadA.Join();
            threadB.Join();

            Assert.That(test.Count, Is.EqualTo(2));
        }
    }
}