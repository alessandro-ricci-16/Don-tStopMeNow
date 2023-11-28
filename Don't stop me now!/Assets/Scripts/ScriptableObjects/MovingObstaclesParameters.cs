using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Moving Obstacles Parameters", menuName = "ScriptableObjects/MovingObstaclesParameters")]
    public class MovingObstaclesParameters : ScriptableObject
    {
        [Header("Movement")]
        public float defaultSpeed = 5.0f;
        public float acceleration = 20.0f;
        [Tooltip("Must be > 0, minus sign is added in code")]
        public float deceleration = 20.0f;
    }
    
}

