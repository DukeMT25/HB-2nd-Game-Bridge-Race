using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

public class AI : Character
{
    //[SerializeField] Transform NavMeshMove;
    [SerializeField] public NavMeshAgent Agent { get; set; }
    [SerializeField] public float IdleTime { get; set; } = 3f;

    [SerializeField] int bridgeIndex = -1;
    public int BridgeIndex { get => bridgeIndex; set => bridgeIndex = value; }

    private List<BrickController> bricksToCollect = new List<BrickController>();

    public AI_Idle IdleState { get; set; }
    public AI_Collect CollectState { get; set; }
    public AI_Bridge BuildState { get; set; }
    public AI_Win WinState { get; private set; }

    protected override void Start()
    {
        OnInit();
    }

    public override void OnInit()
    {
        base.OnInit();

        Agent = GetComponent<NavMeshAgent>();

        IdleState = new AI_Idle(this, _anim, Constraint.idleName, this);
        CollectState = new AI_Collect(this, _anim, Constraint.runName, this);
        BuildState = new AI_Bridge(this, _anim, Constraint.runName, this);
        WinState = new AI_Win(this, _anim, Constraint.danceName);

        StartCoroutine(GetBrickList(_brickType));

        UIManager.onPlayGame += UIManager_onPlayGame;
        Win.onCharacterWin += WinTrigger_onCharacterWin;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    private void UIManager_onPlayGame(object sender, System.EventArgs e)
    {
        StartPlaying();
    }

    private void StartPlaying()
    {
        IsPaused = false;
        StateMachine.Initialize(IdleState);
    }

    private void WinTrigger_onCharacterWin(object sender, Win.OnCharacterWinArgs e)
    {
        Agent.ResetPath();
        StateMachine.ChangeState(IdleState);
        Agent.enabled = false;

        IsPaused = true;
    }

    protected override void AddBrick(BrickController brick)
    {
        base.AddBrick(brick);

        bricksToCollect.Remove(brick);

        //after picking up a brick, the enemy has a 40% chance to pick up another brick, otherwise he will idle
        int random = Random.Range(0, 100);
        if (random <= 75)
        {
            StateMachine.ChangeState(CollectState);
        }
        else
        {
            StateMachine.ChangeState(IdleState);
        }
    }

    public Vector3 GetClosestBrick()
    {
        float minDist = Mathf.Infinity;
        GameObject nearestBrick = null;

        if (bricksToCollect.Count <= 0)
        {
            return Vector3.zero;
        }

        foreach (var brick in bricksToCollect)
        {
            float distance = Vector3.Distance(brick.transform.position, transform.position);
            if (distance < minDist)
            {
                minDist = distance;
                nearestBrick = brick.gameObject;
            }
        }

        return nearestBrick.transform.position;
    }

    private IEnumerator GetBrickList(BrickType brickType)
    {
        while (bricksToCollect == null || bricksToCollect.Count <= 0)
        {
            //Debug.Log("Try get new brickList");
            yield return new WaitForSeconds(0.5f);

            bricksToCollect = PoolManager.Instance.GetBrickList(brickType);
        }
    }

    protected override void NextLevel()
    {
        base.NextLevel();

        transform.position = startPosition;
        Agent.enabled = true;
        bridgeIndex = -1;
        StateMachine.ChangeState(IdleState);

        GetNewBrickList();
        Debug.Log("Next Level");
        IsPaused = false;
    }

    public void MoveTo(Vector3 target)
    {
        Agent.SetDestination(target);
    }

    public override void OnNewStage()
    {
        base.OnNewStage();

        if (StageIndex >= 3)
        {
            MoveTo(LevelManager.Instance.winPos);
            return;
        }

        GetNewBrickList();

        StateMachine.ChangeState(IdleState);

        BridgeIndex = -1;


    }

    private void GetNewBrickList()
    {
        bricksToCollect.Clear();

        StartCoroutine(GetBrickList(_brickType));
    }

    public Vector3 GetBridgeStartPosition()
    {
        List<Vector3> bridgeStartPos = LevelManager.Instance.GetBridgeStartPositionList(StageIndex);

        if (bridgeIndex <= -1)
        {
            bridgeIndex = Random.Range(0, bridgeStartPos.Count);
        }

        return bridgeStartPos[bridgeIndex];
    }

    public override void Dance()
    {
        StateMachine.ChangeState(WinState);
    }
}
