using System;
using UnityEngine;

public class ButtonEventSubscriber : MonoBehaviour
{
    public Action<bool> onPressedEvents; //event�� Action�� �� ���� ���ĳ����ž� ������ ��...
    public Action onUnPressedEvents;
    public Action playerInteractoinEvents;
    public ButtonController.ButtonType buttonType;

    public void SubscribeButton()
    {
        foreach (var button in ButtonManager.Instance.buttons[(int)buttonType])
        {
            //Debug.Log($"{name} door sub button");
            button.onPressed += onPressedEvents;
            button.onUnPressed += onUnPressedEvents;
            button.playerInteration += playerInteractoinEvents;
        }
    }

    public void UnSubscribeButton()
    {
        foreach (var button in ButtonManager.Instance.buttons[(int)buttonType])
        {
            //Debug.Log($"{name} door unsub button");
            button.onPressed -= onPressedEvents;
            button.onUnPressed -= onUnPressedEvents;
            button.playerInteration -= playerInteractoinEvents;
        }
    }
}
