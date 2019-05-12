using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HairColorSlot : MonoBehaviour {
    public GameObject model;
    public Image button;
    private Color buttonColor;

    // match color with color of selected panel
    public void Awake() {
        buttonColor = button.transform.GetChild(0).GetComponent<Image>().color;
    }

    public void OnClick() {
        Transform t = model.transform.GetChild(0).GetChild(0);
        t.GetComponent<Renderer>().material.color = buttonColor;
    }
}
