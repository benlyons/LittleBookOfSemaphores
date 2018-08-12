using BasicSyncPatterns;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DancerType = BasicSyncPatterns.Sec08_Queue.DancerType;

namespace BasicSyncPatternsTest
{
    public class Test08_Queue
    {
        public enum Add
        {
            Leader,
            Follower
        }

        private Sec08_Queue classUnderTest;

        private IList<Thread> threads;

        [SetUp]
        public void SetUp()
        {
            this.classUnderTest = new Sec08_Queue();
            this.threads = new List<Thread>();
        }

        private static IEnumerable<TestCaseData> Tests
        {
            get
            {
                yield return new TestCaseData(new[] { Add.Leader }, 0);
                yield return new TestCaseData(new[] { Add.Leader, Add.Follower }, 1);
                yield return new TestCaseData(new[] { Add.Leader, Add.Leader, Add.Follower }, 1);
                yield return new TestCaseData(new[] { Add.Follower, Add.Leader, Add.Leader }, 1);
            }
        }

        [TestCaseSource("Tests")]
        public void WhenLeadersAndFollowerAdded_WhoShouldDance(IEnumerable<Add> dancersToAdd, int howManyPairs)
        {
            foreach (var dancerToAdd in dancersToAdd)
            {
                if (dancerToAdd == Add.Leader)
                {
                    this.CreateAndStartLeaderThread();
                }
                else
                {
                    this.CreateAndStartFollowerThread();
                }
            }

            this.JoinThreads();

            Assert.That(this.classUnderTest.Dancers.Count, Is.EqualTo(howManyPairs * 2));

            this.AssertThatDancersAreGroupedIntoPairs(howManyPairs);
        }

        private void AssertThatDancersAreGroupedIntoPairs(int howManyPairs)
        {
            for (int i = 0; i < howManyPairs; i++)
            {
                var pair = this.classUnderTest.Dancers.Skip(2 * i).Take(2);

                Assert.That(pair, Is.EquivalentTo(new[] { DancerType.Leader, DancerType.Follower }));
            }
        }

        private void CreateAndStartLeaderThread()
        {
            var leaderThread = new Thread(() => this.classUnderTest.AddLeader());
            leaderThread.Start();

            this.threads.Add(leaderThread);
        }

        private void CreateAndStartFollowerThread()
        {
            var followerThread = new Thread(() => this.classUnderTest.AddFollower());
            followerThread.Start();

            this.threads.Add(followerThread);
        }

        private void JoinThreads()
        {
            foreach (var thread in this.threads)
            {
                thread.Join(1000);
            }
        }
    }
}