using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairSlot : BaseSlot {
    public void OnClick() {
        activeStyle = modelTransform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        ModifyActiveStyle("challenger_hair");
        activeStyle.transform.SetAsFirstSibling();
    }
}
