
using UnityEngine;

public class HairButton : BaseButton {

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


