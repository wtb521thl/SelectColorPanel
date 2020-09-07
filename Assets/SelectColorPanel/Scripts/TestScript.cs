using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {
    public SelectColorPanel selectColorPanel;
    public Color initColor;

	void Start () {
        selectColorPanel.SetValueByColor(initColor);

    }

}
