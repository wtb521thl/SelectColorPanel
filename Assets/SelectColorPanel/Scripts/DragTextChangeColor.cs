using UnityEngine;
using UnityEngine.UI;

public class DragTextChangeColor : MonoBehaviour
{
    RectTransform selfRect;
    SelectColorPanel selectColorPanel;
    public InputField hexInput;
    public InputField rInput;
    public InputField gInput;
    public InputField bInput;
    public InputField aInput;

    public GameObject rNameObj;
    public GameObject gNameObj;
    public GameObject bNameObj;
    public GameObject aNameObj;

    void Awake()
    {
        selfRect = GetComponent<RectTransform>();

        selectColorPanel = GetComponent<SelectColorPanel>();
        selectColorPanel.ChangeColorAction += ChangeColorAction;

        EventTriggerListener.Get(rNameObj).onDrag = DragObj;
        EventTriggerListener.Get(rNameObj).onDown = DownObj;

        EventTriggerListener.Get(gNameObj).onDrag = DragObj;
        EventTriggerListener.Get(gNameObj).onDown = DownObj;

        EventTriggerListener.Get(bNameObj).onDrag = DragObj;
        EventTriggerListener.Get(bNameObj).onDown = DownObj;

        EventTriggerListener.Get(aNameObj).onDrag = DragObj;
        EventTriggerListener.Get(aNameObj).onDown = DownObj;
    }

    Vector2 startMousePos;
    float offsetX;
    float curValue;
    InputField inputField;
    private void DownObj(GameObject go)
    {
        inputField = go.transform.parent.Find(go.name.Replace("Name", "")).GetComponent<InputField>();
        startMousePos = Input.mousePosition;
    }

    private void DragObj(GameObject go)
    {
        offsetX = Input.mousePosition.x - startMousePos.x;
        startMousePos = Input.mousePosition;
        if (float.TryParse(inputField.text, out curValue))
        {
            curValue = Mathf.Clamp01(curValue + offsetX / 1000f);
            inputField.text = curValue.ToString();
            Color tempColor = new Color(float.Parse(rInput.text), float.Parse(gInput.text), float.Parse(bInput.text), float.Parse(aInput.text));
            selectColorPanel.SetValueByColor(tempColor);
        }
    }


    private void ChangeColorAction(Color color)
    {
        hexInput.text = ColorUtility.ToHtmlStringRGB(color);
        rInput.text = color.r.ToString();
        gInput.text = color.g.ToString();
        bInput.text = color.b.ToString();
        aInput.text = color.a.ToString();
    }
}

