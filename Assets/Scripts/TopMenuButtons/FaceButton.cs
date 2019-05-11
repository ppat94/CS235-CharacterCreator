using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceButton : BaseButton {

    public void Awake() {
        ButtonController.AddToList(this);
    }

    public void OnClick() {
        InitButton();
        if (!isPanelActive) return;

        if (modelIsZoomed) return;
        StartCoroutine(ZoomInModel());
    }
}
