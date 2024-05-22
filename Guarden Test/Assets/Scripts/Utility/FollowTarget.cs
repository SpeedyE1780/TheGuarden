using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// FollowTarget makes sure the Camera follows and has all targets in view
    /// </summary>
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField, Tooltip("Camera following the targets")]
        private Camera followCamera;
        [SerializeField, Tooltip("Tower Defence Camera")]
        private Camera towerDefenceCamera;
        [SerializeField, Tooltip("UI Camera")]
        private Camera uiCamera;
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
        [SerializeField, Tooltip("Bounds of the game")]
        private Bounds followBounds;

        private Vector3 center;
        private float boundsSize;
        private Bounds bounds;

        public Camera FollowCamera => followCamera;
        public static Camera ActiveCamera { get; private set; }
        public static Camera UICamera { get; private set; }
        public static Vector3 Center { get; private set; }

#if UNITY_EDITOR
        internal Vector3 DefaultTargetPosition => defaultTarget != null ? defaultTarget.position : Vector3.zero;
        internal Bounds Bounds => followBounds;
#endif

        private void Awake()
        {
            offset = offset.normalized;
            bounds = new Bounds();
            UICamera = uiCamera;
        }

        /// <summary>
        /// Switch to follow camera
        /// </summary>
        public void SwitchToFollowCamera()
        {
            GameLogger.LogInfo("Activate Follow Camera", this, GameLogger.LogCategory.Scene);
            towerDefenceCamera.gameObject.SetActive(false);
            followCamera.gameObject.SetActive(true);
            ActiveCamera = followCamera;
        }

        /// <summary>
        /// Switch to tower defence camera
        /// </summary>
        public void SwitchToTowerDefenceCamera()
        {
            GameLogger.LogInfo("Activate Tower Defence Camera", this, GameLogger.LogCategory.Scene);
            followCamera.gameObject.SetActive(false);
            towerDefenceCamera.gameObject.SetActive(true);
            ActiveCamera = towerDefenceCamera;
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
                bounds.center = targets[0].position;
                bounds.size = Vector3.zero;

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
            Center = center;
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
                followCamera.transform.position = CalculateDesiredPosition();
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

            //Only check if xz are in bounds
            float y = desiredPosition.y;
            desiredPosition.y = 0;

            if (!followBounds.Contains(desiredPosition))
            {
                desiredPosition = followBounds.ClosestPoint(desiredPosition);
            }

            desiredPosition.y = y;

            followCamera.transform.position = Vector3.MoveTowards(followCamera.transform.position, desiredPosition, movementSpeed * Time.deltaTime);
        }

#if UNITY_EDITOR
        internal void UpdateOffset(Vector3 calculatedOffset)
        {
            offset = calculatedOffset;
            addedOffset = calculatedOffset.magnitude;
        }

        internal void MoveToDefault()
        {
            Vector3 intialOffset = offset;
            offset.Normalize();
            Vector3 position = CalculateDesiredPosition();
            followCamera.transform.position = position;
            offset = intialOffset;
        }
#endif
    }
}
