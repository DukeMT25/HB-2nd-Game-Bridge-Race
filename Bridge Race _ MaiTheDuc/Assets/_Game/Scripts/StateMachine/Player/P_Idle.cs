using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Idle : State
{
    private PlayerController player;

    public P_Idle(Character character, Animator anim, int animName, PlayerController player) : base(character, anim, animName)
    {
        this.player = player;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Tick()
    {
        base.Tick();

        if (player.MoveDirection != Vector3.zero)
        {
            player.StateMachine.ChangeState(player.RunState);
        }
    }
}
