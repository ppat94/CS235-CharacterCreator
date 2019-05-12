using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSlot: MonoBehaviour {
    public GameObject assignedStyle;
    public GameObject activeStyle;
    public Transform modelTransform;

    protected void ResetTransform() {
        assignedStyle.transform.position = activeStyle.transform.position;
        assignedStyle.transform.eulerAngles = activeStyle.transform.eulerAngles;
        assignedStyle.transform.localScale = activeStyle.transform.localScale;
    }

    protected Transform FindParent() {
        return modelTransform.GetChild(0).GetChild(0);
    }

    protected void ModifyActiveStyle(string styleName) {
        ResetTransform();
        Destroy(activeStyle);
        activeStyle = Instantiate(assignedStyle);
        activeStyle.transform.SetParent(FindParent());
        activeStyle.name = styleName;
    }


}
