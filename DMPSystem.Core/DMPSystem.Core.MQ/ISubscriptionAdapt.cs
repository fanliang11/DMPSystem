
namespace DMPSystem.Core.EventBus
{
    public interface ISubscriptionAdapt
    {
        void SubscribeAt();

        void PublishAt();
    }
}
