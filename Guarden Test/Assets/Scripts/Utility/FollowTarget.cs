using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// FollowTarget makes sure the Camera follows and has all targets in view
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField, Tooltip("Camera following the targets")]
        private Camera followCamera;
        [SerializeField, Tooltip("Targets that need to stay in camera view")]
        private List<Transform> targets;
        [SerializeField, Tooltip("Offset between the camera and the targets")]
        private Vector3 offset;
        [SerializeField, Tooltip("Minimum distance between the camera and the targets")]
        private float addedOffset = 15;

        private Vector3 center;
        private float boundsSize;

        public Camera Camera => followCamera;

        private void Awake()
        {
            offset = offset.normalized;
        }

        /// <summary>
        /// Add target to camera
        /// </summary>
        /// <param name="player">Target camera needs to follow</param>
        public void AddTarget(Transform target)
        {
            targets.Add(target);
        }

        /// <summary>
        /// Remove target from camera
        /// </summary>
        /// <param name="target">Target camera is no longer following</param>
        public void RemoveTarget(Transform target)
        {
            targets.Remove(target);
        }

        /// <summary>
        /// Calculate the bound encapsulating all targets center and extents
        /// </summary>
        private void CalculateCenter()
        {
            Bounds bounds = new Bounds(targets[0].position, Vector3.zero);

            foreach (Transform target in targets)
            {
                bounds.Encapsulate(target.position);
            }

            center = bounds.center;
            boundsSize = bounds.size.magnitude;
        }

        private void LateUpdate()
        {
            CalculateCenter();
            float offsetMultiplier = MathExtensions.CalculateDistanceBasedOnFrustum(boundsSize, followCamera.aspect, followCamera.fieldOfView) + addedOffset;
            transform.position = center + (offset * offsetMultiplier);
        }
    }
}
