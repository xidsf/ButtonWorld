using DG.Tweening;
using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    ButtonEventSubscriber subscriber;
    Rigidbody2D myRigid;

    [SerializeField] Vector3 movePosition;
    [SerializeField] float openSpeed;
    [SerializeField] float closeSpeed;

    private bool isFixed = false;
    Vector3 openPosition;
    Vector3 originPosition;

    [SerializeField] Ease easeType;

    private void Awake()
    {
        originPosition = transform.position;
        openPosition = transform.position + movePosition; 

        subscriber = GetComponent<ButtonEventSubscriber>();
        myRigid = GetComponent<Rigidbody2D>();

        subscriber.onPressedEvents += OpenDoor;
        subscriber.onUnPressedEvents += CloseDoor;
        subscriber.playerInteractoinEvents += ReleaseDoor;
    }


    private void OpenDoor(bool isFix)
    {
        if(!isFixed)
        {
            isFixed = isFix;
        }
        StopAllCoroutines();
        //StartCoroutine(DoorMoveCoroutine(openPosition));
        DOTween.Kill(myRigid);
        myRigid.DOMove(openPosition, openSpeed).SetSpeedBased(true).SetEase(easeType);
    }

    private void CloseDoor()
    {
        if(isFixed)
        {
            return;
        }
        StopAllCoroutines();
        //StartCoroutine(DoorMoveCoroutine(originPosition));
        DOTween.Kill(myRigid);
        myRigid.DOMove(originPosition, closeSpeed).SetSpeedBased(true).SetEase(easeType);
    }

    private void ReleaseDoor()
    {
        if(isFixed)
        {
            isFixed = false;
            CloseDoor();
        }
    }

    IEnumerator DoorMoveCoroutine(Vector3 targetPosition)
    {
        float moveSpeed = closeSpeed;
        if(targetPosition == openPosition)
        {
            moveSpeed = 3f;
        }
        else
        {
            moveSpeed = closeSpeed;
        }
        while (transform.position != targetPosition)
        {
            myRigid.linearVelocity = (targetPosition - transform.position).normalized * moveSpeed;
            if (Vector3.SqrMagnitude(transform.position - targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                myRigid.linearVelocity = Vector3.zero;
            }
            yield return new WaitForFixedUpdate();
        }
    }

}
