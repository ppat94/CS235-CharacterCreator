using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseRotate : MonoBehaviour {
    public GameObject button;
    public GameObject model;
    protected bool isDown = false;

    protected readonly float rotationSpeed = 150f;

    protected IEnumerator RotateModel(float turnSpeed) {
        while (isDown) {
            model.transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
            yield return null;
        }

        yield return null;
    }

    public void OnUp(BaseEventData b) {
        isDown = false;
    }
}
