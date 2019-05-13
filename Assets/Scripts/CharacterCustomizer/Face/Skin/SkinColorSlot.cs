using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinColorSlot : BaseColorSlot
{
    public new void Awake()
    {
        base.Awake();
    }

    public void OnClick()
    {
        // select the hair renderer child game object
        //Transform t = trans.GetChild(4);
        //t.GetComponent<Renderer>().material.color = buttonColor;
        model.transform.Find("challenger_head").GetComponent<Renderer>().material.color = buttonColor;
        model.transform.Find("challenger_fingers").GetComponent<Renderer>().material.color = buttonColor;
    }
}
