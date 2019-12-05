namespace Sitecore.FakeDb.Configuration
{
    using System;
    using Sitecore.Abstractions;
    using Sitecore.Eventing;

    internal class EventManagerStub : BaseEventManager
    {
        public override bool Enabled => false;

        public override void RaiseEvent<TEvent>(TEvent @event)
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
