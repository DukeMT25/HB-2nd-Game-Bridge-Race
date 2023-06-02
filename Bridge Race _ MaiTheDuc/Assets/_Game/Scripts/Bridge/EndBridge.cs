using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBridge : MonoBehaviour
{
    public static event EventHandler<OnAnyCharacterPassArgs> onAnyCharacterPass;

    public class OnAnyCharacterPassArgs : EventArgs
    {
        public BrickType characterBrickType;
        public int stageIndex;
    }

    private void OnTriggerEnter(Collider other)
    {
        Character _cha = other.GetComponent<Character>();

        if (_cha != null)
        {
            _cha.OnNewStage();

            onAnyCharacterPass?.Invoke(this, new OnAnyCharacterPassArgs()
            {
                characterBrickType = _cha._BrickType,
                stageIndex = _cha.StageIndex
            });

            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
