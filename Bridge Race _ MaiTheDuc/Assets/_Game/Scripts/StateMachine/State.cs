using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    protected Character character;
    protected Animator anim;

    private int animName;

    public State(Character character, Animator anim, int animName)
    {
        this.character = character;
        this.anim = anim;
        this.animName = animName;
    }

    public virtual void Enter()
    {
        anim.SetBool(animName, true);
    }

    public virtual void Tick()
    {

    }

    public virtual void Exit()
    {
        anim.SetBool(animName, false);
    }
}
