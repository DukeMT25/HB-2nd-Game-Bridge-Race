using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public static event EventHandler onPlayGame;
    public static event EventHandler onNextLevel;

    [SerializeField] GameObject playUI;
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject finishUI;

    [SerializeField] Button playButton;
    [SerializeField] Button nextLevel;
    [SerializeField] Button retryButton;

    private void Start()
    {
        Win.onCharacterWin += WinTrigger_onCharacterWin;
        nextLevel.onClick.AddListener(NextLevel);
        playButton.onClick.AddListener(PlayGame);
        retryButton.onClick.AddListener(PlayGame);

        SwitchTo(mainMenuUI);
    }

    private void SwitchTo(GameObject ui)
    {
        DeactiveAll();
        ui.gameObject.SetActive(true);
    }

    private void DeactiveAll()
    {
        mainMenuUI.SetActive(false);
        finishUI.SetActive(false);
    }


    private void WinTrigger_onCharacterWin(object sender, Win.OnCharacterWinArgs e)
    {
        SwitchTo(finishUI);
    }

    private void PlayGame()
    {
        SwitchTo(playUI);
        onPlayGame?.Invoke(this, EventArgs.Empty);
    }

    private void NextLevel()
    {
        LevelManager.Instance.NextLevel();
        SwitchTo(playUI);
        onNextLevel?.Invoke(this, EventArgs.Empty);
    }
}
