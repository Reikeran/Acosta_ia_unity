using System;
using System.Collections;
using UnityEngine;
[Serializable]
public class MinerUnloadingGoldState : FsmState<MinerBehaviour>
{
    private Coroutine unloadingCoroutine;

    protected override void OnInitialize() { }

    public override void Enter()
    {
        unloadingCoroutine = owner.StartCoroutine(UnloadGoldRoutine());
    }

    public override void Exit()
    {
        if (unloadingCoroutine != null)
        {
            owner.StopCoroutine(unloadingCoroutine);
            unloadingCoroutine = null;
        }
    }

    public override void Update()
    {
    }

    private IEnumerator UnloadGoldRoutine()
    {
        Debug.Log("Unloading gold...");

        int goldToUnload = owner.gold;
        float delayPerGold = 0.5f; 

        while (owner.gold > 0)
        {
            yield return new WaitForSeconds(delayPerGold);

            owner.gold--;
            owner.UpdateGoldText();
            GoldManager.AddGold(1);

            Debug.Log($"Unloaded 1 gold. Remaining: {owner.gold}");
        }

        Debug.Log("Finished unloading.");
        owner.OnGoldUnloaded.Invoke();
    }
}
