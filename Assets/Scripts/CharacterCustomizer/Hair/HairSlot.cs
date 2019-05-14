using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairSlot : BaseSlot {

    public void Awake() {
        localPos = new Vector3(-0.04f, -1.74f, 0f);
        localAngles = new Vector3(0f, 87f, 0f);
        localScale = new Vector3(0.95f, 0.95f, 0.95f);
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
