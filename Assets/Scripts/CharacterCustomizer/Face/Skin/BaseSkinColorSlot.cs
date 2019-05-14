using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseSkinColorSlot : MonoBehaviour
{
    public GameObject model;
    public Image button;
    protected Color buttonColor;
    protected Transform trans;  // transform component of hair/beard

    // match color with color of selected panel
    public void Awake()
    {
        buttonColor = button.transform.GetChild(0).GetComponent<Image>().color;
        //trans = model.transform.GetChild(4);

    }
}
