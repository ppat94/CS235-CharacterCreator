using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeClothesColor : MonoBehaviour {
    public GameObject clothing;

    public void OnClick() {
        Debug.Log(clothing.GetComponent<Renderer>().material.color);
    }
}
