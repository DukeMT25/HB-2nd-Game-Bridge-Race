using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { MainMenu, Gameplay, Finish }

public class GameManager : Singleton<GameManager>
{
    private GameState _state;

    void Awake()
    {
        ChangeState(GameState.MainMenu);
    }

    public void ChangeState(GameState gameState)
    {
        _state = gameState;
    }

    public bool IsState(GameState gameState)
    {
        return _state == gameState;
    }
}
