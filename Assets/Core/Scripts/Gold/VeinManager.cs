using System.Collections.Generic;
using UnityEngine;

public class VeinManager : MonoBehaviour
{
    public static List<MineralVein> AllVeins { get; private set; } = new List<MineralVein>();

    public static void RegisterVein(MineralVein vein)
    {
        if (!AllVeins.Contains(vein))
            AllVeins.Add(vein);
    }

    public static void UnregisterVein(MineralVein vein)
    {
        if (AllVeins.Contains(vein))
            AllVeins.Remove(vein);
    }
}
