using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Tutorial
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorial/Instructions")]
    internal class Instructions : Tutorial
    {
        [SerializeField, Multiline, Tooltip("List of instructions")]
        private List<string> instructions = new List<string>();
        [SerializeField, Tooltip("This tutorial's UI")]
        private InstructionUI ui;

        internal override IEnumerator StartTutorial()
        {
            InstructionUI instructionUI = Instantiate(ui);

            foreach (var instruction in instructions)
            {
                instructionUI.SetText(instruction);
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                yield return null;
            }

            Destroy(instructionUI.gameObject);
        }
    }
}
