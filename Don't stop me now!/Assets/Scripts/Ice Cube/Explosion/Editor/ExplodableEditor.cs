using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
/*
[CustomEditor(typeof(Explodable))]
public class ExplodableEditor : Editor {

    public override void OnInspectorGUI()
    {
        Explodable myTarget = (Explodable)target;
        myTarget.shatterType = (Explodable.ShatterType)EditorGUILayout.EnumPopup("Shatter Type", myTarget.shatterType);
        myTarget.extraPoints = EditorGUILayout.IntField("Extra Points", myTarget.extraPoints);
        myTarget.forceMagnitude = EditorGUILayout.FloatField("Force Magnitude", myTarget.forceMagnitude);
        myTarget.subshatterSteps = EditorGUILayout.IntField("Subshatter Steps",myTarget.subshatterSteps);
        if (myTarget.subshatterSteps > 1)
        {
            EditorGUILayout.HelpBox("Use subshatter steps with caution! Too many will break performance!!! Don't recommend more than 1", MessageType.Warning);
        }

        myTarget.fragmentLayer = EditorGUILayout.TextField("Fragment Layer", myTarget.fragmentLayer);
        myTarget.sortingLayerName = EditorGUILayout.TextField("Sorting Layer", myTarget.sortingLayerName);
        myTarget.orderInLayer = EditorGUILayout.IntField("Order In Layer", myTarget.orderInLayer);
        
        
    }
}*/
