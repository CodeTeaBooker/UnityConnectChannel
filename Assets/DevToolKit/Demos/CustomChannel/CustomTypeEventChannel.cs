using DevToolKit.EventChannel.Core.Abstractions;
using UnityEngine;


[CreateAssetMenu(fileName = "NewCustomTypeEventChannel", menuName = "Events/CustomTypeEventChannel")]
public class CustomTypeEventChannel : EventChannelBase<CustomType>
{
}
