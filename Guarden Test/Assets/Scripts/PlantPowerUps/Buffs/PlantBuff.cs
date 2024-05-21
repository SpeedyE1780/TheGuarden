using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.PlantPowerUps.Buffs
{
    /// <summary>
    /// Parent class of plants that apply buff to buffs inside trigger
    /// </summary>
    internal sealed class PlantBuff : PlantPowerUp
    {
        [SerializeField, Tooltip("Buff Modifier applied to IBuff")]
        private BuffModifier modifier;

        [SerializeField, Tooltip("Audio Source")]
        private AudioSource audioSource;
        

    /// <summary>
    /// Get buff from collider
    /// </summary>
    /// <param name="other">Object that entered power up trigger</param>
    /// <returns></returns>
    private IBuff GetIBuff(Collider other)
        {
            IBuff buff = other.GetComponent<IBuff>();

            if (buff == null)
            {
                GameLogger.LogError($"{other.name} has no buff component", this, GameLogger.LogCategory.PlantPowerUp);
            }

            return buff;
        }

        private void OnTriggerEnter(Collider other)
        {
            IBuff buff = GetIBuff(other);

            if (buff == null)
            {
                return;
            }

            modifier.AddAndApplyBuff(buff);

            audioSource.Play(); 
        }

        private void OnTriggerExit(Collider other)
        {
            IBuff buff = GetIBuff(other);

            if (buff == null)
            {
                return;
            }

            modifier.RemoveAndClearBuff(buff);
        }

        private void OnDisable()
        {
            GameLogger.LogInfo($"{name} picked up removing buff from affected objects", this, GameLogger.LogCategory.PlantPowerUp);
            modifier.RemoveAllBuff();
        }
    }
}
