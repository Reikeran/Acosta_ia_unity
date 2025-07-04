using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class MinerMoveState : FsmState<MinerBehaviour>
{
    [SerializeField, Range(0f, 20f)] private float MoveSpeed = 5f;
    [SerializeField] private float reachDistance = 0.5f;
    //[SerializeField] private List<MineralVein> allVeins = new List<MineralVein>();
    [SerializeField] private GameObject home;
    public Vector3 currentTargetPosition;
    private PathFinderAgent agent;
    private MineralVein targetVein;


    public void SetTargetToRandomFreeVein()
    {
        List<MineralVein> freeVeins = VeinManager.AllVeins.FindAll(vein => !vein.occupied);
        if (freeVeins.Count == 0)
        {
            Debug.LogWarning("No free veins available!");
            return;
        }

        owner.targetVein = freeVeins[UnityEngine.Random.Range(0, freeVeins.Count)];
        owner.targetVein.occupied = true;
        currentTargetPosition = owner.targetVein.transform.position;

    }

    public void SetTargetToHome()
    {

        if (owner.targetVein != null)
        {
            owner.targetVein.occupied = false;
            owner.targetVein = null;
        }

        currentTargetPosition = home.transform.position;
    }
    
    protected override void OnInitialize()
    {
        agent = owner.GetComponent<PathFinderAgent>();
    }
    public override void Enter()
    {
        if (owner.movingToHome)
        {
            SetTargetToHome();
        }
        else
        {
            SetTargetToRandomFreeVein();
        }
        
        agent.MovementSpeed = MoveSpeed;
        
        agent.Destination = currentTargetPosition;

        
    }
    public override void Update()
    {
        
        float sqrDist = (currentTargetPosition - owner.transform.position).sqrMagnitude;
        if (sqrDist <= reachDistance * reachDistance)
        {
            if (owner.movingToHome)
            {
                owner.OnHomeReached.Invoke();
            }
            else
            {
                owner.OnGoldReached.Invoke();
            }
        }
    }
    public override void Exit()
    {
       
    }
}
