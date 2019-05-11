using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseButton : MonoBehaviour {
    public GameObject button;
    public GameObject model;
    public RectTransform panelTransform;

    protected Vector3 targetPanelScale = new Vector3(0.3f, 0.6f, 0.5f);
    protected Vector3 startingPanelScale = new Vector3(0f, 0f, 0f);
    protected Vector3 targetModelPos = new Vector3(2.71f, 0f, 0.44f);
    protected Vector3 startingModelPos = new Vector3(0.4f, 0f, 1.41f);
    protected Color panelColor;
    protected readonly float panelExpandSpeed = 2.5f;
    protected readonly float modelExpandSpeed = 2.5f;
    protected readonly float fadeSpeed = 1f;

    protected void Reset() {
        panelTransform.localScale = new Vector3(0f, 0f, 0f);
        startingModelPos = model.transform.localPosition;

    }

    protected IEnumerator ExpandModel() {
        float lerpPercent = 0;

        while (lerpPercent <= 1) {
            lerpPercent += modelExpandSpeed * Time.deltaTime;
            model.transform.localPosition = Hermite(startingModelPos, targetModelPos, lerpPercent);

            yield return null;
        }

        yield return null;
    }

    public IEnumerator ShrinkModel() {
        float lerpPercent = 0;

        while(lerpPercent <= 1) {
            lerpPercent += modelExpandSpeed * Time.deltaTime;
            model.transform.localPosition = Hermite(targetModelPos, startingModelPos, lerpPercent);

            yield return null;
        }
        yield return null;
    }

    public IEnumerator ShrinkPanel() {
        float lerpPercent = 0;

        while(lerpPercent <= 1) {
            lerpPercent += panelExpandSpeed * Time.deltaTime;
            panelTransform.localScale = Hermite(targetPanelScale, startingPanelScale, lerpPercent);

            yield return null;
        }
        yield return null;
    }

    protected IEnumerator ExpandPanel() {
        float lerpPercent = 0;

        while (lerpPercent <= 1) {
            lerpPercent += panelExpandSpeed * Time.deltaTime;
            panelTransform.localScale = Hermite(startingPanelScale, targetPanelScale, lerpPercent);

            yield return null;
        }
        yield return null;
    }

    // easing functions
    protected static float Hermite(float start, float end, float value) {
        return Mathf.Lerp(start, end, value * value * (3.0f - 2.0f * value));
    }

    protected static Vector3 Hermite(Vector3 start, Vector3 end, float value) {
        return new Vector3(Hermite(start.x, end.x, value), Hermite(start.y, end.y, value), Hermite(start.z, end.z, value));
    }

    public abstract void OnClick();

}
