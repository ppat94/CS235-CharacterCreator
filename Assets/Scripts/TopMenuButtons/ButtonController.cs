using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour {
    public static ButtonController instance;
    private static List<BaseButton> buttons = new List<BaseButton>();   // might not be needed

    public void Awake() {
        instance = this;
    }

    public static void AddToList(BaseButton b) {
        buttons.Add(b);
    }

    public static void DeactivateOtherPanels(BaseButton button) {
        foreach (BaseButton b in buttons) {
            if (b == button) continue;
            else if (b.WasActive()) {  // only shrink the panels that were active
                b.DeactivatePanel();
                instance.StartCoroutine(b.ShrinkPanel());
                return;
            }
        }
    }


}
