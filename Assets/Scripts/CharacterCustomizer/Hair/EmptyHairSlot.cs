using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyHairSlot : MonoBehaviour {
    public GameObject activeStyle;
    public GameObject parent;

    public void OnClick() {
        activeStyle = parent.transform.GetChild(0).gameObject;
        activeStyle.SetActive(false);
    }
}
