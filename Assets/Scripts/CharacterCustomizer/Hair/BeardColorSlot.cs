using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeardColorSlot : BaseColorSlot {
    public new void Awake() {
        base.Awake();
    }

    public void OnClick() {
        // select the beard renderer child game object
        Transform t = trans.GetChild(1).GetChild(0);
        t.GetComponent<Renderer>().material.color = buttonColor;
    }
}
