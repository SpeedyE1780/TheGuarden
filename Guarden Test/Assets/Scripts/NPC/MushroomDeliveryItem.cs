using TheGuarden.Interactable;
using TheGuarden.Utility;
using TheGuarden.Utility.Events;
using UnityEngine;

namespace TheGuarden.NPC
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Deliveries/Item/Mushroom")]
    internal class MushroomDeliveryItem : DeliveryItem<Mushroom>
    {
        [SerializeField]
        private MushroomInfo mushroomInfo;
        [SerializeField]
        private TGameEvent<MushroomInfo> unlockedMushroom;

        public override void OnUnlocked()
        {
            unlockedMushroom.Raise(mushroomInfo);
        }
    }
}
