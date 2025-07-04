using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class MinerMiningState : FsmState<MinerBehaviour>
{
    private Quaternion initialPickaxeRotation;
    private Coroutine miningCoroutine;
    private float swingSpeed = 2f; 
    private float swingAngle = 90f;  
    private float swingTime = 0f;

    protected override void OnInitialize() { }

    public override void Enter()
    {
        owner.gold = 0;
        miningCoroutine = owner.StartCoroutine(MiningRoutine());
        if (owner.pickaxeTransform != null)
            initialPickaxeRotation = owner.pickaxeTransform.transform.localRotation;

        swingTime = 0f;
    }

    public override void Exit()
    {
        
        if (miningCoroutine != null)
        {
            owner.StopCoroutine(miningCoroutine);
            miningCoroutine = null;
        }
        if (owner.pickaxeTransform != null) { owner.pickaxeTransform.transform.localRotation = initialPickaxeRotation; }
        owner.targetVein.occupied = false;
        owner.targetVein = null;
    }

    public override void Update()
    {
        if (owner.pickaxeTransform != null)
        {
            swingTime += Time.deltaTime * swingSpeed;
            float angle = Mathf.Sin(swingTime) * swingAngle;

            Quaternion rotationDelta = Quaternion.Euler(angle, 0f, 0f);
            owner.pickaxeTransform.transform.localRotation = initialPickaxeRotation * rotationDelta;
        }
    }

    private IEnumerator MiningRoutine()
    {
        Debug.Log("Started mining...");

        while (owner.gold < owner.maxGold)
        {
            yield return new WaitForSeconds(owner.miningInterval);

            owner.gold++;
            owner.UpdateGoldText();
            Debug.Log($"Gold collected: {owner.gold}");

        }

        Debug.Log("Max gold reached. Leaving mine.");
        owner.movingToHome = true ;
        owner.OnGoldPicked.Invoke();
    }
}
