using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphLegs : MonoBehaviour
{

    public float extrusionAmount { get; set; }
    public Renderer[] rend;
    public GameObject[] gameObjects;

    void Update()
    {
        // rend.material.SetFloat("_fooVal", shininess);//Set float assignes the var in the shader
        //rend.material.SetFloat("_Amount", extrusionAmount);//Set float assignes the var in the shader

        gameObjects = GameObject.FindGameObjectsWithTag("Legs");
        foreach (GameObject go in gameObjects)
        {
            rend = go.GetComponentsInChildren<Renderer>();

            foreach (Renderer r in rend)
            {
                foreach (Material mat in r.materials)
                {
                    mat.shader = Shader.Find("Example/Normal Extrusion");
                    mat.SetFloat("_Amount", extrusionAmount);
                }
            }
        }
    }

    //public float extrusionAmount { get; set; }
    //public Renderer rend;

    //void Start()
    //{
    //    rend = GetComponent<Renderer>();
    //    rend.material.shader = Shader.Find("Example/Normal Extrusion");//Gets the Shader
    //}
    //void Update()
    //{
    //    // rend.material.SetFloat("_fooVal", shininess);//Set float assignes the var in the shader
    //    rend.material.SetFloat("_Amount", extrusionAmount);//Set float assignes the var in the shader
    //}
}