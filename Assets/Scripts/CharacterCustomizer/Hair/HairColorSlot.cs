using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HairColorSlot : BaseColorSlot {
    public new void Awake() {
        base.Awake();
    }

    public void OnClick() {
        // select the hair renderer child game object
        GameObject curStyle = parent.transform.GetChild(0).GetChild(0).gameObject;
        curStyle.GetComponent<Renderer>().material.color = buttonColor;
    }
}
