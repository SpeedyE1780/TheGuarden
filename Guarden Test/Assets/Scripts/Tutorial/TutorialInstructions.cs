using System.Collections;
using System.Collections.Generic;
using TheGuarden.Utility;
using TheGuarden.Utility.Events;
using UnityEngine;

namespace TheGuarden.Tutorial
{
    /// <summary>
    /// Instructions is a list of instructions to show on screen
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorials/Instructions")]
    internal class TutorialInstructions : Tutorial
    {
        [SerializeField, Tooltip("List of instructions")]
        private List<Instruction> instructions = new List<Instruction>();
        [SerializeField, Tooltip("This tutorial's UI")]
        private ObjectSpawner ui;
        [SerializeField, Tooltip("Game Window Active event")]
        private GameEvent gameWindowActive;

        private InstructionUI instructionUI;
        private bool hideWindow = false;

        private void OnEnable()
        {
            HideTutorialInstruction.OnHide += OnHideWindow;
        }

        private void OnDisable()
        {
            HideTutorialInstruction.OnHide -= OnHideWindow;
        }

        /// <summary>
        /// Instantiate and hide ui
        /// </summary>
        internal override void Setup()
        {
            instructionUI = ui.SpawnedObject.GetComponent<InstructionUI>();
            instructionUI.gameObject.SetActive(false);
        }

        public void OnHideWindow()
        {
            hideWindow = true;
        }

        /// <summary>
        /// Show instructions one by one
        /// </summary>
        /// <returns></returns>
        internal override IEnumerator StartTutorial()
        {
            instructionUI.gameObject.SetActive(true);

            foreach (Instruction instruction in instructions)
            {
                hideWindow = false;
                gameWindowActive.Raise();
                instructionUI.SetText(instruction.GetInstructionMessage());
                yield return null;
                yield return new WaitUntil(() => hideWindow);
            }

           instructionUI.gameObject.SetActive(false);
        }
    }
}
