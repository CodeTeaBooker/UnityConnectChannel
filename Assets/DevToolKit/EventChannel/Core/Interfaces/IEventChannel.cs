using System;

namespace DevToolKit.EventChannel.Core.Interfaces
{
    public interface IEventChannel<TEventData>
    {
        event Action<TEventData> OnEventRaised;

        void Raise(TEventData eventData);
        void RaiseIf(TEventData eventData, Func<TEventData, bool> condition);
        void RegisterListener(IEventListener<TEventData> listener);
        void UnregisterListener(IEventListener<TEventData> listener);
        void ClearListeners();
    }
}

