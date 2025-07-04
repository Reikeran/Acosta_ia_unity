using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PathFinderAgent))]
public class MinerBehaviour : MonoBehaviour
{
    private FiniteStateMachine<MinerBehaviour> fsm;
    [Header("States")]
    [SerializeField] private MinerRestState restState = new MinerRestState();
    [SerializeField] private MinerMoveState moveState = new MinerMoveState();
    [SerializeField] private MinerMiningState miningState = new MinerMiningState();
    [SerializeField] private MinerUnloadingGoldState unloadingGoldState = new MinerUnloadingGoldState();

    [SerializeField] private GameObject goldBillboardPrefab;
    private TMP_Text goldTextInstance;

    [Header("Gold Settings")]
    public int gold = 0;
    public int maxGold = 5;
    public GameObject pickaxeTransform;
    public MineralVein targetVein;

    public float miningInterval = 1.5f;
    public bool movingToHome = false;
    public UnityEvent OnGoldReached { get; private set; } = new UnityEvent();
    public UnityEvent OnHomeReached { get; private set; } = new UnityEvent();
    public UnityEvent OnGoldPicked { get; private set; } = new UnityEvent();
    public UnityEvent OnGoldUnloaded { get; private set; } = new UnityEvent();
    public UnityEvent OnRestEnded { get; private set; } = new UnityEvent();
    void Awake()
    {
        restState.Initialize(this);
        moveState.Initialize(this);
        miningState.Initialize(this);
        unloadingGoldState.Initialize(this);
    }
    void Start()
    {
        FsmState<MinerBehaviour>[] states = { restState, moveState, miningState, unloadingGoldState };
        UnityEvent[] events = { OnGoldReached, OnHomeReached, OnGoldPicked, OnGoldUnloaded, OnRestEnded };

        fsm = new FiniteStateMachine<MinerBehaviour>(states, events, restState);

        fsm.ConfigureTransition(restState, moveState, OnRestEnded);
        fsm.ConfigureTransition(moveState, miningState, OnGoldReached);
        fsm.ConfigureTransition(miningState, moveState, OnGoldPicked);
        fsm.ConfigureTransition(moveState, unloadingGoldState, OnHomeReached);
        fsm.ConfigureTransition(unloadingGoldState, restState, OnGoldUnloaded);
        goldTextInstance = GetComponentInChildren<TMP_Text>();
        UpdateGoldText();

    }
    public void UpdateGoldText()
    {
        if (goldTextInstance != null)
            goldTextInstance.text = gold.ToString();
    }
    void Update()
    {
        fsm.Update();
    }
    public void StartRest(float duration)
    {
        StartCoroutine(RestCoroutine(duration));
    }

    private IEnumerator RestCoroutine(float duration)
    {
        Debug.Log("Miner is resting...");
        yield return new WaitForSeconds(duration);
        Debug.Log("Miner finished resting.");
        OnRestEnded.Invoke();
    }
}
