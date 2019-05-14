using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairSlot : BaseSlot {

    public void Awake() {
        localPos = new Vector3(-0.05f, -1.64f, 0f);
        localAngles = new Vector3(0f, 89.1f, 0f);
        localScale = new Vector3(0.9f, 0.9f, 0.9f);
    }

    public void OnClick() {
        activeStyle = parent.transform.GetChild(0).gameObject;
        Destroy(activeStyle);
        activeStyle = Instantiate(assignedStyle);
        activeStyle.transform.SetParent(parent.transform);
        activeStyle.transform.SetAsFirstSibling();
        activeStyle.name = "challenger_hair";
        ResetTransformation();
        activeStyle.SetActive(true);
    }
}
