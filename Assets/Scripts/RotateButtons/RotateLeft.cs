using UnityEngine.EventSystems;

public class RotateLeft : BaseRotate {
    public void OnDown(BaseEventData b) {
        isDown = true;
        StartCoroutine(RotateModel(rotationSpeed));
    }
}
