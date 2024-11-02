namespace ApiAggregator.Net.Impl
{
    internal class EventPublisher
    {
        private readonly ISubscriber<ExecutorResultArgs> subscriber;

        public EventPublisher(ISubscriber<ExecutorResultArgs> subscriber)
        {
            this.subscriber = subscriber;
        }

        public void PublishEvent(IRequestContext context, ExecutorResultArgs args) => subscriber.OnEventHandler(context, args);
    }

    internal class ExecutorResultArgs : EventArgs
    {
        public ExecutorResultArgs(IEnumerable<IApiResult> result)
        {
            Result = result;
        }

        public IEnumerable<IApiResult> Result { get; }
    }

    internal interface ISubscriber<ExecutorResultArgs>
    {
        void OnEventHandler(IRequestContext context, ExecutorResultArgs e);
    }
}