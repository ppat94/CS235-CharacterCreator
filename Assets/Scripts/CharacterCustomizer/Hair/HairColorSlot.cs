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
        //Transform t = trans.GetChild(0).GetChild(0);
        //t.GetComponent<Renderer>().material.color = buttonColor;

    }
}
