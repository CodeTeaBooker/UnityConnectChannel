using DevToolKit.EventChannel.Core.Base;
using DevToolKit.EventChannel.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DevToolKit.EventChannel.Core.Abstractions
{
    public abstract class EventChannelBase<TEventData> : DescriptiveScriptableObject, IEventChannel<TEventData>
    {
        [SerializeField]
        private int _initialListenerCapacity = 1;

        [SerializeField]
        private bool _enableDebugLogging = false;

        private readonly HashSet<IEventListener<TEventData>> _listeners;
        private readonly object _lock = new object();

        private bool _isRaising = false;
        public bool IsRaising => _isRaising;

        private int _raiseCount = 0;
        public int RaiseCount => _raiseCount;

        public event Action<TEventData> OnEventRaised;

        protected EventChannelBase()
        {
            _listeners = new HashSet<IEventListener<TEventData>>(_initialListenerCapacity);
        }

        public int GetListenerCount()
        {
            lock (_lock)
            {
                return _listeners.Count;
            }
        }

        public void EnableDebugLogging(bool enable)
        {
            _enableDebugLogging = enable;
        }

        public virtual void Raise(TEventData eventData)
        {
            if (_isRaising)
            {
                Debug.LogWarning($"[{Guid}] Recursive event raising detected!");
                return;
            }

            _raiseCount++;

            List<IEventListener<TEventData>> tempListeners;
            lock (_lock)
            {
                if (_listeners.Count == 0 && OnEventRaised == null)
                {
                    if (_enableDebugLogging)
                    {
                        Debug.Log($"[{Guid}] No listeners registered for event. Total raises: {_raiseCount}");
                    }
                    return;
                }
                tempListeners = _listeners.ToList();
            }

            try
            {
                _isRaising = true;

                foreach (var listener in tempListeners)
                {
                    if (listener == null) continue;

                    try
                    {
                        if (_enableDebugLogging)
                        {
                            Debug.Log($"[{Guid}] Raising event to {listener.GetType().Name}. Total raises: {_raiseCount}");
                        }
                        listener.OnEventRaised(eventData);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"[{Guid}] Error in event listener {listener.GetType().Name}: {ex}");
                    }
                }


                try
                {
                    if (_enableDebugLogging && OnEventRaised != null)
                    {
                        Debug.Log($"[{Guid}] Raising event to delegate subscribers. Total raises: {_raiseCount}");
                    }
                    OnEventRaised?.Invoke(eventData);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[{Guid}] Error in event delegate: {ex}");
                }
            }
            finally
            {
                _isRaising = false;
            }
        }

        public virtual void RaiseIf(TEventData eventData, Func<TEventData, bool> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            try
            {
                if (condition(eventData))
                {
                    Raise(eventData);
                }
                else if (_enableDebugLogging)
                {
                    Debug.Log($"[{Guid}] Event not raised - condition returned false");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{Guid}] Error evaluating raise condition: {ex}");
                throw;
            }
        }

        public virtual void RegisterListener(IEventListener<TEventData> listener)
        {
            if (listener == null)
            {
                Debug.LogError($"[{Guid}] Attempted to register null listener");
                return;
            }

            lock (_lock)
            {
                if (_listeners.Contains(listener))
                {
                    Debug.LogWarning($"[{Guid}] Listener {listener.GetType().Name} already registered");
                    return;
                }

                _listeners.Add(listener);

                if (_enableDebugLogging)
                {
                    Debug.Log($"[{Guid}] Registered listener {listener.GetType().Name}. Total listeners: {_listeners.Count}, Total raises: {_raiseCount}");
                }
            }
        }

        public virtual void UnregisterListener(IEventListener<TEventData> listener)
        {
            if (listener == null)
            {
                Debug.LogError($"[{Guid}] Attempted to unregister null listener");
                return;
            }

            lock (_lock)
            {
                if (!_listeners.Remove(listener) && _enableDebugLogging)
                {
                    Debug.LogWarning($"[{Guid}] Attempted to unregister listener {listener.GetType().Name} that was not registered");
                    return;
                }

                if (_enableDebugLogging)
                {
                    Debug.Log($"[{Guid}] Unregistered listener {listener.GetType().Name}. Total listeners: {_listeners.Count}, Total raises: {_raiseCount}");
                }
            }
        }

        public virtual void ClearListeners()
        {
            lock (_lock)
            {
                if (_enableDebugLogging && _listeners.Count > 0)
                {
                    Debug.Log($"[{Guid}] Cleared {_listeners.Count} listeners. Total raises: {_raiseCount}");
                }
                _listeners.Clear();
                OnEventRaised = null;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _raiseCount = 0;

            // Add event channel-specific initialization logic here
            // Currently empty, reserved for future extensions
        }

        protected virtual void OnDisable()
        {
            ClearListeners();
        }

        protected override void OnValidate()
        {
            base.OnValidate();

#if UNITY_EDITOR

            // Validate initial capacity
            if (_initialListenerCapacity < 1)
            {
                Debug.LogWarning($"[{Guid}] Initial listener capacity must be at least 1. Adjusting value.");
                _initialListenerCapacity = 1;
            }
            else if (_initialListenerCapacity > 100)
            {
                Debug.LogWarning($"[{Guid}] Very large initial listener capacity detected: {_initialListenerCapacity}");
            }
#endif
        }


    }
}
