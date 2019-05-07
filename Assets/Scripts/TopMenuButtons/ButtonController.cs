using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour {
    public static ButtonController instance;
    private static List<BaseButton> buttons = new List<BaseButton>();
    private static BaseButton prevButton = null;

    public void Awake() {
        instance = this;
    }

    public static void AddToList(BaseButton b) {
        buttons.Add(b);
    }

    public static void Deactivate(BaseButton button) {
        prevButton = button;
        foreach (BaseButton b in buttons) {
            instance.StartCoroutine(b.ShrinkPanel());
        }
    }

    public static BaseButton GetPrevButton() {
        return prevButton;
    }

}
