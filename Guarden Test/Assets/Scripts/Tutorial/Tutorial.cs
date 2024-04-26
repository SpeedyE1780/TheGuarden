using System.Collections;
using UnityEngine;

namespace TheGuarden.Tutorial
{
    internal abstract class Tutorial : ScriptableObject
    {
        internal abstract IEnumerator StartTutorial();
    }
}
