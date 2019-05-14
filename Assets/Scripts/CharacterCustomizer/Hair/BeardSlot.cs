using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeardSlot : BaseSlot {
    public void Awake() {
        localPos = new Vector3(-0.04f, -1.458f, 0f);
        localAngles = new Vector3(0f, 92.1f, 0f);
        localScale = new Vector3(0.8f, 0.8f, 0.8f);
    }

    public void OnClick() {
        activeStyle = parent.transform.GetChild(1).gameObject;
        Destroy(activeStyle);
        activeStyle = Instantiate(assignedStyle);
        activeStyle.transform.SetParent(parent.transform);
        activeStyle.transform.SetSiblingIndex(1);
        activeStyle.name = "challenger_beard";
        ResetTransformation();
        activeStyle.SetActive(true);
    }
}
