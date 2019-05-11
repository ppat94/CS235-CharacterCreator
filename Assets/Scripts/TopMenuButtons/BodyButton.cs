using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyButton : BaseButton {

    public void Awake() {
        ButtonController.AddToList(this);
    }

    public void OnClick() {
        InitButton();
        if (!isPanelActive) return;

        if (modelIsZoomed) StartCoroutine(ZoomOutModel());
    }
}
