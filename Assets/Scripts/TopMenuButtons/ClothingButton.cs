
public class ClothingButton : BaseButton {

    public void Awake() {
        ButtonController.AddToList(this);
    }

    public void OnClick() {
        InitButton();
        if (!isPanelActive) return;

        if (modelIsZoomed) StartCoroutine(ZoomOutModel());

    }

}

