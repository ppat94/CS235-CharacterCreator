using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyButton : BaseButton {

    public void Awake() {
        ButtonController.AddToList(this);
    }

    public override void OnClick() {
        BaseButton prevButton = ButtonController.GetPrevButton();
        if (prevButton == this) return; // do nothing if same button as before

        ButtonController.Deactivate(this);
        panelTransform.gameObject.SetActive(true);
        StartCoroutine(ExpandPanel());

        if (prevButton == null) return;
        else if (prevButton is HairButton || prevButton is FaceButton) {
            StartCoroutine(ShrinkModel());
        }
    }
}
