using UnityEngine;

namespace DevToolKit.EventChannel.Core.Base
{
    public class DescriptiveScriptableObject : GuidScriptableObject
    {
        [TextArea] 
        public string Description;
    }
}

