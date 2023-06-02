using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody),typeof(CapsuleCollider))]
public class PlayerController : Character
{
    [SerializeField] Rigidbody _rb;
    [SerializeField] FloatingJoystick _floatingJoystick;

    public Vector3 MoveDirection { get; set; }
    public bool CanMoveForward { get; set; }
    public bool CanMoveBackward { get; set; }
    public bool CanMoveLeft { get; set; }
    public bool CanMoveRight { get; set; }

    private bool isDance;
    public bool IsDance { get => isDance; set => isDance = value; }

    public P_Idle IdleState { get; set; }
    public P_Run RunState { get; set; }
    public P_Win WinState { get; set; }

    protected override void Start()
    {
        base.Start();
        CanMoveForward = true;
        CanMoveBackward = true;
        CanMoveLeft = true;
        CanMoveRight = true;
    }

    private void ConstrainPlayerMoveArea()
    {
        MoveDirection = new Vector3(_floatingJoystick.Horizontal, _rb.velocity.y, _floatingJoystick.Vertical) * moveSpeed * Time.deltaTime;

        if (!CanMoveForward)
        {
            float zDir = Mathf.Clamp(MoveDirection.z, -1, 0);
            MoveDirection = new Vector3(MoveDirection.x, MoveDirection.y, zDir);
        }

        if (!CanMoveBackward)
        {
            float zDir = Mathf.Clamp(MoveDirection.z, 0, 1);
            MoveDirection = new Vector3(MoveDirection.x, zDir);
        }

        if (!CanMoveRight)
        {
            float xDir = Mathf.Clamp(MoveDirection.x, -1, 0);
            MoveDirection = new Vector3(xDir, MoveDirection.y, MoveDirection.z);
        }

        if (!CanMoveLeft)
        {
            float xDir = Mathf.Clamp(MoveDirection.x, 0, 1);
            MoveDirection = new Vector3(xDir, MoveDirection.y, MoveDirection.z);;
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        IdleState = new P_Idle(this, _anim, Constraint.idleName, this);
        RunState = new P_Run(this, _anim, Constraint.runName, this);
        WinState = new P_Win(this, _anim, Constraint.danceName, this);

        StateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();
        ConstrainPlayerMoveArea();
        Moving();
        PullDown();
    }

    private void Moving()
    {
        //ConstrainPlayerMoveArea();
        transform.position += MoveDirection;
        if (MoveDirection != Vector3.zero)
        {
            RotateTowards(gameObject, MoveDirection);
        }
    }

    protected override void NextLevel()
    {
        base.NextLevel();

        transform.position = startPosition;
        MoveDirection = Vector3.zero;
        StateMachine.ChangeState(IdleState);
    }

    public override void Dance()
    {

        StateMachine.ChangeState(WinState);
    }
}
