using DevToolKit.EventChannel.Core.Interfaces;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace DevToolKit.EventChannel.Core.Abstractions
{
    public class EventListener<TEventData, TEventChannel> : MonoBehaviour, IEventListener<TEventData>
        where TEventChannel : EventChannelBase<TEventData>
    {
        [SerializeField] private TEventChannel _eventChannel;

        [SerializeField]
        private UnityEvent<TEventData> _unityEventResponse;

        [SerializeField] 
        private bool _enableDebugLogging = false;

        private bool _isRegistered = false;

        protected virtual void OnEnable()
        {
            RegisterToChannel();
        }

        protected virtual void OnDisable()
        {
            UnregisterFromChannel();
        }

        public virtual void OnEventRaised(TEventData eventData)
        {
            if (_enableDebugLogging)
            {
                Debug.Log($"[{gameObject.name}] Event received: {eventData}");
            }

            try
            {
                _unityEventResponse?.Invoke(eventData);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{gameObject.name}] Error in event response: {ex}");
            }
        }

        private void RegisterToChannel()
        {
            if (_eventChannel == null)
            {
                Debug.LogError($"[{gameObject.name}] No event channel assigned to {GetType().Name}");
                return;
            }

            if (_isRegistered)
            {
                Debug.LogWarning($"[{gameObject.name}] Already registered to event channel");
                return;
            }

            _eventChannel.RegisterListener(this);
            _isRegistered = true;

            if (_enableDebugLogging)
            {
                Debug.Log($"[{gameObject.name}] Registered to event channel");
            }
        }

        private void UnregisterFromChannel()
        {
            if (_eventChannel == null || !_isRegistered) return;

            _eventChannel.UnregisterListener(this);
            _isRegistered = false;

            if (_enableDebugLogging)
            {
                Debug.Log($"[{gameObject.name}] Unregistered from event channel");
            }
        }
    }
}

