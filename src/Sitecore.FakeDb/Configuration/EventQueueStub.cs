namespace Sitecore.FakeDb.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sitecore.Eventing;

    internal class EventQueueStub : IEventQueue
    {
        public string Name { get; }

        public bool ListenToRemoteEvents { get; set; }


        public void Cleanup(uint daysToKeep)
        {
        }

        public void Cleanup(TimeSpan intervalToKeep)
        {
        }

        public object DeserializeEvent(QueuedEvent queuedEvent)
        {
            return null;
        }

        public QueuedEvent GetLastEvent()
        {
            return null;
        }

        public long GetQueuedEventCount()
        {
            return 0;
        }

        public IEnumerable<QueuedEvent> GetQueuedEvents()
        {
            return Enumerable.Empty<QueuedEvent>();
        }

        public IEnumerable<QueuedEvent> GetQueuedEvents(string targetInstanceName)
        {
            return Enumerable.Empty<QueuedEvent>();
        }

        public IEnumerable<QueuedEvent> GetQueuedEvents(EventQueueQuery query)
        {
            return Enumerable.Empty<QueuedEvent>();
        }

        public void ProcessEvents(Action<object, Type> handler)
        {
        }

        public void QueueEvent<TEvent>(TEvent @event)
        {
        }

        public void QueueEvent<TEvent>(TEvent @event, bool addToGlobalQueue, bool addToLocalQueue)
        {
        }

        public void RemoveQueuedEvents(EventQueueQuery query)
        {
        }
    }
}