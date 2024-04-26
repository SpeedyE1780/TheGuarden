using System.Collections;
using System.Collections.Generic;
using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.Tutorial
{
    /// <summary>
    /// TutorialGroup group multiple tutorials together
    /// </summary>
    [CreateAssetMenu(menuName ="Scriptable Objects/Tutorials/Group")]
    internal class TutorialGroup : Tutorial
    {
        [SerializeField, Tooltip("List of tutorials")]
        private List<Tutorial> tutorials = new List<Tutorial>();

        /// <summary>
        /// Run each tutorial sequentially
        /// </summary>
        /// <returns></returns>
        internal override IEnumerator StartTutorial()
        {
            foreach (Tutorial tutorial in tutorials)
            {
                yield return tutorial.StartTutorial();
                GameLogger.LogInfo($"{name}:{tutorial.name} completed", this, GameLogger.LogCategory.Tutorial);
            }
        }
    }
}
