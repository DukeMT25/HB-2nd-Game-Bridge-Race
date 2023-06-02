using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public static LevelManager Instance { get; set; }

    public List<Vector3> bridgeStartPositionStageOne;
    public List<Vector3> bridgeStartPositionStageTwo;
    public List<Vector3> bridgeStartPositionStageThree;

    public Vector3 winPos;

    public List<Level> levelScriptableObjList;

    [SerializeField] private int currentLevel = -1;

    private GameObject levelInstance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //UIManager.Instance.OpenMainMenuUI();
        if (currentLevel == -1)
        {
            currentLevel = 0;
            SetLevelInfo(levelScriptableObjList[currentLevel]);
        }
    }

    public void OnStart()
    {
        GameManager.Instance.ChangeState(GameState.Gameplay);
    }

    public void OnFinish()
    {
        //UIManager.Instance.OpenFinishUI();
        GameManager.Instance.ChangeState(GameState.Finish);
    }

    public void NextLevel()
    {
        currentLevel = (currentLevel + 1) % levelScriptableObjList.Count;
        SetLevelInfo(levelScriptableObjList[currentLevel]);
    }

    public void LoadLevelX()
    {
        ResetLevel();
        //OnInit();
    }

    public void ResetLevel()
    {
        SetLevelInfo(levelScriptableObjList[currentLevel]);
    }

    private void SetLevelInfo(Level level)
    {
        if (levelInstance != null)
        {
            Destroy(levelInstance.gameObject);
        }

        levelInstance = Instantiate(level.levelPrefab, Vector3.zero, Quaternion.identity);

        bridgeStartPositionStageOne = level.bridgeStartPositionStageOne;
        bridgeStartPositionStageTwo = level.bridgeStartPositionStageTwo;
        bridgeStartPositionStageThree = level.bridgeStartPositionStageThree;

        winPos = level.winPos;

        PoolManager.Instance.SetBrickStartPosition(level.startPositionStageOne, level.startPositionStageTwo, level.startPositionStageTree);
    }

    public List<Vector3> GetBridgeStartPositionList(int stageIndex)
    {
        switch (stageIndex)
        {
            case 1:
                return bridgeStartPositionStageOne;

            case 2:
                return bridgeStartPositionStageTwo;

            case 3:
                return bridgeStartPositionStageThree;

            default:
                return null;
        }
    }
}
