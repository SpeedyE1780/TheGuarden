using UnityEngine;
using TheGuarden.Utility;

namespace TheGuarden.NPC
{
    /// <summary>
    /// RoadLane is a road representation where the trucks will start and end their delivery route
    /// </summary>
    internal class RoadLane : MonoBehaviour
    {
        [SerializeField, Tooltip("Start of lane")]
        private Transform start;
        [SerializeField, Tooltip("End of lane")]
        private Transform end;

        public Vector3 StartPosition => start.position;
        public Quaternion StartRotation => start.rotation;
        public Vector3 EndPosition => end.position;
        public float Length => Vector3.Distance(start.position, end.position);

#if UNITY_EDITOR
        private void OnValidate()
        {
            start = transform.Find("Start");
            end = transform.Find("End");

            if (start == null || end == null)
            {
                GameLogger.LogWarning("Lane has missing start/end point", gameObject, GameLogger.LogCategory.Scene);
            }
        }

        private void OnDrawGizmos()
        {
            if (start != null && end != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(start.position, 0.5f);
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(end.position, 0.5f);
            }
        }
#endif
    }
}
