using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceButton : BaseButton {

    public void Awake() {
        ButtonController.AddToList(this);
    }

    public override void OnClick() {
        BaseButton prevButton = ButtonController.GetPrevButton();
        if (prevButton == this) return; // do nothing if same button as before

        ButtonController.Deactivate(this);
        panelTransform.gameObject.SetActive(true);
        StartCoroutine(ExpandPanel());

        if (prevButton == null) StartCoroutine(ExpandModel());
        else if (prevButton == this || prevButton is HairButton) return;
        else StartCoroutine(ExpandModel());
    }
}
