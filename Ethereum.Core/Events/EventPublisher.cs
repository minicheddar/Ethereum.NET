using System;
using System.Linq;

namespace Ethereum.Core
{
    public class EventPublisher : IEventPublisher
    {
        private readonly ISubscriptionService subscriptionService;

        public EventPublisher(ISubscriptionService subscriptionService)
        {
            Ensure.Argument.IsNotNull(subscriptionService, "subscriptionService");

            this.subscriptionService = subscriptionService;
        }

        public void Publish<T>(T eventMessage)
        {
            var subscriptions = subscriptionService.GetSubscriptions<T>();

            subscriptions.ToList().ForEach(x => PublishToConsumer(x, eventMessage));
        }

        private static void PublishToConsumer<T>(IEventHandler<T> eventHandler, T eventMessage)
        {
            try
            {
                eventHandler.Handle(eventMessage);
            }
            catch (Exception ex)
            {
                // LOG
                //"EventPublisher.PublishToConsumer".Log().Error(ex.Message, ex);
            }
            finally
            {
                var instance = eventHandler as IDisposable;

                if (instance != null)
                {
                    instance.Dispose();
                }
            }
        }
    }
}
