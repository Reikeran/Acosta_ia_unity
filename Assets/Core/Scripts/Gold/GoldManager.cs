using System;

public static class GoldManager
{
    public static int TotalGold { get; private set; } = 0;

    public static event Action<int> OnGoldChanged;

    public static void AddGold(int amount)
    {
        TotalGold += amount;
        OnGoldChanged?.Invoke(TotalGold);
    }

    public static void Reset()
    {
        TotalGold = 0;
        OnGoldChanged?.Invoke(TotalGold);
    }
}

