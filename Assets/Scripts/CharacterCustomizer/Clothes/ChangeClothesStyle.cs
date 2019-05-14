using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeClothesStyle : MonoBehaviour {
    public GameObject curClothingItem;
    public Texture assignedTexture;

    public void OnClick() {
        curClothingItem.GetComponent<Renderer>().material.mainTexture = assignedTexture;

    }
}
