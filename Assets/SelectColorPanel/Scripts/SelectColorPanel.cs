using UnityEngine;
using UnityEngine.UI;
using System;

public class SelectColorPanel : MonoBehaviour
{
    /// <summary>
    /// 饱和度
    /// </summary>
    public Image saturation;
    /// <summary>
    /// 颜色
    /// </summary>
    public Image hue;
    /// <summary>
    /// 透明度
    /// </summary>
    public Image alpha;
    /// <summary>
    /// 预览颜色image组件
    /// </summary>
    public Image preImage;
    /// <summary>
    /// 大面板的游标
    /// </summary>
    public RectTransform point_Stauration;
    /// <summary>
    /// hsv面板的游标
    /// </summary>
    public RectTransform point_Hue;
    public RectTransform point_Alpha;

    /// <summary>
    /// 生成的大面板彩色图片
    /// </summary>
    private Sprite saturation_Sprite;
    /// <summary>
    /// 生成的hsv面板彩色图片
    /// </summary>
    private Sprite hue_Sprite;

    /// <summary>
    /// 透明度面板彩色图片
    /// </summary>
    private Sprite alpha_Sprite;


    private Color32 currentHue = Color.red;
    private Vector3 currentHueHsv;
    private float curAlpha;

    Vector2 saturationPoint;
    Vector2 huePoint;
    Vector2 alphaPoint;
    /// <summary>
    /// 外部可以使用的接口，改变颜色后执行
    /// </summary>
    public Action<Color> ChangeColorAction;

    private void Awake()
    {
        EventTriggerListener.Get(saturation.gameObject).onDrag += SetSaturation;
        EventTriggerListener.Get(hue.gameObject).onDrag += SetHue;
        EventTriggerListener.Get(alpha.gameObject).onDrag += SetAlpha;

        EventTriggerListener.Get(saturation.gameObject).onDown += SetSaturation;
        EventTriggerListener.Get(hue.gameObject).onDown += SetHue;
        EventTriggerListener.Get(alpha.gameObject).onDown += SetAlpha;
    }


    /// <summary>
    /// 外部调用，初始化的时候设置下颜色
    /// </summary>
    /// <param name="color"></param>
    public void SetValueByColor(Color color)
    {
        currentHue = color;
        curAlpha = color.a;
        Color.RGBToHSV(currentHue, out currentHueHsv.x, out currentHueHsv.y, out currentHueHsv.z);
        point_Hue.anchoredPosition = new Vector2(currentHueHsv.x * hue.rectTransform.sizeDelta.x, 0);
        point_Stauration.anchoredPosition = new Vector2(saturation.rectTransform.sizeDelta.x * currentHueHsv.y, -saturation.rectTransform.sizeDelta.y * (1 - currentHueHsv.z));
        point_Alpha.anchoredPosition = new Vector2(curAlpha * alpha.rectTransform.sizeDelta.x, 0);
        UpdateHue();
        UpdateStauration();
        UpdateAlpha();
        UpdateColor();
    }

    private void SetHue(GameObject go)
    {
        OnHueClick();
    }
    private void SetAlpha(GameObject go)
    {
        OnAlphaClick();
    }

    private void SetSaturation(GameObject go)
    {
        OnStaurationClick();
    }


    /// <summary>
    /// 更新饱和度
    /// </summary>
    private void UpdateStauration()
    {
        float sWidth = 200, sHeight = 200;
        saturation_Sprite = Sprite.Create(new Texture2D((int)sWidth, (int)sHeight), new Rect(0, 0, sWidth, sHeight), new Vector2(0, 0));

        Color.RGBToHSV(currentHue, out currentHueHsv.x, out currentHueHsv.y, out currentHueHsv.z);


        for (int y = 0; y < sHeight; y++)
        {
            for (int x = 0; x < sWidth; x++)
            {
                var pixColor = Color.HSVToRGB(currentHueHsv.x, x / sWidth, y / sHeight);
                saturation_Sprite.texture.SetPixel(x, y, pixColor);
            }
        }
        saturation_Sprite.texture.Apply();

        saturation.sprite = saturation_Sprite;

    }

    /// <summary>
    /// 更新色泽度 
    /// </summary>
    private void UpdateHue()
    {

        float w = 50, h = 10;

        hue_Sprite = Sprite.Create(new Texture2D((int)w, (int)h), new Rect(0, 0, w, h), new Vector2(0, 0));


        for (int x = 0; x <= w; x++)
        {
            Color pixColor = Color.HSVToRGB(x / w, 1, 1);
            for (int y = 0; y < h; y++)
            {
                hue_Sprite.texture.SetPixel(x, y, pixColor);
            }
        }
        hue_Sprite.texture.Apply();

        hue.sprite = hue_Sprite;
    }


    /// <summary>
    /// 更新色泽度 
    /// </summary>
    private void UpdateAlpha()
    {
        Color.RGBToHSV(currentHue, out currentHueHsv.x, out currentHueHsv.y, out currentHueHsv.z);
        Color color = Color.HSVToRGB(currentHueHsv.x, point_Stauration.anchoredPosition.x / saturation.rectTransform.sizeDelta.x, 1 + point_Stauration.anchoredPosition.y / saturation.rectTransform.sizeDelta.y);

        float w = 50, h = 10;

        alpha_Sprite = Sprite.Create(new Texture2D((int)w + 2, (int)h), new Rect(1, 0, w - 1, h), new Vector2(0, 0));

        for (int x = 0; x <= w; x++)
        {

            Color pixColor = new Color(color.r, color.g, color.b, x / w);
            for (int y = 0; y < h; y++)
            {
                alpha_Sprite.texture.SetPixel(x, y, pixColor);
            }
        }
        alpha_Sprite.texture.Apply();

        alpha.sprite = alpha_Sprite;
    }

    public void OnHueClick()
    {
        huePoint = hue.rectTransform.InverseTransformPoint(Input.mousePosition);
        var w = hue.rectTransform.sizeDelta.x;
        var x = Mathf.Clamp(huePoint.x, 0, w);
        point_Hue.anchoredPosition = new Vector2(x, 0);

        currentHue = Color.HSVToRGB(x / hue.rectTransform.sizeDelta.x, 1, 1);
        UpdateStauration();
        UpdateAlpha();
        UpdateColor();
    }

    public void OnAlphaClick()
    {
        alphaPoint = alpha.rectTransform.InverseTransformPoint(Input.mousePosition);
        var w = alpha.rectTransform.sizeDelta.x;
        var x = Mathf.Clamp(alphaPoint.x, 0, w);
        point_Alpha.anchoredPosition = new Vector2(x, 0);

        curAlpha = x / hue.rectTransform.sizeDelta.x;
        UpdateColor();
    }


    public void OnStaurationClick()
    {
        saturationPoint = saturation.rectTransform.InverseTransformPoint(Input.mousePosition);
        var size2 = saturation.rectTransform.sizeDelta;
        var pos = Vector2.zero;
        pos.x = Mathf.Clamp(saturationPoint.x, 0, size2.x);
        pos.y = Mathf.Clamp(saturationPoint.y, -size2.y, 0);
        point_Stauration.anchoredPosition = pos;
        UpdateAlpha();
        UpdateColor();
    }

    public void UpdateColor()
    {
        Color.RGBToHSV(currentHue, out currentHueHsv.x, out currentHueHsv.y, out currentHueHsv.z);
        Color color = Color.HSVToRGB(currentHueHsv.x, point_Stauration.anchoredPosition.x / saturation.rectTransform.sizeDelta.x, 1 + point_Stauration.anchoredPosition.y / saturation.rectTransform.sizeDelta.y);
        color.a = curAlpha;
        preImage.color = color;
        if (ChangeColorAction != null)
        {
            ChangeColorAction(color);
        }
    }


}
