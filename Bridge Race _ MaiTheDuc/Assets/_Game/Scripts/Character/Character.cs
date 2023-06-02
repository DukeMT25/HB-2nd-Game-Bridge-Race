using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    [SerializeField] protected Animator _anim;
    [SerializeField] Transform Stack;
    [SerializeField] protected float moveSpeed;
    [SerializeField] Transform Ray;
    [SerializeField] protected BrickType _brickType;

    public BrickType _BrickType { get => _brickType; }

    public bool IsPaused { get; set; }

    [SerializeField] int stageIndex;
    public int StageIndex { get => stageIndex; set => stageIndex = value; }

    public List<BrickController> BrickList = new List<BrickController>();

    public StateMachine StateMachine { get; set; }

    protected Vector3 startPosition;

    protected virtual void Start()
    {
        OnInit();
    }

    protected virtual void Update()
    {
        //if (StateMachine.CurrentState != null && !IsPaused)
        //{
            StateMachine.CurrentState.Tick();
        //}
    }    

    public virtual void OnInit()
    {
        UIManager.onNextLevel += UIManager_onNextLevel;

        ClearBricks();
        StateMachine = new StateMachine();

        stageIndex = 1;

        startPosition = transform.position;
    } 

    public virtual void OnNewStage()
    {
        stageIndex++;
    }

    //PLayer
    protected void RotateTowards(GameObject gameObject, Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        BrickController brick = other.GetComponent<BrickController>();
        if (brick != null && brick._brickType == _brickType)
        {
            AddBrick(brick);
        }
               
    }

    protected void PullDown()
    {
        Vector3 dwn = transform.TransformDirection(Vector3.down);
        RaycastHit hit;
        Debug.DrawRay(transform.position, dwn * 10f);
        if (Physics.Raycast(Ray.transform.position, dwn, out hit, 5f))
        {
            if(!hit.transform.GetComponent<EndBridge>())
            {
                transform.position = hit.point;
            }
        }
    }
    
    protected void WallDetect(Collider other)
    {
        Vector3 dwn = transform.TransformDirection(Vector3.down);
        RaycastHit hit;
        LayerMask wall = other.gameObject.layer;
        //LayerMask.GetMask
        if (Physics.Raycast(transform.position, dwn, out hit, 10f))
        {
            transform.position = hit.point;
        }
    }

    protected virtual void AddBrick(BrickController brick)
    {
        BrickList.Add(brick);
        brick.transform.SetParent(Stack, false);
        brick.transform.localPosition = Vector3.up * 0.3f * BrickList.Count;
        brick.transform.localRotation = Quaternion.identity;
    }

    protected List<GameObject> sortListBuyDistance(List<GameObject> listObj)
    {
        //sap xep theo khoang cach gan nhat voi Enemy
        GameObject gameObject;
        for (int i = 0; i < listObj.Count - 1; i++)
        {
            for (int j = 0; j < listObj.Count; j++)
            {
                if (Vector3.Distance(transform.position, listObj[i].gameObject.transform.position) < Vector3.Distance(transform.position, listObj[j].gameObject.transform.position))
                {
                    gameObject = listObj[i];
                    listObj[i] = listObj[j];
                    listObj[j] = gameObject;
                }
            }
        }
        return listObj;
    }

    protected void ClearBricks()
    {
        if (BrickList.Count > 0)
        {
            foreach (var brick in BrickList)
            {
                brick.Release();
            }
        }

        BrickList.Clear();
    }

    private void UIManager_onNextLevel(object sender, System.EventArgs e)
    {
        NextLevel();
    }

    protected virtual void NextLevel()
    {
        stageIndex = 1;

        ClearBricks();
    }

    public virtual void Dance()
    {
        LevelManager.Instance.OnFinish();
    }
}
