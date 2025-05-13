using DevToolKit.EventChannel.Core.Abstractions;
using UnityEngine;

namespace DevToolKit.EventChannel.Channels
{
    [CreateAssetMenu(fileName = "NewBoolEventChannel", menuName = "Events/BoolEventChannel")]
    public class BoolEventChannel : EventChannelBase<bool>
    {
    }
}

