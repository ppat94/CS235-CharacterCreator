using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseHairSlotColorChanger : MonoBehaviour {
    public GameObject model;
    public Image button;
    protected Color buttonColor;

    public void Awake() {
        buttonColor = button.color;
    }

    public abstract void OnClick();

}
