using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World2Cutscene : MonoBehaviour
{
    public float speed;


    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.right;
    }
}