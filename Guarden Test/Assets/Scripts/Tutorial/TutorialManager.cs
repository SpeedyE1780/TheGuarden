using System.Collections;
using System.Collections.Generic;
using TheGuarden.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace TheGuarden.Tutorial
{
    internal class TutorialManager : MonoBehaviour
    {
        [SerializeField, Tooltip("List of tutorials")]
        private List<Tutorial> tutorials = new List<Tutorial>();

        public UnityEvent OnTutorialCompleted;

        private void Start()
        {
            StartCoroutine(RunTutorials());
        }

        private IEnumerator RunTutorials()
        {
            foreach (Tutorial tutorial in tutorials)
            {
                yield return tutorial.StartTutorial();
                GameLogger.LogInfo($"{tutorial.name} completed", this, GameLogger.LogCategory.Tutorial);
            }

            GameLogger.LogInfo("TUTORIAL COMPLETED", this, GameLogger.LogCategory.Tutorial);
            OnTutorialCompleted.Invoke();
        }
    }
}
