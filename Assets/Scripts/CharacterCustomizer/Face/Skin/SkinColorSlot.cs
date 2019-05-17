using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinColorSlot : MonoBehaviour {
    public GameObject parent;
    public Texture assignedColor;

    public void OnClick() {
        Transform parentTransform = parent.transform;

        for(int i = 0; i < parentTransform.childCount; i++) {
            parentTransform.GetChild(i).GetComponent<Renderer>().sharedMaterial.mainTexture = assignedColor;
        }
    }
}
