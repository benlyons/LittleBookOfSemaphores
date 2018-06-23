using System.Collections.Generic;
using System.Threading;

namespace CypressTree.BasicSyncPatterns
{
    public class Sec01_Signaling
    {
        public enum Event
        {
            ThreadA,
            ThreadB
        }

        public IList<Event> Events = new List<Event>();

        private Semaphore semaphore;
        
        public Sec01_Signaling()
        {
            this.semaphore = new Semaphore(0, 1);
        }

        public void ThreadA()
        {
            this.Events.Add(Event.ThreadA);
            this.semaphore.Release();
        }

        public void ThreadB()
        {
            this.semaphore.WaitOne();
            this.Events.Add(Event.ThreadB);
        }
    }
}