using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendShapesManager : MonoBehaviour
{

    public float extrusionAmount { get; set; }
    public Renderer[] rend;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public GameObject go;

    void Update()
    {
        go = GameObject.Find("challenger_head");
        skinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
        skinnedMeshRenderer.SetBlendShapeWeight(0, 100);
    }
}