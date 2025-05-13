using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField, Header("Void Event Signal")]
    private Image VoidEventSignalImage;

    [SerializeField, Header("Int Event Signal")]
    private Image IntEventSignalImage;
    [SerializeField]
    private Text IntEventSignalText;

    [SerializeField, Header("String Event Signal")]
    private Image StringEventSignalImage;
    [SerializeField]
    private Text StringEventSignalText;

    [SerializeField, Header("Bool Event Signal")]
    private Image BoolEventSignalImage;
    [SerializeField]
    private Text BoolEventSignalText;

    [SerializeField, Header("Custom Type Event Signal")]
    private Image CustomTypeEventSignalImage;
    [SerializeField]
    private Text ID;
    [SerializeField]
    private Text Name;


    public void UpdateVoidEventSignal()
    {
        VoidEventSignalImage.color = Color.green;
    }

    public void UpdateIntEventSignal(int value)
    {
        IntEventSignalImage.color = Color.green;
        IntEventSignalText.text = value.ToString();
    }

    public void UpdateStringEventSignal(string value)
    {
        StringEventSignalImage.color = Color.green;
        StringEventSignalText.text = value;
    }

    public void UpdateBoolEventSignal(bool value)
    {
        BoolEventSignalImage.color = Color.green;
        BoolEventSignalText.text = value.ToString();
    }

    public void UpdateCustomTypeEventSignal(CustomType value)
    {
        CustomTypeEventSignalImage.color = Color.green;
        ID.text = "ID: " + value.id.ToString();
        Name.text = "Name: " + value.name;
    }
}