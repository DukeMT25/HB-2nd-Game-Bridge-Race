using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Win : State
{
    private PlayerController player;

    public P_Win(Character character, Animator anim, int animName, PlayerController player) : base(character, anim, animName)
    {
        this.player = player;
    }

    public override void Enter()
    {
        base.Enter();

        player.IsDance = true;
    }

    public override void Exit()
    {
        base.Exit();

        player.IsDance = false;
    }

    public override void Tick()
    {
        base.Tick();
    }
}
