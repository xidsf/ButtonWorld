using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEventSubscriber : MonoBehaviour
{
    public Action<bool> onPressedEvents;
    public Action onUnPressedEvents;
    public Action playerInteractoinEvents;
    public ButtonController.ButtonType buttonType;
    
    private void Start()
    {
        SubscribeButton();
        SceneManager.sceneUnloaded += UnSubscribeButton;
    }

    private void SubscribeButton()
    {
        foreach (var button in ButtonManager.Instance.buttons[(int)buttonType])
        {
            button.onPressed += onPressedEvents;
            button.onUnPressed += onUnPressedEvents;
            button.playerInteration += playerInteractoinEvents;
        }
    }

    private void UnSubscribeButton(Scene scene)
    {
        foreach (var button in ButtonManager.Instance.buttons[(int)buttonType])
        {
            button.onPressed -= onPressedEvents;
            button.onUnPressed -= onUnPressedEvents;
            button.playerInteration -= playerInteractoinEvents;
        }
        SceneManager.sceneUnloaded -= UnSubscribeButton;
    }
}
