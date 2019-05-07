using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseRotate : MonoBehaviour {
    public GameObject button;
    public GameObject model;
    protected bool isDown = false;

    protected readonly float rotationSpeed = 150f;

    protected IEnumerator RotateModel(float rotationSpeed) {
        while (isDown) {
            //rotate model about y axis to the left
            model.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        yield return null;
    }

    public void OnUp(BaseEventData b) {
        isDown = false;
    }
}
