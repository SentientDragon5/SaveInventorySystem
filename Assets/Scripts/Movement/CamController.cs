using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Copyright SentientDragon5 2022
 * Please Attribute
 * 
 * A simple orthographic camera controller.
 */

public class CamController : MonoBehaviour
{
    Vector3 offset = Vector3.one * 10;
    public Transform target;

    [ContextMenu("UPDATE")]
    void Update()
    {
        transform.position = target.position + offset;
        transform.LookAt(target.position + target.up);
    }
}
