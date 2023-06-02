using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickController : MonoBehaviour
{
    public enum BrickState
    {
        OnGround,
        IsCollected,
        Dropped
    }

    private BoxCollider _col;
    [SerializeField] MeshRenderer _mesh;
    [SerializeField] BrickColor BCScriptable;

    public BrickType _brickType = BrickType.Purple;
    public BrickType _Default = BrickType.Purple;

    private BrickState _brickState;
    public BrickState _BrickState
    {
        get => _brickState;
        set
        {
            _brickState = value;

            switch (_brickState)
            {
                case BrickState.OnGround:
                    break;

                case BrickState.IsCollected:
                    _col.enabled = false;
                    break;

                case BrickState.Dropped:
                    StartCoroutine(SetupDroppedBrick());
                    break;

                default:
                    break;
            }
        }
    }

    private IEnumerator SetupDroppedBrick()
    {
        _mesh.material = BCScriptable.GetMaterial(BrickType.Gray);
        gameObject.tag = "GrayBrick";

        yield return new WaitForSeconds(0.1f);

        _col.enabled = true;
    }

    //???
    private void OnValidate()
    {
        SetupBrickColor();
    }

    private void SetupBrickColor()
    {
        //gameObject.tag = brickType.ToString();

        _mesh.material = BCScriptable.GetMaterial(_brickType);
    }

    public ObjectPool<BrickController> Pool { get; set; }

    void Start()
    {
        //StartCoroutine(ResetBrick(0f));

        //
        _BrickState = BrickState.OnGround;
        _Default = _brickType;
    }

    public void Release()
    {
        SetupBrick(_Default);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        Pool.ReturnToPool(this);
    }

    public void SetupBrick(BrickType brickType)
    {
        _brickType = brickType;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public IEnumerator ResetBrick(float timeDelay)
    //{
    //    yield return new WaitForSeconds(timeDelay);
    //    _mesh.gameObject.SetActive(true);
    //    this.GetComponent<BoxCollider>().enabled = true;
    //    _brickType = (BrickType)Random.Range(0, 4);
    //    _mesh.material = BCScriptable.Mats[(int)_brickType];
    //}

    //public void DestroyBrick()
    //{
    //    _mesh.gameObject.SetActive(false);
    //    this.GetComponent<BoxCollider>().enabled = false;
    //    StartCoroutine(ResetBrick(5f));
    //}

    //public void ChangeColor(BrickType brickType)
    //{
    //    _mesh.material = BCScriptable.Mats[(int)brickType];
    //}
}
