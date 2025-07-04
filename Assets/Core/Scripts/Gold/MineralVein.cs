using UnityEngine;

public class MineralVein : MonoBehaviour
{
    public bool occupied = false;



    void Awake()
    {
        VeinManager.RegisterVein(this);
    }

    void OnDestroy()
    {
        VeinManager.UnregisterVein(this);
    }

}
