using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Animator myAnim;
    private Rigidbody2D myRigid;
    private PlayerInput myInput;
    private Collider2D myColli;
    private PlayerSensors mySensors;
    public Animator MyAnim { get { return myAnim; } }
    public Rigidbody2D MyRigid { get { return myRigid; } }
    public Collider2D MyColli { get { return myColli; } }
    public PlayerInput MyInput { get { return myInput; } }
    public PlayerSensors MySensors { get { return mySensors; } }

    private LayerMask groundLayerMask;
    private LayerMask buttonLayerMask;
    public LayerMask GetGroundLayerMask() { return groundLayerMask; }
    public LayerMask ButtonLayerMask() { return buttonLayerMask; }

    private PlayerBaseState currentState;
    private Dictionary<PlayerStateType, PlayerBaseState> states = new Dictionary<PlayerStateType, PlayerBaseState>();
    [SerializeField] private PlayerStateType currentStateType = PlayerStateType.Idle;
    public PlayerStateType StateType { get { return currentStateType; } } 

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;
    public float MoveSpeed { get { return moveSpeed; } }
    public float JumpForce { get { return jumpForce; } }

    private Vector2 inputDir;
    public Vector2 InputDir { get { return inputDir; } }

    private Collider2D goalCollider;
    public Collider2D GoalCollider { get { return goalCollider; } }

    private void Start()
    {
        myAnim = GetComponent<Animator>();
        myRigid = GetComponent<Rigidbody2D>();
        myColli = GetComponent<Collider2D>();
        myInput = GetComponent<PlayerInput>();
        mySensors = GetComponent<PlayerSensors>();

        InitStates();

        groundLayerMask = LayerMask.GetMask("Ground");
        buttonLayerMask = LayerMask.GetMask("Button");
    }

    private void InitStates()
    {
        states.Add(PlayerStateType.Idle, new PlayerIdleState());
        states.Add(PlayerStateType.Move, new PlayerMoveState());
        states.Add(PlayerStateType.Death, new PlayerDeathState());
        states.Add(PlayerStateType.Clear, new PlayerClearState());
        states.Add(PlayerStateType.AirJump, new PlayerAirJumpState());
        states.Add(PlayerStateType.AirFall, new PlayerAirFallState());

        currentState = states[PlayerStateType.Idle];
        currentState.OnEnter(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Trap"))
        {
            SwitchState(PlayerStateType.Death);
        }
    }

    public void SwitchState(PlayerStateType newState)
    {
        currentState.OnExit();
        currentState = states[newState];
        currentStateType = newState;
        currentState.OnEnter(this);
    }

    public void SetClearCollider(Collider2D coll)
    {
        goalCollider = coll;
    }

    //State패턴 코드
    //------------------------------

    private void Update()
    {
        currentState.OnUpdate();
    }

    private void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (UIManager.Instance.IsOpenIngameMenu) return;
        if (context.started)
        {
            inputDir = context.ReadValue<Vector2>();
            currentState.OnMove(inputDir);
        }
        if(context.canceled)
        {
            inputDir = Vector2.zero;
            currentState.OnMoveCanceled();
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (UIManager.Instance.IsOpenIngameMenu) return;
        if (context.started)
        {
            currentState.OnJump();
        }
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (UIManager.Instance.IsOpenIngameMenu) return;
        if (context.started)
        {
            currentState.OnInteract();
        }
    }

    public void OnRestartInput(InputAction.CallbackContext context)
    {
        if (UIManager.Instance.IsOpenIngameMenu) return;
        if (context.started)
        {
            currentState.OnRestart();
        }
    }

    public void OnEscapeInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            currentState.OnEscape();
        }
    }

}
