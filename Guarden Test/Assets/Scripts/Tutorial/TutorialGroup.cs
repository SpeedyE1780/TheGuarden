using System.Collections;
using System.Collections.Generic;
using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.Tutorial
{
    [CreateAssetMenu(menuName ="Scriptable Objects/Tutorial/Group")]
    internal class TutorialGroup : Tutorial
    {
        [SerializeField, Tooltip("List of tutorials")]
        private List<Tutorial> tutorials = new List<Tutorial>();

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
