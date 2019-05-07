using UnityEngine.EventSystems;

public class RotateRight : BaseRotate {
    public void OnDown(BaseEventData b) {
        isDown = true;
        StartCoroutine(RotateModel(-rotationSpeed));
    }
}
