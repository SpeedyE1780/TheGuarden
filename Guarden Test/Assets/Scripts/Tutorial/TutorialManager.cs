using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField, Tooltip("List of tutorials")]
        private List<Tutorial> tutorials = new List<Tutorial>();

        private void Start()
        {
            StartCoroutine(RunTutorials());
        }

        private IEnumerator RunTutorials()
        {
            foreach (Tutorial tutorial in tutorials)
            {
                yield return tutorial.StartTutorial();
            }
        }
    }
}
