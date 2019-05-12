using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairSlot: MonoBehaviour {
    public GameObject assignedStyle;
    private GameObject activeHair;
    public Transform modelTransform;


    private void ResetTransform() {
        activeHair = modelTransform.GetChild(0).gameObject;
        assignedStyle.transform.position = activeHair.transform.position;
        assignedStyle.transform.eulerAngles = activeHair.transform.eulerAngles;
        assignedStyle.transform.localScale = activeHair.transform.localScale;
    }

    public void OnClick() {
        ResetTransform();
        Destroy(activeHair);
        activeHair = Instantiate(assignedStyle);
        activeHair.transform.SetParent(modelTransform);
        activeHair.transform.SetAsFirstSibling();
    }


}
