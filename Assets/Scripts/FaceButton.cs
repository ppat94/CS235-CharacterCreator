using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceButton : BaseButtonBehavior {

    public FaceButton() : base() {
        targetModelPos = new Vector3(450.4f, 167f, -833.7f);
    }

    public override void OnClick() {
        bool isPanelActive = panelTransform.gameObject.activeInHierarchy;
        panelTransform.gameObject.SetActive(!isPanelActive);
        Reset();
        StartCoroutine(ExpandPanel());
        StartCoroutine(ExpandModel());
    }
}
