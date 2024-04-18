using TheGuarden.NPC;

public abstract class PlantBuff : PlantPowerUp
{
    public abstract void ApplyBuff(Animal animal);
    public abstract void RemoveBuff(Animal animal);
}
