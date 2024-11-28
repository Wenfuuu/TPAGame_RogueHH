using UnityEngine;

public static class EnemyFactory
{
    private const int BaseMaxHP = 20;
    private const int BaseAttack = 10;
    private const int BaseDefense = 10;
    private const int BaseZhenDrop = 50;
    private const int BaseExpDrop = 50;

    public static EnemyStats CreateEnemy(string type)
    {
        float multiplier = GetMultiplier(type);

        int maxHP = Mathf.RoundToInt(BaseMaxHP * multiplier);
        int attack = Mathf.RoundToInt(BaseAttack * multiplier);
        int defense = Mathf.RoundToInt(BaseDefense * multiplier);
        int zhenDrop = Mathf.RoundToInt(BaseZhenDrop * multiplier);
        int expDrop = Mathf.RoundToInt(BaseExpDrop * multiplier);

        //Debug.Log("enemy with type " + type + " got atk: " + attack);

        return new EnemyStats(maxHP, attack, defense, zhenDrop, expDrop);
    }

    private static float GetMultiplier(string type)
    {
        switch (type)
        {
            case "Medium":
                return 1.5f;
            case "Elite":
                return 2f;
            default:
                return 1f;
        }
    }
}