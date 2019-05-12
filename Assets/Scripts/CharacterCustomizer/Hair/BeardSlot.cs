using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeardSlot : BaseSlot {
    public void OnClick() {
        activeStyle = modelTransform.GetChild(0).GetChild(0).GetChild(1).gameObject;
        ModifyActiveStyle("challenger_beard");
        activeStyle.transform.SetSiblingIndex(1);
    }

}
