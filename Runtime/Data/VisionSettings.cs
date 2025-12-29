using UnityEngine;

namespace Popieyes.AI
{
    [CreateAssetMenu(fileName = "VisionSettings", menuName = "AI/VisionSettings")]
    public class VisionSettings : ScriptableObject
    {
        public float ViewRadius = 10f;
        [Range(0, 360)]
        public float ViewAngle = 90f;
        public LayerMask TargetMask;
        public LayerMask ObstacleMask;
        
    }
}
