using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseColorSlot : MonoBehaviour {
    public GameObject parent;
    public Image button;
    protected Color buttonColor;

    // match color with color of selected panel
    public void Awake() {
        buttonColor = button.transform.GetChild(0).GetComponent<Image>().color;
    }
}
