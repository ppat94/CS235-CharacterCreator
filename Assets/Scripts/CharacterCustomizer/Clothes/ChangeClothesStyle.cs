using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeClothesStyle : MonoBehaviour {
    public GameObject assignedBodyPart;
    public Texture assignedTexture;

    public void OnClick() {
        Transform assignedTransform = assignedBodyPart.transform;

        for(int i = 0; i < assignedTransform.childCount; i++) {
            assignedTransform.GetChild(i).GetComponent<Renderer>().sharedMaterial.mainTexture = assignedTexture;
        }
    }
}
