using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyButton : BaseButtonBehavior {

    public BodyButton() : base() {}

    public override void OnClick() {
        bool isPanelActive = panelTransform.gameObject.activeInHierarchy;
        panelTransform.gameObject.SetActive(!isPanelActive);
        Reset();
        StartCoroutine(ExpandPanel());
    }
}
