using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// MushroomInfo contains tutorial info
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Mushrooms/Mushroom Info")]
    public class MushroomInfo : ScriptableObject
    {
        [SerializeField, Tooltip("Name that should appear")]
        private string mushroomName;
        [SerializeField, Multiline, Tooltip("Description explaining the mushroom power ups")]
        private string mushroomDescription;
        [SerializeField, Tooltip("Icon shown in window and inventory")]
        private Sprite mushroomSprite;

        public string Name => mushroomName;
        public string Description => mushroomDescription;
        public Sprite Sprite => mushroomSprite;
    }
}
