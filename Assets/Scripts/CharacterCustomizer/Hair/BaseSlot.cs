using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSlot : MonoBehaviour {
    public GameObject parent;
    public GameObject assignedStyle = null;
    public GameObject activeStyle;
    protected Vector3 localPos;
    protected Vector3 localAngles;
    protected Vector3 localScale;

    protected void ResetTransformation() {
        activeStyle.transform.localPosition = localPos;
        activeStyle.transform.localEulerAngles = localAngles;
        activeStyle.transform.localScale = localScale;
    }
}
