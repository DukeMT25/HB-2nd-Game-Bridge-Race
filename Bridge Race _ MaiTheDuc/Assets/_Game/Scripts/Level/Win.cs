using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    public static event EventHandler<OnCharacterWinArgs> onCharacterWin;
    public class OnCharacterWinArgs : EventArgs
    {
        public Character character;
    }

    private void OnTriggerEnter(Collider other)
    {
        Character cha = other.GetComponent<Character>();
        if (cha)
        {
            onCharacterWin?.Invoke(this, new OnCharacterWinArgs { character = cha });
            cha.Dance();
        }
    }
}
