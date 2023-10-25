using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "HeatableSettings", menuName = "ScriptableObjects/HeatableSettings")]
    public class HeatableSettings : ScriptableObject
    {
        public float recoveryScale;
        public float heatScale;
        public float maxTime;
    }
}