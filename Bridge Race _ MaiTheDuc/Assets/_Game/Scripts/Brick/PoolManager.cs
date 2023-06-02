using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; set; }

    [SerializeField] int NoBrick_color = 11;
    [SerializeField] int unit = 2;

    [SerializeField] BrickPool green;
    [SerializeField] BrickPool red;
    [SerializeField] BrickPool purple;
    [SerializeField] BrickPool yellow;

    private BrickPool GetBrickPoolByType(BrickType type)
    {
        switch (type)
        {
            case BrickType.Green:
                return green;

            case BrickType.Red:
                return red;

            case BrickType.Purple:
                return purple;

            case BrickType.Yellow:
                return yellow;
            default:
                return null;
        }
    }

    private List<BrickController> redBricks = new List<BrickController>();
    private List<BrickController> greenBricks = new List<BrickController>();
    private List<BrickController> purpleBricks = new List<BrickController>();
    private List<BrickController> yellowBricks = new List<BrickController>();

    private List<BrickController> bricks = new List<BrickController>();

    private bool isPlaced;

    private bool[,] hasBrickStageOne = new bool[10, 9];
    private bool[,] hasBrickStageTwo = new bool[10, 9];
    private bool[,] hasBrickStageThree = new bool[10, 9];

    private bool[,] GetCheckArray(int stage)
    {
        switch (stage)
        {
            case 1:
                return hasBrickStageOne;

            case 2:
                return hasBrickStageTwo;

            case 3:
                return hasBrickStageThree;

            default:
                return null;
        }
    }

    [SerializeField] Vector3 startPositionStageOne;
    [SerializeField] Vector3 startPositionStageTwo;
    [SerializeField] Vector3 startPositionStageTree;

    private Vector3 GetStartPositionByStage(int stageIndex)
    {
        switch (stageIndex)
        {
            case 1:
                return startPositionStageOne;

            case 2:
                return startPositionStageTwo;

            case 3:
                return startPositionStageTree;

            default:
                return Vector3.one * -1;
        }
    }

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
        EndBridge.onAnyCharacterPass += EndBridge_onAnyCharacterPass;

        UIManager.onNextLevel += UIManager_onNextLevel;

        Invoke(nameof(PlaceBricks), 0.5f);
    }

    private void UIManager_onNextLevel(object sender, System.EventArgs e)
    {
        ReleaseAllBrick();

        Invoke(nameof(PlaceBricks), 0.1f);
    }

    public void SetBrickStartPosition(Vector3 stageOne, Vector3 stageTwo, Vector3 stageThree)
    {
        startPositionStageOne = stageOne;
        startPositionStageTwo = stageTwo;
        startPositionStageTree = stageThree;
    }

    private void EndBridge_onAnyCharacterPass(object sender, EndBridge.OnAnyCharacterPassArgs characterPassArgs)
    {
        //Debug.Log("Doi stage chay");

        ReleaseAllBrickOfType(characterPassArgs.characterBrickType);

        SpawnBrickOnNewStage(characterPassArgs);
    }

    private void ReleaseAllBrickOfType(BrickType brickType)
    {
        foreach (var brick in GetBrickList(brickType))
        {
            if(brick != null)
                brick.Release();
        }
    }

    public void ReleaseAllBrick()
    {
        for (int i = 0; i < 4; i++)
        {
            ReleaseAllBrickOfType((BrickType)i);
        }
    }

    private void SpawnBrickOnNewStage(EndBridge.OnAnyCharacterPassArgs e)
    {
        Debug.Log(e.stageIndex);
        if (e.stageIndex >= 3) return;

        List<BrickController> list = GetBrickList(e.characterBrickType);
        list.Clear();

        isPlaced = false;

        BrickPool brickPool = GetBrickPoolByType(e.characterBrickType);
        Vector3 startPos = GetStartPositionByStage(e.stageIndex);
        bool[,] checkArray = GetCheckArray(e.stageIndex);
        Debug.Log(NoBrick_color);

        for (int i = 0; i < NoBrick_color; i++)
        {
            BrickController newBrick = brickPool.GetPooledObject();
            newBrick.transform.SetParent(null);

            int randomX = 0;
            int randomY = 0;

            while (checkArray[randomX, randomY])
            {
                randomX = Random.Range(0, 10);
                randomY = Random.Range(0, 9);
            }

            newBrick.transform.position = startPos + new Vector3(randomX, 0f, randomY) * unit;
            newBrick.transform.rotation = Quaternion.identity;
            checkArray[randomX, randomY] = true;

            list.Add(newBrick);
            Debug.Log(newBrick.transform.position);
        }

        isPlaced = true;
    }

    private void PlaceBricks()
    {
        for (int i = 0; i < NoBrick_color; i++)
        {
            BrickController newBrick = green.GetPooledObject();
            newBrick.transform.rotation = Quaternion.identity;
            greenBricks.Add(newBrick);
            bricks.Add(newBrick);
        }

        for (int i = 0; i < NoBrick_color; i++)
        {
            BrickController newBrick = red.GetPooledObject();
            newBrick.transform.rotation = Quaternion.identity;
            redBricks.Add(newBrick);
            bricks.Add(newBrick);
        }

        for (int i = 0; i < NoBrick_color; i++)
        {
            BrickController newBrick = purple.GetPooledObject();
            newBrick.transform.rotation = Quaternion.identity;
            purpleBricks.Add(newBrick);
            bricks.Add(newBrick);
        }

        for (int i = 0; i < NoBrick_color; i++)
        {
            BrickController newBrick = yellow.GetPooledObject();
            newBrick.transform.rotation = Quaternion.identity;
            yellowBricks.Add(newBrick);
            bricks.Add(newBrick);
        }

        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                int randomIndex = Random.Range(0, bricks.Count);
                bricks[randomIndex].transform.position = startPositionStageOne + new Vector3(x, 0, y) * unit;
                bricks[randomIndex] = bricks[bricks.Count - 1];
                bricks.RemoveAt(bricks.Count - 1);

                hasBrickStageOne[x, y] = true;
            }
        }

        isPlaced = true;
    }

    public List<BrickController> GetBrickList(BrickType brickType)
    {
        switch (brickType)
        {
            case BrickType.Green:
                return GetGreenBrickList();

            case BrickType.Red:
                return GetRedBrickList();

            case BrickType.Purple:
                return GetPurpleBrickList();

            case BrickType.Yellow:
                return GetYellowBrickList();
            default:
                return null;
        }
    }

    public List<BrickController> GetGreenBrickList() => isPlaced ? greenBricks : null;
    public List<BrickController> GetRedBrickList() => isPlaced ? redBricks : null;
    public List<BrickController> GetPurpleBrickList() => isPlaced ? purpleBricks : null;
    public List<BrickController> GetYellowBrickList() => isPlaced ? yellowBricks : null;
}
