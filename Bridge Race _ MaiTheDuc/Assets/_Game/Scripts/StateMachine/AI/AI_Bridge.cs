using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Bridge : State
{
    private AI ai;
    private NavMeshAgent agent;

    public AI_Bridge(Character character, Animator anim, int animName, AI ai) : base(character, anim, animName)
    {
        this.ai = ai;
        this.agent = ai.Agent;
    }

    public override void Enter()
    {
        base.Enter();

        Vector3 target = ai.GetBridgeStartPosition();

        agent.SetDestination(target);
    }

    public override void Exit()
    {
        base.Exit();
        agent.ResetPath();
    }

    public override void Tick()
    {
        base.Tick();

        if (ai.BrickList.Count <= 0)
        {
            ai.StateMachine.ChangeState(ai.IdleState);
        }
    }
}
