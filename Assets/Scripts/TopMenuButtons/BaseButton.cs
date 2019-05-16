using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseButton : MonoBehaviour {
    public GameObject button;
    public GameObject model;
    public RectTransform panelTransform;
    protected bool isPanelActive = false;
    protected static bool modelIsZoomed = false;
    private readonly static Vector3 defaultCamPos = new Vector3(3.55f, 1.8498f, 0.84f); // need different camPos values since
    private readonly static Vector3 walkingCamPos = new Vector3(4.05f, 1.65f, 0.8392f); // animation changes stance of the character
    protected static Vector3 targetPanelScale = new Vector3(0.25f, 0.45f, 0.3f);
    protected static Vector3 startingPanelScale = new Vector3(0f, 0f, 0f);
    protected static Vector3 targetCamPos = defaultCamPos;
    protected static Vector3 startingCamPos = new Vector3(7.02f, 1.62f, 0.07f);

    protected readonly float panelExpandSpeed = 2.5f;
    protected readonly float camMovementSpeed = 2.5f;

    protected void Reset() {
        panelTransform.localScale = new Vector3(0f, 0f, 0f);
    }

    public static void ResetCamPos() {
        targetCamPos = defaultCamPos;
        MoveCamToTargetPos();
    }

    public static void SetCamPosWalking() {
        targetCamPos = walkingCamPos;
        MoveCamToTargetPos();
    }

    private static void MoveCamToTargetPos() {  // moves camera if animation was changed
        if(modelIsZoomed) Camera.main.transform.position = targetCamPos;
    }

    public IEnumerator ShrinkPanel() {
        float lerpPercent = 0;
        while (lerpPercent <= 1) {
            lerpPercent += panelExpandSpeed * Time.deltaTime;
            panelTransform.localScale = Hermite(targetPanelScale, startingPanelScale, lerpPercent);

            yield return null;
        }
        panelTransform.gameObject.SetActive(false);
        yield return null;
    }

    public IEnumerator ZoomInModel() {
        float lerpPercent = 0;
        modelIsZoomed = true;
        while (lerpPercent <= 1) {
            lerpPercent += camMovementSpeed * Time.deltaTime;
            Camera.main.transform.position = Hermite(startingCamPos, targetCamPos, lerpPercent);
            yield return null;
        }

        yield return null;
    }

    public IEnumerator ZoomOutModel() {
        float lerpPercent = 0;
        modelIsZoomed = false;
        while (lerpPercent <= 1) {
            lerpPercent += camMovementSpeed * Time.deltaTime;
            Camera.main.transform.position = Hermite(targetCamPos, startingCamPos, lerpPercent);
            yield return null;
        }

        yield return null;
    }

    protected IEnumerator ExpandPanel() {
        panelTransform.gameObject.SetActive(true);

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


    public void DeactivatePanel() {
        isPanelActive = false;
    }

    public bool WasActive() {
        return isPanelActive;
    }

    protected void InitButton() {
        if (isPanelActive) {
            isPanelActive = false;
            StartCoroutine(ShrinkPanel());
            if (this is HairButton || this is FaceButton)
                StartCoroutine(ZoomOutModel());
            return;
        }

        isPanelActive = true;
        ButtonController.DeactivateOtherPanels(this);
        StartCoroutine(ExpandPanel());

    }

}



