using UnityEngine;

namespace Popieyes.AI
{
    public class VisionSensor : MonoBehaviour
    {
        [SerializeField] private VisionSettings settings;
        internal bool CanSeeTarget {get; private set;}

        public bool IsTargetInView(Vector3 targetPosition)
        {
            Vector3 directionToTarget = (targetPosition - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            if (distanceToTarget < settings.ViewRadius)
            {
                float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
                if (angleToTarget < settings.ViewAngle / 2)
                {
                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, settings.ObstacleMask))
                    {
                        CanSeeTarget = true;
                        return true;
                    }
                }
            }
            CanSeeTarget = false;
            return false;
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, settings.ViewRadius);

            Vector3 viewAngleA = DirectionFromAngle(-settings.ViewAngle / 2, false);
            Vector3 viewAngleB = DirectionFromAngle(settings.ViewAngle / 2, false);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + viewAngleA * settings.ViewRadius);
            Gizmos.DrawLine(transform.position, transform.position + viewAngleB * settings.ViewRadius);
        }
        public Vector3 DirectionFromAngle(float angleInDegrees, bool isGlobal)
        {
            if (!isGlobal)
            {
                angleInDegrees += transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}
