[System.Serializable]
public class PlayerData
{
    public int playerLevel;
    public int hpLevel;
    public int hpCost;
    public int atkLevel;
    public int atkCost;
    public int defLevel;
    public int defCost;
    public int crLevel;
    public int crCost;
    public int cdLevel;
    public int cdCost;
    public int unlockFloor;
    public int currentZhen;
    public int hp;
    public int maxhp;
    public int exp;
    public int maxexp;
    public int atk;
    public int def;
    public int cr;
    public int cd;

    public PlayerData(PlayerStatsSO playerStats)
    {
        playerLevel = playerStats.PlayerLevel;
        exp = playerStats.CurrentEXP;
        maxexp = playerStats.MaxEXP;
        hpLevel = playerStats.HPLevel;
        hpCost = playerStats.HPCost;
        atkLevel = playerStats.AtkLevel;
        atkCost = playerStats.AtkCost;
        defLevel = playerStats.DefLevel;
        defCost = playerStats.DefCost;
        crLevel = playerStats.CRLevel;
        crCost = playerStats.CRCost;
        cdLevel = playerStats.CDLevel;
        cdCost = playerStats.CDCost;
        unlockFloor = playerStats.UnlockedFloor;
        currentZhen = playerStats.Zhen;
        hp = playerStats.CurrentHP;
        maxhp = playerStats.MaxHP;
        atk = playerStats.Attack;
        def = playerStats.Defense;
        cr = playerStats.CritRate;
        cd = playerStats.CritDamage;
    }

    public void ApplyTo(PlayerStatsSO playerStats)
    {
        playerStats.PlayerLevel = playerLevel;
        playerStats.CurrentEXP = exp;
        playerStats.MaxEXP = maxexp;
        playerStats.HPLevel = hpLevel;
        playerStats.HPCost = hpCost;
        playerStats.AtkLevel = atkLevel;
        playerStats.AtkCost = atkCost;
        playerStats.DefLevel = defLevel;
        playerStats.DefCost = defCost;
        playerStats.CRLevel = crLevel;
        playerStats.CRCost = crCost;
        playerStats.CDLevel = cdLevel;
        playerStats.CDCost = cdCost;
        playerStats.UnlockedFloor = unlockFloor;
        playerStats.Zhen = currentZhen;
        playerStats.CurrentHP = hp;
        playerStats.MaxHP = maxhp;
        playerStats.Attack = atk;
        playerStats.Defense = def;
        playerStats.CritRate = cr;
        playerStats.CritDamage = cd;
    }
}