using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseButtonBehavior : MonoBehaviour {
    public GameObject button;
    public GameObject model;
    public RectTransform panelTransform;
    public Image panelImage;

    protected Vector3 targetPanelScale;
    protected Vector3 startingPanelScale;
    protected Vector3 targetModelPos;
    protected Vector3 startingModelPos;
    protected Color panelColor;
    protected float percentModelExpandLerp;
    protected float percentPanelExpandLerp;
    protected float percentPanelFadeLerp;

    protected readonly float panelExpandSpeed = 2.5f;
    protected readonly float modelExpandSpeed = 2.5f;
    protected readonly float fadeSpeed = 1f;

    public BaseButtonBehavior() {
        startingPanelScale = new Vector3(0f, 0f, 0f);
        targetPanelScale = new Vector3(0.3f, 0.6f, 0.5f);
    }

    protected void Reset() {
        panelTransform.localScale = new Vector3(0f, 0f, 0f);
        startingModelPos = model.transform.localPosition;
        percentModelExpandLerp = 0;
        percentPanelExpandLerp = 0;

        // Fading
        panelColor = panelImage.color;
        panelColor.a = 0;
        panelImage.color = panelColor;
    }

    // easing functions
    protected static float Hermite(float start, float end, float value) {
        return Mathf.Lerp(start, end, value * value * (3.0f - 2.0f * value));
    }

    protected static Vector3 Hermite(Vector3 start, Vector3 end, float value) {
        return new Vector3(Hermite(start.x, end.x, value), Hermite(start.y, end.y, value), Hermite(start.z, end.z, value));
    }

    protected IEnumerator ExpandModel() {
        bool isExpanding = true;

        while (isExpanding) {
            percentModelExpandLerp += modelExpandSpeed * Time.deltaTime;
            model.transform.localPosition = Hermite(startingModelPos, targetModelPos, percentModelExpandLerp);

            if (percentModelExpandLerp >= 1) isExpanding = false;

            yield return null;
        }
        yield return null;
    }

    // this only fades the panel, not components within panel
    protected IEnumerator FadePanel() {
        bool isFading = true;
        float targetAlpha = 0.4f;

        while (isFading) {
            panelColor.a += fadeSpeed * Time.deltaTime;

            if (panelColor.a > targetAlpha) {
                panelColor.a = targetAlpha;
                isFading = false;
            }

            panelImage.color = panelColor;
            yield return null;
        }
        yield return null;
    }

    protected IEnumerator ExpandPanel() {
        bool isExpanding = true;

        while (isExpanding) {
            percentPanelExpandLerp += panelExpandSpeed * Time.deltaTime;
            panelTransform.localScale = Hermite(startingPanelScale, targetPanelScale, percentPanelExpandLerp);

            if (percentPanelExpandLerp >= 1) isExpanding = false;

            yield return null;
        }
        yield return null;
    }

    public abstract void OnClick();

}
