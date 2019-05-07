using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RotateLeft : MonoBehaviour {
    public GameObject button;
    public GameObject model;
    private Vector3 startingRotation = new Vector3(0f, -171.4f, 0f);
    private readonly float rotationSpeed = 0.0001f;
    private bool isDown = false;

    public void Awake() {
 
    }

    public void OnDown(BaseEventData b) {
        isDown = true;

    }

    public void OnUp(BaseEventData b) {
        isDown = false;
    }

    public void Update() {
        if(isDown) {
            //model.transform.Rotate(new Vector3(0,0,90));
            float y = model.transform.rotation.eulerAngles.y;
            y += rotationSpeed * Time.deltaTime;
            model.transform.Rotate(new Vector3(0, y, 0));
        }
    }

}
