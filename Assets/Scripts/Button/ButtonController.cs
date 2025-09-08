using System;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public enum ButtonType
    {
        Stone, 
        Brown, 
        Yellow
    }

    [SerializeField] ButtonEventDetector buttonEventDetectArea;
    [SerializeField] bool isPressedMaintained;
    [SerializeField] Rigidbody2D buttonRigid;
    [SerializeField] SpriteRenderer bottomSpriteRenderer;
    [SerializeField] Transform buttonTransform;
    [SerializeField] Sprite goldBottomSprite;

    RigidbodyConstraints2D rigidConstrain = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

    Vector3 originLocalTransform;

    public ButtonType buttonType;
    public event Action<bool> onPressed;
    public event Action onUnPressed;
    public event Action playerInteration;

    private bool isPressed = false;
    private float pressedY = 0.29f;

    private void Start()
    {
        buttonEventDetectArea.changePressed += PressEvent;
        originLocalTransform = buttonTransform.localPosition;
        ChangeButtonBottom();
    }

    private void ChangeButtonBottom()
    {
        if(isPressedMaintained)
        {
            bottomSpriteRenderer.sprite = goldBottomSprite;
        }
    }

    private void PressEvent(bool isPressed)
    {
        if (isPressed)
        {
            onPressed?.Invoke(isPressedMaintained);
            if(isPressedMaintained)
            {
                SetSameColorButtonsPressed();
            }
        }
        else
        {
            onUnPressed?.Invoke();
        }
    }

    private void SetSameColorButtonsPressed()
    {
        foreach (var button in ButtonManager.Instance.buttons[(int)buttonType])
        {
            if (button.isPressedMaintained && button.buttonType == buttonType)
            {
                button.SetPressedState();
            }
        }
    }

    private void SetPressedState()
    {
        buttonTransform.localPosition = new Vector3(buttonTransform.localPosition.x, originLocalTransform.y - pressedY, buttonTransform.localPosition.z);
        buttonRigid.constraints = RigidbodyConstraints2D.FreezeAll;
        isPressed = true;
    }

    public void ReleaseSameColorPressedButton()
    {
        foreach (var button in ButtonManager.Instance.buttons[(int)buttonType])
        {
            if (button.isPressedMaintained && button.buttonType == buttonType)
            {
                button.ReleaseButton();
            }
        }
    }

    private void ReleaseButton()
    {
        if (isPressedMaintained && isPressed)
        {
            buttonTransform.localPosition = new Vector3(buttonTransform.localPosition.x, buttonTransform.localPosition.y + 0.1f, buttonTransform.localPosition.z);
            buttonRigid.constraints = rigidConstrain;
            isPressed = false;
            playerInteration?.Invoke();
        }
    }

    private void OnDisable()
    {
        buttonEventDetectArea.changePressed -= PressEvent;
    }

}
