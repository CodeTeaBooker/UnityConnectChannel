namespace DevToolKit.EventChannel.Core.Interfaces
{
    public interface IEventListener<in TEventData>
    {
        void OnEventRaised(TEventData eventData);
    }

}
