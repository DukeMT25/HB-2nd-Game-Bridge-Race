using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Idle : State
{
    private AI ai;

    private float idleTimer;

    public AI_Idle(Character character, Animator anim, int animName, AI ai) : base(character, anim, animName)
    {
        this.ai = ai;
    }

    public override void Enter()
    {
        base.Enter();

        idleTimer = ai.IdleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Tick()
    {
        base.Tick();

        idleTimer -= Time.deltaTime;

        if (idleTimer < 0f)
        {
            if (ai.BrickList.Count >= 15)
            {
                Debug.Log("ChangeTo Build");
                ai.StateMachine.ChangeState(ai.BuildState);
            }
            else
            {
                Debug.Log("ChangeTo CollectState");
                ai.StateMachine.ChangeState(ai.CollectState);
            }

        }

    }
}
