using DevToolKit.EventChannel.Core.Abstractions;
using UnityEngine;

namespace DevToolKit.EventChannel.Channels
{
    [CreateAssetMenu(fileName = "NewStringEventChannel", menuName = "Events/StringEventChannel")]
    public class StringEventChannel : EventChannelBase<string>
    {
    }
}

