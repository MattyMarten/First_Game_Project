using System;

[Serializable]
public class RecruitStats
{
    public int health;
    public int strength;
    public int endurance;
    public int sense;
    public int stealth;

    public int GetTotal()
    {
        return health + strength + endurance + sense + stealth;
    }
}
