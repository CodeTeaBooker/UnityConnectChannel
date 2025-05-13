using DevToolKit.EventChannel.Channels;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    [SerializeField, Header("Values")]
    private int intValue;

    [SerializeField, Space]
    private string stringValue;

    [SerializeField, Space]
    private bool boolValue;

    [SerializeField, Space]
    private CustomType customTypeValue;


    [SerializeField, Header("Channels"), Space]
    private VoidEventChannel VoidEventChannel;

    [SerializeField, Space]
    private IntEventChannel IntEventChannel;

    [SerializeField, Space]
    private StringEventChannel StringEventChannel;

    [SerializeField, Space]
    private BoolEventChannel BoolEventChannel;

    [SerializeField, Space]
    private CustomTypeEventChannel CustomTypeEventChannel;

    public void RaiseVoidEvent()
    {
        VoidEventChannel.Raise();
    }

    public void RaiseIntEvent(int value)
    {
        IntEventChannel.Raise(value);
    }

    public void RaiseStringEvent(string value)
    {
        StringEventChannel.Raise(value);
    }

    public void RaiseBoolEvent(bool value)
    {
        BoolEventChannel.Raise(value);
    }

    public void RaiseCustomTypeEvent(CustomType value)
    {
        CustomTypeEventChannel.Raise(value);
    }


#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(DemoScript))]
    public class DemoScriptEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            DemoScript demoScript = (DemoScript)target;

            if (GUILayout.Button("Raise Void Event"))
            {
                demoScript.RaiseVoidEvent();
            }
            if (GUILayout.Button("Raise Int Event"))
            {
                demoScript.RaiseIntEvent(demoScript.intValue);
            }
            if (GUILayout.Button("Raise String Event"))
            {
                demoScript.RaiseStringEvent(demoScript.stringValue);
            }
            if (GUILayout.Button("Raise Bool Event"))
            {
                demoScript.RaiseBoolEvent(demoScript.boolValue);
            }
            if (GUILayout.Button("Raise Custom Type Event"))
            {
                demoScript.RaiseCustomTypeEvent(demoScript.customTypeValue);
            }
        }
    }
#endif

}




