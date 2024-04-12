using UnityEngine;

public static class Tags
{
    public const string Plant = "Plant";
    public const string PlantBehavior = "PlantBehavior";
    public const string PlantSoil = "PlantSoil";
    public const string Enemy = "Enemy";
    public const string Bucket = "Bucket";
    public const string PlantBuff = "PlantBuff";

    public static bool HasTag(GameObject gameObject, params string[] tags)
    {
        foreach (string tag in tags)
        {
            if (gameObject.CompareTag(tag))
            {
                return true;
            }
        }

        return false;
    }
}
