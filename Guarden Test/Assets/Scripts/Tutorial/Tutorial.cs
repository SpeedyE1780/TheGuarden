using System.Collections;
using UnityEngine;

namespace TheGuarden.Tutorial
{
    /// <summary>
    /// Tutorial is the base class of all tutorials
    /// </summary>
    internal abstract class Tutorial : ScriptableObject
    {
        /// <summary>
        /// Do any necessary setup for the tutorial
        /// </summary>
        abstract internal void Setup();

        /// <summary>
        /// Start tutorial and wait until completed
        /// </summary>
        /// <returns></returns>
        internal abstract IEnumerator StartTutorial();
    }
}
