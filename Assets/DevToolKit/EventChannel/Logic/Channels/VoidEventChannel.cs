using DevToolKit.EventChannel.Core.Abstractions;
using DevToolKit.EventChannel.Types;
using UnityEngine;

namespace DevToolKit.EventChannel.Channels
{
    [CreateAssetMenu(fileName = "NewVoidEventChannel", menuName = "Events/VoidEventChannel")]
    public class VoidEventChannel : EventChannelBase<Void>
    {
        private static readonly Void _voidInstance = new Void();

        public void Raise()
        {
            base.Raise(_voidInstance);
        }
    }
}

