using System;

namespace DMPSystem.Core.EventBus.Subscription
{
    public interface IConsumer
    {
    }

    public interface IConsumer<in T> : IConsumer, IDisposable
    {
        void HandleEvent(T eventMessage);
    }
}
