using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.NPC
{
    /// <summary>
    /// RoadLane is a road representation where the trucks will start and end their delivery route
    /// </summary>
    internal class RoadLane : MonoBehaviour
    {
        [SerializeField, Tooltip("Autofilled. Start of lane")]
        private Transform start;
        [SerializeField, Tooltip("Autofilled. End of lane")]
        private Transform end;

        internal Vector3 StartPosition => start.position;
        internal Quaternion StartRotation => start.rotation;
        internal Vector3 EndPosition => end.position;
        internal float Length => Vector3.Distance(start.position, end.position);

#if UNITY_EDITOR
        internal Transform Start => start;
        internal Transform End => end;

        internal void AutofillVariables()
        {
            start = transform.Find("Start");
            end = transform.Find("End");

            if (start == null || end == null)
            {
                GameLogger.LogWarning("Lane has missing start/end point", gameObject, GameLogger.LogCategory.Scene);
            }
        }
#endif
    }
}
