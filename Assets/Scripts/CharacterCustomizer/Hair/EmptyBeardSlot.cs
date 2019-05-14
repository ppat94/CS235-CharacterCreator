using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyBeardSlot : MonoBehaviour {
    public GameObject activeStyle;
    public GameObject parent;

    public void OnClick() {
        activeStyle = parent.transform.GetChild(1).gameObject;
        activeStyle.SetActive(false);
    }
}
