using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlendShapesManager : MonoBehaviour
{

    public SkinnedMeshRenderer skinnedMeshRenderer;
    public GameObject blendShapeGameObject;
    public Slider baseSlider;
    public Slider lipsSlider;
    public Slider scaredSlider;
    public Slider angryUpperSlider;
    public Slider angryLowerSlider;

    public void Update()
    {
        //Adds a listener to the main slider and invokes a method when the value changes.
        baseSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        lipsSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        scaredSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        angryUpperSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        angryLowerSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    // Invoked when the value of the slider changes.
    public void ValueChangeCheck()
    {
        skinnedMeshRenderer = blendShapeGameObject.GetComponent<SkinnedMeshRenderer>();
        skinnedMeshRenderer.SetBlendShapeWeight(0, baseSlider.value);
        skinnedMeshRenderer.SetBlendShapeWeight(1, lipsSlider.value);
        skinnedMeshRenderer.SetBlendShapeWeight(2, scaredSlider.value);
        skinnedMeshRenderer.SetBlendShapeWeight(3, angryUpperSlider.value);
        skinnedMeshRenderer.SetBlendShapeWeight(4, angryLowerSlider.value);
    }
}