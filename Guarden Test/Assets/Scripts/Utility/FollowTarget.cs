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
        [SerializeField, Tooltip("Default Target that camera should aim at when there are no players in the scene")]
        private Transform defaultTarget;
        [SerializeField, Tooltip("Maximum follow movement speed")]
        private float movementSpeed = 5.0f;
        [SerializeField, Tooltip("Snap to desired position when player joins/leaves")]
        private bool snapToPosition = true;

        private Vector3 center;
        private float boundsSize;

        public Camera Camera => followCamera;

        private void Awake()
        {
            offset = offset.normalized;
        }

        /// <summary>
        /// Calculate the bound encapsulating all targets center and extents
        /// </summary>
        private void CalculateCenter()
        {
            if (targets.Count == 0)
            {
                center = defaultTarget.position;
                boundsSize = 0.0f;
                return;
            }

            if (targets.Count > 0)
            {
                Bounds bounds = new Bounds(targets[0].position, Vector3.zero);

                foreach (Transform target in targets)
                {
                    bounds.Encapsulate(target.position);
                }

                center = bounds.center;
                boundsSize = bounds.size.magnitude;
            }
        }

        /// <summary>
        /// Calculate the desired camera position
        /// </summary>
        /// <returns>Return the desired camera position</returns>
        private Vector3 CalculateDesiredPosition()
        {
            CalculateCenter();
            float offsetMultiplier = MathExtensions.CalculateDistanceBasedOnFrustum(boundsSize, followCamera.aspect, followCamera.fieldOfView) + addedOffset;
            return center + (offset * offsetMultiplier);
        }

        /// <summary>
        /// Snap camera to desired position
        /// </summary>
        private void SnapToPosition()
        {
            if (snapToPosition && targets.Count != 0)
            {
                transform.position = CalculateDesiredPosition();
            }
        }

        /// <summary>
        /// Add target to camera and snap to new position
        /// </summary>
        /// <param name="player">Target camera needs to follow</param>
        public void AddTarget(Transform target)
        {
            targets.Add(target);
            SnapToPosition();
        }

        /// <summary>
        /// Remove target from camera and snap to new position
        /// </summary>
        /// <param name="target">Target camera is no longer following</param>
        public void RemoveTarget(Transform target)
        {
            targets.Remove(target);
            SnapToPosition();
        }

        private void FixedUpdate()
        {
            Vector3 desiredPosition = CalculateDesiredPosition();
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, movementSpeed * Time.deltaTime);
        }
    }
}
