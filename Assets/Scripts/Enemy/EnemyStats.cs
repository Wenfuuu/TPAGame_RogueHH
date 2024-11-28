public class EnemyStats
{
    public int MaxHP;
    public int CurrentHP;
    public int Attack;
    public int Defense;
    public int ZhenDrop;
    public int ExpDrop;

    public EnemyStats(int maxHP, int attack, int defense, int zhenDrop, int expDrop)
    {
        MaxHP = maxHP;
        CurrentHP = maxHP;
        Attack = attack;
        Defense = defense;
        ZhenDrop = zhenDrop;
        ExpDrop = expDrop;
    }

    public void DecreaseHealth(int damage)
    {
        CurrentHP -= damage;
    }
}