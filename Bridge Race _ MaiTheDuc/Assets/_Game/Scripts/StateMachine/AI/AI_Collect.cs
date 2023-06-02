using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Collect : State
{
    private AI ai;
    private NavMeshAgent agent;

    public AI_Collect(Character character, Animator anim, int animName, AI ai) : base(character, anim, animName)
    {
        this.ai = ai;
        agent = ai.Agent;
    }

    public override void Enter()
    {
        base.Enter();

        Vector3 target = ai.GetClosestBrick();
        //Debug.Log(target);

        if (target == Vector3.zero)
        {
            ai.StateMachine.ChangeState(ai.BuildState);
            return;
        }

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
    }
}
