using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairButton : MonoBehaviour {
    public GameObject hairBtn;
    public GameObject model;
    public RectTransform hairPanelTransform;
    private Vector3 targetPanelScale = new Vector3(0.3f, 0.6f, 0.5f);
    private Vector3 startingPanelScale = new Vector3(0f, 0f, 0f);
    private Vector3 targetModelPos = new Vector3(450.4f, 167f, -833.7f);
    private Vector3 startingModelPos;
    private float percentModelLerp;
    private float percentPanelLerp;

    private readonly float panelExpandSpeed = 2.5f;
    private readonly float modelExpandSpeed = 2.5f;

    private void Reset() {
        hairPanelTransform.localScale = new Vector3(0f,0f,0f);
        startingModelPos = model.transform.localPosition;
        percentModelLerp = 0;
        percentPanelLerp = 0;
    }

    public void OnClick() {
        bool isPanelActive = hairPanelTransform.gameObject.activeInHierarchy;
        hairPanelTransform.gameObject.SetActive(!isPanelActive);
        Reset();
        StartCoroutine(ExpandPanel());
        StartCoroutine(ExpandModel());
    }

    private IEnumerator ExpandModel() {
        bool isExpanding = true;

        while(isExpanding) {
            percentModelLerp += modelExpandSpeed * Time.deltaTime;
            model.transform.localPosition = Hermite(startingModelPos, targetModelPos, percentModelLerp);

            if (percentModelLerp >= 1) isExpanding = false;

            yield return null;
        }
        yield return null;
    }

    private IEnumerator ExpandPanel() {
        bool isExpanding = true;

        while(isExpanding) {
            percentPanelLerp += panelExpandSpeed * Time.deltaTime;
            hairPanelTransform.localScale = Hermite(startingPanelScale, targetPanelScale, percentPanelLerp);

            if (percentPanelLerp >= 1) isExpanding = false;

            yield return null;
        }
        yield return null;
    }

    private static float Hermite(float start, float end, float value) {
        return Mathf.Lerp(start, end, value * value * (3.0f - 2.0f * value));
    }

    private static Vector3 Hermite(Vector3 start, Vector3 end, float value) {
        return new Vector3(Hermite(start.x, end.x, value), Hermite(start.y, end.y, value), Hermite(start.z, end.z, value));
    }
}


