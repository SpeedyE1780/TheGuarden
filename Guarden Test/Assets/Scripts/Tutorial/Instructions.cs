using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Tutorial
{
    /// <summary>
    /// Instructions is a list of instructions to show on screen
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorials/Instructions")]
    internal class Instructions : Tutorial
    {
        [SerializeField, Multiline, Tooltip("List of instructions")]
        private List<string> instructions = new List<string>();
        [SerializeField, Tooltip("This tutorial's UI")]
        private InstructionUI ui;

        private InstructionUI instructionUI;

        /// <summary>
        /// Instantiate and hide ui
        /// </summary>
        internal override void Setup()
        {
            instructionUI = Instantiate(ui);
            instructionUI.gameObject.SetActive(false);
        }

        /// <summary>
        /// Show instructions one by one
        /// </summary>
        /// <returns></returns>
        internal override IEnumerator StartTutorial()
        {
            instructionUI.gameObject.SetActive(true);

            foreach (string instruction in instructions)
            {
                instructionUI.SetText(instruction);
                yield return null;
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            }

            Destroy(instructionUI.gameObject);
        }
    }
}
