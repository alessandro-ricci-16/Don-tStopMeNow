using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CameraParameters", menuName = "ScriptableObjects/CameraParameters")]
public class CameraParameters : ScriptableObject
{
    
    public float defaultAcceleration = 40;
    public float maxAcceleration = 60;
    
    [Header("Soft and dead zones")]
    public float deadZoneX = 2;
    public float deadZoneY = 8;
    public float softZoneX = 4;
    public float softZoneY = 10;
}
