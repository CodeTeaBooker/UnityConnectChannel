using DevToolKit.EventChannel.Core.Abstractions;
using UnityEngine;

namespace DevToolKit.EventChannel.Channels
{
    [CreateAssetMenu(fileName = "NewIntEventChannel", menuName = "Events/IntEventChannel")]
    public class IntEventChannel : EventChannelBase<int>
    {
    }

}
