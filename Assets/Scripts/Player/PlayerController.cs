using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    Animator myAnim;
    Rigidbody2D myRigid;
    Collider2D myColli;
    PlayerInput myInput;

    [SerializeField] bool isAir;
    bool isFall;
    float halfColliderHeight;
    float halfColliderWidth;
    float offset = 0.05f;
    bool isDeath = false;
    bool isClear = false;

    public bool IsDeath { get { return isDeath; } }

    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float moveSpeed = 5f;

    string isRunString = "isRun";
    string isJumpString = "isJump";
    string isFallingString = "isFalling";
    string isAirString = "isAir";
    string deathString = "isDeath";
    string clearString = "isClear";

    LayerMask groundLayerMask;
    LayerMask buttonLayerMask;

    Vector2 moveDir;

    public event Action<bool> onDeath;


    private void Start()
    {
        myAnim = GetComponent<Animator>();
        myRigid = GetComponent<Rigidbody2D>();
        myColli = GetComponent<Collider2D>();
        myInput = GetComponent<PlayerInput>();


        halfColliderHeight = myColli.bounds.extents.y;
        halfColliderWidth = myColli.bounds.extents.x;
        groundLayerMask = LayerMask.GetMask("Ground");
        buttonLayerMask = LayerMask.GetMask("Button");
    }

    private void Update()
    {
        if(isDeath || isClear) return;
        //GroundCheck();
        ApplyMovement();
    }

    private void FixedUpdate()
    {
        CheckStuck();
        GroundCheck();
    }

    private void GroundCheck()
    {
        bool[] isRayHit = new bool[3];
        Vector3 center = myColli.bounds.center;
        for (int i = -1; i < 2; i++)
        {
            center = new Vector3(myColli.bounds.center.x + halfColliderWidth * i, center.y, center.z);
            isRayHit[i + 1] = Physics2D.Raycast(center, Vector2.down, halfColliderHeight + offset, groundLayerMask) || 
                Physics2D.Raycast(center, Vector2.down, halfColliderHeight + offset, buttonLayerMask);
        }

        if (isRayHit[0] || isRayHit[1] || isRayHit[2])
        {
            isAir = false;
            isFall = false;
        }
        else
        {
            if (myRigid.linearVelocityY < 0)
            {
                isFall = true;
            }
            else
            {
                isFall = false;
            }
            isAir = true;
        }
        myAnim.SetBool(isAirString, isAir);
        myAnim.SetBool(isFallingString, isFall);
    }

    private void CheckStuck()
    {
        if (isAir) return;
        if(isDeath || isClear) return;
        Vector3 center = myColli.bounds.center;
        bool isStucked = false;

        var coli = Physics2D.OverlapBox(myColli.bounds.center, myColli.bounds.size * 0.5f, 0f, groundLayerMask);
        if (coli != null) //가끔 벽 안으로 들어가는 버그를 막기 위한 코드
        {
            isStucked = true;
        }

        for (int i = -1; i < 2; i++)
        {
            if (isStucked) break;
            Vector3 rayOrigin = new Vector3(center.x + halfColliderWidth * i, center.y, center.z);

            RaycastHit2D hitUp = Physics2D.Raycast(rayOrigin, Vector2.up, halfColliderHeight, groundLayerMask);
            RaycastHit2D hitDown = Physics2D.Raycast(rayOrigin, Vector2.down, halfColliderHeight, groundLayerMask);
            Debug.DrawRay(rayOrigin, Vector2.up * halfColliderHeight, Color.red);
            Debug.DrawRay(rayOrigin, Vector2.down * halfColliderHeight, Color.red);

            if (hitUp.collider != null && hitDown.collider != null)
            {
                if(hitUp.rigidbody?.linearVelocityY < 0)
                {
                    if(hitDown.rigidbody?.linearVelocityY >= 0)
                    {
                        isStucked = true;
                        hitUp.rigidbody.linearVelocityY = 0;
                        hitUp.rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                    }
                    
                }
                if (hitDown.rigidbody?.linearVelocityY > 0)
                {
                    if(hitUp.rigidbody?.linearVelocityY <= 0)
                    {
                        isStucked = true;
                        hitDown.rigidbody.linearVelocityY = 0;
                        hitDown.rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                    }
                }
            }
        }

        for (int i = -1; i < 2; i++)
        {
            if (isStucked) break;
            Vector3 rayOrigin = new Vector3(center.x, center.y + halfColliderHeight * i, center.z);

            RaycastHit2D hitLeft = Physics2D.Raycast(rayOrigin, Vector2.left, halfColliderWidth, groundLayerMask);
            RaycastHit2D hitRight = Physics2D.Raycast(rayOrigin, Vector2.right, halfColliderWidth, groundLayerMask);
            Debug.DrawRay(rayOrigin, Vector2.left * halfColliderWidth, Color.red);
            Debug.DrawRay(rayOrigin, Vector2.right * halfColliderWidth, Color.red);

            if (hitLeft.collider != null && hitRight.collider != null)
            {
                if (hitLeft.rigidbody?.linearVelocityX < 0)
                {
                    if(hitRight.rigidbody?.linearVelocityX >= 0)
                    {
                        isStucked = true;
                        hitLeft.rigidbody.linearVelocityX = 0;
                        hitLeft.rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                    }
                }
                if (hitRight.rigidbody?.linearVelocityX > 0)
                {
                    if(hitLeft.rigidbody?.linearVelocityX >= 0)
                    {
                        isStucked = true;
                        hitRight.rigidbody.linearVelocityX = 0;
                        hitRight.rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                    }
                }
            }
        }

        if (isStucked)
        {
            AudioManager.Instance.PlaySFX(SFX.PlayerDie);
            StartCoroutine(DeathCoroutine());
        }
        
    }

    private void ApplyMovement()
    {
        if (UIManager.Instance.IsOpenIngameMenu)
        {
            myAnim.SetBool(isRunString, false);
            moveDir = Vector2.zero;
        }

        myRigid.linearVelocityX = moveDir.x * moveSpeed;
        if (moveDir.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveDir.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (UIManager.Instance.IsOpenIngameMenu) return;
        if (context.started)
        {
            myAnim.SetBool(isRunString, true);
        }
        else if (context.canceled)
        {
            myAnim.SetBool(isRunString, false);
        }
        moveDir = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (UIManager.Instance.IsOpenIngameMenu) return;
        if (!isAir && context.started)
        {
            myAnim.SetTrigger(isJumpString);
            myRigid.linearVelocityY = jumpSpeed;
        }
    }

    public void Interacat(InputAction.CallbackContext context)
    {
        if (UIManager.Instance.IsOpenIngameMenu) return;
        if (context.started)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(myColli.bounds.center, halfColliderWidth * 2f, buttonLayerMask);
            Debug.DrawRay(myColli.bounds.center, Vector2.right * halfColliderWidth * 2f, Color.green, 5);
            foreach (Collider2D col in colliders)
            {
                ButtonController button = col.GetComponentInParent<ButtonController>();
                if (button != null)
                {
                    button.ReleaseSameColorPressedButton();
                }
            }
        }
    }

    public void Restart(InputAction.CallbackContext context)
    {
        if (UIManager.Instance.IsOpenIngameMenu) return;
        if (context.started)
        {
            if (isDeath || isClear) return;
            if(GameManager.Instance.IsStageResetable)
            {
                myInput.currentActionMap.Disable();
                onDeath?.Invoke(false);
            }
            
        }
    }

    public void Exit(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isDeath || isClear) return;
            if (!UIManager.Instance.IsOpenIngameMenu)
            {
                UIManager.Instance.EnableIngameMenuUI();
            }
            else
            {
                UIManager.Instance.DisableIngameMenuUI();
            }
        }
            
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            AudioManager.Instance.PlaySFX(SFX.PlayerDie);
            StartCoroutine(DeathCoroutine());
        }
    }

    IEnumerator DeathCoroutine()
    {
        UIManager.Instance.DisableIngameMenuUI();
        if (isDeath || isClear) yield break;
        myRigid.constraints = RigidbodyConstraints2D.FreezeAll;
        isDeath = true;
        myAnim.SetTrigger(deathString);
        myInput.currentActionMap.Disable();
        yield return new WaitForSeconds(0.5f);
        onDeath?.Invoke(false);
    }

    public void OnCollisionGoal(Collider2D goalColli)
    {
        UIManager.Instance.DisableIngameMenuUI();
        if (isDeath || isClear) return;
        isClear = true;
        var center = goalColli.bounds.center;
        myRigid.constraints = RigidbodyConstraints2D.FreezeAll;
        myInput.currentActionMap.Disable();
        myAnim.SetTrigger(clearString);
        transform.DOMove(center, 1f).SetEase(Ease.OutQuart);
    }
}
