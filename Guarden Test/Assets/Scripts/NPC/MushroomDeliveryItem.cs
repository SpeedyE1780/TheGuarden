using TheGuarden.Interactable;
using TheGuarden.Utility;
using TheGuarden.Utility.Events;
using UnityEngine;

namespace TheGuarden.NPC
{
    /// <summary>
    /// Mushroom Delivery Item that will show tutorial once unlocked
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Deliveries/Item/Mushroom")]
    internal class MushroomDeliveryItem : DeliveryItem<Mushroom>
    {
        [SerializeField, Tooltip("Mushrooms tutorial info")]
        private MushroomInfo mushroomInfo;
        [SerializeField, Tooltip("On mushroom unlocked game event")]
        private TGameEvent<MushroomInfo> unlockedMushroom;

        /// <summary>
        /// Raise unlockedMushroom event
        /// </summary>
        public override void OnUnlocked()
        {
            unlockedMushroom.Raise(mushroomInfo);
        }
    }
}
