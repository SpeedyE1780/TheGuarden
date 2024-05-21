using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace TheGuarden.Utility
{
    /// <summary>
    /// PooledVisualEffect plays VFX and and adds it to pool
    /// </summary>
    [RequireComponent(typeof(VisualEffect))]
    public class PooledVisualEffect : MonoBehaviour, IPoolObject
    {
        [SerializeField, Tooltip("Pooled VFX")]
        private VisualEffect vfx;
        [SerializeField, Tooltip("Pool this item is returned to")]
        private ObjectPool<PooledVisualEffect> pool;
        [SerializeField, Tooltip("Delay before added back to pool")]
        private float delay = 0.5f;

        private void OnEnable()
        {
            vfx.Play();
            StartCoroutine(ReturnToPool());
        }

        /// <summary>
        /// Deactivate gameobject
        /// </summary>
        public void OnEnterPool()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Activate gameobject
        /// </summary>
        public void OnExitPool()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Return object to pool after delay
        /// </summary>
        /// <returns></returns>
        private IEnumerator ReturnToPool()
        {
            yield return new WaitForSeconds(delay);
            pool.AddObject(this);
        }
    }
}
