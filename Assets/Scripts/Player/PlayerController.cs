using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Animator myAnim;
    Rigidbody2D myRigid;
    PlayerInput myInput;
    Collider2D myColli;
    PlayerSensors mySensors;

    public Animator MyAnim { get { return myAnim; } }
    public Rigidbody2D MyRigid { get { return myRigid; } }
    public Collider2D MyColli { get { return myColli; } }
    public PlayerInput MyInput { get { return myInput; } }
    public PlayerSensors MySensors { get { return mySensors; } }

    LayerMask groundLayerMask;
    LayerMask buttonLayerMask;

    public LayerMask GetGroundLayerMask() { return groundLayerMask; }
    public LayerMask ButtonLayerMask() { return buttonLayerMask; }

    PlayerBaseState currentState;
    private Dictionary<PlayerStateType, PlayerBaseState> states = new Dictionary<PlayerStateType, PlayerBaseState>();

    [SerializeField] private PlayerStateType currentStateType = PlayerStateType.Idle;
    public PlayerStateType StateType { get { return currentStateType; } } 

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    public float MoveSpeed { get { return moveSpeed; } }
    public float JumpForce { get { return jumpForce; } }

    private void Start()
    {
        myAnim = GetComponent<Animator>();
        myRigid = GetComponent<Rigidbody2D>();
        myColli = GetComponent<Collider2D>();
        myInput = GetComponent<PlayerInput>();

        InitStates();

        groundLayerMask = LayerMask.GetMask("Ground");
        buttonLayerMask = LayerMask.GetMask("Button");
    }

    private void InitStates()
    {
        states.Add(PlayerStateType.Idle, new PlayerIdleState());
        states.Add(PlayerStateType.Move, new PlayerMoveState());
        states.Add(PlayerStateType.Air, new PlayerAirState());
        states.Add(PlayerStateType.Death, new PlayerDeathState());
        states.Add(PlayerStateType.Menu, new PlayerMenuState());
        states.Add(PlayerStateType.Clear, new PlayerClearState());

        currentState = states[PlayerStateType.Idle];
        currentState.OnEnter(this);
    }

    public void SwitchState(PlayerStateType newState)
    {
        currentState.OnExit();
        currentState = states[newState];
        currentStateType = newState;
        currentState.OnEnter(this);
    }

    private void Update()
    {
        currentState.OnUpdate();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            currentState.OnMoveStarted(context.ReadValue<Vector2>());
        }
        else if(context.performed)
        {
            currentState.OnMovePerformed(context.ReadValue<Vector2>());
        }
        else if (context.canceled)
        {
            currentState.OnMoveCanceled();
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            currentState.OnJump();
        }
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            currentState.OnInteract();
        }
    }

    public void OnRestartInput(InputAction.CallbackContext context)
    {
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
