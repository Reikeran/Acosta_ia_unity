using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;
[Serializable]
public class MinerRestState : FsmState<MinerBehaviour>
{
    [SerializeField, Range(0f, 20f)] private float wanderSpeed = 5f;
    [SerializeField, Range(0f, 20f)] private float circleWanderRadius = 2f;
    [SerializeField, Range(4, 64)] private int circleSearchPrecision = 16;
    [SerializeField] private float restDuration = 10f;
    private PathFinderAgent agent;
    private Vector3 wanderStartPoint;
    private float currentWanderAngle = 0f;

    
    private Vector3 GetNextDestination()
    {
        Vector3 nextDestination;

        float radians = currentWanderAngle * Mathf.Deg2Rad;
        float offsetX = Mathf.Cos(radians);
        float offsetZ = Mathf.Sin(radians);

        nextDestination = wanderStartPoint + new Vector3(offsetX, 0f, offsetZ) * circleWanderRadius;

        currentWanderAngle += 360f / circleSearchPrecision;

        if (currentWanderAngle > 360f)
            currentWanderAngle -= 360f;

        return nextDestination;
    }
    protected override void OnInitialize()
    {
        agent = owner.GetComponent<PathFinderAgent>();
    }
    public override void Enter()
    {
        wanderStartPoint = owner.transform.position;
        agent.MovementSpeed = wanderSpeed;
        agent.Destination = GetNextDestination();
        
        owner.StartRest(restDuration);

    }
    public override void Update()
    {
        if (agent.HasReachedDestination)
            agent.Destination = GetNextDestination();

        
    }
    public override void Exit()
    {
        owner.movingToHome = false;
        
    }
}
