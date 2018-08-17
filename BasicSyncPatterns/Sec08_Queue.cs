using System;
using System.Collections.Generic;
using System.Threading;

namespace BasicSyncPatterns
{
    public class Sec08_Queue
    {
        public enum DancerType
        {
            Leader,
            Follower
        }

        private int leaders = 0;
        private int followers = 0;

        private readonly Semaphore mutex;
        private readonly Semaphore leaderQueue;
        private readonly Semaphore followerQueue;
        private readonly Semaphore rendezvous;

        public Sec08_Queue()
        {
            this.mutex = new Semaphore(1, 1);
            this.leaderQueue = new Semaphore(0, 1);
            this.followerQueue = new Semaphore(0, 1);
            this.rendezvous = new Semaphore(0, 1);
        }

        public IList<DancerType> Dancers { get; } = new List<DancerType>();

        public void AddLeader()
        {
            this.mutex.WaitOne();
            
            if (this.followers > 0)
            {
                this.followers--;
                this.followerQueue.Release();
            }
            else
            {
                this.leaders++;
                this.mutex.Release();

                this.leaderQueue.WaitOne();
            }

            this.Dancers.Add(DancerType.Leader);

            this.rendezvous.WaitOne();

            this.mutex.Release();
        }

        public void AddFollower()
        {
            this.mutex.WaitOne();

            if (this.leaders > 0)
            {
                this.leaders--;
                this.leaderQueue.Release();
            }
            else
            {
                this.followers++;
                this.mutex.Release();

                this.followerQueue.WaitOne();
            }

            this.Dancers.Add(DancerType.Follower);

            this.rendezvous.Release();
        }
    }
}