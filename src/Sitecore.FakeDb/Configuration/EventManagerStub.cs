namespace Sitecore.FakeDb.Configuration
{
    using System;
    using Sitecore.Abstractions;
    using Sitecore.Eventing;

    internal class EventManagerStub : BaseEventManager
    {
        public override bool Enabled => false;

        [Obsolete]
        public override void QueueEvent<TEvent>(TEvent @event)
        {
        }

        [Obsolete]
        public override void QueueEvent<TEvent>(TEvent @event, bool addToGlobalQueue, bool addToLocalQueue)
        {
        }

        public override void RaiseEvent<TEvent>(TEvent @event)
        {
        }

        [Obsolete]
        public override void RaiseQueuedEvents()
        {
        }

        [Obsolete]
        public override void RemoveQueuedEvents(EventQueueQuery query)
        {
        }

        public override void Initialize()
        {
        }

        public override SubscriptionId Subscribe<TEvent>(Action<TEvent> eventHandler)
        {
            return null;
        }

        public override SubscriptionId Subscribe<TEvent>(Action<TEvent, EventContext> eventHandler)
        {
            return null;
        }

        public override SubscriptionId Subscribe<TEvent>(Action<TEvent> eventHandler, Predicate<TEvent> filter)
        {
            return null;
        }

        public override SubscriptionId Subscribe<TEvent>(Action<TEvent, EventContext> eventHandler, Func<TEvent, EventContext, bool> filter)
        {
            return null;
        }

        public override void Unsubscribe(SubscriptionId subscriptionId)
        {
        }
    }
}