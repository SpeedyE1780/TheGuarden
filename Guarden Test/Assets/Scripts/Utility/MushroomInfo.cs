using UnityEngine;

namespace TheGuarden.Utility
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Mushrooms/Mushroom Info")]
    public class MushroomInfo : ScriptableObject
    {
        [SerializeField]
        private string mushroomName;
        [SerializeField, Multiline]
        private string mushroomDescription;
        [SerializeField]
        private Sprite mushroomSprite;

        public string Name => mushroomName;
        public string Description => mushroomDescription;
        public Sprite Sprite => mushroomSprite;
    }
}
