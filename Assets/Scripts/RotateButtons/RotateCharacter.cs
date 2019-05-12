using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCharacter : MonoBehaviour
{
    public float angle;

    void OnMouseDrag()
    {
        float x = Input.GetAxis("Mouse X");
        transform.RotateAround(transform.position, new Vector3(0, 1, 0) * Time.deltaTime * -1 * x, angle);
    }
}
